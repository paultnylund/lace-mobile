using System;
using System.Collections;
using System.Linq;
// using Newtonsoft.Json.Linq;

namespace Path {
    public class AStar {
        private ArrayList openList = new ArrayList();
        private ArrayList closedList = new ArrayList();
        public static Node startNode = new Node(0, 0, 0);
        public static Node endNode = new Node(10, 10, 0);
        private static Node currentNode = startNode;
        private ArrayList successorNodes = new ArrayList();
        public void BestPath() {

            openList.Add(startNode);

            while (openList.Count > 0) {
                NodeMinFCost();
                if (currentNode == endNode) {
                    break;
                }
                Successors();
            }
            if (currentNode != endNode) {
                Environment.Exit(-1);
            }
        }
        private void Successors() {

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

            foreach (Node successor in successorNodes) {
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
            closedList.Add(currentNode);
        }

        private void NodeMinFCost() {
            double[] openFCosts = new double[openList.Count];
            int index = 0;
            foreach (Node node in openList) {
                index++;
                openFCosts[index] = node.fCost;
            }
            double minFCost = openFCosts.Min();
            index = 0;
            foreach (Node node in openList) {
                index++;
                if (node.fCost == minFCost) {
                    currentNode = node;
                }
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
                gCost = Math.Sqrt(Math.Pow((endNode.xCoord - xCoord), 2) + Math.Pow((endNode.yCoord - yCoord), 2));
                adjGCost = currentNode.gCost + distanceToNode(currentNode);
                hCost = Math.Sqrt(Math.Pow((startNode.xCoord - xCoord), 2) + Math.Pow((startNode.yCoord - yCoord), 2));
                fCost = gCost + hCost + density;
            }
        }
    }
}