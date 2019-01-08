using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Populate : MonoBehaviour {

    // public static long distance = Global.Instance.response.distance;

    public LineRenderer parent;
    public GameObject waypoint;
    public GameObject goal;

    void Start() {

      parent.startColor = Global.Instance.deepBlue;
      parent.endColor = Global.Instance.transparent;
    }

    void LateUpdate() {

        if (Global.Instance.wayFinding == true) {

            foreach (Transform child in parent.transform) {

                Destroy(child.gameObject);
            }

            foreach (AStar.Node n in Global.Instance.grid.path) {

                // print(n.position.X + ", " + n.position.Y);

                GameObject newWaypoint = Instantiate(waypoint, new Vector3(n.position.X, 0, n.position.Y), Quaternion.identity);
                newWaypoint.transform.parent = parent.transform;
            };

            AStar.Node goalNode = Global.Instance.grid.path.Last();

            GameObject newGoal = Instantiate(goal, new Vector3(goalNode.position.X, 0, goalNode.position.Y), Quaternion.identity);
            newGoal.transform.parent = parent.transform;

            print("Waypoints Instantiated");
        }
    }
}
