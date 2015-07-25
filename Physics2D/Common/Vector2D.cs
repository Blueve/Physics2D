using System;
using static System.Math;

namespace Physics2D.Common
{
    public struct Vector2D
    {
        public float X;
        public float Y;

        private static Vector2D ZeroVector = new Vector2D();
        private static Vector2D OneVector = new Vector2D(1f, 1f);
        private static Vector2D UnitXVector = new Vector2D(1f, 0f);
        private static Vector2D UnitYVector = new Vector2D(0f, 1f);

        #region 特殊向量

        public static Vector2D Zero
        {
            get { return ZeroVector; }
        }

        public static Vector2D One
        {
            get { return OneVector; }
        }

        public static Vector2D UnitX
        {
            get { return UnitXVector; }
        }

        public static Vector2D UnitY
        {
            get { return UnitYVector; }
        }

        #endregion 特殊向量

        #region 构造函数

        public Vector2D(float x, float y)
        {
            X = x;
            Y = y;
        }

        public Vector2D(Vector2D vec)
        {
            X = vec.X;
            Y = vec.Y;
        }

        #endregion 构造函数

        #region 公共方法

        public static float DistanceSquared(Vector2D value1, Vector2D value2)
        {
            return (value1.X - value2.X) * (value1.X - value2.X) + (value1.Y - value2.Y) * (value1.Y - value2.Y);
        }

        public static float Distance(Vector2D value1, Vector2D value2)
        {
            return (float)Sqrt(DistanceSquared(value1, value2));
        }

        public float LengthSquared()
        {
            return DistanceSquared(this, ZeroVector);
        }

        public float Length()
        {
            return Distance(this, ZeroVector);
        }

        public static Vector2D Normalize(ref Vector2D value, out Vector2D result)
        {
            float factor;
            float distance = Distance(value, Zero);

            // 零向量标准化仍为零向量
            if (distance == 0f) return result = Vector2D.Zero;

            factor = 1f / distance;

            result = new Vector2D(value.X * factor, value.Y * factor);
            return result;
        }

        public Vector2D Normalize()
        {
            Normalize(ref this, out this);
            return this;
        }

        public void Set(float x, float y)
        {
            X = x;
            Y = y;
        }

        #endregion 公共方法

        #region 运算

        public static Vector2D operator +(Vector2D left, Vector2D right) => new Vector2D(left.X + right.X, left.Y + right.Y);

        public static Vector2D operator -(Vector2D left, Vector2D right) => new Vector2D(left.X - right.X, left.Y - right.Y);

        public static Vector2D operator -(Vector2D right) => new Vector2D(0.0f - right.X, 0.0f - right.Y);

        public static float operator *(Vector2D left, Vector2D right) => left.X* right.X + left.Y* right.Y;

        public static Vector2D operator *(Vector2D left, float factor) => new Vector2D(left.X * factor, left.Y * factor);

        public static Vector2D operator *(float factor, Vector2D right) => right * factor;

        public static Vector2D operator /(Vector2D left, float divisor) => new Vector2D(left.X / divisor, left.Y / divisor);

        public static bool operator ==(Vector2D left, Vector2D right) => Equals(left, right);

        public static bool operator !=(Vector2D left, Vector2D right) => !Equals(left, right);

        #endregion 运算

        #region 重载方法

        public override bool Equals(object obj)
        {
            Vector2D right = (Vector2D)obj;
            return Abs(X - right.X) < Settings.percision && Abs(Y - right.Y) < Settings.percision;
        }

        public override string ToString() => $"({X:f2}, {Y:f2})";

        public override int GetHashCode() => base.GetHashCode();

        #endregion 重载方法
    }
}