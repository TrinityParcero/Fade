using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace Fade
{

    //tank states
    
    enum TankState
    {
      
      BuildUp,
      ChargeUpL
     }
     
    class Tank : Enemy
    {
        //two or three health
        //half player speed
        Rectangle tankRect;
        Texture2D tankSprite;
        int ground;
        //initialize the TankState
        TankState tState = TankState.ChargeUpL;
        //make a bool for checking the tank's charge
        bool chargePrep;
        //have a property for the charge build up frames
        public int chargeFrame { get; set; } = 50;

        public Tank(Texture2D asset, Rectangle loc, int speed, double hp, double dmg, SoundEffect sound) : base(asset,loc,speed,hp,dmg,sound)
        {
            //save off the tank's current x position this will be used in
            int tankCurrLoc = tankRect.Y;
            
            ground = location.Y;
            tankRect = loc;
            tankSprite = asset;
            color = Color.White;
            //initialize the chargePrep
            chargePrep = false;
            Death = sound;
            ///define a stopping distance = player location.x - a length of the tank himself;
        }


        /// <summary>
        /// have a check to see if the tank is a distance 6 from the player
        /// and set the boolean chargePrep to true
        /// </summary>
        /// <param name="playerX">the x int position of the player rectangle</param>
        //public void chargeCheck(int playerX)
        //{
        //    if (tankRect.X - playerX <= 300)
        //    {
        //        //some distance we want to charge from
                

        //        if (chargeFrame > 0)
        //        {
        //            chargeFrame--;
        //            chargePrep = false;
        //        }
               
                
        //        if (chargeFrame <= 0)
        //        {
        //            chargePrep = true;
                    
        //            chargeFrame = 200;
        //        }
                
        //    }

        //    else if (tankRect.X - playerX <= 0)
        //    {
        //        chargePrep = false;
        //    }
         
        //}

        /// <summary>
        /// check to see if the chargePrep is true, and if it is then incerement the tanks position by a lrger number than its usual speed so that it eventually catches up to the player
        /// and once it has (the distance between the tank and player is 0, the n set the charge prep to false)
        /// </summary>
        /// <param name="chargeSpeed">the amount of speed that the tank increments</param>
        /// <param name="playerX">the x int position of the player rectangle</param>
        public void chargeUpdate(int chargeSpeed, Player p )
        {
            //chargeCheck(p.location.X);

            if (location.X - p.location.X <= 450)
            {
                //some distance we want to charge from
                //chargePrep = true;
                //Death.Play();
                location.X -= chargeSpeed;
            }

            else if (location.X - p.location.X >= 0)
            {
                //chargePrep = false;
            }

            if (chargePrep == true)
          {
                //tankRect.X -= chargeSpeed;
                
          }
         
         
        }












    }
}
