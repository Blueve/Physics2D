using Physics2D;
using Physics2D.Common;
using Physics2D.Force;
using Physics2D.Factories;
using Physics2D.Object;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WPFDemo.Graphic;

namespace WPFDemo.CircleDemo
{
    public class CircleDemo : PhysicsGraphic, IDrawable
    {
        private Particle centerObj;

        private List<Particle> objList = new List<Particle>();

        public CircleDemo(Image image)
            : base(image)
        {
            // 初始化中心点
            centerObj = physicsWorld.CreateFixedParticle
            (
                new Vector2D
                (
                    ConvertUnits.ToSimUnits(250f),
                    ConvertUnits.ToSimUnits(200f)
                )
            );
            // 注册绘制对象
            drawQueue.Add(this);
        }

        protected override void UpdatePhysics(float duration)
        {
            foreach (var item in objList)
            {
                Vector2D v = centerObj.Position - item.Position;
                float d = v.Length();
                item.AddForce(v.Normalize() * 30f);
            }
            physicsWorld.Update(duration);
        }

        public void Fire()
        {
            if (!Start)
            {
                // 全局增加一个小阻尼
                ZoneFactory.CreateGlobalZone(physicsWorld, new ParticleDrag(0.01f, 0.02f));
                Start = true;
            }
            var item = physicsWorld.CreateParticle
            (
                new Vector2D(ConvertUnits.ToSimUnits(200f), ConvertUnits.ToSimUnits(200f)),
                new Vector2D(0f, 5f),
                1f
            );
            objList.Add(item);
        }

        public void Draw(WriteableBitmap bitmap)
        {
            bitmap.FillEllipseCentered
            (
                centerObj.Position.X.ToDisplayUnits(),
                centerObj.Position.Y.ToDisplayUnits(), 6, 6, Colors.Red
            );
            for (int i = objList.Count - 1; i >= 0; i--)
            {
                int x = objList[i].Position.X.ToDisplayUnits();
                int y = objList[i].Position.Y.ToDisplayUnits();

                bitmap.FillEllipseCentered(x, y, 4, 4, Colors.Black);
            }
        }
    }
}