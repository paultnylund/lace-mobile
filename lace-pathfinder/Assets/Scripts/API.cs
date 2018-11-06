using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

public class API {

    public void Post(MonoBehaviour instance) {
    
        instance.StartCoroutine(PostRequest());
    }

    IEnumerator PostRequest() {
        
        UnityWebRequest www = new UnityWebRequest("https://api.lace.guide/graph/getGraphModel", "POST");
        
        DownloadHandlerBuffer dH = new DownloadHandlerBuffer();
        www.downloadHandler = dH;
        
        www.SetRequestHeader("Content-Type", "application/json");
        www.SetRequestHeader("auth", "afjCEsnkK3bJ@#$dz%3JRTMtWJIAZs@Cc$Me*%!KkXpNR9G1MS$2xtfn5!FfGsy!caK5#kVd4l%ghDyFWp2jAVGaPYdAaerCDW9Snu0G#IOXVBIb*uCx5gt7O0&c1&tUg#G7Nd5nUHTQM7d32nzRlRa3D&WqWN9y&Bqe3SCv7C*mS4LFV5kM37wFbgDgvjELZI%mvx*v&a!w0Ie3XWy$Gdu6NJJUJ#eN^&Q!pCUVyWkZ9B7py8p^a*92r80iOrX3v@BSREqS^MEkx3$#2kUtP%#X5Oq!L*Ovg9Fg5$6xR0oX");
        
        yield return www.SendWebRequest();
        
        if (www.isNetworkError || www.isHttpError) {
            
            Debug.Log(www.error);
            
        } else {
            
            string data = www.downloadHandler.text;
            data = data.Trim('[');
            data = data.Trim(']');
            
            Global.Instance.response = JsonConvert.DeserializeObject<Response>(data);
            yield return Global.Instance.response;
            Global.Instance.responseObtained = true;
        }
    }
    
    public class Response {
        
        [JsonProperty("_id")]
        public string _id { get; set; }

        [JsonProperty("graph")]
        public int[][] graph { get; set; }

        [JsonProperty("distance")]
        public long distance { get; set; }
    }
}