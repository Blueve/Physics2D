using Physics2D;
using Physics2D.Common;
using Physics2D.Factories;
using Physics2D.Force;
using Physics2D.Force.Zones;
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
        private string type;

        // 空间作用力
        private ParticleGravity g          = new ParticleGravity(new Vector2D(0, 10f));
        private ParticleDrag drag          = new ParticleDrag(2f, 1f);
        private ParticleConstantForce wind = new ParticleConstantForce(new Vector2D(30f, 0f));

        private Zone dragZone = null;
        private Zone windZone = null;

        // 粒子队列
        private List<Particle> objList = new List<Particle>();

        private int worldHeight = 400;
        private int worldWidth  = 500;

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

        public void Fire(float x, float y, string type)
        {
            if (!Start)
            {
                Start = true;
                physicsWorld.ZoneSet.Clear();
                // 增加重力
                ZoneFactory.CreateGloablZone(physicsWorld, g);
            }
            
            
            if(type == "Water + G" && this.type != type)
            {
                this.type = type;
                if (dragZone == null)
                {
                    // 增加阻力
                    dragZone = ZoneFactory.CreateRectangleZone(physicsWorld, drag,
                        ConvertUnits.ToSimUnits(0f),
                        ConvertUnits.ToSimUnits(worldHeight * 2 / 3f),
                        ConvertUnits.ToSimUnits(500f),
                        ConvertUnits.ToSimUnits(400f)
                    );
                }
                else
                {
                    if (windZone != null)
                    {
                        physicsWorld.ZoneSet.Remove(windZone);
                    }
                    physicsWorld.ZoneSet.Add(dragZone);
                }

            }
            else if(type == "Wind + G" && this.type != type)
            {
                this.type = type;
                if (windZone == null)
                {
                    // 增加恒定的风力
                    windZone = ZoneFactory.CreateRectangleZone(physicsWorld, wind,
                        ConvertUnits.ToSimUnits(0f),
                        ConvertUnits.ToSimUnits(worldHeight * 1 / 3f),
                        ConvertUnits.ToSimUnits(500f),
                        ConvertUnits.ToSimUnits(worldHeight * 2 / 3f)
                    );
                }
                else
                {
                    if(dragZone != null)
                    {
                        physicsWorld.ZoneSet.Remove(dragZone);
                    }
                    physicsWorld.ZoneSet.Add(windZone);
                }
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