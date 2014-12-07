using Physics2D;
using Physics2D.Common;
using Physics2D.Factories;
using Physics2D.Force;
using Physics2D.Object;
using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WPFDemo.Graphic;

namespace WPFDemo.FireworksDemo
{
    public class FireworksDemo : PhysicsGraphic, IDrawable
    {
        private ParticleGravity g = new ParticleGravity(new Vector2D(0, 10f));
        private ParticleDrag particleDrag = new ParticleDrag(2f, 1f);

        private List<Particle> objList = new List<Particle>();

        private int worldHeight = 400;
        private int worldWidth = 500;

        public FireworksDemo(Image image)
            : base(image)
        {
            drawQueue.Add(this);
        }

        protected override void UpdatePhysics(float duration)
        {
            physicsWorld.Update(duration);
        }

        void IDrawable.Draw(WriteableBitmap bitmap)
        {
            bitmap.FillRectangle(0, worldHeight * 2 / 3, worldWidth, worldHeight, Colors.SkyBlue);
            for (int i = objList.Count - 1; i >= 0; i--)
            {
                int x = ConvertUnits.ToDisplayUnits(objList[i].Position.X);
                int y = ConvertUnits.ToDisplayUnits(objList[i].Position.Y);

                if (y > worldHeight || x > worldWidth || x < 0 || y < 0)
                {
                    physicsWorld.RemoveObject(objList[i]);
                    objList.Remove(objList[i]);
                }
                else
                {
                    if (y > worldHeight * 2 / 3)
                        bitmap.FillEllipseCentered(x, y, 4, 4, Colors.DarkBlue);
                    else
                        bitmap.FillEllipseCentered(x, y, 4, 4, Colors.Black);
                }
            }
        }

        public void Fire(float x, float y)
        {
            if (!Start)
            {
                Start = true;
                physicsWorld.ZoneSet.Clear();
                // 增加重力
                ZoneFactory.CreateGloablZone(physicsWorld, g);
                // 增加阻力
                ZoneFactory.CreateRectangleZone(physicsWorld, particleDrag,
                    ConvertUnits.ToSimUnits(0f),
                    ConvertUnits.ToSimUnits(worldHeight * 2 / 3f),
                    ConvertUnits.ToSimUnits(500f),
                    ConvertUnits.ToSimUnits(400f)
                );
            }
            Random rnd = new Random();

            for (int i = 0; i < 10; i++)
            {
                var paritcle = physicsWorld.CreateParticle
                (
                    new Vector2D(x, y),
                    new Vector2D((float)rnd.NextDouble() * 6 - 3, (float)rnd.NextDouble() * 6 - 3),
                    1f
                );
                objList.Add(paritcle);
            }
        }
    }
}