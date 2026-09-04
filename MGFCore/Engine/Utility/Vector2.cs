using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace MGFCore.Engine.Utility;

/// <summary>
/// A vector in 2D space represented by floating-point coordinates.
/// </summary>
[DebuggerDisplay("{X}, {Y}")]
public struct Vector2
{
    public float X, Y;

    public static readonly Vector2 Zero = new Vector2(0.0f, 0.0f);
    public static readonly Vector2 One = new Vector2(1.0f, 1.0f);

    /// <summary>
    /// Creates a new 2D vector.
    /// </summary>
    /// <param name="x">The X component.</param>
    /// <param name="y">The Y component.</param>
    public Vector2(float x, float y)
    {
        X = x;
        Y = y;
    }

    public override string ToString()
    {
        return $"({X}, {Y})";
    }

    /// <summary>
    /// Returns the length of this vector.
    /// </summary>
    public float Length()
    {
        return (float)Math.Sqrt(X * X + Y * Y);
    }

    /// <summary>
    /// Returns a copy of this vector rotated clockwise around the origin.
    /// </summary>
    /// <param name="degrees">The angle to rotate (in degrees).</param>
    public Vector2 Rotated(float degrees)
    {
        float radians = (float)(degrees * Math.PI / 180);
        float sin = (float)Math.Sin(radians);
        float cos = (float)Math.Cos(radians);
        return new Vector2(
            X * cos - Y * sin,
            X * sin + Y * cos);
    }

    /// <summary>
    /// Returns a copy of this vector normalized so that its length is one. If the length is zero it will be unchanged.
    /// </summary>
    public Vector2 Normalized()
    {
        float length = Length();
        if (length == 0)
        {
            return Zero;
        }
        else
        {
            return this / length;
        }
    }

    public Vector2 WithLength(float length)
    {
        return Normalized() * length;
    }

    /// <summary>
    /// Returns a copy of this vector scaled to the given length. Assumes the vector is already normalized.
    /// </summary>
    /// <param name="v"></param>
    /// <param name="length"></param>
    /// <returns></returns>
    public static Vector2 WithLengthPreNormalized(Vector2 v, float length)
    {
        return v * length;
    }

    public Vector2 Perpendicular()
    {
        return new Vector2(-Y, X);
    }

    public Vector2 Abs()
    {
        return new Vector2(Math.Abs(X), Math.Abs(Y));
    }

    public Vector2 Min(Vector2 other)
    {
        return new Vector2(Math.Min(X, other.X), Math.Min(Y, other.Y));
    }

    public Vector2 Max(Vector2 other)
    {
        return new Vector2(Math.Max(X, other.X), Math.Max(Y, other.Y));
    }

    public Vector2 Clamp(Vector2 min, Vector2 max)
    {
        return new Vector2(
            Math.Clamp(X, min.X, max.X),
            Math.Clamp(Y, min.Y, max.Y));
    }

    public Vector2 ClampMin(Vector2 min)
    {
        return new Vector2(
            Math.Max(X, min.X),
            Math.Max(Y, min.Y));
    }

    public Vector2 ClampMax(Vector2 max)
    {
        return new Vector2(
            Math.Min(X, max.X),
            Math.Min(Y, max.Y));
    }

    public float GetAngle()
    {
        return (float)(Math.Atan2(Y, X) * 180 / Math.PI);
    }

    public float DistanceTo(Vector2 other)
    {
        return (other - this).Length();
    }

    public Point2 ToPoint2()
    {
        return new Point2((int)X, (int)Y);
    }

    /// <summary>
    /// Returns the dot product of two vectors.
    /// </summary>
    /// <param name="a">The first vector.</param>
    /// <param name="b">The second vector.</param>
    public static float Dot(Vector2 a, Vector2 b)
    {
        return a.X * b.X + a.Y * b.Y;
    }

    /// <summary>
    /// Returns the Z component of the cross product of two vectors.
    /// </summary>
    /// <param name="a">The first vector.</param>
    /// <param name="b">The second vector.</param>
    public static float Cross(Vector2 a, Vector2 b)
    {
        return a.X * b.Y - a.Y * b.X;
    }

    public static Vector2 operator -(Vector2 a)
    {
        return new Vector2(-a.X, -a.Y);
    }

    public static Vector2 operator +(Vector2 a, Vector2 b)
    {
        return new Vector2(a.X + b.X, a.Y + b.Y);
    }

    public static Vector2 operator -(Vector2 a, Vector2 b)
    {
        return new Vector2(a.X - b.X, a.Y - b.Y);
    }

    public static Vector2 operator *(float s, Vector2 v)
    {
        return new Vector2(s * v.X, s * v.Y);
    }

    public static Vector2 operator *(Vector2 v, float s)
    {
        return new Vector2(s * v.X, s * v.Y);
    }

    public static Vector2 operator *(int s, Vector2 v)
    {
        return new Vector2(s * v.X, s * v.Y);
    }

    public static Vector2 operator *(Vector2 v, int s)
    {
        return new Vector2(s * v.X, s * v.Y);
    }

    public static Vector2 operator /(Vector2 v, float s)
    {
        return new Vector2(v.X / s, v.Y / s);
    }

    public static Vector2 operator /(Vector2 v, int s)
    {
        return new Vector2(v.X / s, v.Y / s);
    }

    // implicit conversion from Point2 to Vector2
    public static implicit operator Vector2(Point2 p)
    {
        return new Vector2(p.X, p.Y);
    }
}
