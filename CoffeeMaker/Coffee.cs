using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeMaker
{
    class Coffee
    {
        public int ID { get; set; }

        public double SugarQuantity { get; set; } // quantity of sugar needed for coffee

        public double WaterQuantity { get; set; } //quantity of  water needed for coffee

        public double CoffeeQuantity { get; set; } //quantity of coffee in ready coffee

        public int Price { get; set; }

        public Coffee(int id , int price) //water,sugar,coffee quanities are equal to id*0.1 for all coffees !!
        {
            ID = id;
            SugarQuantity = id * 0.1;
            WaterQuantity = id * 0.1;
            CoffeeQuantity = id * 0.1;
            Price = price;
        }
        
    }
}
