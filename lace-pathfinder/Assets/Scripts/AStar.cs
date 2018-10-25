using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using static Lace.API;

namespace Lace {
    public class AStar {
        public static JToken graph;
        public static Grid grid;
        static Coordinate startPos = new Coordinate() {X = 2, Y = 3};
        static Coordinate targetPos = new Coordinate() {X = 3, Y = 0};
        public static void FindPath(JToken _data) {
            graph = _data[0]["graph"];
            grid = new Grid();
            Node startNode = grid.grid[startPos.X,startPos.Y];
            Node targetNode = grid.grid[targetPos.X,targetPos.Y];
            Console.WriteLine(grid.grid[targetPos.X,targetPos.Y]);
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
                    Console.WriteLine("FOUND PATH!");
                    RetracePath(startNode,targetNode);
                    return;
                }
                foreach (Node neighbor in grid.GetNeighbors(node)) {
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
            List<Node> path = new List<Node>();
            Node currentNode = targetNode;
            while (currentNode != startNode) {
                path.Add(currentNode);
                currentNode = currentNode.parentNode;
            }
            path.Add(startNode);
            path.Reverse();
            grid.path = path;
            foreach (Node n in grid.path) {
                Console.WriteLine($"{n.position.X}, {n.position.Y}");
            }
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
        public class Grid {
            public Node[,] grid;
            int width, height;
            static JToken data {get; set;}
            bool isWalkable;
            public List<Node> path;
            public Grid() {
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
}