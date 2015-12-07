using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Physics2D.Collision;
using Physics2D.Object;
using Physics2D.Collision.Shapes;
using Physics2D.Common.Events;
using System.Diagnostics;

namespace Physics2D.Core
{
    public sealed class ContactRegistry
    {
        #region 私有字段
        /// <summary>
        /// 物理世界物体列表的引用
        /// </summary>
        private readonly HashSet<PhysicsObject> _objects;

        /// <summary>
        /// 物理世界边缘表的引用
        /// </summary>
        private readonly HashSet<Edge> _edges;

        /// <summary>
        /// 质体碰撞发生器集合
        /// </summary>
        private readonly HashSet<ParticleContactGenerator> _generators = new HashSet<ParticleContactGenerator>();

        /// <summary>
        /// 质体碰撞表
        /// </summary>
        private readonly List<ParticleContact> _contactList = new List<ParticleContact>();

        /// <summary>
        /// 碰撞解决器
        /// </summary>
        private readonly ParticleContactResolver _particleContactResolver = new ParticleContactResolver(0);

        /// <summary>
        /// 碰撞计数器
        /// </summary>
        private int _contactCounter = 0;
        #endregion

        #region 私有方法
        /// <summary>
        /// 向碰撞表中添加一个新的碰撞
        /// </summary>
        /// <param name="contact">碰撞信息</param>
        /// <returns>完成添加后若不允许继续添加则返回false，否则返回true</returns>
        private bool AddToContactList(ParticleContact contact)
        {
            if (_contactCounter++ < Settings.MaxContacts)
            {
                _contactList.Add(contact);
                return true;
            }
            return false;
        }
        #endregion

        #region 公开字段
        /// <summary>
        /// 检测到碰撞
        /// </summary>
        public event ContactHandle OnContact;
        #endregion

        #region 构造方法
        public ContactRegistry(HashSet<PhysicsObject> objects, HashSet<Edge> edges)
        {
            _objects = objects;
            _edges = edges;
        }
        #endregion

        #region 碰撞发生器的注册与注销
        /// <summary>
        /// 注册碰撞发生器
        /// </summary>
        /// <param name="contactGenerator"></param>
        public void Add(ParticleContactGenerator contactGenerator) => _generators.Add(contactGenerator);

        /// <summary>
        /// 移除碰撞发生器
        /// </summary>
        /// <param name="contactGenerator"></param>
        public void Remove(ParticleContactGenerator contactGenerator) => _generators.Remove(contactGenerator);
        #endregion

        /// <summary>
        /// 进行碰撞检测并解决碰撞
        /// </summary>
        /// <param name="duration"></param>
        public void ResolveContacts(double duration)
        {
            _contactCounter = 0;

            // 产生碰撞表
            for (int i = 0; i < Settings.ContactIteration; i++)
            {
                _contactList.Clear();

                List<Shape> shapes = CollectAllShapes(_objects, _edges);

                // 执行质体碰撞检测器
                var contacts = ExcuteParticleCollisionDetector(shapes);
                while (contacts.MoveNext())
                {
                    AddToContactList(contacts.Current);
                }

                // 执行碰撞发生器
                foreach (var contactGenerator in _generators)
                {
                    foreach (var contact in contactGenerator)
                    {
                        if (!AddToContactList(contact)) goto CONTACT_RESOLVE;
                    }
                }

                // 当不再产生新的碰撞时退出
                if (_contactList.Count == 0) break;

                CONTACT_RESOLVE:
                // 解决质体碰撞
                OnContact?.Invoke(this, new ContactEventArgs(_contactList));
                _particleContactResolver.Iterations = _contactList.Count * 2;
                _particleContactResolver.ResolveContacts(_contactList, duration);
            }

        }

        #region 静态方法
        public static List<Shape> CollectAllShapes(HashSet<PhysicsObject> objects, HashSet<Edge> edges)
        {
            var shapes = (from obj in objects
                          where /*obj is Particle &&*/ obj.Shape.Type != ShapeType.Point
                          select obj.Shape).ToList();
            shapes.AddRange(edges);
            return shapes;
        }

        public static IEnumerator<ParticleContact> ExcuteParticleCollisionDetector(List<Shape> sharps)
        {
            ParticleContact contact;
            for (int indexA = 0; indexA < sharps.Count; indexA++)
            {
                for (int indexB = indexA + 1; indexB < sharps.Count; indexB++)
                {
                    // 对形状标识符一致的物体不执行碰撞检测
                    if (sharps[indexA].Id != 0 && sharps[indexA].Id == sharps[indexB].Id) continue;

                    var typeA = sharps[indexA].Type;
                    var typeB = sharps[indexB].Type;

                    if (typeA <= typeB)
                    {
                        contact = DispatchToDetector(ContactTypeMap[(int)typeA, (int)typeB], sharps[indexA], sharps[indexB]);
                    }
                    else
                    {
                        contact = DispatchToDetector(ContactTypeMap[(int)typeA, (int)typeB], sharps[indexB], sharps[indexA]);
                    }
                    if (contact != null) yield return contact;
                }
            }
        }

        /// <summary>
        /// 根据碰撞类型为两个指定的形状执行碰撞检测
        /// </summary>
        /// <param name="type"></param>
        /// <param name="sharpA"></param>
        /// <param name="sharpB"></param>
        public static ParticleContact DispatchToDetector(ContactType type, Shape sharpA, Shape sharpB)
        {
            Debug.Assert(sharpA.Type <= sharpB.Type);

            ParticleContact contact = null;

            switch (type)
            {
                case ContactType.CircleAndCircle:
                    contact = ParticleCollisionDetector.CircleAndCircle(sharpA as Circle, sharpB as Circle);
                    break;
                case ContactType.CircleAndEdge:
                    contact = ParticleCollisionDetector.CircleAndEdge(sharpA as Circle, sharpB as Edge);
                    break;
                case ContactType.CircleAndBox:
                    throw new NotImplementedException("未实现圆与盒的碰撞检测");
                case ContactType.EdgeAndBox:
                    throw new NotImplementedException("未实现边沿与盒的碰撞检测");
                case ContactType.BoxAndBox:
                    throw new NotImplementedException("未实现盒与盒的碰撞检测");
            }
            return contact;
        }
        #endregion

        #region 静态查询表
        /// <summary>
        /// 碰撞类型查询表
        /// </summary>
        public static readonly ContactType[,] ContactTypeMap = new[,]
        {
            {
                ContactType.CircleAndCircle,
                ContactType.CircleAndEdge,
                ContactType.CircleAndBox
            },
            {
                ContactType.CircleAndEdge,
                ContactType.NotSupport,
                ContactType.EdgeAndBox
            },
            {
                ContactType.CircleAndBox,
                ContactType.EdgeAndBox,
                ContactType.BoxAndBox
            }
        };
        #endregion
    }
}
