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
            TankDb.Current.Initialize();

            var simulation = new Simulation(1000, 100);
            simulation.LogStream = Console.OpenStandardOutput();
            simulation.Start();
        }
    }
}
