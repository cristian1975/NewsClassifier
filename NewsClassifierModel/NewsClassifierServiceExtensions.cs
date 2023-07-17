using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.ML;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NewsClassifierModel
{
    public static class NewsClassifierServiceExtensions
    {
        private static readonly string _modelFile =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Model", "NewsClassificationModel.zip");

        public static void AddNewsClassifierPredictionEnginePool(this IServiceCollection services)
        {
            services.AddPredictionEnginePool<ModelInput, ModelOutput>()
               .FromFile(filePath: _modelFile, watchForChanges: true);
        }
    }
}
