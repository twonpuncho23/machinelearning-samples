﻿// This file was auto-generated by ML.NET Model Builder. 
using Microsoft.ML;
using Microsoft.ML.Data;
using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
namespace LandUse_Console
{
    public partial class LandUse
    {
        /// <summary>
        /// model input class for LandUse.
        /// </summary>
        #region model input class
        public class ModelInput
        {
            [ColumnName(@"Label")]
            public string Label { get; set; }

            [ColumnName(@"ImageSource")]
            public string ImageSource { get; set; }

        }

        #endregion

        /// <summary>
        /// model output class for LandUse.
        /// </summary>
        #region model output class
        public class ModelOutput
        {
            [ColumnName("output1")]
            public float[] Output1 { get; set; }

            public string[] ClassificationLabels = new string[] { "AnnualCrop", "Forest", "HerbaceousVegetation", "Highway", "Industrial", "Pasture", "PermanentCrop", "Residential", "River", "SeaLake", };

            public string Prediction
            {
                get
                {
                    var maxScore = this.Score.Max();
                    var maxIndex = Array.IndexOf(this.Score, maxScore);
                    return this.ClassificationLabels[maxIndex];
                }
            }

            public float[] Score
            {
                get
                {
                    var exp = this.Output1.Select(x => (float)Math.Exp(x));
                    var exp_sum = exp.Sum();
                    return exp.Select(x => x / exp_sum).ToArray();
                }
            }
        }


        #endregion

        private static string MLNetModelPath = Path.GetFullPath("LandUse.zip");

        public static readonly Lazy<PredictionEngine<ModelInput, ModelOutput>> PredictEngine = new Lazy<PredictionEngine<ModelInput, ModelOutput>>(() => CreatePredictEngine(), true);

        /// <summary>
        /// Use this method to predict on <see cref="ModelInput"/>.
        /// </summary>
        /// <param name="input">model input.</param>
        /// <returns><seealso cref=" ModelOutput"/></returns>
        public static ModelOutput Predict(ModelInput input)
        {
            var predEngine = PredictEngine.Value;
            return predEngine.Predict(input);
        }

        private static PredictionEngine<ModelInput, ModelOutput> CreatePredictEngine()
        {
            var mlContext = new MLContext();
            ITransformer mlModel = mlContext.Model.Load(MLNetModelPath, out var _);
            return mlContext.Model.CreatePredictionEngine<ModelInput, ModelOutput>(mlModel);
        }
    }
}
