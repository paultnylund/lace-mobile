using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DestinationSelector : MonoBehaviour {

	public Canvas thisCanvas;
	public Button startButton;
	public ToggleGroup destinationList;
	public Toggle destinationCard;
	List<Destination> destinations = new List<Destination>();

	void Update () {

		if (destinationList.AnyTogglesOn()) {
		   startButton.interactable = true;
		} else {
		   startButton.interactable = false;
		}

		foreach (Toggle toggle in destinationList.ActiveToggles()) {
				 foreach (Destination n in destinations) {
					 if (n.name == toggle.name) {
						 Global.Instance.endX = n.x;
	 					 Global.Instance.endY = n.y;
						 Debug.Log(Global.Instance.endX + ", " + Global.Instance.endY);
					 }
				 }
    }
	}

	void Awake () {

		destinations.Add(new Destination("Swing Set", 3, 7));
		destinations.Add(new Destination("Apartment", 12, 19));
		destinations.Add(new Destination("Bike Rack", 16, 11));
		destinations.Add(new Destination("Entryway", 15, 0));
		destinations.Add(new Destination("Garden", 0, 13));

		int numDestinations = destinations.Count;

		int yOffset = -120;

		foreach (Destination n in destinations) {

			Toggle newDestinationCard = Instantiate(destinationCard, Vector2.up * yOffset, Quaternion.identity);
			newDestinationCard.name = n.name;
			newDestinationCard.group = destinationList;

			Text newDestinationName = newDestinationCard.GetComponentInChildren(typeof(Text)) as Text;
			newDestinationName.text = n.name;

			newDestinationCard.transform.SetParent(destinationList.transform, false);

			yOffset -= 240;
		};

		RectTransform destinationListRectTrans = destinationList.GetComponent(typeof(RectTransform)) as RectTransform;
		destinationListRectTrans.sizeDelta = new Vector2(destinationListRectTrans.rect.width, Math.Abs(yOffset) + 210);
		destinationListRectTrans.localPosition = new Vector2(0, -(numDestinations * 240 + 330));

		startButton.onClick.AddListener(StartNavigation);
	}

	public void StartNavigation() {
      Debug.Log("You have tapped the button!");
			thisCanvas.enabled = false;
			Global.Instance.wayFinding = true;
  }

	public class Destination {

		public string name;
		public int x, y;
		public double distanceFromStart;

		public Destination(string destName, int xCoord, int yCoord) {

			name = destName;
			x = xCoord;
			y = yCoord;
			distanceFromStart = Math.Sqrt(Math.Pow((xCoord - Global.Instance.startX), 2) + Math.Pow((yCoord - Global.Instance.startY), 2));
		}
	}
}
