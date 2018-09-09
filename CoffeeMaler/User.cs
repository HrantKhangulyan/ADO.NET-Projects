using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;

namespace CoffeeMaker
{
    static class User
    {
        public static void InsertCoin(int amount, CoffeeMachine cm)
        {
            cm.MoneyIn += amount;
        }

        public static void Buy(Coffee c, CoffeeMachine cm)
        {
            string connectionstring = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionstring))
            {
                connection.Open();
                if (c.WaterQuantity <= cm.Water && c.SugarQuantity <= cm.Sugar && c.CoffeeQuantity <= cm.Coffee)
                {
                    cm.Water -= c.WaterQuantity;
                    cm.Sugar -= c.SugarQuantity;
                    cm.Coffee -= c.CoffeeQuantity;
                    cm.MoneyIn -= c.Price;
                    string updateQuerry = $"update Storage set water = {cm.Water},  sugar = {cm.Sugar}, coffee = {cm.Coffee}";

                    SqlCommand com = new SqlCommand(updateQuerry, connection);

                    com.ExecuteNonQuery();
                }
            }
        }
    }
}
