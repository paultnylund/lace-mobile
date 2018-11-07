using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Main : MonoBehaviour {
    
    API api = new API();
    
    void Start() {
        
        Global.Instance.startX = 0;
        Global.Instance.startY = 0;
        Global.Instance.endX = 3;
        Global.Instance.endY = 0;
    }
    
    void Update() {
        
        api.Post(this);
        
        if (Global.Instance.responseObtained) {
            
            print("Got response! Starting AStar");
            
            AStar.Start();
            
            if (Global.Instance.pathFound) {
                
                print("Path found! Instantiating waypoints");
                Global.Instance.wayFinding = true;
                Global.Instance.responseObtained = false;
            }
        }
    }
}