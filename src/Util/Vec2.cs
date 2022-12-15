namespace aoc2022.Util;

public struct ivec2 : IEquatable<ivec2>, IComparable<ivec2>, IComparable
{
    public int x = 0;
    public int y = 0;

    public static readonly ivec2 ZERO = new ivec2(0, 0);
    public static readonly ivec2 ONE = new ivec2(1, 1);

    public static readonly ivec2 LEFT = new ivec2(-1, 0);
    public static readonly ivec2 RIGHT = new ivec2(1, 0);
    public static readonly ivec2 UP = new ivec2(0, -1);
    public static readonly ivec2 DOWN = new ivec2(0, 1);
    public static readonly ivec2[] FOURWAY = {RIGHT, LEFT, UP, DOWN};
    public static readonly ivec2[] EIGHTWAY = {UP, UP + RIGHT, RIGHT, RIGHT + DOWN, DOWN, DOWN + LEFT, LEFT, LEFT + UP};

    public ivec2(int xv, int yv)
    {
        x = xv;
        y = yv;
    }

    public bool IsZero() => x == 0 && y == 0;
    public int Sum => x + y;
    public int Product => x * y;
    public int MaxElement => System.Math.Max(x, y);

    public ivec2 GetRotatedLeft() => new ivec2(y, -x);
    public void RotateLeft() => this = GetRotatedLeft();

    public ivec2 GetRotatedRight() => new ivec2(-y, x);
    public void RotateRight() => this = GetRotatedRight();

    public int Dot(ivec2 v) => (x * v.x) + (y * v.y);
    public int LengthSquared => (x * x) + (y * y);
    public float Length => MathF.Sqrt(LengthSquared);

    public int ManhattanDistance => Abs(this).Sum;
    public int ManhattanDistanceTo(ivec2 other) => (this - other).ManhattanDistance;

    public ivec2 GetBestDirectionTo(ivec2 p)
    {
        ivec2 diff = p - this;
        if (diff.IsZero())
        {
            return ZERO;
        }

        ivec2 dir = diff / Abs(diff).MaxElement;
        return Sign(dir);
    }

    // get a point in the 8 cells around me closest to p
    public ivec2 GetNearestNeighbor(ivec2 p)
    {
        ivec2 dir = GetBestDirectionTo(p);
        return this + dir;
    }

    public int this[int i]
    {
        get => (i == 0) ? x : y;
        set
        {
            if (i == 0)
            {
                x = value;
            }
            else
            {
                y = value;
            }
        }
    }

    public static ivec2 operator +(ivec2 v) => v;
    public static ivec2 operator -(ivec2 v) => new ivec2(-v.x, -v.y);
    public static ivec2 operator +(ivec2 a, ivec2 b) => new ivec2(a.x + b.x, a.y + b.y);
    public static ivec2 operator -(ivec2 a, ivec2 b) => new ivec2(a.x - b.x, a.y - b.y);
    public static ivec2 operator *(ivec2 a, ivec2 b) => new ivec2(a.x * b.x, a.y * b.y);
    public static ivec2 operator *(int a, ivec2 v) => new ivec2(a * v.x, a * v.y);
    public static ivec2 operator *(ivec2 v, int a) => new ivec2(a * v.x, a * v.y);
    public static ivec2 operator /(ivec2 v, int a) => new ivec2(v.x / a, v.y / a);
    public static bool operator ==(ivec2 a, ivec2 b) => (a.x == b.x) && (a.y == b.y);
    public static bool operator !=(ivec2 a, ivec2 b) => (a.x != b.x) || (a.y != b.y);
    public static bool operator <(ivec2 a, ivec2 b) => (a.x < b.x) && (a.y < b.y);
    public static bool operator <=(ivec2 a, ivec2 b) => (a.x <= b.x) && (a.y <= b.y);
    public static bool operator >(ivec2 a, ivec2 b) => (a.x > b.x) && (a.y > b.y);
    public static bool operator >=(ivec2 a, ivec2 b) => (a.x >= b.x) && (a.y >= b.y);

    public bool Equals(ivec2 other)
    {
        return x == other.x && y == other.y;
    }

    public override bool Equals(object? obj)
    {
        return obj is ivec2 other && Equals(other);
    }

    public int CompareTo(ivec2 other)
    {
        if (this < other)
        {
            return -1;
        }

        if (this > other)
        {
            return 1;
        }

        return 0;
    }

    public int CompareTo(object? obj)
    {
        if (ReferenceEquals(null, obj)) return 1;
        return obj is ivec2 other ? CompareTo(other) : throw new ArgumentException($"Object must be of type {nameof(ivec2)}");
    }

    public static ivec2 Sign(ivec2 v) => new ivec2(System.Math.Sign(v.x), System.Math.Sign(v.y));
    public static ivec2 Min(ivec2 a, ivec2 b) => new ivec2(System.Math.Min(a.x, b.x), System.Math.Min(a.y, b.y));
    public static ivec2 Max(ivec2 a, ivec2 b) => new ivec2(System.Math.Max(a.x, b.x), System.Math.Max(a.y, b.y));

    public static ivec2 Clamp(ivec2 v, ivec2 lh, ivec2 rh) => Min(rh, Max(lh, v));

    public static ivec2 Abs(ivec2 v) => new ivec2(System.Math.Abs(v.x), System.Math.Abs(v.y));

    public static ivec2 Mod(ivec2 val, int den)
    {
        int x = val.x % den;
        int y = val.y % den;

        if (x < 0)
        {
            x += den;
        }

        if (y < 0)
        {
            y += den;
        }

        return new ivec2(x, y);
    }

    public static int Dot(ivec2 a, ivec2 b) => (a.x * b.x) + (a.y * b.y);

    public static ivec2 Parse(string s)
    {
        string[] parts = s.Split(',', 2);
        int x = int.Parse(parts[0]);
        int y = int.Parse(parts[1]);
        return new ivec2(x, y);
    }

    public override int GetHashCode() => HashCode.Combine(x, y);

    public override string ToString() => $"{x},{y}";
}
