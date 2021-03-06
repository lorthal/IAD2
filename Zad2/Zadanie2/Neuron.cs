﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Neuron
{
    public List<double> Weights { get; }
    public List<double> PreviousWeights { get; private set; }

    public Point Position { get; private set; }

    public IntPoint Coord { get; }

    public double Potential { get; private set; }

    public bool Used { get; set; }

    private readonly double minPotential;
    
    public Neuron(Point position, IntPoint coord, double minPotential)
    {
        Used = false;
        Weights = new List<double>();
        PreviousWeights = new List<double>();
        Position = position;
        Coord = coord;
        Potential = 1;
        this.minPotential = minPotential;

        Weights.AddRange(Position.ToArray());
        PreviousWeights.AddRange(Weights);
    }

    public double GetDistance(Point point)
    {
        return Helper.EuclideanDistance(point.ToArray(), Weights.ToArray());
    }

    public void UpdataWeights(Point point, double learningRate, double influence)
    {
        Used = true;
        PreviousWeights.Clear();
        PreviousWeights.AddRange(Weights);

        for (int i = 0; i < Weights.Count; i++)
        {
            Weights[i] = PreviousWeights[i] + influence * learningRate * (point.ToArray()[i] - PreviousWeights[i]);
        }
        Position = new Point(Weights[0], Weights[1]);
    }

    public void UpdatePotential(Neuron bmu, int neuronCount)
    {
        if (bmu == this && Potential > 0)
        {
            Potential -= minPotential;
            if (Potential < 0)
            {
                Potential = 0;
            }
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