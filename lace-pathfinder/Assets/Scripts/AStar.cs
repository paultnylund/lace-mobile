/**********************
Import classes
***********************/

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

/**********************
Public Unity MonoBehaviour class
***********************/

public class AStar : MonoBehaviour {

    /**********************
    Define local variables
    ***********************/

    public static int[][] graph; // Creates a 2-dimensional int array. This will represent the weights of all the nodes in the room
    public static Coordinate startPos; // Creates and initiates an object of type Coordinate. This represents the route's start position
    public static Coordinate targetPos; // Creates and initiates an object of type Coordinate. This represents the route's end, or target, position

    /**********************
    Start method will run upon being called from Main.cs
    ***********************/

    public static void startAStar() {

        /**********************
        Define method variables
        ***********************/

        startPos = new Coordinate() {X = Global.Instance.startX, Y = Global.Instance.startY};
        targetPos = new Coordinate() {X = Global.Instance.endX, Y = Global.Instance.endY};

        graph = Global.Instance.response.graph; // Sets the graph array equal to the graph array in the global response object

        Global.Instance.grid = new GridObj(); // Initiates the global grid object
        Node startNode = Global.Instance.grid.grid[startPos.X,startPos.Y]; // Sets the start coordinates of the start node equal to those in startPos
        Node targetNode = Global.Instance.grid.grid[targetPos.X,targetPos.Y]; // Sets the start coordinates of the start node equal to those in startPos

        List<Node> openSet = new List<Node>(); // Creates and initiates a List of nodes as openSet. This will include all of the nodes that the algorithm has yet to evaluate.
        HashSet<Node> closedSet = new HashSet<Node>(); // Creates and initiates a HashSet of nodes as closedSet. This will include all of the nodes that the algorithm has successfully evaluated.

        openSet.Add(startNode); // Adds the start node to the open set

        /**********************
        Loop while there are still nodes in the open set (to be evaluated)
        **********************/

        while (openSet.Count > 0) {

            Node node = openSet[0]; // Sets the current node equal to the first node in the open set

            /**********************
            Iterate through the open set
            **********************/

            for (int i = 1; i < openSet.Count; i++) {

                if (openSet[i].fCost <= node.fCost) { // Checks if the f-cost of node i in the open set is less than or equal to the f-fost of the current node

                    if (openSet[i].hCost < node.hCost) { // Checks if the h-cost of node i in the open set is less than the h-fost of the current node

                        node = openSet[i]; // Sets the current node equal to the node i in the open set
                    }
                }
            }

            openSet.Remove(node); // Removes the current node from the open set
            closedSet.Add(node); // Adds the current node to the closed set

            if (node == targetNode) { // Checks if the current node is equal to the target node

                RetracePath(startNode,targetNode); // Runs the RetracePath method with arguments startNode and targetNode
                Global.Instance.pathFound = true; // Sets the global boolean value, pathFound, to true
            }

            /**********************
            Iterate through each neighboring node of current node
            **********************/

            foreach (Node neighbor in Global.Instance.grid.GetNeighbors(node)) {

                if (!neighbor.isWalkable || closedSet.Contains(neighbor)) { // Checks if the neighboring node is either not walkable or has been added to the closed set

                    continue; // Continues to next iteration of loop
                }

                double newCostToNeighbor = node.gCost + GetDistance(node, neighbor); // Creates an updated cost and sets it equal to the g-cost of the current node plus the distance from the current node to that neighboring node

                if (newCostToNeighbor < neighbor.gCost || !openSet.Contains(neighbor)) { // Checks if the aforementioned cost is less than the g-cost of the current neighboring node or if the open set does not contain the current neighboring node

                    neighbor.gCost = newCostToNeighbor; // Sets the g-cost of the current neighboring node equal to the updated cost
                    neighbor.hCost = GetDistance(neighbor, targetNode); // Sets the h-cost of the current neighboring node equal to its distance from the target node
                    neighbor.parentNode = node; // Sets the parent node of the current neighboring node equal to the current node

                    if (!openSet.Contains(neighbor)) { // Checks if the the open set does not include the current neighboring node

                        openSet.Add(neighbor); // Adds the current neighboring node to the open set
                    }
                }
            }
        }
    }

    /**********************
    RetracePath method retraces the path backwards from the target node via its parent nodes, their respective parent nodes, and so forth
    ***********************/

    static void RetracePath(Node startNode, Node targetNode) {

        List<Node> p = new List<Node>(); // Creates and instantiates a new list of nodes. This will represent the final optimized path from the start node to the end node
        Node currentNode = targetNode; // Sets the current node equal to the target node

        while (currentNode != startNode) { // Loops while the current node is not equal to the start node

            p.Add(currentNode); // Adds the current node to the optimized path
            currentNode = currentNode.parentNode; // Sets the current node equal to its parent node
        }

        p.Add(startNode); // Adds the start node the optimized path
        p.Reverse(); // Reverses the order of the optimized path, so that the first node is the start node
        Global.Instance.grid = new GridObj() {path = p}; // Sets the path object of the global grid object equal to the optimized path
    }

    /**********************
    GetDistance method utilizes the pythagorean theorem to calculate the distance between two node objects
    ***********************/

    public static double GetDistance(Node nodeA, Node nodeB) {

        return Math.Sqrt(Math.Pow((nodeB.position.X - nodeA.position.X), 2) + Math.Pow((nodeB.position.Y - nodeA.position.Y), 2)); // Returns the calculated distance
    }

    /**********************
    This is the Coordinate class
    ***********************/

    public class Coordinate {

        public int X, Y; // It consists of two int objects, X and Y
    }

    /**********************
    This is the Node class
    ***********************/

    public class Node {

        /**********************
        Define class variables
        ***********************/

        public Node parentNode; // The node's parent
        public Coordinate position; // The node's position in a GridObj
        public bool isWalkable; // If the node is traversable by foot
        public double gCost, hCost; // The node's primary weights. g-cost represents its distance to the start node, while f-cost represents its distance to the end node
        public int density; // The crowd-density weight of the node

        /**********************
        Return the node's f-cost
        ***********************/

        public double fCost {

            get { // Returns a property

                return gCost + hCost + density; // The f-cost is equal to sum of the g-cost, h-cost, and the crowd-density weight
            }
        }

        /**********************
        Optional method to be run upon instantiation
        ***********************/

        public Node (bool _isWalkable, Coordinate _position) {

            isWalkable = _isWalkable; // Sets the walkability of the node equal to the associated argument
            position = _position; // Sets the position of the node equal to the associated argument
            density = (int)graph[position.X][position.Y]; // Sets the crowd-density weight of the node equal to that of the graph array at the same position as the node
        }
    }

    /**********************
    RetracePath method retraces the path backwards from the target node via its parent nodes, their respective parent nodes, and so forth
    ***********************/

    public class GridObj { // This is the GridObj class

        /**********************
        Define class variables
        ***********************/

        public Node[,] grid; // 2-dimensional grid of objects of type Node
        public static int[][] data {get; set;} // A 2-dimensional array of type int
        public List<Node> path; // List of Nodes, to be populated with final optimized path
        int width, height; // The maximum number of nodes in the x and y axes
        bool isWalkable; // Boolean representing walkability. Defaults to true.

        /**********************
        GridObj method runs upon instantiation of the GridObj type class
        ***********************/

        public GridObj() {

            data = graph; // Sets the data array equal to the graph array
            width = data[0].Count(); // Sets the width equal to the number of values in each row of the data array
            height = data.Count(); // Sets the height equal to the number of rows in the data array

            CreateGrid(); // Runs the CreateGrid method
        }

        /**********************
        CreateGrid method populates the grid object
        ***********************/

        void CreateGrid() {

            grid = new Node[width, height]; // Instaniates the grid object with the calculated width and height

            /**********************
            Iterate through each instance of the grid object
            ***********************/

            for (int x = 0; x < width; x++) {

                for (int y = 0; y < width; y++) {

                    isWalkable = !((int)data[x][y] == 9); // Checks if the int value at the current coordinates is equal to the predetermined highest possible density value
                    grid[x,y] = new Node(isWalkable, new Coordinate() {X = x, Y = y}); // Instantiates a new object of type Node at the current coordinates with its corresponsing arguments.
                }
            }
        }

        /**********************
        GetNeighbors method determines the valid neighbors of a specified node
        ***********************/

        public List<Node> GetNeighbors(Node node) {

            List<Node> neighbors = new List<Node>(); // Creates and instantiates a new list of nodes

            /**********************
            Iterate through each adjacent node of the specified node
            ***********************/

            for (int x = -1; x <=1; x++) {

                for (int y = -1; y <=1; y++) {

                    if (x == 0 && y == 0) { // Checks if the current adjacent node is equal to the specified node

                        continue; // Continues to the next iteration of the loop
                    }

                    int checkX = node.position.X + x; // Sets a temporary x-value equal to the sum of the specified node's x-position and the current x-value of the iteration
                    int checkY = node.position.Y + y; // Sets a temporary y-value equal to the sum of the specified node's y-position and the current y-value of the iteration

                    if (checkX >= 0 && checkX < width && checkY >= 0 && checkY < height) { // Checks if the temporary x-value is more than or equal to 0, if the temporary x-value is less than the maximum width of the grid, if the temporary y-value is more than or equal to 0, and if the temporary y-value is less than the maximum height of the grid

                        neighbors.Add(grid[checkX,checkY]); // Adds the Node at the grid position of the temporary x and y values to the list of neighboring nodes
                    }
                }
            }

            return neighbors; // Returns the neighboring nodes of the specified node
        }
    }
}
