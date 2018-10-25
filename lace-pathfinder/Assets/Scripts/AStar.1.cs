using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Lace {
    public static class AStar_1 {
        public static Coordinate coordStart = new Coordinate() {X = 0, Y = 0};
        public static Coordinate coordEnd = new Coordinate() {X = 3, Y = 3};
        public static int maxDensity = 9;
        public static JToken _id, distance, graph;
        public static int numRows, numColumns, numNodes;
        public static Node startNode, endNode, currentNode;
        public static List<Coordinate> Path = new List<Coordinate>();
        public static List<Node> OpenList = new List<Node>();
        public static List<Node> ClosedList = new List<Node>();
        public static List<Node> SuccessorNodes = new List<Node>();
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

            JArray graphParsedRows = (JArray)graph[0];
            numRows = graphParsedRows.Count;
            JArray graphParsedColumns = (JArray)data[0]["graph"];
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

            startNode = new Node() {xCoord = coordStart.X, yCoord = coordStart.Y, parentNode = null};
            startNode.initCosts();

            currentNode = startNode;
            currentNode.initCosts();

            endNode = new Node() {xCoord = coordEnd.X, yCoord = coordEnd.Y};
            endNode.initCosts();
            endNode._adjGCost();

            Console.WriteLine("~~~~~~~~~~~~~~~~~~~\n");
            Console.WriteLine($"STARTNODE");
            Console.WriteLine($"Coords: ({startNode.xCoord}, {startNode.yCoord})");
            // Console.WriteLine($"gCost: {startNode.gCost}");
            // Console.WriteLine($"hCost: {startNode.hCost}");
            // Console.WriteLine($"fCost: {startNode.fCost}");
            // Console.WriteLine($"adjGCost: {startNode.adjGCost}");
            Console.WriteLine($"density: {startNode.density}");
            // Console.WriteLine();
            // Console.WriteLine($"CURRENTNODE");
            // Console.WriteLine($"Coords: ({currentNode.xCoord}, {currentNode.yCoord})");
            // Console.WriteLine($"gCost: {currentNode.gCost}");
            // Console.WriteLine($"hCost: {currentNode.hCost}");
            // Console.WriteLine($"fCost: {currentNode.fCost}");
            // Console.WriteLine($"adjGCost: {currentNode.adjGCost}");
            // Console.WriteLine($"density: {currentNode.density}");
            // Console.WriteLine();
            Console.WriteLine($"ENDNODE");
            Console.WriteLine($"Coords: ({endNode.xCoord}, {endNode.yCoord})");
            // Console.WriteLine($"gCost: {endNode.gCost}");
            // Console.WriteLine($"hCost: {endNode.hCost}");
            // Console.WriteLine($"fCost: {endNode.fCost}");
            // Console.WriteLine($"adjGCost: {endNode.adjGCost}");
            Console.WriteLine($"density: {endNode.density}");
            Console.WriteLine("\n~~~~~~~~~~~~~~~~~~~\n");
        }
        public static void RunSearch() {
            OpenList.Add(startNode);
            while (OpenList.Count >= 0) {
                NodeWithMinF();
                ValidSuccessors();
                CheckNodes();
                if ((ClosedList.Exists(n => n.xCoord == endNode.xCoord && n.yCoord == endNode.yCoord) && !OpenList.Exists(n => n.xCoord == endNode.xCoord && n.yCoord == endNode.yCoord))) {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Found Solution!\n");
                    Console.ForegroundColor = ConsoleColor.White;
                    // while (currentNode != null) {
                    //     Path.Add(new Coordinate() {
                    //         X = currentNode.xCoord,
                    //         Y = currentNode.yCoord
                    //     });
                    //     currentNode = currentNode.parentNode;
                    // }
                    // Path.Add(new Coordinate() {
                    //     X = currentNode.xCoord,
                    //     Y = currentNode.yCoord
                    // });
                    // currentNode = currentNode.parentNode;
                    foreach (Coordinate _coord in Path) {
                        Console.WriteLine($"{_coord.X}, {_coord.Y}");
                    }
                    Console.WriteLine();
                    break;
                }
            }
        }
        public static void CheckNodes() {
            for (int i = 0; i < SuccessorNodes.Count; i++) {
                if (ClosedList.FirstOrDefault(_node => _node.xCoord == SuccessorNodes[i].xCoord && _node.yCoord == SuccessorNodes[i].yCoord) != null) {
                    continue;
                }
                if (OpenList.FirstOrDefault(_node => _node.xCoord == SuccessorNodes[i].xCoord && _node.yCoord == SuccessorNodes[i].yCoord) == null) {
                    SuccessorNodes[i].parentNode = new Node () {
                        xCoord = currentNode.xCoord,
                        yCoord = currentNode.yCoord
                    };
                    SuccessorNodes[i] = new Node () {
                        gCost = currentNode.gCost,
                        hCost = currentNode.hCost,
                        fCost = SuccessorNodes[i].gCost + SuccessorNodes[i].hCost,
                    };
                    OpenList.Insert(0, SuccessorNodes[i]);

                } else {
                    if (currentNode.gCost + SuccessorNodes[i].hCost < SuccessorNodes[i].fCost) {
                        SuccessorNodes[i].parentNode = new Node () {
                            xCoord = currentNode.xCoord,
                            yCoord = currentNode.yCoord
                        };
                        SuccessorNodes[i] = new Node () {
                            gCost = currentNode.gCost,
                            fCost = SuccessorNodes[i].gCost + SuccessorNodes[i].hCost,
                        };
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
            public int xCoord, yCoord;
            public double gCost, adjGCost, hCost, fCost, density;
            public void initCosts() {
                gCost = Math.Sqrt(Math.Pow((coordEnd.X - xCoord), 2) + Math.Pow((coordEnd.Y - yCoord), 2));
                hCost = Math.Sqrt(Math.Pow((coordStart.X - xCoord), 2) + Math.Pow((coordStart.Y - yCoord), 2));
                density = graph[xCoord][yCoord].ToObject<double>();
                fCost = gCost + hCost * density;
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