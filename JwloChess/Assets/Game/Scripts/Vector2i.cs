using System;


/// <summary>
/// A 2D integer coordinate.
/// </summary>
public struct Vector2i
{
	public static Vector2i Zero { get { return new Vector2i(0, 0); } }


	public int x, y;

	public static Vector2i operator+(Vector2i lhs, Vector2i rhs)
	{
		return new Vector2i(lhs.x + rhs.x, lhs.y + rhs.y);
	}
	public static Vector2i operator-(Vector2i lhs, Vector2i rhs)
	{
		return new Vector2i(lhs.x - rhs.x, lhs.y - rhs.y);
	}
	public static Vector2i operator*(Vector2i lhs, int rhs)
	{
		return new Vector2i(lhs.x * rhs, lhs.y * rhs);
	}
	public static Vector2i operator/(Vector2i lhs, int rhs)
	{
		return new Vector2i(lhs.x / rhs, lhs.y / rhs);
	}

	public static bool operator==(Vector2i a, Vector2i b)
	{
		return a.x == b.x && a.y == b.y;
	}
	public static bool operator !=(Vector2i a, Vector2i b)
	{
		return a.x != b.x || a.y != b.y;
	}
	
	
	public Vector2i LessX { get { return new Vector2i(x - 1, y); } }
	public Vector2i LessY { get { return new Vector2i(x, y - 1); } }
	public Vector2i MoreX { get { return new Vector2i(x + 1, y); } }
	public Vector2i MoreY { get { return new Vector2i(x, y + 1); } }

	
	public Vector2i(int _x, int _y) { x = _x; y = _y; }


	public override bool Equals(object obj)
	{
		return (obj is Vector2i) && (((Vector2i)obj) == this);
	}
	public override int GetHashCode()
	{
		return (x * 73856093) ^ (y * 19349663);
	}

	public override string ToString()
	{
		return "{" + x.ToString() + ", " + y.ToString() + "}";
	}
}