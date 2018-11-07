using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Populate : MonoBehaviour {
    
    // public static long distance = Global.Instance.response.distance;
    
    public GameObject waypoint;

    void Update() {
        
        if (Global.Instance.wayFinding == true) {
            
            foreach (Transform child in GameObject.Find("PathRenderer").transform) {
                
                Destroy(child.gameObject);
            }
            
            foreach (AStar.Node n in Global.Instance.grid.path) {

                print(n.position.X + ", " + n.position.Y);
                
                GameObject newWaypoint = Instantiate(waypoint, new Vector3(n.position.X, 0, n.position.Y), Quaternion.identity);
                newWaypoint.transform.parent = GameObject.Find("PathRenderer").transform;
            };
            
            print("Waypoints Instantiated");
        }
    }
}