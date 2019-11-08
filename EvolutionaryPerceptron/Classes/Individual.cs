using System;
using EvolutionaryPerceptron.Classes;

namespace EvolutionaryPerceptron.Classes
{
    public class Individual
    {
        #region Properties

        public double[] Genes { get; set; } = new double[0];
        public double Fitness { get; set; } = 0;
        public double MutationChance = 0;

        private Random Random;

        #endregion

        #region Constructors

        public Individual()
        {

        }

        //Initializes an Individual with N genes.
        public Individual(int genesSize, double mutationChance, Random random, bool shouldInitGenes = true)
        {
            this.Genes = new double[genesSize];
            this.MutationChance = mutationChance;
            this.Random = random;

            if (shouldInitGenes)
            {
                for (int i = 0; i < this.Genes.Length; i++)
                    this.Genes[i] = GetRandomGene();
            }
        }

        #endregion

        #region Functions

        //Mates (breeds) the Individual with another given Individual.
        public Individual Corssover(Individual otherParent)
        {
            Individual child = new Individual(this.Genes.Length, this.MutationChance, this.Random, false);

            for (int i = 0; i < child.Genes.Length; i++)
            {
                child.Genes[i] = (Random.NextDouble() > 0.5) ? this.Genes[i] : otherParent.Genes[i];
            }

            return child;
        }

        //Randomly mutates the Individual.
        public void Mutate()
        {
            for (int i = 0; i < this.Genes.Length; i++)
            {
                if (this.MutationChance > Random.NextDouble())
                    this.Genes[i] = GetRandomGene();
            }
        }

        //Returns a random gene.
        private double GetRandomGene()
        {
            return Random.NextDouble() * 
                (Globals.MaxRndDoubleValue - Globals.MinRndDoubleValue) + Globals.MinRndDoubleValue;
        }

        #endregion
    }
}