using Physics2D;
using Physics2D.Collision;
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
        public const int WATER_G = 1;
        public const int WIND_G = 2;

        private int type = 0;
        public int Type
        {
            get
            {
                return type;
            }
            set
            {
                type = value;
                // 设置力场
                if (type == WATER_G)
                {
                    if (!physicsWorld.ZoneSet.Contains(dragZone))
                        physicsWorld.ZoneSet.Add(dragZone);
                    if (physicsWorld.ZoneSet.Contains(windZone))
                        physicsWorld.ZoneSet.Remove(windZone);

                }
                else if (type == WIND_G)
                {
                    if (!physicsWorld.ZoneSet.Contains(windZone))
                        physicsWorld.ZoneSet.Add(windZone);
                    if (physicsWorld.ZoneSet.Contains(dragZone))
                        physicsWorld.ZoneSet.Remove(dragZone);
                }
            }
        }

        // 空间作用力
        private ParticleGravity g = new ParticleGravity(new Vector2D(0, 10f));
        private ParticleDrag drag = new ParticleDrag(2f, 1f);
        private ParticleConstantForce wind = new ParticleConstantForce(new Vector2D(20f, -5f));

        private Zone dragZone;
        private Zone windZone;

        // 粒子队列
        private List<Particle> objList = new List<Particle>();

        private int worldHeight = 400;
        private int worldWidth = 500;

        public FireworksDemo(Image image)
            : base(image)
        {
            drawQueue.Add(this);

            dragZone = new RectangleZone
            (
                ConvertUnits.ToSimUnits(0f),
                ConvertUnits.ToSimUnits(worldHeight * 2 / 3f),
                ConvertUnits.ToSimUnits(500f),
                ConvertUnits.ToSimUnits(400f)
            );
            dragZone.particleForceGenerators.Add(drag);

            windZone = new RectangleZone
            (
                ConvertUnits.ToSimUnits(0f),
                ConvertUnits.ToSimUnits(worldHeight * 1 / 3f),
                ConvertUnits.ToSimUnits(500f),
                ConvertUnits.ToSimUnits(worldHeight * 2 / 3f)
            );
            windZone.particleForceGenerators.Add(wind);
        }

        protected override void UpdatePhysics(float duration)
        {
            physicsWorld.Update(duration);
        }

        void IDrawable.Draw(WriteableBitmap bitmap)
        {
            
            if (type == WATER_G)
                bitmap.FillRectangle(0, worldHeight * 2 / 3, worldWidth, worldHeight, Colors.SkyBlue);
            else if (type == WIND_G)
                bitmap.FillRectangle(0, worldHeight * 1 / 3, worldWidth, worldHeight * 2 / 3, Colors.LightGray);

            bitmap.DrawLineAa(100, 350, 400, 200, Colors.Black);

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
                    if(type == WATER_G)
                    {
                        if (y > worldHeight * 2 / 3)
                            bitmap.FillEllipseCentered(x, y, 4, 4, Colors.DarkBlue);
                        else
                            bitmap.FillEllipseCentered(x, y, 4, 4, Colors.Black);
                    }
                    else
                    {
                        bitmap.FillEllipseCentered(x, y, 4, 4, Colors.Black);
                    }
                }
            }
        }

        public void Fire(float x, float y)
        {
            if (!Start)
            {
                Start = true;
                // 增加重力
                ZoneFactory.CreateGlobalZone(physicsWorld, g);
                // 添加边缘
                physicsWorld.RegistryContactGenerator(contact);

                slot = 1 / 240f;
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
                contact.AddBall(paritcle, ConvertUnits.ToSimUnits(10));
            }
        }

        private ParticleEdge contact = new ParticleEdge(0.02f,
                                                        ConvertUnits.ToSimUnits(100f),
                                                        ConvertUnits.ToSimUnits(350f),
                                                        ConvertUnits.ToSimUnits(400f),
                                                        ConvertUnits.ToSimUnits(200f));
    }
}