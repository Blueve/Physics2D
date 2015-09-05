using Physics2D.Common;

namespace Physics2D.Object
{
    public abstract class PhysicsObject : IUpdatable
    {
        public Vector2D Position;
        public Vector2D Velocity;
        public Vector2D Acceleration;

        public Vector2D PrePosition;

        public abstract void Update(double duration);
    }
}