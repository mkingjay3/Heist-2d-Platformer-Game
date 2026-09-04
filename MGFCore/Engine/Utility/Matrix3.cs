using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGFCore.Engine.Utility;

/// <summary>
/// A 3x3 matrix for 2D transformations.
/// </summary>
public class Matrix3
{
    public float m11, m12, m13;
    public float m21, m22, m23;
    public float m31, m32, m33;

    public Matrix3()
    {
        m11 = m12 = m13 = 0;
        m21 = m22 = m23 = 0;
        m31 = m32 = m33 = 0;
    }

    public static Matrix3 Identity { get; } = new()
    {
        m11 = 1, m12 = 0, m13 = 0,
        m21 = 0, m22 = 1, m23 = 0,
        m31 = 0, m32 = 0, m33 = 1
    };

    public static Matrix3 CreateTranslation(Vector2 v)
    {
        return CreateTranslation(v.X, v.Y);
    }

    public static Matrix3 CreateTranslation(float tx, float ty)
    {
        return new Matrix3()
        {
            m11 = 1,
            m12 = 0,
            m13 = tx,
            m21 = 0,
            m22 = 1,
            m23 = ty,
            m31 = 0,
            m32 = 0,
            m33 = 1
        };
    }

    public static Matrix3 CreateRotationRadians(float radians)
    {
        float cos = (float)Math.Cos(radians);
        float sin = (float)Math.Sin(radians);
        return new Matrix3()
        {
            m11 = cos,
            m12 = -sin,
            m13 = 0,
            m21 = sin,
            m22 = cos,
            m23 = 0,
            m31 = 0,
            m32 = 0,
            m33 = 1
        };
    }

    public static Matrix3 CreateRotation(float degrees)
    {
        float radians = (float)(degrees * Math.PI / 180);
        float cos = (float)Math.Cos(radians);
        float sin = (float)Math.Sin(radians);
        return new Matrix3()
        {
            m11 = cos,
            m12 = -sin,
            m13 = 0,
            m21 = sin,
            m22 = cos,
            m23 = 0,
            m31 = 0,
            m32 = 0,
            m33 = 1
        };
    }

    public static Matrix3 CreateScale(Vector2 s)
    {
        return CreateScale(s.X, s.Y);
    }

    public static Matrix3 CreateScale(float sx, float sy)
    {
        return new Matrix3()
        {
            m11 = sx,
            m12 = 0,
            m13 = 0,
            m21 = 0,
            m22 = sy,
            m23 = 0,
            m31 = 0,
            m32 = 0,
            m33 = 1
        };
    }

    public static Matrix3 operator *(Matrix3 a, Matrix3 b)
    {
        return new Matrix3()
        {
            m11 = a.m11 * b.m11 + a.m12 * b.m21 + a.m13 * b.m31,
            m12 = a.m11 * b.m12 + a.m12 * b.m22 + a.m13 * b.m32,
            m13 = a.m11 * b.m13 + a.m12 * b.m23 + a.m13 * b.m33,
            m21 = a.m21 * b.m11 + a.m22 * b.m21 + a.m23 * b.m31,
            m22 = a.m21 * b.m12 + a.m22 * b.m22 + a.m23 * b.m32,
            m23 = a.m21 * b.m13 + a.m22 * b.m23 + a.m23 * b.m33,
            m31 = a.m31 * b.m11 + a.m32 * b.m21 + a.m33 * b.m31,
            m32 = a.m31 * b.m12 + a.m32 * b.m22 + a.m33 * b.m32,
            m33 = a.m31 * b.m13 + a.m32 * b.m23 + a.m33 * b.m33,
        };
    }

    public static Vector2 operator *(Matrix3 m, Vector2 v)
    {
        return new Vector2(
            m.m11 * v.X + m.m12 * v.Y + m.m13,
            m.m21 * v.X + m.m22 * v.Y + m.m23
        );
    }

    public static Matrix3 operator +(Matrix3 a, Matrix3 b)
    {
        return new Matrix3()
        {
            m11 = a.m11 + b.m11,
            m12 = a.m12 + b.m12,
            m13 = a.m13 + b.m13,
            m21 = a.m21 + b.m21,
            m22 = a.m22 + b.m22,
            m23 = a.m23 + b.m23,
            m31 = a.m31 + b.m31,
            m32 = a.m32 + b.m32,
            m33 = a.m33 + b.m33,
        };
    }

    public static Matrix3 operator -(Matrix3 a, Matrix3 b)
    {
        return new Matrix3()
        {
            m11 = a.m11 - b.m11,
            m12 = a.m12 - b.m12,
            m13 = a.m13 - b.m13,
            m21 = a.m21 - b.m21,
            m22 = a.m22 - b.m22,
            m23 = a.m23 - b.m23,
            m31 = a.m31 - b.m31,
            m32 = a.m32 - b.m32,
            m33 = a.m33 - b.m33,
        };
    }

    public static Matrix3 operator *(Matrix3 a, float b)
    {
        return new Matrix3()
        {
            m11 = a.m11 * b,
            m12 = a.m12 * b,
            m13 = a.m13 * b,
            m21 = a.m21 * b,
            m22 = a.m22 * b,
            m23 = a.m23 * b,
            m31 = a.m31 * b,
            m32 = a.m32 * b,
            m33 = a.m33 * b,
        };
    }

    public static Matrix3 operator /(Matrix3 a, float b)
    {
        return new Matrix3()
        {
            m11 = a.m11 / b,
            m12 = a.m12 / b,
            m13 = a.m13 / b,
            m21 = a.m21 / b,
            m22 = a.m22 / b,
            m23 = a.m23 / b,
            m31 = a.m31 / b,
            m32 = a.m32 / b,
            m33 = a.m33 / b,
        };
    }
}
