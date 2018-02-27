﻿using Accord.Math.Random;
using Accord.Statistics.Filters;
using Accord.Statistics.Models.Regression.Linear;
using Autodesk.DesignScript.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AI.Algorithms.Regression
{
    /// <summary>
    ///  In linear regression, the model specification is that the dependent variable, y is a linear combination of the parameters (but need not be linear in the independent variables). 
    /// </summary>
    public class MultipleLinearRegression :IAlgorithm
    {
        #region Interface Properties

        // Metadata
        public string Name { get; set; }
        public AlgorithmType Type { get; }
        public bool IsTrainingDataLoaded => HasTrainingData();
        public bool IsTrained { get; set; }

        // Type support
        public Type PredictionType { get; }
        public Type ResultType { get; private set; }

        // dataset
        public object LastTestValue => TestValue;
        public object LastResult => Result;

        #endregion

        #region Custom properties

        private double[][] Inputs;
        private double[] Outputs;
        private double[] TestValue;
        private double? Result;

        // Learner & predictor - these are not part of the interface
        private Accord.Statistics.Models.Regression.Linear.MultipleLinearRegression regression;
        private Codification codebook;
        private bool Codify;
        private string CodifyColumn = string.Empty;
        private OrdinaryLeastSquares ols;

        #endregion

        #region Constructor

        /// <summary>
        /// Construct a new Simple Linear Regression algorithm, using the specified training data.
        /// </summary>
        /// <param name="inputList">Use inputList as rows with equal numbers of featurs, which used for learning.</param>
        /// <param name="outputList">Use outputList as the rows that define the result column for each</param>
        public MultipleLinearRegression(List<List<double>> inputList, List<double> outputList, string codifyColumn=null)
        {
            Name = "Multiple Linear Regression";
            Type = AlgorithmType.Regression;
            IsTrained = false;
            PredictionType = typeof(double[]);
            ResultType = typeof(double);
            Inputs = null;
            Outputs = null;
            TestValue = null;
            Result = null;
            if (!string.IsNullOrWhiteSpace(codifyColumn))
            {
                Codify = true;
                CodifyColumn = codifyColumn;
            }

            // initialise seed value for Accord framework
            Generator.Seed = new Random().Next();

            // Process training data
            LoadTrainingData(inputList, outputList);

            // set up linear regression using OrdinaryLeastSquares
            regression = new Accord.Statistics.Models.Regression.Linear.MultipleLinearRegression();
            ols = new OrdinaryLeastSquares() { UseIntercept = true };
        }

        [IsVisibleInDynamoLibrary(false)]
        public MultipleLinearRegression()
        {

        }
        #endregion

        #region ML

        public bool Learn()
        {
            if (Codify)
            {
                // ADD CODE FOR CODIFICATION SUPPORT HERE.
            }
            try
            {
                regression = this.ols.Learn(Inputs, Outputs);
                IsTrained = true;
                return true;
            }
            catch (Exception e)
            {
                throw new Exception(
                    "Failed to learn using specified training data." + Environment.NewLine +
                    "Inner exception : " + e.Message
                    );
            }
            // return this as IAlgorithm;
        }

        public dynamic Predict(dynamic input)
        {
            throw new NotImplementedException();
        }

        public dynamic Predict(List<double> inputData)
        {
            var input = inputData.ToArray();

            // predict & cache test value
            this.TestValue = inputData.ToArray();
            this.Result = this.regression.Transform(input);

            return this.Result;
        }

        #endregion

        #region Utils

        private void LoadTrainingData(List<List<double>> inputList, List<double> outputList)
        {
            // validation
            if (inputList == null || outputList == null) throw new ArgumentNullException("Neither the input list nor the output list can be NULL");

            // process input and output lists into arrays
            Inputs = inputList.Select(x => x.ToArray()).ToArray();
            Outputs = outputList.ToArray();

            if (Codify)
            {
                /*
                
                ADD CODE FOR CODIFICATION SUPPORT HERE.    


                // Create a new codification codebook to convert strings into discrete symbols
                this.codebook = new Codification(CodifyColumn, codebook.Columns.);

                // Extract input and output pairs to train
                int[][] symbols = this.codebook.Transform(CodifyColumn,);
                this.codifiedDataset = symbols.Get(null, 0, -1); // Gets all rows, from 0 to the last (but not the last)
                this.codifiedOutputs = symbols.GetColumn(-1);     // Gets only the last column
                */
                
            }
        }

        private bool HasTrainingData()
        {
            if (this.Inputs == null) return false;
            if (this.Inputs.Length == 0) return false;
            return true;
        }

        #endregion
    }
}
