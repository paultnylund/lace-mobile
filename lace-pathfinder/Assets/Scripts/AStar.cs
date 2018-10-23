using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Lace {
    public static class AStar {
        
        public static int xStart = 0;
        public static int yStart = 0;
        public static int xEnd = 4;
        public static int yEnd = 4;
        public static JToken _id, distance, graph;
        public static int numRows, numColumns, numNodes;
        public static Node startNode, endNode, currentNode, tempNode;
        public static List<Node> OpenList = new List<Node>();
        public static List<Node> ClosedList = new List<Node>();
        public static List<Node> SuccessorNodes = new List<Node>();
        public static List<Double> OpenFCosts = new List<Double>();
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
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"ID: {_id}\n");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"DISTANCE: {distance}\n");
            Console.ForegroundColor = ConsoleColor.Magenta;
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
            Console.WriteLine();
        }
        public static void InitValues() {

            startNode = new Node() {xCoord = xStart, yCoord = yStart};
            startNode.initCosts();

            currentNode = startNode;
            currentNode.initCosts();

            endNode = new Node() {xCoord = xEnd, yCoord = yEnd};
            endNode.initCosts();
            endNode._adjGCost();

            Console.WriteLine("~~~~~~~~~~~~~~~~~~~\n");
            Console.WriteLine($"STARTNODE");
            Console.WriteLine($"gCost: {startNode.gCost}");
            Console.WriteLine($"hCost: {startNode.hCost}");
            Console.WriteLine($"fCost: {startNode.fCost}");
            Console.WriteLine($"adjGCost: {startNode.adjGCost}");
            Console.WriteLine();
            Console.WriteLine($"CURRENTNODE");
            Console.WriteLine($"gCost: {currentNode.gCost}");
            Console.WriteLine($"hCost: {currentNode.hCost}");
            Console.WriteLine($"fCost: {currentNode.fCost}");
            Console.WriteLine($"adjGCost: {currentNode.adjGCost}");
            Console.WriteLine();
            Console.WriteLine($"ENDNODE");
            Console.WriteLine($"gCost: {endNode.gCost}");
            Console.WriteLine($"hCost: {endNode.hCost}");
            Console.WriteLine($"fCost: {endNode.fCost}");
            Console.WriteLine($"adjGCost: {endNode.adjGCost}");
            Console.WriteLine("\n~~~~~~~~~~~~~~~~~~~\n");
        }
        public static void RunSearch() {

            OpenList.Add(startNode);

            while (OpenList.Count >= 0) {
                NodeWithMinF();
                OpenList.Find(node => {
                    if (node.xCoord == endNode.xCoord && node.yCoord == endNode.yCoord) {
                        Console.WriteLine("Found Solution!");
                        return true;
                    } else {return false;}
                });
                Successors();
                CheckNodes();
                // OpenList.ForEach(i => Console.WriteLine($"X: {i.xCoord}, Y: {i.yCoord}"));
                // Console.WriteLine(OpenList.Count);
            }
            if (currentNode != endNode) {
                Console.WriteLine("Error!");
                Environment.Exit(-1);
            }
        }
        public static void CheckNodes() {

            for (int i = 0; i <= SuccessorNodes.Count - 1; i++) {

                // if (currentNode.parentNode != null && i == 0) {
                //     Console.WriteLine($"X: {currentNode.parentNode.xCoord}, Y: {currentNode.parentNode.yCoord}");
                // }

                SuccessorNodes.ElementAt(i).initCosts();
                SuccessorNodes.ElementAt(i)._adjGCost();
                Node successor = SuccessorNodes.ElementAt(i);

                // if (successor.xCoord < 0 || successor.yCoord < 0 || ClosedList.Exists(node => node == successor)) {
                    
                //     ClosedList.Add(successor);
                //     continue;

                // } else if (!OpenList.Exists(node => node == successor) && !ClosedList.Exists(node => node == successor)) {

                //     successor.hCost = successor.distanceToNode(endNode);
                //     successor.gCost = successor.adjGCost;
                //     successor.parentNode = currentNode;
                //     OpenList.Add(successor);

                // } else if (OpenList.Exists(node => node == successor) || successor.gCost <= successor.adjGCost) {
                    
                //     continue;
                // }

                if ((successor.xCoord >= 0 || successor.yCoord >= 0) && !(successor.xCoord <= 0 || successor.yCoord <= 0)) {

                    // Console.WriteLine($"X: {successor.xCoord}, Y: {successor.yCoord}");
                    
                    if (OpenList.Exists(node => node == successor)) {

                        if (successor.gCost <= successor.adjGCost) {
                            // Console.WriteLine($"X: {successor.xCoord}, Y: {successor.yCoord}");
                            continue;
                        }

                    } else if (ClosedList.Exists(node => node == successor)) {

                        // Console.WriteLine($"X: {successor.xCoord}, Y: {successor.yCoord}");

                        if (successor.gCost <= successor.adjGCost) {
                            continue;
                        }

                        ClosedList.Remove(successor);
                        OpenList.Add(successor);

                    } else {

                        // Console.WriteLine($"X: {successor.xCoord}, Y: {successor.yCoord}");

                        OpenList.Add(successor);
                        SuccessorNodes.ElementAt(i).hCost = successor.distanceToNode(endNode);
                    }
                }

                SuccessorNodes.ElementAt(i).gCost = successor.adjGCost;
                SuccessorNodes.ElementAt(i).parentNode = currentNode;
            }
            ClosedList.Add(currentNode);
        }
        private static void Successors() {

            int currentX = OpenList[OpenList.Count - 1].xCoord;
            int currentY = OpenList[OpenList.Count - 1].yCoord;

            SuccessorNodes.Add(new Node() {xCoord = currentX, yCoord = currentY + 1});
            SuccessorNodes.Add(new Node() {xCoord = currentX + 1, yCoord = currentY + 1});
            SuccessorNodes.Add(new Node() {xCoord = currentX + 1, yCoord = currentY});
            SuccessorNodes.Add(new Node() {xCoord = currentX + 1, yCoord = currentY - 1});
            SuccessorNodes.Add(new Node() {xCoord = currentX, yCoord = currentY - 1});
            SuccessorNodes.Add(new Node() {xCoord = currentX - 1, yCoord = currentY});
            SuccessorNodes.Add(new Node() {xCoord = currentX - 1, yCoord = currentY});
            SuccessorNodes.Add(new Node() {xCoord = currentX - 1, yCoord = currentY + 1});
        }
        public static void NodeWithMinF() {

            foreach (Node node in OpenList) {
                OpenFCosts.Add(node.fCost);
            }

            double minFCost = OpenFCosts.Min();

            foreach (Node node in OpenList) {
                if (node.fCost == minFCost) {
                    currentNode = node;
                }
            }
        }
        public class Node {
            public Node parentNode;
            public int xCoord, yCoord, density;
            public double gCost, adjGCost, hCost, fCost;
            public void initCosts() {
                gCost = Math.Sqrt(Math.Pow((xEnd - xCoord), 2) + Math.Pow((yEnd - yCoord), 2));
                hCost = Math.Sqrt(Math.Pow((xStart - xCoord), 2) + Math.Pow((yStart - yCoord), 2));
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