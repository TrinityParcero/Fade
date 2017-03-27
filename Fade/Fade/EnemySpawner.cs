using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Fade
{
    class EnemySpawner
    {
        private Enemy Grunt;
        private Enemy Tank;
        private int Distance = 100;
        private int Wave = 1;
        private int Amount = 1;
        List<Enemy> wave = new List<Enemy>();

        public void spawnEnemies(string file, Texture2D grunt, Texture2D tank, Rectangle playerLoc)
        {
            string line = "";
            StreamReader reader = new StreamReader(file);
            int.TryParse(reader.ReadLine(), out Distance);
            int.TryParse(reader.ReadLine(), out Wave);

            line = reader.ReadLine();
            for(int i = 0; i < line.Length; i++)
            {
                if (line[i] == 'g')
                {
                    wave.Add(Grunt = new Grunt(grunt,new Rectangle(playerLoc.X + 280, playerLoc.Y, 100, 100),2,1,0.5));
                }
                else if (line[i] == 't')
                {
                    wave.Add(Tank = new Tank(tank, new Rectangle(playerLoc.X + 280, playerLoc.Y, 100, 100),1,2,1));
                }
            }
        }
    }
}
