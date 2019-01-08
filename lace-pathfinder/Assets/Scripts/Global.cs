/**********************
Import classes
***********************/

using UnityEngine;

/**********************
Public Global Singleton class
***********************/

public class Global : Singleton<Global> {

    protected Global () {}

    public Color deepBlue = new Color(56, 102, 230, 100);
    public Color transparent = new Color(0, 0, 0, 0);

    public bool responseObtained = false; // The http POST request response is not obtained at the start of the program
    public API.Response response; // This is the http POST request response object

    public int startX, startY, endX, endY; // The start and end coordinates
    public AStar.GridObj grid; // The main grid object
    public bool pathFound = false; // The path is undetermined at the start of the program

    public bool wayFinding = false; // The path will not be drawn at the start of the program

    public DestinationSelector.Destination destination;

    public double timeToDest;
}
