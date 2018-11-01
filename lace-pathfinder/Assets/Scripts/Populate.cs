using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;

public class Populate : MonoBehaviour {
    
    public List<AStar.Node> listWaypoints;
    // public long distance = API.response.distance;
    
    public GameObject waypoint;
    [HideInInspector] public GameObject newWaypoint;
    
    public void GenerateWaypoints () {
        
        print("Populate");
        
        if (AStar.found) {
            listWaypoints = AStar.grid.path;
            int count = 0;
            foreach (AStar.Node n in listWaypoints) {
                print(n.position.X);
                print(n.position.Y);
                newWaypoint = GameObject.Instantiate(waypoint, new Vector3(n.position.X, -2, n.position.Y), Quaternion.identity);
                newWaypoint.transform.parent = GameObject.Find("PathRenderer").transform;
                count++;
            }; 
        }
    }
};