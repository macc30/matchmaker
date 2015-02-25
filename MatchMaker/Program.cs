using MatchMaker.Algorithms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MatchMaker.Core;

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
                TankDb.Current.SaveToFile("tanks.json");
                Console.WriteLine(" Done!");
            }

            var simulation = new Simulation<Wargaming>(23000, 250);
            simulation.Verbocity = Simulation<Wargaming>.LogVerbocity.Off;
            simulation.RealTime = false;
            simulation.LogStream = Console.OpenStandardOutput();
            simulation.Start();

            var results = simulation.CollectResults();

            Console.Clear();

            Console.WriteLine();
            Console.WriteLine("Results");
            Console.WriteLine("-------");
            Console.WriteLine();

            Console.WriteLine("         Tier Placement Average: " + results.TierPlacementAverage.ToString());
            Console.WriteLine("Tier Placement Average (Lights): " + results.TierPlacementAverageLights.ToString());
            Console.WriteLine("              Queue Depth (Avg): " + results.QueueDepthAverage.ToString());
            Console.WriteLine();
            Console.WriteLine("         Player Wait Time (Avg): " + ((long)results.WaitTimeAverage).ClockFormat());
            Console.WriteLine("      Player Wait Time (StdDev): " + ((long)results.WaitTimeStandardDeviation).ClockFormat());
            Console.WriteLine();
            Console.WriteLine("             Match Length (Avg): " + ((long)results.MatchLengthAverage).ClockFormat());
            Console.WriteLine();
            Console.WriteLine("              Heavy Count (Avg): " + results.HeavyCountAverage.ToString());
            Console.WriteLine("             Medium Count (Avg): " + results.MediumCountAverage.ToString());
            Console.WriteLine("              Light Count (Avg): " + results.LightCountAverage.ToString());
            Console.WriteLine("                 TD Count (Avg): " + results.TankDestroyerCountAverage.ToString());
            Console.WriteLine("          Artillery Count (Avg): " + results.ArtilleryCountAverage.ToString());

            Console.WriteLine();

            Console.WriteLine("Tier Distribution");
            Console.WriteLine("-----------------");
            Console.WriteLine();

            for (var tier = 1; tier <= 10; tier++)
            {
                Console.WriteLine(String.Format("{0}: {1}", tier, results.MatchTierDistribution[tier]));
            }

            Console.ReadKey();
        }
    }
}
