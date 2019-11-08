using System;
using System.Linq;
using System.Collections.Generic;
using EvolutionaryPerceptron.Classes;

namespace EvolutionaryPerceptron.Classes
{
    public class GeneticAlgorithm
    {
        #region Properties

        public Individual[] Population { get; private set; } = new Individual[0];
        public Individual BestIndividual { get; private set; } = new Individual();
        public int Generation { get; private set; } = 0;

        private Random Random;

        private List<Individual> MatingPool = new List<Individual>();

        #endregion

        #region Constructors

        //Initializes a GeneticAlgorithm with a population with of N size.
        public GeneticAlgorithm(int populationSize, int individualGenesSize, double mutationChance, Random random)
        {
            this.Population = new Individual[populationSize];
            this.Random = random;

            for (int i = 0; i < populationSize; i++)
            {
                Population[i] = new Individual(individualGenesSize, mutationChance, this.Random, true);
            }
        }

        #endregion

        #region Functions

        /*Evolves the Population. In other words, preforms 
         * one iteration of the genetic algorithm.*/
        public void EvolvePopulation()
        {
            Generation++;
            UpdateBestIndividual();
            PreformSelection();
            CrossoverAndMutation();
        }

        //Selects individuals for breeding.
        private void PreformSelection()
        {
            for (int i = 0; i < Population.Length; i++)
            {
                if (ShouldSelectIndividual(Population[i]))
                    MatingPool.Add(Population[i]);

                else
                {
                    int randomIndex = Random.Next(0, Population.Length);
                    MatingPool.Add(Population[randomIndex]);
                }
            }
        }

        /*Breeds the next generation from the individuals in the mating pool.
        Mutation also happens in this function.*/
        private void CrossoverAndMutation()
        {
            for (int i = 0; i < Population.Length; i++)
            {
                Individual parent1 = ChooseParent();
                Individual parent2 = ChooseParent();
                Individual child = parent1.Corssover(parent2);
                child.Mutate();

                Population[i] = child;
            }
        }

        #region Helper functions

        //Selects an Individual from the breeding pool for mating.
        private Individual ChooseParent()
        {
            while (true)
            {
                int randomIndex = Random.Next(0, MatingPool.Count);

                if (ShouldSelectIndividual(MatingPool[randomIndex]))
                    return MatingPool[randomIndex];
            }
        }

        //Determines if an individual should be selected for mating.
        private bool ShouldSelectIndividual(Individual individual)
        {
            double selectionChance = individual.Fitness / BestIndividual.Fitness;

            if (selectionChance > Random.NextDouble())
                return true;
            else
                return false;
        }

        //Finds the best individual of the generation and records it.
        private void UpdateBestIndividual()
        {
            BestIndividual = (Population.OrderByDescending(o => o.Fitness).ToArray())[0];
        }

        #endregion

        #endregion
    }
}