using System;
using System.Collections.Generic;
using System.Text;

public class Neuron
{
    public List<double> Weights { get; }

    public Point Position { get; private set; }

    public IntPoint Coord { get; }

    public double Potential { get; private set; }

    private readonly double minPotential;

    public Neuron(Point position, IntPoint coord, double minPotential)
    {
        Weights = new List<double>();
        Position = position;
        Coord = coord;
        Potential = 1;
        this.minPotential = minPotential;

        Weights.AddRange(Position.ToArray());
    }

    public double GetDistance(Point point)
    {
        return Helper.EuclideanDistance(point.ToArray(), Weights.ToArray());
    }

    public void UpdataWeights(Point point, double learningRate, double influence)
    {
        double[] prevWeights = Weights.ToArray();

        for (int i = 0; i < Weights.Count; i++)
        {
            Weights[i] = prevWeights[i] + influence * learningRate * (point.ToArray()[i] - prevWeights[i]);
        }
        Position = new Point(Weights[0], Weights[1]);
    }

    public void UpdatePotential(Neuron bmu, int neuronCount)
    {
        if (bmu == this)
        {
            Potential -= minPotential;
        }
        else if (Potential < 1)
        {
            Potential += 1 / (double) neuronCount;
            if (Potential > 1)
            {
                Potential = 1;
            }
        }
    }
}