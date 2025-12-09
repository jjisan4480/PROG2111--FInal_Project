//
// FILE               : Fine.cs
// PROJECT            : RBLibraryManagement
// PROGRAMMER		  : Josiah Williams
// FIRST VERSION      : 2025-12-09
// DESCRIPTION        : This class handles CRUD operations for the Fine entity in the library management system.
//   
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace RBLibraryManagement
{
    internal class Fine
    {
        public void Run()
        {
            bool stay = true;

            while (stay)
            {
                char choice = FineMenu();

                switch (choice)
                {
                    case '1':
                        CreateFine();
                        break;

                    case '2':
                        ReadFines();
                        break;

                    case '3':
                        UpdateFine();
                        break;

                    case '4':
                        DeleteFine();
                        break;

                    case '5':
                        // Go back to main menu
                        stay = false;
                        break;

                    default:
                        Console.WriteLine("Invalid choice!");
                        break;
                }
            }
        }

        public char FineMenu()
        {
            Console.WriteLine("\nCRUD Options");
            Console.WriteLine("1. Create Fine");
            Console.WriteLine("2. Read Fine Table");
            Console.WriteLine("3. Update Fine");
            Console.WriteLine("4. Delete Fine");
            Console.WriteLine("5. Go back to Main Menu");
            Console.Write("Select an option (1-5): ");
            return Console.ReadKey(true).KeyChar;
        }

        private void CreateFine()
        {
            Console.WriteLine("Create Fine (not implemented)");
        }
        private void ReadFines()
        {
            Console.WriteLine("\n--- Fines ---");

            string connectionstring = "Server=localhost;Port=3306;Uid=root;Pwd=root;Database=Library_Management_System;";
            MySqlConnection connection = new MySqlConnection(connectionstring);
            string query = "SELECT * FROM Fines";

            try
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand(query, connection);
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                 Console.WriteLine(
                 $"Fine ID: {reader["fine_ID"]}, " +
                 $"Borrow ID: {reader["borrow_ID"]}, " +
                 $"Amount: {reader["fine_amount"]}, " +
                 $"Due Date: {reader["due_date"]}, " +
                 $"Paid Date: {reader["paid_date"]}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
                //Console.WriteLine("Connection closed");
            }
        }

        private void UpdateFine()
        {
            Console.WriteLine("Update Fine (not implemented)");
        }
        private void DeleteFine()
        {
            Console.WriteLine("Update Fine (not implemented)");
        }
    }
}
