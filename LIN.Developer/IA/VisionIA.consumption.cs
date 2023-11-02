﻿// This file was auto-generated by ML.NET Model Builder. 
using Microsoft.ML;
using Microsoft.ML.Data;
using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
namespace LIN.Developer
{
    public partial class VisionIA
    {
        /// <summary>
        /// model input class for VisionIA.
        /// </summary>
        #region model input class
        public class ModelInput
        {
            [ColumnName(@"Label")]
            public string Label { get; set; } = string.Empty;

            [ColumnName(@"ImageSource")]
            [Microsoft.ML.Transforms.Image.ImageType(224, 224)]
            public System.Drawing.Bitmap? ImageSource { get; set; }

        }

        #endregion

        /// <summary>
        /// model output class for VisionIA.
        /// </summary>
        #region model output class
        public class ModelOutput
        {
            [ColumnName("output1")]
            public float[]? Output1 { get; set; }

            public string[] ClassificationLabels = new string[] { "Agricultura", "Alimentos", "Animales", "Arte", "Automoviles", "Deporte", "Frutas", "Hogar", "Juguetes", "Limpieza", "Mascotas", "Moda", "Salud", "Tecnologia", };

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
                    if (this.Output1 == null)
                        return new float[0];
                    var exp = this.Output1.Select(x => (float)System.Math.Exp(x));
                    var exp_sum = exp.Sum();
                    return exp.Select(x => x / exp_sum).ToArray();
                }
            }
        }


        #endregion

        private static string MLNetModelPath = Path.GetFullPath("wwwroot/Models/VisionIA.zip");



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
