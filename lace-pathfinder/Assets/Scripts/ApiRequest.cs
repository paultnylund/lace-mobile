using System;
using RestSharp;
using RestSharp.Authenticators;
using Newtonsoft.Json.Linq;

namespace Lace {
    public static class API {
        public static JToken ApiRequest() {

            var client = new RestClient("https://api.lace.guide/graph/getGraphModel");
            var request = new RestRequest(Method.POST);
            request.AddHeader("Postman-Token", "c238e69e-0ccf-4450-ab8d-34b18d971b0f");
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("auth", "afjCEsnkK3bJ@#$dz%3JRTMtWJIAZs@Cc$Me*%!KkXpNR9G1MS$2xtfn5!FfGsy!caK5#kVd4l%ghDyFWp2jAVGaPYdAaerCDW9Snu0G#IOXVBIb*uCx5gt7O0&c1&tUg#G7Nd5nUHTQM7d32nzRlRa3D&WqWN9y&Bqe3SCv7C*mS4LFV5kM37wFbgDgvjELZI%mvx*v&a!w0Ie3XWy$Gdu6NJJUJ#eN^&Q!pCUVyWkZ9B7py8p^a*92r80iOrX3v@BSREqS^MEkx3$#2kUtP%#X5Oq!L*Ovg9Fg5$6xR0oX");
            IRestResponse response = client.Execute(request);
            return JArray.Parse(response.Content);
        }
    }
}