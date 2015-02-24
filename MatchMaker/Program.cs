using MatchMaker.Algorithms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatchMaker
{
    class Program
    {
        static void Main(string[] args)
        {
            if(!System.IO.File.Exists("tanks.json"))
            {
                Console.Write("Fetching Tank Data ...");
                TankDb.Current.InitializeFromWeb();
                Console.WriteLine(" Done!");
                Console.Write("Saving Tank Data ...");
                TankDb.Current.SaveToFile("tanks.json");
                Console.WriteLine(" Done!");
            } 
            else
            {
                Console.Write("Fetching Tank Data ...");
                TankDb.Current.InitializeFromFile("tanks.json");
                Console.WriteLine(" Done!");
            }

            var simulation = new Simulation<InOrderCreation>(23000, 100);
            simulation.RealTime = false;
            simulation.LogStream = Console.OpenStandardOutput();
            simulation.Start();
        }
    }
}
