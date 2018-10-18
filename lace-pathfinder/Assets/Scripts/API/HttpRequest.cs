// using System.Collections;
using System.Diagnostics;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

public class HttpRequest {

    public static HttpClient client = new HttpClient();

    public class Product {
        public string RoomMatrix { get; set; }
        public string Distance { get; set; }
    }
    
    void Start () {

        var url = new Uri("http://104.248.29.252:8080/graph/getGraphModel");
        postRequest("http://104.248.29.252:8080/graph/getGraphModel");

        using (client) {
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36");
            var response = client.GetStringAsync(url).Result;
            var releases = JArray.Parse(response);
        }
    }

    public string postRequest(string url){
        var client = new WebClient();        
        var response = client.DownloadString(url);
        Debug.WriteLine(response);
        return response;
    }
};