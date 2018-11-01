using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour {
    
	// Use this for initialization
	void Start () {
	    
	    API api = new API();
	    api.MakeRequest();
	    
	    AStar astar = new AStar();
		astar.BestPath(api.response);
		
		Populate populate = new Populate();
		populate.GenerateWaypoints();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
