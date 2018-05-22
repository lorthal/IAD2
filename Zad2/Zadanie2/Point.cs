using System;

public struct Point
{
    public double x { get; set; }
    public double y { get; set; }

    public Point(double x, double y)
    {
        this.x = x;
        this.y = y;
    }

    public override string ToString()
    {
        return string.Format("({0},{1})", x, y);
    }

    public double[] ToArray()
    {
        return new double[] { x, y };
    }
}

public struct IntPoint
{
    public int x { get; set; }
    public int y { get; set; }

    public IntPoint(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public override string ToString()
    {
        return string.Format("({0},{1})", x, y);
    }

    public double[] ToArray()
    {
        return new double[] { x, y };
    }
}