using System;

namespace EvolutionaryPerceptron.Classes
{
    public class Perceptron
    {
        #region Properties

        public double[] Weights { get; set; } = new double[0];
        public double Bias { get; set; } = 0;

        #endregion

        #region Constructors

        //Initializes a Perceptron with N size (size = number of inputs).
        public Perceptron(int size)
        {
            Weights = new double[size];
        }

        #endregion

        #region Functions

        //Activates the Perceptron with a given set of inputs.
        public double Activate(double[] inputs)
        {
            if (inputs.Length == this.Weights.Length)
            {
                double weightedSum = 0;

                for (int i = 0; i < inputs.Length; i++)
                    weightedSum += inputs[i] * Weights[i];

                weightedSum += Bias;

                return Sigmoid(weightedSum);
            }

            else
                throw new Exception("The size of the Perceptron does not match the size of the inputs!");
        }

        //Preforms the Sigmoid function on a given input.
        private double Sigmoid(double x)
        {
            return 1.0d / (1.0d + Math.Pow(Math.E, -x));
        }

        #endregion
    }
}