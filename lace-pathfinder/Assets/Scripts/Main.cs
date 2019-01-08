using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System;
using UnityEngine;

public class Main : MonoBehaviour {

    API api = new API();

    void Start() {

      Global.Instance.startX = 15;
      Global.Instance.startY = 0;
      Global.Instance.endX = 10;
      Global.Instance.endY = 10;
    }

    void Update() {

        api.Post(this);

        if (Global.Instance.responseObtained) {

                AStar.startAStar();

                if (Global.Instance.pathFound) {

                    Global.Instance.responseObtained = false;
                }
        }

        Global.Instance.startX = (int)Camera.main.gameObject.transform.position.x;
        Global.Instance.startY = (int)Camera.main.gameObject.transform.position.y;
        Global.Instance.timeToDest = Math.Sqrt(Math.Pow((Global.Instance.endX - Global.Instance.startX), 2) + Math.Pow((Global.Instance.endY - Global.Instance.startY), 2)) / Camera.main.velocity.magnitude;

        // Debug.Log(Global.Instance.endX + ", " + Global.Instance.endY + ", " + Camera.main.velocity.magnitude);
    }
}
