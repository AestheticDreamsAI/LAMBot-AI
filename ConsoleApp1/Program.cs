using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.ML;
using Microsoft.ML.Data;
using Newtonsoft.Json;

// Models for intents and responses
public class Intent
{
    public string Tag { get; set; }
    public List<string> Patterns { get; set; }
    public List<string> Responses { get; set; }
    public List<string> Actions { get; set; }
}

public class IntentCollection
{
    public List<Intent> Intents { get; set; }
}

public class IntentData
{
    public string Text { get; set; }
    public string Label { get; set; }
}

public class IntentPrediction
{
    [ColumnName("PredictedLabel")]
    public string PredictedLabel { get; set; }
}

class Program
{
    static void Main(string[] args)
    {
        if (!File.Exists(".\\data\\"))
            Directory.CreateDirectory(".\\data\\");

        if (!File.Exists(".\\model\\"))
            Directory.CreateDirectory(".\\model\\");
        var intents = LoadIntents(".\\data\\intents.json");

        var mlContext = new MLContext();
        ITransformer model;
        // Check if the model file exists
        if (File.Exists(".\\model\\model.bin")) 
        { 
            // Load the model from the file
            model = mlContext.Model.Load(".\\model\\model.bin", out var modelInputSchema);
            Console.WriteLine("Model loaded from .\\model\\model.zip");
        }
        else
        {
            // Train a new model
            var data = PrepareData(intents);
            var dataView = mlContext.Data.LoadFromEnumerable(data);

            var pipeline = mlContext.Transforms.Conversion.MapValueToKey("Label")
                .Append(mlContext.Transforms.Text.FeaturizeText("Features", "Text"))
                .Append(mlContext.Transforms.Concatenate("Features", "Features"))
                .Append(mlContext.MulticlassClassification.Trainers.SdcaMaximumEntropy("Label", "Features"))
                .Append(mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel"));

            model = pipeline.Fit(dataView);

            // Save the model to a file
            mlContext.Model.Save(model, dataView.Schema, ".\\model\\model.bin");
            Console.WriteLine("Model trained and saved to .\\model\\model.bin");
        }

        Console.WriteLine("Chatbot started. Type 'exit' to quit.");

        while (true)
        {
            Console.Write("You: ");
            string userInput = Console.ReadLine();
            if (userInput.ToLower() == "exit")
                break;

            var prediction = Predict(mlContext, model, userInput);
            var response = GenerateResponse(prediction.PredictedLabel, intents);
            Console.WriteLine("Bot: " + response);

            ExecuteAction(prediction.PredictedLabel, intents);
        }
    }

    static IntentCollection LoadIntents(string filePath)
    {
        using (StreamReader r = new StreamReader(filePath))
        {
            string json = r.ReadToEnd();
            return JsonConvert.DeserializeObject<IntentCollection>(json);
        }
    }

    static IEnumerable<IntentData> PrepareData(IntentCollection intents)
    {
        var data = new List<IntentData>();
        foreach (var intent in intents.Intents)
        {
            foreach (var pattern in intent.Patterns)
            {
                data.Add(new IntentData { Text = pattern, Label = intent.Tag });
            }
        }
        return data;
    }

    static IntentPrediction Predict(MLContext mlContext, ITransformer model, string text)
    {
        var predictionEngine = mlContext.Model.CreatePredictionEngine<IntentData, IntentPrediction>(model);
        return predictionEngine.Predict(new IntentData { Text = text });
    }

    static string GenerateResponse(string tag, IntentCollection intents)
    {
        var intent = intents.Intents.FirstOrDefault(i => i.Tag == tag);
        if (intent != null)
        {
            Random rnd = new Random();
            int index = rnd.Next(intent.Responses.Count);
            return intent.Responses[index];
        }
        return "Sorry, I don't understand.";
    }

    static void ExecuteAction(string tag, IntentCollection intents)
    {
        var intent = intents.Intents.FirstOrDefault(i => i.Tag == tag);
        if (intent != null && intent.Actions != null && intent.Actions.Count > 0)
        {
            foreach (var action in intent.Actions)
            {
                try
                {
                    Process.Start(action);
                    Console.WriteLine($"Executing action: {action}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to execute action: {action}, Error: {ex.Message}");
                }
            }
        }
    }
}
