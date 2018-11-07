using UnityEngine;

public class Global : Singleton<Global> {
    
    protected Global () {}
    
    public bool responseObtained = false;
    public API.Response response;
    
    public int startX, startY, endX, endY;
    public AStar.GridObj grid;
    public bool pathFound = false;
    
    public bool wayFinding = false;
}