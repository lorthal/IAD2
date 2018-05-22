using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

public class Network
{
    public enum Method
    {
        Kohonen,
        NeuralGas
    }

    private List<Neuron> neurons;
    private readonly List<Point> Inputs;
    private int epochs;
    private double startingLearningRate;
    private double learningRate;
    private double sigma;
    private double lambda;
    private double minPotential;

    private Method method;

    public Network(List<Point> inputData, int epochs, int neuronsCount, double learningRate, double sigma0, Method method)
    {
        this.neurons = new List<Neuron>();
        this.Inputs = inputData;
        this.epochs = epochs;
        this.learningRate = learningRate;
        this.startingLearningRate = learningRate;
        this.sigma = sigma0;
        this.method = method;
        this.minPotential = 0.5;
        lambda = epochs / Math.Log(sigma);

        if (method == Method.NeuralGas)
        {
            for (int i = 0; i < neuronsCount; i++)
            {
                neurons.Add(new Neuron(Helper.GenerateRandomPoint(-10, 10), new IntPoint(0, 0), minPotential));
            }
        }
        else
        {
            int neuronsAdded = 0;
            int neuronCountSq = (int)Math.Round(Math.Sqrt(neuronsCount));
            for (int i = 0; neuronsAdded < neuronsCount; i++)
            {
                for (int j = 0; j < neuronCountSq && neuronsAdded < neuronsCount; j++)
                {
                    neurons.Add(new Neuron(Helper.GenerateRandomPoint(-10, 10), new IntPoint(i, j), minPotential));
                    neuronsAdded++;
                }
            }
        }
    }

    private Neuron FindBMU(Point point)
    {
        double minDistance = neurons[0].GetDistance(point);
        Neuron bmu = neurons[0];

        foreach (var neuron in neurons)
        {
            double distance = neuron.GetDistance(point);
            if (distance < minDistance)
            {
                minDistance = distance;
                bmu = neuron;
            }
        }

        return bmu;
    }

    private void Kohonen()
    {
        Random r = new Random();
        for (int i = 1; i <= epochs; i++)
        {
            int index = r.Next(0, Inputs.Count);

            Neuron bmu = FindBMU(Inputs[index]);

            double neighbourhoodRadius = sigma * Math.Exp(-(double)i / lambda);

            foreach (var neuron in neurons)
            {
                if (neuron.Potential > minPotential)
                {
                    double distanceToNeuronSq =
                        Helper.SquaredEuclideanDistance(neuron.Coord.ToArray(), bmu.Coord.ToArray());

                    double widthSq = neighbourhoodRadius * neighbourhoodRadius;

                    if (distanceToNeuronSq < widthSq)
                    {
                        double influence = Math.Exp(-distanceToNeuronSq / (2 * widthSq));

                        neuron.UpdataWeights(Inputs[index], learningRate, influence);
                    }
                }

                neuron.UpdatePotential(bmu, neurons.Count);
            }
            learningRate = startingLearningRate * Math.Exp(-(double)i / epochs);
            DisplayData(i);
        }
    }

    private void NeuralGas()
    {
        Random r = new Random();
        for (int i = 1; i <= epochs; i++)
        {
            int index = r.Next(0, Inputs.Count);

            Neuron bmu = FindBMU(Inputs[index]);

            double neighbourhoodRadius = sigma * Math.Exp(-(double)i / lambda);

            List<Neuron> potentialNeurons = neurons.FindAll(n => n.Potential > minPotential);

            potentialNeurons = potentialNeurons.OrderBy(n => Helper.SquaredEuclideanDistance(n.Position.ToArray(), bmu.Position.ToArray())).ToList();

            foreach (var neuron in potentialNeurons)
            {
                double distanceToNeuronSq =
                    Helper.SquaredEuclideanDistance(new double[]{potentialNeurons.IndexOf(neuron)}, new double[] { potentialNeurons.IndexOf(bmu) });

                double widthSq = neighbourhoodRadius * neighbourhoodRadius;

                if (distanceToNeuronSq < widthSq)
                {
                    double influence = Math.Exp(-potentialNeurons.IndexOf(neuron) / (2 * widthSq));

                    neuron.UpdataWeights(Inputs[index], learningRate, influence);
                }
            }

            foreach (var neuron in neurons)
            {
                neuron.UpdatePotential(bmu, neurons.Count);
            }

            learningRate = startingLearningRate * Math.Exp(-(double)i / epochs);
            DisplayData(i);
        }
    }

    private void DisplayData(int epoch)
    {
        if (Helper.Plot)
        {
            Helper.PlotPoints(GetNeuronsPositions(), 
                string.Format("title sprintf('epoch={0}') lc rgb 'red', '{1}shape.txt' using 1:2 title 'shape' with points pt '+' lc rgb 'black'", epoch,Helper.outputPath));
        }

        double error = CalculateError();
        double unusedNeurons = neurons.FindAll(n => !n.Used).ToArray().Length;

        Console.WriteLine("Epoch " + epoch + " error: " + error + ", unused neurons: " + unusedNeurons);

        if (!File.Exists(Helper.outputPath + Helper.outputFilename))
        {
            using (var fs = File.Create(Helper.outputPath + Helper.outputFilename)) { }
        }
        using (var stream = File.AppendText(Helper.outputPath + Helper.outputFilename))
        {
            stream.WriteLine("{0} {1} {2}", epoch, error, unusedNeurons);
        }

        foreach (var neuron in neurons)
        {
            neuron.Used = false;
        }
    }

    private double CalculateError()
    {
        double sum = 0;

        for (int i = 0; i < Inputs.Count; i++)
        {
            Neuron best = FindBMU(Inputs[i]);
            sum += Helper.SquaredEuclideanDistance(Inputs[i].ToArray(), best.Position.ToArray());
        }

        return (1 / (double)Inputs.Count) * sum;
    }

    public void Learn()
    {
        switch (method)
        {
            case Method.Kohonen:
                Kohonen();
                break;
            case Method.NeuralGas:
                NeuralGas();
                break;
        }
    }

    public List<Point> GetNeuronsPositions()
    {
        return neurons.Select(n => n.Position).ToList();
    }
}
