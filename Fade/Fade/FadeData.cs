using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Fade
{
    class FadeData
    {
        public int Score { get; set; }
        public int HighScore { get; set; }

        public int loadHighScore()
        {
            int hs = 0;
            string line = "";
            StreamReader reader = new StreamReader("HighScore.txt");
            while ((line = reader.ReadLine()) != null)
            {
                hs = Convert.ToInt32(line);
            }
            reader.Close();
            HighScore = hs;
            return hs;
        }
        public void newHighScore(int score)
        {
            try
            {
                StreamWriter writer = new StreamWriter("HighScore.txt");
                writer.WriteLine(score);
                writer.Close();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
