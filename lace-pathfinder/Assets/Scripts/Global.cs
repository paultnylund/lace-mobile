using UnityEngine;

public class Global : Singleton<Global> {
    
    protected Global () {}
    
    public bool responseObtained = false;
    public API.Response response;
    public AStar.GridObj grid;
    public bool pathFound = false;
}