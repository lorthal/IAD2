using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

class Program
{
    static void Main(string[] args)
    {
        System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
        customCulture.NumberFormat.NumberDecimalSeparator = ".";

        System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;

        GnuPlot.Set("xrange[-10:10]", "yrange[-10:10]", "size square"/*, "terminal gif animate delay 5"*/, "output 'output.gif'");

        #region shape

        var points1 = Helper.GeneratePointsInSector(new IntPoint(4, 0), 4, false, 50);
        var points2 = Helper.GeneratePointsInSector(new IntPoint(0, 4), 4, true, 50);
        var points3 = Helper.GeneratePointsInSector(new IntPoint(-4, 0), 4, false, 50);
        var points4 = Helper.GeneratePointsInSector(new IntPoint(0, -4), 4, true, 50);

        var pointsInCircle1 = Helper.GeneretePointsInCircle(new IntPoint(3, 0), 2, 100);
        var pointsInCircle2 = Helper.GeneretePointsInCircle(new IntPoint(-3, 0), 2, 100);

        Helper.ShapePoints = new List<Point>();
        //Helper.ShapePoints.AddRange(pointsInCircle1);
        //Helper.ShapePoints.AddRange(pointsInCircle2);

        Helper.ShapePoints.AddRange(points1);
        Helper.ShapePoints.AddRange(points2);
        Helper.ShapePoints.AddRange(points3);
        Helper.ShapePoints.AddRange(points4);

        #endregion

        Helper.PlotPoints(Helper.ShapePoints, "with points pt '+' lc rgb 'black'", true);

        Network network = new Network(Helper.ShapePoints, 1000, 100, 0.6, 3, Network.Method.Kohonen);
        network.Learn();
        Console.ReadKey();
    }

}
