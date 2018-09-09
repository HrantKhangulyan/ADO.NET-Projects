using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeMaker
{
    static class UI //Interface for Customer for communication with him ! 
    {
        public static void InterFace() //method for communication with customer
        {
            ConsoleKeyInfo c;
            CoffeeMachine cm = GetCoffeeMachine();
            if (cm.Water < 0.1 || cm.Sugar < 0.1 || cm.Coffee < 0.1) //if ingredients in machine are over
            {
                Console.WriteLine("There are no ingredients to prepare any coffee for you :( ");
                Console.WriteLine("We appologise for tecnical inconviniances :(");
                Console.WriteLine("Press any key to continue");
                Console.ReadKey();
                return;
            }
            Console.WriteLine("\t\t**** Wlcome To My CoffeeMachine ****");
            Console.WriteLine("Only coins 50 , 100 , 200 and 500 are available !");
            Console.WriteLine("Press any key to start !");
            Console.ReadKey();
            Console.Clear();
            do
            {
                Console.WriteLine($"Press '1' to insert a coin or '0' to make order .. (Money in Machine - {cm.MoneyIn})");
                Console.WriteLine("Press 'E' to exit !");
                c = Console.ReadKey();

                switch (c.Key)
                {
                    case (ConsoleKey.E):
                        if (cm.MoneyIn == 0) return;
                        else Console.WriteLine($"\nTake You money ` {cm.MoneyIn}"); //return customers money if it exists
                        return;

                    case (ConsoleKey.NumPad1):
                    case (ConsoleKey.D1):
                        Console.WriteLine("\nEnter the value of coin (50,100,200,500)");
                        int coinvalue;
                        bool ParseSucceeded = Int32.TryParse(Console.ReadLine(), out coinvalue);
                        while ((!(coinvalue == 50 || coinvalue == 100 || coinvalue == 200 || coinvalue == 500)) || !ParseSucceeded) //check correctness of imput
                        {
                            Console.WriteLine("Only coins 50 , 100 , 200 and 500 are available !");
                            Console.WriteLine("Enter proper value !");
                            ParseSucceeded = Int32.TryParse(Console.ReadLine(), out coinvalue);
                        }
                        User.InsertCoin(coinvalue, cm);
                        break;

                    case (ConsoleKey.NumPad0):
                    case (ConsoleKey.D0):
                        if (cm.MoneyIn == 0)
                        {
                            Console.WriteLine("\nThere is no money in Machine !");
                            System.Threading.Thread.Sleep(1100);
                            break;
                        }
                        List<Coffee> availablecoffees = GetAvailableCoffees(cm.MoneyIn);
                        foreach (var item in availablecoffees.ToList()) //removing those coffees which cant be prepared because of ingrediend loss
                        {
                            if (item.WaterQuantity > cm.Water || item.SugarQuantity > cm.Sugar || item.CoffeeQuantity > cm.Coffee)
                            {
                                availablecoffees.Remove(item);
                            }
                        }
                        if (availablecoffees.Count == 0)
                        {
                            Console.WriteLine("\nNo coffees available !");
                            break;
                        }
                        Console.Clear();
                        Console.WriteLine($"You can buy the following coffees... (You have {cm.MoneyIn}$) \n");
                        foreach (var coffee in availablecoffees)
                        {
                            Console.WriteLine($"CoffeeNumber - {coffee.ID} , Price - {coffee.Price}$");
                        }

                        Console.WriteLine("\nEnter the number of the coffee you want");
                        int number;
                        bool parsed = int.TryParse(Console.ReadLine(), out number);
                        while (!parsed)
                        {
                            Console.WriteLine("Type something reasonable");
                            parsed = int.TryParse(Console.ReadLine(), out number);
                        }
                        foreach (var cof in availablecoffees)
                        {
                            if (number == cof.ID)
                            {
                                User.Buy(cof, cm);
                                Console.WriteLine($"Thank you ! your change is {cm.MoneyIn}$ ");
                                cm.MoneyIn = 0;
                                Console.WriteLine("Press Any key to continue ");
                                Console.ReadKey();
                                return;
                            }
                        }
                        Console.WriteLine("Wrong Number !!!"); //inform the customer that he inserted wrong coffee number
                        break;

                    default:
                        Console.WriteLine("\nPress 1 or 2 or 'e' !!!\n");
                        System.Threading.Thread.Sleep(2500);
                        break;
                }
            } while (true);
        }

        private static List<Coffee> GetAvailableCoffees(int MoneyInMachine)
        {
            List<Coffee> coffees = new List<Coffee>();
            string connectionstring = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionstring))
            {
                connection.Open();
                string command = "select * from Coffees";
                SqlCommand com = new SqlCommand(command, connection);

                SqlDataReader reader = com.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        int id = (int)reader["ID"];
                        int price = (int)reader["Price"];

                        if (MoneyInMachine >= price)
                            coffees.Add(new Coffee(id, price));
                    }
                }

                reader.Close();
            }
            return coffees;
        }

        private static CoffeeMachine GetCoffeeMachine()
        {
            string connectionstring = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionstring))
            {
                connection.Open();
                string command = "select * from Storage";
                SqlCommand com = new SqlCommand(command, connection);
                CoffeeMachine cm = null;
                SqlDataReader reader = com.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        double water = (double)reader["Water"];
                        double sugar = (double)reader["Sugar"];
                        double coffee = (double)reader["Coffee"];
                        cm = new CoffeeMachine(water, sugar, coffee);
                    }
                }
                reader.Close();
                return cm;
            }
        }
    }
}
