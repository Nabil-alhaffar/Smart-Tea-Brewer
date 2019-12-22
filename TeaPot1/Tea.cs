using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TeaPot1
{

    class Tea
    {
        public string teaName { get; set;}
        public int healthScore { get; set;}
        public int steepingTime { get; set; }
        public int steepingTemperature{ get; set; }
        public int controlButton;
        public Dictionary<Tea,int > TeaDict;


        public Tea()
        {
   
        }
        public Tea(string teaName, int healthScore, int steepingTime,int steepingTemperature,int controlButton)
        {
            this.teaName = teaName;
            this.healthScore = healthScore;
            this.steepingTime = steepingTime;
            this.steepingTemperature = steepingTemperature;
            this.controlButton = controlButton;

        }
        

    }
    
}
