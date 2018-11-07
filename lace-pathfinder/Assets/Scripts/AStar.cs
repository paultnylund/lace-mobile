using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

public class AStar : MonoBehaviour {
    
    public static int[][] graph;
    public static Coordinate startPos = new Coordinate() {X = Global.Instance.startX, Y = Global.Instance.startY};
    public static Coordinate targetPos = new Coordinate() {X = Global.Instance.endX, Y = Global.Instance.endY};
    
    public static void Start() {
    
        graph = Global.Instance.response.graph;
        
        Global.Instance.grid = new GridObj();
        Node startNode = Global.Instance.grid.grid[startPos.X,startPos.Y];
        Node targetNode = Global.Instance.grid.grid[targetPos.X,targetPos.Y];
        
        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(startNode);
        
        while (openSet.Count > 0) {
            Node node = openSet[0];
            for (int i = 1; i < openSet.Count; i++) {
                if (openSet[i].fCost < node.fCost || openSet[i].fCost == node.fCost) {
                    if (openSet[i].hCost < node.hCost) {
                        node = openSet[i];
                    }
                }
            }
            openSet.Remove(node);
            closedSet.Add(node);
            if (node == targetNode) {
                RetracePath(startNode,targetNode);
                Global.Instance.pathFound = true;
            }
            foreach (Node neighbor in Global.Instance.grid.GetNeighbors(node)) {
                if (!neighbor.isWalkable || closedSet.Contains(neighbor)) {
                    continue;
                }
                double newCostToNeighbor = node.gCost + GetDistance(node, neighbor);
                if (newCostToNeighbor < neighbor.gCost || !openSet.Contains(neighbor)) {
                    neighbor.gCost = newCostToNeighbor;
                    neighbor.hCost = GetDistance(neighbor, targetNode);
                    neighbor.parentNode = node;
                    if (!openSet.Contains(neighbor)) {
                        openSet.Add(neighbor);
                    }
                }
            }
        }
    }
    static void RetracePath(Node startNode, Node targetNode) {
        List<Node> p = new List<Node>();
        Node currentNode = targetNode;
        while (currentNode != startNode) {
            p.Add(currentNode);
            currentNode = currentNode.parentNode;
        }
        p.Add(startNode);
        p.Reverse();
        Global.Instance.grid = new GridObj() {path = p};
    }
    static double GetDistance(Node nodeA, Node nodeB) {
        return Math.Sqrt(Math.Pow((nodeB.position.X - nodeA.position.X), 2) + Math.Pow((nodeB.position.Y - nodeA.position.Y), 2));
    }
    public class Coordinate {
        public int X, Y;
    }
    public class Node {
        public Node parentNode;
        public Coordinate position;
        public bool isWalkable;
        public double gCost, hCost;
        public int density;
        public double fCost {
            get {
                return gCost + hCost + density;
            }
        }
        public Node (bool _isWalkable, Coordinate _position) {
            isWalkable = _isWalkable;
            position = _position;
            density = (int)graph[position.X][position.Y];
        }
    }
    public class GridObj {
        public Node[,] grid;
        int width, height;
        public static int[][] data {get; set;}
        bool isWalkable;
        public List<Node> path;
        public GridObj() {
            data = graph;
            width = data[0].Count();
            height = data.Count();
            CreateGrid();
        }
        void CreateGrid() {
            grid = new Node[width, height];
            for (int x = 0; x < width; x++) {
                for (int y = 0; y < width; y++) {
                    isWalkable = !((int)data[x][y] == 9);
                    grid[x,y] = new Node(isWalkable, new Coordinate() {X = x, Y = y});
                }
            }
        }
        public List<Node> GetNeighbors(Node node) {
            List<Node> neighbors = new List<Node>();
            for (int x = -1; x <=1; x++) {
                for (int y = -1; y <=1; y++) {
                    if (x == 0 && y == 0) {
                        continue;
                    }
                    int checkX = node.position.X + x;
                    int checkY = node.position.Y + y;
                    if (checkX >= 0 && checkX < width && checkY >= 0 && checkY < height) {
                        neighbors.Add(grid[checkX,checkY]);
                    }
                }
            }
            return neighbors;
        }
    }
}