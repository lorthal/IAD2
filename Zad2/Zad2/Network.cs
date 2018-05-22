using System;
using System.Collections.Generic;
using System.Globalization;
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
        this.minPotential = 0.75;
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
            //int neuronCountSq = (int)Math.Round(Math.Sqrt(neuronsCount));
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    neurons.Add(new Neuron(Helper.GenerateRandomPoint(-10, 10), new IntPoint(i, j), minPotential));
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

            foreach (var neuron in neurons.FindAll(n => n.Potential > minPotential))
            {
                double distanceToNeuronSq =
                    Helper.SquaredEuclideanDistance(neuron.Coord.ToArray(), bmu.Coord.ToArray());

                double widthSq = neighbourhoodRadius * neighbourhoodRadius;

                if (distanceToNeuronSq < widthSq)
                {
                    double influence = Math.Exp(-distanceToNeuronSq / (2 * widthSq));

                    neuron.UpdataWeights(Inputs[index], learningRate, influence);
                }

                neuron.UpdatePotential(bmu, neurons.Count);
            }

            Helper.PlotPoints(GetNeuronsPositions(), string.Format("title sprintf('epoch={0}') lc rgb 'red', 'shape.txt' using 1:2 title 'shape' with points pt '+' lc rgb 'black'", i));
            Console.WriteLine("Epoch: " + i);
            learningRate = startingLearningRate * Math.Exp(-(double)i / epochs);
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
                double widthSq = neighbourhoodRadius * neighbourhoodRadius;


                double influence = Math.Exp(-potentialNeurons.IndexOf(neuron) / (2 * widthSq));

                neuron.UpdataWeights(Inputs[index], learningRate, influence);

                neuron.UpdatePotential(bmu, neurons.Count);
            }

            Helper.PlotPoints(GetNeuronsPositions(), string.Format("title sprintf('epoch={0}') lc rgb 'red', 'shape.txt' using 1:2 title 'shape' with points pt '+' lc rgb 'black'", i));
            Console.WriteLine("Epoch: " + i);
            learningRate = startingLearningRate * Math.Exp(-(double)i / epochs);
        }
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
            default:
                break;
        }
    }

    public List<Point> GetNeuronsPositions()
    {
        return neurons.Select(n => n.Position).ToList();
    }
}
