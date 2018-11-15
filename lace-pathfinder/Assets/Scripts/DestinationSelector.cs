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

		foreach (var item in destinationList.ActiveToggles()) {
			Text itemText = item.GetComponentInChildren(typeof(Text)) as Text;
			foreach (Destination dest in destinations) {
				if (dest.name == itemText.text) {
					Global.Instance.endX = dest.x;
					Global.Instance.endY = dest.y;
					print(Global.Instance.endX + ", " + Global.Instance.endY);
				}
			}
			// item.GetComponentInChildren<Text>().color = new Color(255, 255, 255, 255);
			break;
		}

		if (destinationList.AnyTogglesOn()) {
		   startButton.interactable = true;
		} else {
		   startButton.interactable = false;
		}
	}

	void Awake () {

		destinations.Add(new Destination("Cafe", 3, 3));
		destinations.Add(new Destination("Lounge", 2, 3));
		destinations.Add(new Destination("Library", 2, 3));
		destinations.Add(new Destination("Bar", 0, 2));
		destinations.Add(new Destination("Garden", 3, 0));
		destinations.Add(new Destination("Wardrobe", 2, 1));
		destinations.Add(new Destination("Gallery", 3, 2));
		destinations.Add(new Destination("Terminal", 3, 3));

		int numDestinations = destinations.Count;

		int yOffset = -120;

		foreach (Destination n in destinations) {

			Toggle newDestinationCard = Instantiate(destinationCard, Vector2.up * yOffset, Quaternion.identity);
			// newDestinationCard.onValueChanged.AddListener(destCardToggleChanged);
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
      Debug.Log("You have clicked the button!");
			thisCanvas.enabled = false;
  }

	// public void destCardToggleChanged(bool isOn) {
  //  if (isOn) {
	//
  //      Debug.Log ("Leaderboard On");
	//
  //  } else {
	//
  //      Debug.Log ("Leaderboard Off");
  //  }
	// }

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
