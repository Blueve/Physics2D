using System;
using static System.Math;

namespace Physics2D.Common
{
    public struct Vector2D : IEquatable<Vector2D>
    {
        public double X;
        public double Y;

        #region 特殊向量

        public static Vector2D Zero { get; } = new Vector2D();

        public static Vector2D One { get; } = new Vector2D(1, 1);

        public static Vector2D UnitX { get; } = new Vector2D(1, 0);

        public static Vector2D UnitY { get; } = new Vector2D(0, 1);

        #endregion

        #region 构造函数

        public Vector2D(double x, double y)
        {
            X = x;
            Y = y;
        }

        public Vector2D(Vector2D vec)
        {
            X = vec.X;
            Y = vec.Y;
        }

        #endregion

        #region 公共方法

        public static double DistanceSquared(Vector2D value1, Vector2D value2)
        {
            return (value1.X - value2.X) * (value1.X - value2.X) + (value1.Y - value2.Y) * (value1.Y - value2.Y);
        }

        public static double Distance(Vector2D value1, Vector2D value2) => Sqrt(DistanceSquared(value1, value2));

        public double LengthSquared() => DistanceSquared(this, Zero);

        public double Length() => Distance(this, Zero);

        public static Vector2D Normalize(Vector2D value)
        {
            double distance = Distance(value, Zero);

            // 零向量标准化仍为零向量
            if (distance == 0) return Zero;

            var factor = 1 / distance;

            return new Vector2D(value.X * factor, value.Y * factor);
        }

        public Vector2D Normalize() =>Normalize(this);

        public void Set(double x, double y)
        {
            X = x;
            Y = y;
        }

        #endregion

        #region 运算

        public static Vector2D operator +(Vector2D left, Vector2D right) => new Vector2D(left.X + right.X, left.Y + right.Y);

        public static Vector2D operator -(Vector2D left, Vector2D right) => new Vector2D(left.X - right.X, left.Y - right.Y);

        public static Vector2D operator -(Vector2D right) => new Vector2D(.0 - right.X, .0 - right.Y);

        public static double operator *(Vector2D left, Vector2D right) => left.X* right.X + left.Y* right.Y;

        public static Vector2D operator *(Vector2D left, double factor) => new Vector2D(left.X * factor, left.Y * factor);

        public static Vector2D operator *(double factor, Vector2D right) => right * factor;

        public static Vector2D operator /(Vector2D left, double divisor) => new Vector2D(left.X / divisor, left.Y / divisor);

        public static bool operator ==(Vector2D left, Vector2D right) => Abs(left.X - right.X) < Settings.Percision && Abs(left.Y - right.Y) < Settings.Percision;

        public static bool operator !=(Vector2D left, Vector2D right) => !(left == right);

        #endregion 运算

        #region 接口实现
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Vector2D && Equals((Vector2D)obj);
        }

        public bool Equals(Vector2D other) => Abs(X - other.X) < Settings.Percision && Abs(Y - other.Y) < Settings.Percision;

        public override string ToString() => $"({X:f2}, {Y:f2})";

        public override int GetHashCode() => base.GetHashCode();

        #endregion
    }
}