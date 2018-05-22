using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

public class Helper
{
    public enum Shape
    {
        Sector,
        Square,
        SquareFilled,
        Circle,
        Circumference
    }

    public struct ShapeParams
    {
        public Shape shape;
        public object[] args;
    }

    public const string outputPath = "D:\\Semestr 6\\IAD\\Zad2Res\\";
    public static string outputFilename = "output.txt";

    public static List<Point> ShapePoints = new List<Point>();

    public static void SetShape(Shape shape, params object[] args)
    {
        switch (shape)
        {
            case Shape.Sector:
                ShapePoints.AddRange(GeneratePointsInSector((IntPoint)args[0], (int) args[1], (bool) args[2], (int) args[3]));
                break;
            case Shape.Square:
                ShapePoints.AddRange(GeneratePointsInSquare((IntPoint)args[0], (int)args[1], (int)args[2]));
                break;
            case Shape.SquareFilled:
                ShapePoints.AddRange(GeneratePointsInSquareFilled((IntPoint)args[0], (int)args[1], (int)args[2]));
                break;
            case Shape.Circle:
                ShapePoints.AddRange(GeneretePointsInCircle((IntPoint)args[0], (int)args[1], (int)args[2]));
                break;
            case Shape.Circumference:
                ShapePoints.AddRange(GeneretePointsInCircumference((IntPoint)args[0], (int)args[1], (int)args[2]));
                break;
        }
    }

    public static List<Point> GeneretePointsInCircle(IntPoint center, int radius, int count)
    {
        List<Point> points = new List<Point>();

        Random r = new Random();
        double minX = center.x - radius;
        double maxX = center.x + radius;

        for (int i = 0; i < count; i++)
        {
            double x = r.NextDouble(minX, maxX);
            double minY = -Math.Sqrt((radius * radius) - (x * x - 2 * x * center.x + center.x * center.x)) + center.y;
            double maxY = -minY;
            double y = r.NextDouble(minY, maxY);

            Point p = new Point(x, y);
            points.Add(p);
        }

        return points;
    }

    public static List<Point> GeneretePointsInCircumference(IntPoint center, int radius, int count)
    {
        List<Point> points = new List<Point>();

        Random r = new Random();
        double minX = center.x - radius;
        double maxX = center.x + radius;

        for (int i = 0; i < count; i++)
        {
            double x = r.NextDouble(minX, maxX);
            double minY = -Math.Sqrt((radius * radius) - (x * x - 2 * x * center.x + center.x * center.x)) + center.y;
            double maxY = -minY;
            double y = r.Next(0, 2) == 1 ? maxY : minY;

            Point p = new Point(x, y);
            points.Add(p);
        }

        return points;
    }

    public static List<Point> GeneratePointsInSector(IntPoint center, int halfLength, bool xAxis, int count)
    {
        List<Point> points = new List<Point>();

        Random r = new Random();

        double minX = center.x - halfLength;
        double maxX = center.x + halfLength;

        double minY = center.y - halfLength;
        double maxY = center.y + halfLength;

        for (int i = 0; i < count; i++)
        {
            if (xAxis)
            {
                double x = r.NextDouble(minX, maxX);
                double y = center.y;

                Point p = new Point(x, y);
                points.Add(p);
            }
            else
            {
                double y = r.NextDouble(minY, maxY);
                double x = center.x;

                Point p = new Point(x, y);
                points.Add(p);
            }
        }

        return points;
    }

    public static List<Point> GeneratePointsInSquare(IntPoint center, int halfLength, int count)
    {
        List<Point> square = new List<Point>();
        square.AddRange(GeneratePointsInSector(new IntPoint(center.x + halfLength, center.y), halfLength, false, count / 4));
        square.AddRange(GeneratePointsInSector(new IntPoint(center.x - halfLength, center.y), halfLength, false, count / 4));
        square.AddRange(GeneratePointsInSector(new IntPoint(center.x, center.y + halfLength), halfLength, true, count / 4));
        square.AddRange(GeneratePointsInSector(new IntPoint(center.x, center.y - halfLength), halfLength, true, count / 4));

        return square;
    }

    public static List<Point> GeneratePointsInSquareFilled(IntPoint center, int halfLength, int count)
    {
        List<Point> square = new List<Point>();
        Random r = new Random();

        double minX = center.x - halfLength;
        double maxX = center.x + halfLength;

        double minY = center.y - halfLength;
        double maxY = center.y + halfLength;

        for (int i = 0; i < count; i++)
        {
            double x = r.NextDouble(minX, maxX);
            double y = r.NextDouble(minY, maxY);
            square.Add(new Point(x,y));
        }

        return square;
    }

    public static List<Point> GenerateRandomPoints(double min, double max, int count)
    {
        List<Point> points = new List<Point>();
        for (int i = 0; i < count; i++)
        {
            points.Add(GenerateRandomPoint(min, max));
        }
        return points;
    }

    public static Point GenerateRandomPoint(double min, double max)
    {
        Random r = new Random();
        Point point = new Point(r.NextDouble(min, max), r.NextDouble(min, max));

        return point;
    }

    public static void PlotPoints(List<Point> points, string options, string saveFileName = "")
    {
        double[] X = points.Select(p => p.x).ToArray();
        double[] Y = points.Select(p => p.y).ToArray();
        GnuPlot.Plot(X, Y, options);
        if (saveFileName != "")
        {
            GnuPlot.SaveData(X, Y, saveFileName);
        }
    }

    public static double EuclideanDistance(double[] a, double[] b)
    {
        var length = a.Length;
        if (length != b.Length)
        {
            throw new ArgumentException("Lenghts of weights arrays are differ.");
        }

        return Math.Sqrt(SquaredEuclideanDistance(a, b));
    }

    public static double SquaredEuclideanDistance(double[] a, double[] b)
    {
        var length = a.Length;
        if (length != b.Length)
        {
            throw new ArgumentException("Lenghts of weights arrays are differ.");
        }

        double distance = 0;
        for (int i = 0; i < length; i++)
        {
            var diff = a[i] - b[i];
            distance += diff * diff;
        }
        return distance;
    }
}

public static class Extentions
{
    public static double NextDouble(this Random r, double minimum, double maximum)
    {
        return r.NextDouble() * (maximum - minimum) + minimum;
    }
}