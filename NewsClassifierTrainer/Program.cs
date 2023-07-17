﻿using Common;
using Microsoft.ML;
using Microsoft.ML.AutoML;
using NewsClassifierModel;
using System;
using System.IO;
using System.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace NewsClassifierTrainer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("News classification trainer started");

            //FindTheBestModel();

            TrainTheModel();

        }

        private static void TrainTheModel()
        {
            Console.WriteLine("The News classifier model trainer");

            var mlContext = new MLContext(seed: 0);

            var trainDataPath = @"Data\uci-news-aggregator.csv";
            IDataView trainingDataView = mlContext.Data.LoadFromTextFile<ModelInput>(
                trainDataPath,
                hasHeader: true,
                separatorChar: ',',
                allowQuoting: true);

            var preProcessingPipeline = mlContext.Transforms.Conversion
                .MapValueToKey(inputColumnName: "Category", outputColumnName: "Label")
                .Append(mlContext.Transforms.Text.FeaturizeText(inputColumnName: "Title",
                    outputColumnName: "Features"))
                .Append(mlContext.Transforms.NormalizeMinMax("Features"))
                .AppendCacheCheckpoint(mlContext);

            var trainer = mlContext
                    .MulticlassClassification
                    .Trainers
                    .OneVersusAll(mlContext.BinaryClassification.Trainers.AveragedPerceptron());

            var trainingPipeline = preProcessingPipeline
                                    .Append(trainer)
                                    .Append(
                                        mlContext
                                                .Transforms
                                                .Conversion
                                                .MapKeyToValue("PredictedLabel")
                                           );
            //Console.WriteLine("Cross validating model");
            //var cvResults = mlContext
            //                    .MulticlassClassification
            //                    .CrossValidate(trainingDataView, trainingPipeline);

            //var microAccuracy = cvResults.Average(m => m.Metrics.MicroAccuracy);
            //var macroAccuracy = cvResults.Average(m => m.Metrics.MacroAccuracy);
            //var logLossReduction = cvResults.Average(m => m.Metrics.LogLossReduction);

            //Console.WriteLine();
            //Console.WriteLine($"Cross validation Metrics for our model");
            //Console.WriteLine($"--------------------------------------");
            //Console.WriteLine($" MicroAccuracy:    {microAccuracy:0.###}");
            //Console.WriteLine($" MacroAccuracy:    {macroAccuracy:0.###}");
            //Console.WriteLine($" LogLossReduction: {logLossReduction:#.###}");
            //Console.WriteLine($"--------------------------------------");
            //Console.WriteLine();

            //Train final model on all data
            Console.WriteLine("Training model...");
            var startingTime = DateTime.Now;

            var finalModel = trainingPipeline.Fit(trainingDataView);

            Console.WriteLine($"Model training finished in {(DateTime.Now - startingTime).TotalSeconds} seconds");
            Console.WriteLine();

            //Save model
            Console.WriteLine("Saving model");

            if (!Directory.Exists("Model"))
            {
                Directory.CreateDirectory("Model");
            }

            var modelPath = @"D:\Projects\ML_NewsClassifier\NewsClassifierModel\Model\NewsClassificationModel.zip";
            mlContext.Model.Save(finalModel, trainingDataView.Schema, modelPath);

            Console.WriteLine($"Model saved to {modelPath}");

        }


        private static void FindTheBestModel()
        {

            Console.WriteLine("Finding the best model using AutoML");

            var mlContext = new MLContext(seed: 0);

            var trainingDataPath = "Data\\uci-news-aggregator.csv";

            IDataView trainingDataView = mlContext.Data.LoadFromTextFile<ModelInput>(
                trainingDataPath,
                hasHeader: true,
                separatorChar: ',',
                allowQuoting: true);

            var preProcessingPipeline = mlContext.Transforms.Conversion
                .MapValueToKey(inputColumnName: "Category", outputColumnName: "Category");

            var mappedInputData = preProcessingPipeline.Fit(trainingDataView).Transform(trainingDataView);

            var experimentSettings = new MulticlassExperimentSettings
            {
                MaxExperimentTimeInSeconds = 300,
                CacheBeforeTrainer = CacheBeforeTrainer.On,
                OptimizingMetric = MulticlassClassificationMetric.MicroAccuracy,
                CacheDirectory = null
            };

            var experiment =
                mlContext.Auto().CreateMulticlassClassificationExperiment(experimentSettings);

            Console.WriteLine("Starting experiments");

            var experimentResult =
                        experiment.Execute(
                            trainData: mappedInputData,
                            labelColumnName: "Category",
                            progressHandler: new MulticlassExperimentProgressHandler()
                );

            Console.WriteLine("Metrics from best run:");

            var metrics = experimentResult.BestRun.ValidationMetrics;

            Console.WriteLine($"MicroAccuracy: {metrics.MicroAccuracy:0.##}");
            Console.WriteLine($"MacroAccuracy: {metrics.MacroAccuracy:0.##}");


        }
    }
}
