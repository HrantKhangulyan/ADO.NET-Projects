using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeMaker
{
    class CoffeeMachine
    {
        public double Water;
        public double Sugar;
        public double Coffee; 
        public int MoneyIn; 

        public CoffeeMachine(double w , double s, double c)
        {
            Water = w;
            Sugar = s;
            Coffee = c;
        }
    }
}
