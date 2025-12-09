//
// FILE               : Borrow.cs
// PROJECT            : RBLibraryManagement
// PROGRAMMER		  : Josiah Williams
// FIRST VERSION      : 2025-12-09
// DESCRIPTION        : This class handles CRUD operations for the Borrow entity in the library management system.
//   
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace RBLibraryManagement
{
    internal class Borrow
    {
        public void Run()
        {
            bool stay = true;

            while (stay)
            {
                char choice = BorrowMenu();

                switch (choice)
                {
                    case '1':
                        CreateBorrow();
                        break;

                    case '2':
                        ReadBorrows();
                        break;

                    case '3':
                        UpdateBorrow();
                        break;

                    case '4':
                        DeleteBorrow();
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

        public char BorrowMenu()
        {
            Console.WriteLine("\nCRUD Options");
            Console.WriteLine("1. Create Borrow");
            Console.WriteLine("2. Read Borrow Table");
            Console.WriteLine("3. Update Borrow");
            Console.WriteLine("4. Delete Borrow");
            Console.WriteLine("5. Go back to Main Menu");
            Console.Write("Select an option (1-5): ");
            return Console.ReadKey(true).KeyChar;
        }

        private void CreateBorrow()
        {
            Console.WriteLine("Create Borrow (not implemented)");
        }
      
           private void ReadBorrows()
        {
            Console.WriteLine("\n--- Borrow Records ---");

            string connectionstring = "Server=localhost;Port=3306;Uid=root;Pwd=root;Database=Library_Management_System;";
            MySqlConnection connection = new MySqlConnection(connectionstring);
            string query = "SELECT * FROM Borrow";

            try
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand(query, connection);
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                  Console.WriteLine(
                $"Borrow ID: {reader["borrow_ID"]}, " +
                $"Book ID: {reader["book_ID"]}, " +
                $"Member ID: {reader["member_ID"]}, " +
                $"Issued By: {reader["issuedByLibrarian"]}, " +
                $"Borrow Date: {reader["borrowing_date"]}, " +
                $"Return Date: {reader["return_date"]}");
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

        
        private void UpdateBorrow()
        {
            Console.WriteLine("Update Borrow (not implemented)");
        }
        private void DeleteBorrow()
        {
            Console.WriteLine("Update Borrow (not implemented)");
        }
    }
}
