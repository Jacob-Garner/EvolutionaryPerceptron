using System;
using System.IO;
using System.Collections.Generic;

namespace EvolutionaryPerceptron.Classes
{
    public class DataSet
    {
        #region Properties

        public string Name { get; private set; } = "";
        public string Description { get; private set; } = "";

        public List<double[]> Data = new List<double[]>();
        public List<double> DataSolutions = new List<double>();
        public List<double[]> TestData = new List<double[]>();

        #endregion

        #region Constructors

        //Initializes a DataSet by reading data from a data set file.
        public DataSet(string DataSetFilePath)
        {
            ReadDataFromFile(DataSetFilePath);
        }

        #endregion

        #region Functions

        //Reads data from a data set file and stores it in the DataSet.
        private void ReadDataFromFile(string DataSetFilePath)
        {
            if (File.Exists(DataSetFilePath))
            {
                StreamReader fileReader = new StreamReader(DataSetFilePath);

                try
                {
                    while (fileReader.Peek() > -1)
                    {
                        string fileLine = fileReader.ReadLine();

                        if (fileLine.Contains("@"))
                            this.Name = fileLine.Replace("@", "");

                        else if (fileLine.Contains("-"))
                            this.Description += fileLine.Remove(0, 1) + "\n";

                        else if (fileLine.Contains(">"))
                        {
                            var data = ParseData(fileLine);

                            this.Data.Add(data.Item1);
                            this.DataSolutions.Add(data.Item2);
                        }

                        else if (fileLine.Contains("?"))
                        {
                            double[] data = ParseTestData(fileLine);
                            this.TestData.Add(data);
                        }
                    }
                }

                catch
                {
                    throw new Exception("An error occurred while reading: " + DataSetFilePath);
                }

                finally
                {
                    fileReader.Close();
                    fileReader.Dispose();
                }
            }

            else
                throw new Exception("The file you are trying to access is either " +
                    "referenced incorrectly, missing, or is in a different path.");
        }

        //Takes a line of data from a data set file, parses it, then return it.
        private Tuple<double[], double> ParseData(string fileLine)
        {
            fileLine = fileLine.Replace("> ", "");
            string[] dataAsString = fileLine.Split(',');
            double[] dataAsDouble = Array.ConvertAll(dataAsString, double.Parse);

            double[] data = new double[dataAsDouble.Length - 1];
            Array.Copy(dataAsDouble, data, data.Length);
            double dataSolution = dataAsDouble[dataAsDouble.Length - 1];

            return new Tuple<double[], double>(data, dataSolution);
        }

        //Takes a line of test data from a data set file, parses it, then return it.
        private double[] ParseTestData(string fileLine)
        {
            fileLine = fileLine.Replace("? ", "");
            string[] dataAsString = fileLine.Split(',');
            
            return Array.ConvertAll(dataAsString, double.Parse);
        }

        #endregion
    }
}