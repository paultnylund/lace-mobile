using System;
using System.Collections;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Lace {
    public static class AStar {
        public static JToken _id, distance, graph;
        public static int numRows, numColumns;
        public static ArrayList openList, closedList, successorNodes;
        public static Node startNode, endNode, currentNode;
        public static string whichNode;
        public static void BestPath(JToken data) {

            ParseData(data);

            // InitValues();
            // RunSearch();
        }
        public static void ParseData(JToken data) {
            _id = data[0]["_id"];
            distance = data[0]["distance"];
            graph = data[0]["graph"];

            JArray graphParsedRows = (JArray)data[0]["graph"];
            numRows = graphParsedRows.Count;
            JArray graphParsedColumns = (JArray)graph[0];
            numColumns = graphParsedColumns.Count;

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
        public static void RunSearch() {
            openList.Add(startNode);

            while (openList.Count > 0) {
                NodeWithMinF();
                if (currentNode == endNode) {
                    Console.WriteLine("Found Solution!");
                    break;
                }
                Successors();
                CheckNodes();
            }
            if (currentNode != endNode) {
                Console.WriteLine("Error!");
                Environment.Exit(-1);
            }
        }
        public static void InitValues() {
            whichNode = "start";
            startNode = new Node(0, 0, 0);
            currentNode = startNode;
            whichNode = "end";
            endNode = new Node(1, 1, 0);
            whichNode = "";
            openList = new ArrayList();
            closedList = new ArrayList();
            successorNodes = new ArrayList();
        }
        public static void CheckNodes() {
            foreach (Node successor in successorNodes) {

                if (successor.xCoord > numColumns || successor.yCoord > numRows) {
                    successorNodes.Remove(successor);
                    continue;

                } else {
                    Console.WriteLine($"{successor.xCoord}, {successor.yCoord}");
                    successor.adjGCost = currentNode.gCost + successor.distanceToNode(currentNode);

                    if (openList.Cast<Node>().Any( i => i == successor)) {
                        if (successor.gCost <= successor.adjGCost) {
                            continue;
                        }
                    } else if (closedList.Cast<Node>().Any( i => i == successor)) {
                            if (successor.gCost <= successor.adjGCost) {
                                continue;
                            }
                            closedList.Remove(successor);
                            openList.Add(successor);

                    } else {
                        openList.Add(successor);
                        successor.hCost = successor.distanceToNode(endNode);
                    }

                    successor.gCost = successor.adjGCost;
                    successor.parentNode = currentNode;
                }
            }
            closedList.Add(currentNode);
        }
        private static void Successors() {

            int currentX = currentNode.xCoord;
            int currentY = currentNode.yCoord;

            Node n = new Node((currentX), (currentY + 1), 0);
            Node ne = new Node((currentX + 1), (currentY + 1), 0);
            Node e = new Node((currentX + 1), (currentY), 0);
            Node se = new Node((currentX + 1), (currentY - 1), 0);
            Node s = new Node((currentX), (currentY - 1), 0);
            Node sw = new Node((currentX - 1), (currentY - 1), 0);
            Node w = new Node((currentX - 1), (currentY), 0);
            Node nw = new Node((currentX - 1), (currentY + 1), 0);

            successorNodes.Add(n);
            successorNodes.Add(ne);
            successorNodes.Add(e);
            successorNodes.Add(se);
            successorNodes.Add(s);
            successorNodes.Add(sw);
            successorNodes.Add(w);
            successorNodes.Add(nw);
        }
        public static void NodeWithMinF() {

            // int index = 0;
            // foreach (Node node in openList) {
            //     var currentNode = openList[index];
            //     var f = currentNode.fCost;
            //     index++;
            // }
            double[] openFCosts = new double[openList.Count];
            int index = 0;
            foreach (Node node in openList) {
                openFCosts[index] = node.fCost;
                index++;
            }
            double minFCost = openFCosts.Min();
            index = 0;
            foreach (Node node in openList) {
                if (node.fCost == minFCost) {
                    currentNode = node;
                }
                index++;
            }
        }
        public class Node {
            public Node parentNode;
            public int xCoord, yCoord, density;
            public double gCost, adjGCost, hCost, fCost;
            public double distanceToNode(Node node) {
                return Math.Sqrt(Math.Pow((node.xCoord - xCoord), 2) + Math.Pow((node.yCoord - yCoord), 2));
            }
            public Node(int x, int y, int d) {
                xCoord = x;
                yCoord = y;
                density = d;
                if (whichNode == "start") {
                    gCost = 0;
                } else {
                    if (whichNode == "end") {
                        hCost = 0;
                    } else {
                        gCost = Math.Sqrt(Math.Pow((endNode.xCoord - xCoord), 2) + Math.Pow((endNode.yCoord - yCoord), 2));
                        hCost = Math.Sqrt(Math.Pow((startNode.xCoord - xCoord), 2) + Math.Pow((startNode.yCoord - yCoord), 2));
                    }
                    adjGCost = currentNode.gCost + distanceToNode(currentNode);
                }
                fCost = gCost + hCost + density;
            }
        }
    }
}