using System.Collections;
using UnityEngine;

public class Populate : MonoBehaviour {
    
    public float[,] arrWaypoints = new float[,] {
        {2, 3},
        {5, 10},
        {13, 6},
        {20, 20}
    };
    
    public GameObject waypoint;
    [HideInInspector] public GameObject newWaypoint;
    
    void Start () {
        for (int i = 0; i <= 4; i++) {
            newWaypoint = GameObject.Instantiate(waypoint, new Vector3(arrWaypoints[i,0], 0, arrWaypoints[i,1]), Quaternion.identity);
            newWaypoint.transform.parent = GameObject.Find("Path").transform;
        };
    }
};