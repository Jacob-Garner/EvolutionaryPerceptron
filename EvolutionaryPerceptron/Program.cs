using System;
using EvolutionaryPerceptron.Classes;

namespace EvolutionaryPerceptron
{
    class Program
    {
        static void Main(string[] args)
        {
            //Start prompt.
            Console.Title = "Evolutionary Perceptron";
            Console.WriteLine("Evolutionary Perceptron is ready. Press enter to start...");
            Console.ReadLine();

            //Initialize everything.
            Random random = new Random();
            DataSet dataSet = new DataSet("Training data\\CloudsDataSet.txt");
            GeneticAlgorithm geneticAlgorithm = new GeneticAlgorithm(500, 3, 0.5d, random);
            Perceptron perceptron = new Perceptron(2);

            //Load info about the data set.
            Console.WriteLine("Loaded data set:\n" + dataSet.Name + "\n");
            Console.WriteLine("Description:\n" + dataSet.Description);

            //Evolve the perceptron.
            while (true)
            {
                /* Test all the Individuals (which evolve the weights and bias 
                 * for the perceptron) to see which ones have generated the best 
                 * weights and bias for the perceptron. */
                for (int i = 0; i < geneticAlgorithm.Population.Length; i++)
                {
                    geneticAlgorithm.Population[i].Fitness =
                        RunTest(ref perceptron, dataSet, geneticAlgorithm.Population[i]);
                }

                Console.Write("\rGeneration: " + geneticAlgorithm.Generation + 
                    ", Fitness: " + geneticAlgorithm.BestIndividual.Fitness);

                //Check to exit the loop (see note at the bottom of this file).
                if (geneticAlgorithm.BestIndividual.Fitness >= dataSet.Data.Count)
                {
                    ApplyWeightAndBias(ref perceptron, geneticAlgorithm.BestIndividual);
                    break;
                }

                geneticAlgorithm.EvolvePopulation();
            }

            Console.WriteLine("\n\nRunning test data...\n");

            //Run the test data through the perceptron.
            foreach (double[] testData in dataSet.TestData)
            {
                double perceptronResult = perceptron.Activate(testData);

                string cloudShape = testData[1] == 0 ? "puffy" : "thin";
                string cloudAltitude = testData[0].ToString();
                string cloudType = perceptronResult < 0.5 ? "Cumulus" : "Cirrus";

                Console.WriteLine("Test data: " + cloudAltitude + "," + cloudShape);
                Console.WriteLine("A cloud that is " + cloudShape +  
                    " in shape and has an altitude of " + cloudAltitude + 
                    " is a " + cloudType + " cloud.\n");
            }

            Console.WriteLine("\nExecution finished. Press enter to exit...");
            Console.ReadLine();
        }

        //Tests an Individual's "solution" (the weights and bias it generated)
        static int RunTest(ref Perceptron perceptron, DataSet dataSet, Individual individual)
        {
            int score = 0;
            ApplyWeightAndBias(ref perceptron, individual);

            for (int i = 0; i < dataSet.Data.Count; i++)
            {
                double perceptronResult = 
                    Math.Round(perceptron.Activate(dataSet.Data[i]), MidpointRounding.AwayFromZero);

                if (perceptronResult == dataSet.DataSolutions[i])
                    score++;
            }

            return score;
        }

        //Applies the weights and bias from a given individual to the perceptron.
        static void ApplyWeightAndBias(ref Perceptron perceptron, Individual individual)
        {
            Array.Copy(individual.Genes, perceptron.Weights, 2);
            perceptron.Bias = individual.Genes[individual.Genes.Length - 1];
        }
    }
}