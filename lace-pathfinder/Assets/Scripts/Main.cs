using System;
using static API.Request;
using static Path.AStar;

namespace Scripts {
    class Program {
        static void Main() {
            
            apiRequest();

            Console.WriteLine(BestPath(0, 0, 10, 10));
        }
    }
}