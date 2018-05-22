using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

class Program
{
    static int epochs = 1000;
    static int neurons = 100;
    static double learningRate = 0.6;
    static double sigma0 = 3;
    static Network.Method method = Network.Method.Kohonen;
    static List<Helper.ShapeParams> shapes = new List<Helper.ShapeParams>() { new Helper.ShapeParams() { shape = Helper.Shape.Circle, args = new object[] { new IntPoint(0, 0), 3, 200 } } };

    static void Main(string[] args)
    {
        System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
        customCulture.NumberFormat.NumberDecimalSeparator = ".";

        System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;

        ParseArgs(args);

        foreach (var shape in shapes)
        {
            Helper.SetShape(shape.shape, shape.args);
        }

        if (Helper.Plot)
        {
            GnuPlot.Set("", "xrange[-10:10]", "yrange[-10:10]", "size square"/*, "terminal gif animate delay 5"*/,
                $"output '{Helper.outputPath}{Helper.outputFilename.Replace(".txt", ".gif")}'");
            Helper.PlotPoints(Helper.ShapePoints, "with points pt '+' lc rgb 'black'", Helper.outputPath + "shape.txt");
        }

        Network network = new Network(Helper.ShapePoints, epochs, neurons, learningRate, sigma0, method);
        network.Learn();
    }

    static void ParseArgs(string[] args)
    {
        switch (args.Length)
        {
            case 0:
                break;
            default:
            case 6:
                Helper.outputFilename = args[5];
                goto case 5;
            case 5:
                switch (args[4])
                {
                    case "kohonen":
                        method = Network.Method.Kohonen;
                        break;
                    case "neural_gas":
                        method = Network.Method.NeuralGas;
                        break;
                    default:
                        Console.WriteLine("Wrong method");
                        throw new ArgumentException("Wrong method");
                }
                goto case 4;
            case 4:
                sigma0 = double.Parse(args[3]);
                goto case 3;
            case 3:
                learningRate = double.Parse(args[2]);
                goto case 2;
            case 2:
                neurons = int.Parse(args[1]);
                goto case 1;
            case 1:
                epochs = int.Parse(args[0]);
                break;
        }

        if (args.Length > 6)
        {
            shapes.Clear();
        }

        for (int i = 6; i < args.Length;)
        {
            switch (args[i])
            {
                case "sector":
                    shapes.Add(new Helper.ShapeParams()
                    {
                        shape = Helper.Shape.Sector,
                        args = new object[]
                        {
                            new IntPoint(int.Parse(args[i + 1]), int.Parse(args[i + 2])),
                            int.Parse(args[i + 3]),
                            bool.Parse(args[i + 4]),
                            int.Parse(args[i + 5])
                        }
                    });
                    i += 6;
                    break;
                case "square":
                    shapes.Add(new Helper.ShapeParams()
                    {
                        shape = Helper.Shape.Square,
                        args = new object[]
                        {
                            new IntPoint(int.Parse(args[i + 1]), int.Parse(args[i + 2])),
                            int.Parse(args[i + 3]),
                            int.Parse(args[i + 4])
                        }
                    });
                    i += 5;
                    break;
                case "square_filled":
                    shapes.Add(new Helper.ShapeParams()
                    {
                        shape = Helper.Shape.SquareFilled,
                        args = new object[]
                        {
                            new IntPoint(int.Parse(args[i + 1]), int.Parse(args[i + 2])),
                            int.Parse(args[i + 3]),
                            int.Parse(args[i + 4])
                        }
                    });
                    i += 5;
                    break;
                case "circle":
                    shapes.Add(new Helper.ShapeParams()
                    {
                        shape = Helper.Shape.Circle,
                        args = new object[]
                        {
                            new IntPoint(int.Parse(args[i + 1]), int.Parse(args[i + 2])),
                            int.Parse(args[i + 3]),
                            int.Parse(args[i + 4])
                        }
                    });
                    i += 5;
                    break;
                case "circumference":
                    shapes.Add(new Helper.ShapeParams()
                    {
                        shape = Helper.Shape.Circumference,
                        args = new object[]
                        {
                            new IntPoint(int.Parse(args[i + 1]), int.Parse(args[i + 2])),
                            int.Parse(args[i + 3]),
                            int.Parse(args[i + 4])
                        }
                    });
                    i += 5;
                    break;
            }
        }
    }

}
