using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FADE__External_Tool
{
    class Program
    {
        static void Main(string[] args)
        {
            //Should write a file for all variables FADE might need
            StreamWriter writer = new StreamWriter("values.txt");
            writer.WriteLine("280");//distance between each enemy spawn, int
            
            writer.WriteLine("6");//sets health value for player
            writer.WriteLine("3");//sets player speed;

            //random value for choosing an enemy wave
            Random rng = new Random();
            int rngValue = rng.Next(1, 4);
            writer.WriteLine(rngValue);

            writer.WriteLine("gggtg");//enemy pattern1, string of chars
            writer.WriteLine("ggtgt");//enemy pattern2, string of chars
            writer.WriteLine("tgtgg");//enemy pattern3, string of chars

            writer.WriteLine("5");//enemy amount, int

            writer.WriteLine("2");//spawn amount, dependent on distance travelled, determines wave #

            writer.WriteLine("1");//spawn speed, timing between waves
            writer.Close();
        }
    }
}
