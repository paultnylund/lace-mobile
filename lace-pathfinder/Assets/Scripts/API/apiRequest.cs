using System;
using RestSharp;
using RestSharp.Authenticators;
using Newtonsoft.Json.Linq;

namespace API {
    public static class Request {
        public static JToken apiRequest() {

            var client = new RestClient("https://lace.guide/api/graph/getGraphModel");
            var request = new RestRequest(Method.POST);
            
            request.AddHeader("Postman-Token", "26dd6e85-9f57-42c8-b91c-0cbe5b439df6");
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("auth", "afjCEsnkK3bJ@#$dz%3JRTMtWJIAZs@Cc$Me*%!KkXpNR9G1MS$2xtfn5!FfGsy!caK5#kVd4l%ghDyFWp2jAVGaPYdAaerCDW9Snu0G#IOXVBIb*uCx5gt7O0&c1&tUg#G7Nd5nUHTQM7d32nzRlRa3D&WqWN9y&Bqe3SCv7C*mS4LFV5kM37wFbgDgvjELZI%mvx*v&a!w0Ie3XWy$Gdu6NJJUJ#eN^&Q!pCUVyWkZ9B7py8p^a*92r80iOrX3v@BSREqS^MEkx3$#2kUtP%#X5Oq!L*Ovg9Fg5$6xR0oX");
            
            IRestResponse response = client.Execute(request);
            JArray dataPackage = JArray.Parse(response.Content);

            var _id = dataPackage[0]["_id"];
            var distance = dataPackage[0]["distance"];
            var graph = dataPackage[0]["graph"];

            JArray graphParsedRows = (JArray)dataPackage[0]["graph"];
            int numRows = graphParsedRows.Count;
            JArray graphParsedColumns = (JArray)graph[0];
            int numColumns = graphParsedColumns.Count;

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"ID: {_id}\n");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"DISTANCE: {distance}\n");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"GRAPH DIMENSIONS: {numRows}, {numColumns}\n");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"GRAPH:\n");
            for (int i = 0; i < numRows; i++) {
                for (int j = 0; j < numColumns; j++) {
                    Console.Write($"{graph[i][j]} ");
                }
                Console.Write("\n");
            }
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();

            return dataPackage;
        }
    }
}