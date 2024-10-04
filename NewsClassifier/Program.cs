using NewsClassifierModel;
using System;

namespace NewsClassifier
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("News classifier");
            Console.WriteLine();
            var consumeModel = new ConsumeModel();

            string[] headlines = new[]
            {
                "Stocks move lower on discouraging news from space",
                "Respawn: Patch to increase game resolution likely",
                "'Some movie' sequel is in the works",
                "Measles Outbreak In Some County"
            };

            foreach (var headline in headlines)
            {
                PredictResponse prediction = consumeModel.Predict(headline);

                Console.WriteLine($"{headline} ");
                Console.WriteLine($" --- Category: { prediction.PredictionCategory}");
                Console.WriteLine($" --- Prediction - Business:{prediction.BusinessPercent}% Technology:{prediction.TechnologyPercent}% Entertainment:{prediction.EntertainmentPercent}% Health:{prediction.HealthPercent}% ");
                Console.WriteLine();
            }

            Console.WriteLine();
        }
    }
}
