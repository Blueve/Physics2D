namespace Physics2D.Factories
{
    using System.Collections.Generic;
    using Physics2D.Collision;
    using Physics2D.Collision.Basic;
    using Physics2D.Collision.Shapes;
    using Physics2D.Common;
    using Physics2D.Common.Exceptions;
    using Physics2D.Core;
    using Physics2D.Object;

    public static class ContactFactory
    {
        /// <summary>
        /// 在物理世界创建一条边缘
        /// </summary>
        /// <param name="world"></param>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <returns></returns>
        public static Edge CreateEdge(this World world, Vector2D pointA, Vector2D pointB)
        {
            var edge = new Edge(pointA, pointB);
            world.AddEdge(edge);
            return edge;
        }

        /// <summary>
        /// 在物理世界创建一条边缘
        /// </summary>
        /// <param name="world"></param>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <returns></returns>
        public static Edge CreateEdge(this World world, double x1, double y1, double x2, double y2)
        {
            var edge = new Edge(x1, y1, x2, y2);
            world.AddEdge(edge);
            return edge;
        }

        /// <summary>
        /// 在物理世界创建一个由边缘围成的封闭多边形
        /// </summary>
        /// <param name="world"></param>
        /// <param name="points">多边形点集(逆时针)</param>
        /// <returns></returns>
        public static IEnumerable<Edge> CreatePolygonEdge(this World world, params Vector2D[] points)
        {
            List<Edge> result = new List<Edge>();

            if (points.Length < 3)
            {
                throw new InvalidArgumentException(
                    $"Can't create a polygon by given points. points.Length = {points.Length}", nameof(points));
            }

            for (int i = 1; i < points.Length; i++)
            {
                result.Add(world.CreateEdge(points[i - 1], points[i]));
            }

            result.Add(world.CreateEdge(points[points.Length - 1], points[0]));
            return result;
        }

        /// <summary>
        /// 在物理世界创建一条质体绳索
        /// 该绳索由两个质体（质点）组成，两个质体的距离被限定在指定的数值内
        /// </summary>
        /// <param name="world"></param>
        /// <param name="maxLength"></param>
        /// <param name="restitution"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static ParticleRope CreateRope(this World world, double maxLength, double restitution, Particle a, Particle b)
        {
            var rope = new ParticleRope(maxLength, restitution, a, b);
            return world.CreateContact(rope);
        }

        /// <summary>
        /// 在物理世界创建一根质体硬棒
        /// 该硬棒由两个质体（质点）组成，两个质体的距离不会发生变化
        /// </summary>
        /// <param name="world"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static ParticleRod CreateRod(this World world, Particle a, Particle b)
        {
            var rod = new ParticleRod(a, b);
            return world.CreateContact(rod);
        }

        /// <summary>
        /// 在物理世界中创建一组关联
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="world"></param>
        /// <param name="contact"></param>
        /// <returns></returns>
        public static T CreateContact<T>(this World world, T contact)
            where T : ParticleContactGenerator
        {
            world.ContactGenerators.Add(contact);
            return contact;
        }
    }
}
