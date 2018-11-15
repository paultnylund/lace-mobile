using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DestinationSelector : MonoBehaviour {

	public Button startButton;
	public RectTransform destinationList;
	public GameObject destinationCard;
	List<Destination> destinations = new List<Destination>();

	void Awake () {

		destinations.Add(new Destination("cafe", 3, 3));
		destinations.Add(new Destination("lounge", 2, 3));
		destinations.Add(new Destination("library", 1, 3));
		destinations.Add(new Destination("bar", 0, 3));
		destinations.Add(new Destination("garden", 3, 0));
		destinations.Add(new Destination("wardrobe", 3, 1));
		destinations.Add(new Destination("gallery", 3, 2));
		destinations.Add(new Destination("terminal", 3, 3));

		int yOffset = -120;

		foreach (Destination n in destinations) {

			GameObject newDestinationCard = Instantiate(destinationCard, Vector2.up * yOffset, Quaternion.identity);
			newDestinationCard.transform.SetParent(destinationList.transform, false);

			print(n.name);

			yOffset -= 240;
		};

		RectTransform destinationListRectTrans = destinationList.GetComponent(typeof(RectTransform)) as RectTransform;
		destinationListRectTrans.sizeDelta = new Vector2(destinationListRectTrans.rect.width, Math.Abs(yOffset) + 220);

		startButton.onClick.AddListener(StartNavigation);
	}

	public void StartNavigation() {
      Debug.Log("You have clicked the button!");
  }

	public class Destination {
		public string name;
		// public AStar.Node thisNode;
		public int x, y;
		public double distanceFromStart;

		public Destination(string destName, int xCoord, int yCoord) {
			name = destName;
			x = xCoord;
			y = yCoord;
			// thisNode = new AStar.Node(true, new AStar.Coordinate() { X = xCoord, Y = yCoord });
			distanceFromStart = Math.Sqrt(Math.Pow((xCoord - Global.Instance.startX), 2) + Math.Pow((yCoord - Global.Instance.startY), 2));
		}
	}
}
