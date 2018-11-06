using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Populate : MonoBehaviour {
    
    // public long distance = API.response.distance;
    
    public static GameObject waypoint;
    [HideInInspector] public static GameObject newWaypoint;
    
    public static void Start() {
        // waypoint = Resources.Load("Waypoint") as GameObject;
        int count = 0;
        foreach (AStar.Node n in Global.Instance.grid.path) {
            // print(n.position.X + ", " + n.position.Y);
            newWaypoint = Instantiate(waypoint, new Vector3(n.position.X, -2, n.position.Y), Quaternion.identity) as GameObject;
            newWaypoint.transform.parent = GameObject.Find("PathRenderer").transform;
            count++;
        };
    }
};