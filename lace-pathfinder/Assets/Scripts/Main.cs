using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Main : MonoBehaviour {
    
    API api = new API();
    
    void Start() {
    
        api.Post(this);
    }
    
    void Update() {
        
        if (Global.Instance.responseObtained == true) {
            
            print("Got response! Starting AStar");
            
            AStar.Start();
            
            if (Global.Instance.pathFound) {
                
                print("Path found! Instantiating waypoints");
                print(Global.Instance.grid.path);
            
                try {
                    
                    Populate.Start();
                }
                
                catch {
                    
                    print("Instantiation Failed");
                }
                
                print("Waypoints Instantiated");
                
                print("Finished");
                Global.Instance.responseObtained = false;
                
                Application.Quit();
            }
            
        } else {
            
            print("Waiting for server...");
        }
    }
}