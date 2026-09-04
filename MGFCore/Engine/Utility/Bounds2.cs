using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace MGFCore.Engine.Utility;

/// <summary>
/// Represents a 2D rectangular boundary defined by a position and size.
/// </summary>
/// <remarks>The <see cref="Bounds2"/> structure is used to define a rectangular area in 2D space.  It provides
/// methods to check for containment of points and overlap with other bounds.</remarks>
[DebuggerDisplay("{Position}, {Size}")]
public struct Bounds2
{
    public Vector2 Position;
    public Vector2 Size;

    public Vector2 Min => Position;
    public Vector2 Max => Position + Size;

    public float Left => Position.X;
    public float Right => Position.X + Size.X;
    public float Top => Position.Y;
    public float Bottom => Position.Y + Size.Y;

    /// <summary>
    /// Creates a new 2D bounds rectangle.
    /// </summary>
    /// <param name="position">The origin of the bounds.</param>
    /// <param name="size">The size of the bounds.</param>
    public Bounds2(Vector2 position, Vector2 size)
    {
        Position = position;
        Size = size;
    }

    /// <summary>
    /// Creates a new 2D bounds rectangle.
    /// </summary>
    /// <param name="x">The X component of the origin of the bounds.</param>
    /// <param name="y">The Y component of the origin of the bounds.</param>
    /// <param name="width">The width of the bounds.</param>
    /// <param name="height">The height of the bounds.</param>
    public Bounds2(float x, float y, float width, float height)
    {
        Position = new Vector2(x, y);
        Size = new Vector2(width, height);
    }

    public override string ToString()
    {
        return $"({Position}, {Size})";
    }

    /// <summary>
    /// Returns true if a point is within these bounds.
    /// </summary>
    /// <param name="point">The point to test.</param>
    public bool Contains(Vector2 point)
    {
        return !(point.X < Min.X || Max.X < point.X || point.Y < Min.Y || Max.Y < point.Y);
    }

    /// <summary>
    /// Returns true if another bounds rectangle overlaps these bounds.
    /// </summary>
    /// <param name="bounds">The bounds to test.</param>
    public bool Overlaps(Bounds2 bounds)
    {
        return !(bounds.Max.X < Min.X || bounds.Min.X > Max.X || bounds.Max.Y < Min.Y || bounds.Min.Y > Max.Y);
    }
}
