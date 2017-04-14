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
        //private Grunt Grunt;//temp 
        //private Tank Tank;//temp
        private List<Enemy> enemyList = new List<Enemy>();

        public List<Enemy> EnemyList
        {
            get { return enemyList; }
        }

        private int health;
        private int speed;
        private int Distance = 100;
        private int Wave = 1;
        private int Amount = 1;

        public void CreateSpawn(string file, Texture2D grunt, Texture2D tank, Rectangle playerLoc)
        {
            
            string line = "";
            int count = 0;//to help with specific lines
            StreamReader reader = new StreamReader(file);
            while ((line = reader.ReadLine()) != null)
            {
                if (count == 0)
                {
                    //parsing the string for Distance
                    int.TryParse(line, out Distance);
                }
                else if (count == 1)
                {
                    //parsing the string for enemy wave number
                    int.TryParse(line, out Wave);
                }
                else if (count == 2)
                {
                    //creates a new enemy for each respective char in string
                    //& places enemy certain distance away from player
                    for (int i = 0; i < line.Length; i++)
                    {
                        if (line[i] == 'g')
                        {
                            enemyList.Add(new Grunt(grunt, new Rectangle(playerLoc.X+(Distance*(i+1)), playerLoc.Y, 100, 100),1,1,1,new Rectangle(100,100,100,100)));
                        }
                        else if (line[i] == 't')
                        {
                            enemyList.Add(new Tank(tank, new Rectangle(playerLoc.X+(Distance/ (i+1)), playerLoc.Y, 100, 100),2,2,2));
                        }
                    }//end of for loop
                }
                count++;
            }//end of while loop
        }
    }
}
