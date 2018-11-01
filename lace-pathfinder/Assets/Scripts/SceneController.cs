using UnityEngine;
using GoogleARCore;

public class SceneController : MonoBehaviour {
    
    public Camera firstPersonCamera;
    public PlacedObject cube; 

	void Start () {
        QuitOnConnectionErrors();
    }
	void QuitOnConnectionErrors() {
        if (Session.Status ==  SessionStatus.ErrorPermissionNotGranted) {
            StartCoroutine(CodelabUtils.ToastAndExit(
                "Camera permission is needed to run this application.", 5));
        } else if (Session.Status.IsError()) {
            StartCoroutine(CodelabUtils.ToastAndExit(
                "ARCore encountered a problem connecting. Please restart the app.", 5));
        }
    }
    void Update() {
        if (Session.Status != SessionStatus.Tracking) {
            int lostTrackingSleepTimeout = 15;
            Screen.sleepTimeout = lostTrackingSleepTimeout;
            return;
        }
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        ProcessTouches();
    }
    
    void ProcessTouches () {
        Touch touch;
        if (Input.touchCount != 1 || (touch = Input.GetTouch (0)).phase != TouchPhase.Began) {
            return;
        } 
        TrackableHit hit;
        TrackableHitFlags raycastFilter = TrackableHitFlags.PlaneWithinBounds | TrackableHitFlags.PlaneWithinPolygon;
        if (Frame.Raycast (touch.position.x, touch.position.y, raycastFilter, out hit)) {
            SetSelectedPlane (hit.Trackable as DetectedPlane);
        }
    }
    void SetSelectedPlane (DetectedPlane selectedPlane) {
        Debug.Log ("Selected plane centered at " + selectedPlane.CenterPose.position);
        cube.SetSelectedPlane(selectedPlane);
    }
}
