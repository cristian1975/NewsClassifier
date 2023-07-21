using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.ML;
using Newtonsoft.Json.Linq;

namespace NewsClassifierModel
{
    public class ConsumeModel
    {
        private readonly MLContext _mlContext = new MLContext();
        private readonly PredictionEngine<ModelInput, ModelOutput> _predictionEngine;

        public ConsumeModel()
        {
           //  var path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName;

            var loadedModel = _mlContext
               .Model
               .Load(@"D:\Projects\ML_NewsClassifier\NewsClassifierModel\Model\NewsClassificationModel.zip", out _);

            _predictionEngine = _mlContext
               .Model
               .CreatePredictionEngine<ModelInput, ModelOutput>(loadedModel);
        }

       
        public string Predict(string newsTitle)
        {
            Dictionary<string, string> category = new Dictionary<string, string>()
            {
                { "b" ,"Business" },
                { "t" ,"Science and technology" },
                { "e" , "Entertainment" },
                { "m" , "Health" }
            };

            var modelInput = new ModelInput { Title = newsTitle };
            ModelOutput prediction = _predictionEngine.Predict(modelInput);

            if (category.TryGetValue(prediction.Category, out string description))
            {
                return $"Category: { description} -- Prediction - Business:{prediction.Score[0]} Technology:{prediction.Score[1]} Entertainment:{prediction.Score[2]} Health:{prediction.Score[3]}";
            }

            return "No Category";
            
        }

    }

}