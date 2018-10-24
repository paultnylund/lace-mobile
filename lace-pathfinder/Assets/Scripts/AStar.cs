using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Lace {
    public static class AStar {
        public static int xStart = 0;
        public static int yStart = 0;
        public static int xEnd = 3;
        public static int yEnd = 3;
        public static int maxDensity = 9;
        public static JToken _id, distance, graph;
        public static int numRows, numColumns, numNodes;
        public static Node startNode, endNode, currentNode;
        public static List<Coordinate> PathCoords = new List<Coordinate>();
        public static List<Node> OpenList = new List<Node>();
        public static List<Node> ClosedList = new List<Node>();
        public static List<Node> SuccessorNodes = new List<Node>();
        public static List<Double> OpenFCosts = new List<Double>();
        public static bool solutionFlag = false;
        public static void BestPath(JToken data) {
            ParseData(data);
            InitValues();
            RunSearch();
        }
        public static void ParseData(JToken data) {
            _id = data[0]["_id"];
            distance = data[0]["distance"];
            graph = data[0]["graph"];

            JArray graphParsedRows = (JArray)data[0]["graph"];
            numRows = graphParsedRows.Count;
            JArray graphParsedColumns = (JArray)graph[0];
            numColumns = graphParsedColumns.Count;
            numNodes = numRows * numColumns;

            Console.WriteLine();
            Console.WriteLine($"ID: {_id}");
            Console.WriteLine($"DISTANCE: {distance}");
            Console.WriteLine($"GRAPH DIMENSIONS: {numRows}, {numColumns}\n");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"GRAPH:\n");
            for (int i = 0; i < numRows; i++) {
                for (int j = 0; j < numColumns; j++) {
                    Console.Write($"{graph[i][j]} ");
                }
                Console.Write("\n");
            }
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\n~~~~~~~~~~~~~~~~~~~\n");
        }
        public static void InitValues() {

            startNode = new Node() {xCoord = xStart, yCoord = yStart};
            startNode.initCosts();

            currentNode = startNode;
            currentNode.initCosts();

            endNode = new Node() {xCoord = xEnd, yCoord = yEnd};
            endNode.initCosts();
            endNode.adjGCost = currentNode.gCost + endNode.distanceToNode(currentNode);;

            Console.WriteLine("~~~~~~~~~~~~~~~~~~~\n");
            Console.WriteLine($"STARTNODE");
            Console.WriteLine($"Coords: ({startNode.xCoord}, {startNode.yCoord})");
            Console.WriteLine($"gCost: {startNode.gCost}");
            Console.WriteLine($"hCost: {startNode.hCost}");
            Console.WriteLine($"fCost: {startNode.fCost}");
            Console.WriteLine($"adjGCost: {startNode.adjGCost}");
            Console.WriteLine($"density: {startNode.density}");
            Console.WriteLine();
            Console.WriteLine($"CURRENTNODE");
            Console.WriteLine($"Coords: ({currentNode.xCoord}, {currentNode.yCoord})");
            Console.WriteLine($"gCost: {currentNode.gCost}");
            Console.WriteLine($"hCost: {currentNode.hCost}");
            Console.WriteLine($"fCost: {currentNode.fCost}");
            Console.WriteLine($"adjGCost: {currentNode.adjGCost}");
            Console.WriteLine($"density: {currentNode.density}");
            Console.WriteLine();
            Console.WriteLine($"ENDNODE");
            Console.WriteLine($"Coords: ({endNode.xCoord}, {endNode.yCoord})");
            Console.WriteLine($"gCost: {endNode.gCost}");
            Console.WriteLine($"hCost: {endNode.hCost}");
            Console.WriteLine($"fCost: {endNode.fCost}");
            Console.WriteLine($"adjGCost: {endNode.adjGCost}");
            Console.WriteLine($"density: {endNode.density}");
            Console.WriteLine("\n~~~~~~~~~~~~~~~~~~~\n");
        }
        public static void RunSearch() {
            OpenList.Add(startNode);
            while (OpenList.Count >= 0) {
                NodeWithMinF();
                ValidSuccessors();
                CheckNodes();
                CheckSolution();
                if (solutionFlag == true) {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Found Solution! – {PathCoords.Count} vertices");
                    Console.ForegroundColor = ConsoleColor.White;
                    foreach (Coordinate _coord in PathCoords) {
                        // _coord._distFromStart();
                        Console.WriteLine($"{_coord.X}, {_coord.Y}");
                    }
                    Console.WriteLine();
                    break; 
                }
            }
        }
        public static void CheckSolution() {
            foreach (Node _node in OpenList) {
                if (OpenList.Exists(n => n.xCoord == endNode.xCoord && n.yCoord == endNode.yCoord)) {
                    solutionFlag = true;
                    PathCoords.Add(new Coordinate() {X = _node.xCoord, Y = _node.yCoord});
                }
            }
        }
        public static void CheckNodes() {
            foreach (Node successor in SuccessorNodes) {
                if (ClosedList.Exists(_node => _node.xCoord == successor.xCoord && _node.yCoord == successor.yCoord)) {
                    continue;
                }
                if (!OpenList.Exists(_node => _node.xCoord == successor.xCoord && _node.yCoord == successor.yCoord)) {
                    successor.parentNode = currentNode.parentNode;
                    successor.adjGCost = currentNode.gCost + successor.distanceToNode(currentNode);
                    OpenList.Add(successor);
                } else {
                    if (currentNode.gCost + successor.hCost < successor.fCost) {
                        successor.gCost = currentNode.gCost;
                        successor.fCost = successor.gCost + successor.hCost;
                        successor.parentNode = currentNode.parentNode;
                    }
                }
            }
        }
        private static void ValidSuccessors() {
            List<Node> Temp = new List<Node>();
            Temp.Add(new Node() {xCoord = currentNode.xCoord, yCoord = currentNode.yCoord + 1});
            Temp.Add(new Node() {xCoord = currentNode.xCoord + 1, yCoord = currentNode.yCoord + 1});
            Temp.Add(new Node() {xCoord = currentNode.xCoord + 1, yCoord = currentNode.yCoord});
            Temp.Add(new Node() {xCoord = currentNode.xCoord + 1, yCoord = currentNode.yCoord - 1});
            Temp.Add(new Node() {xCoord = currentNode.xCoord, yCoord = currentNode.yCoord - 1});
            Temp.Add(new Node() {xCoord = currentNode.xCoord - 1, yCoord = currentNode.yCoord});
            Temp.Add(new Node() {xCoord = currentNode.xCoord - 1, yCoord = currentNode.yCoord});
            Temp.Add(new Node() {xCoord = currentNode.xCoord - 1, yCoord = currentNode.yCoord + 1});
            foreach (Node _node in Temp) {
                if ((_node.density <= maxDensity && _node.density >= 0) &&
                    (_node.xCoord > 0 && _node.yCoord > 0) &&
                    (_node.xCoord <= numColumns - 1 && _node.yCoord <= numRows - 1)) {
                    _node.initCosts();
                    _node._adjGCost();
                    SuccessorNodes.Add(_node);
                }
            }
        }
        public static void NodeWithMinF() {
            double minFCost = OpenList.Min(_node => _node.fCost);
            currentNode = OpenList.First(_node => _node.fCost == minFCost);
            ClosedList.Add(currentNode);
            OpenList.Remove(currentNode);
        }
        public class Coordinate {
            public int X, Y;
            public double distFromStart;
            // public void _distFromStart() {
            //     distFromStart = Math.Sqrt(Math.Pow((X - startNode.xCoord), 2) + Math.Pow((Y - startNode.yCoord), 2));
            // }
        }
        public class Node {
            public Node parentNode;
            public int xCoord, yCoord, density;
            public double gCost, adjGCost, hCost, fCost;
            public void initCosts() {
                gCost = Math.Sqrt(Math.Pow((xEnd - xCoord), 2) + Math.Pow((yEnd - yCoord), 2));
                hCost = Math.Sqrt(Math.Pow((xStart - xCoord), 2) + Math.Pow((yStart - yCoord), 2));
                density = graph[xCoord][yCoord].ToObject<int>();
                fCost = gCost + hCost + density;
            }
            public double distanceToNode(Node node) {
                return Math.Sqrt(Math.Pow((node.xCoord - xCoord), 2) + Math.Pow((node.yCoord - yCoord), 2));
            }
            public void _adjGCost() {
                adjGCost = currentNode.gCost + distanceToNode(currentNode);
            }
        }
    }
}