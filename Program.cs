using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace ConsoleApp7
{
    internal class Program
    {
        private static string? connectionString;
        private static IDbConnection? connection;
        private static bool isConnected = false;

        static void Main(string[] args)
        {
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();

            connectionString = config.GetConnectionString("DefaultConnection");

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                Console.WriteLine(" Connection string not found!");
                Console.ReadLine();
                return;
            }

            bool exit = false;

            while (!exit)
            {
                Console.Clear();

                Console.WriteLine("========================================");
                Console.WriteLine("         STATIONERY COMPANY");
                Console.WriteLine("========================================");
                Console.WriteLine($"Status: {(isConnected ? "CONNECTED" : "DISCONNECTED")}");
                Console.WriteLine("========================================");

                Console.WriteLine("1. Connect to Database");
                Console.WriteLine("2. Disconnect from Database");
                Console.WriteLine("3. Show All Stationery Items");
                Console.WriteLine("4. Show All Stationery Types");
                Console.WriteLine("5. Show All Sales Managers");
                Console.WriteLine("6. Show Item with Maximum Quantity");
                Console.WriteLine("7. Show Item with Minimum Quantity");
                Console.WriteLine("8. Show Item with Minimum Cost Price");
                Console.WriteLine("9. Show Item with Maximum Cost Price");
                Console.WriteLine("0. Exit");

                Console.Write("\nChoose option: ");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": Connect(); break;
                    case "2": Disconnect(); break;
                    case "3": ShowAllItems(); break;
                    case "4": ShowAllTypes(); break;
                    case "5": ShowAllManagers(); break;
                    case "6": ShowMaxQuantity(); break;
                    case "7": ShowMinQuantity(); break;
                    case "8": ShowMinCost(); break;
                    case "9": ShowMaxCost(); break;
                    case "0": exit = true; break;
                    default:
                        Console.WriteLine("Invalid option");
                        Pause();
                        break;
                }
            }
        }

        static void Connect()
        {
            if (isConnected)
            {
                Console.WriteLine("\n Already connected!");
                Pause();
                return;
            }

            try
            {
                connection = new SqlConnection(connectionString);
                connection.Open();

                isConnected = true;

                Console.WriteLine("\n✓ Successfully connected to database!");
            }
            catch (Exception ex)
            {
                isConnected = false;
                connection = null;

                Console.WriteLine("\n Connection failed!");
                Console.WriteLine(ex.Message);
            }

            Pause();
        }

        static void Disconnect()
        {
            if (!isConnected || connection == null)
            {
                Console.WriteLine("\n Not connected!");
                Pause();
                return;
            }

            connection.Close();
            connection.Dispose();

            connection = null;
            isConnected = false;

            Console.WriteLine("\n✓ Successfully disconnected!");
            Pause();
        }

        static bool EnsureConnected()
        {
            if (!isConnected || connection == null)
            {
                Console.WriteLine("\n Please connect to database first!");
                Pause();
                return false;
            }
            return true;
        }

        static void ShowAllItems()
        {
            if (!EnsureConnected()) return;

            try
            {
                string sql = @"
                    SELECT SI.ItemName, ST.TypeName, SI.QuantityInStock, SI.CostPrice
                    FROM StationeryItems SI
                    JOIN StationeryTypes ST ON SI.TypeID = ST.TypeID
                    ORDER BY SI.ItemName";

                var items = connection!.Query(sql);

                Console.WriteLine("\n===== ALL ITEMS =====\n");

                foreach (var item in items)
                {
                    Console.WriteLine($"Name     : {item.ItemName}");
                    Console.WriteLine($"Type     : {item.TypeName}");
                    Console.WriteLine($"Quantity : {item.QuantityInStock}");
                    Console.WriteLine($"Cost     : {item.CostPrice}");
                    Console.WriteLine("--------------------------------");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Pause();
        }

        static void ShowAllTypes()
        {
            if (!EnsureConnected()) return;

            var types = connection!.Query<string>(
                "SELECT TypeName FROM StationeryTypes ORDER BY TypeName");

            Console.WriteLine("\n===== TYPES =====\n");

            foreach (var t in types)
                Console.WriteLine(t);

            Pause();
        }

        static void ShowAllManagers()
        {
            if (!EnsureConnected()) return;

            var managers = connection!.Query(
                "SELECT FirstName, LastName FROM Managers ORDER BY LastName");

            Console.WriteLine("\n===== MANAGERS =====\n");

            foreach (var m in managers)
                Console.WriteLine($"{m.FirstName} {m.LastName}");

            Pause();
        }

        static void ShowMaxQuantity()
        {
            if (!EnsureConnected()) return;

            ShowSingle(@"
                SELECT TOP 1 ItemName, QuantityInStock, CostPrice
                FROM StationeryItems
                ORDER BY QuantityInStock DESC");
        }

        static void ShowMinQuantity()
        {
            if (!EnsureConnected()) return;

            ShowSingle(@"
                SELECT TOP 1 ItemName, QuantityInStock, CostPrice
                FROM StationeryItems
                ORDER BY QuantityInStock ASC");
        }

        static void ShowMinCost()
        {
            if (!EnsureConnected()) return;

            ShowSingle(@"
                SELECT TOP 1 ItemName, QuantityInStock, CostPrice
                FROM StationeryItems
                ORDER BY CostPrice ASC");
        }

        static void ShowMaxCost()
        {
            if (!EnsureConnected()) return;

            ShowSingle(@"
                SELECT TOP 1 ItemName, QuantityInStock, CostPrice
                FROM StationeryItems
                ORDER BY CostPrice DESC");
        }

        static void ShowSingle(string sql)
        {
            try
            {
                var item = connection!.QueryFirstOrDefault(sql);

                if (item == null)
                {
                    Console.WriteLine("\nNo data found.");
                }
                else
                {
                    Console.WriteLine("\n===== RESULT =====\n");
                    Console.WriteLine($"Name     : {item.ItemName}");
                    Console.WriteLine($"Quantity : {item.QuantityInStock}");
                    Console.WriteLine($"Cost     : {item.CostPrice}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Pause();
        }

        static void Pause()
        {
            Console.WriteLine("\nPress Enter...");
            Console.ReadLine();
        }
    }
}