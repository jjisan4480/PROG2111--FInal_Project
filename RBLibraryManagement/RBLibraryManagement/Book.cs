//
// FILE               : Book.cs
// PROJECT            : RBLibraryManagement
// PROGRAMMER		  : Josiah Williams
// FIRST VERSION      : 2025-12-09
// DESCRIPTION        : This class handles CRUD operations for the Book entity in the library management system.
// 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace RBLibraryManagement
{
    internal class Book
    {
        public void Run()
        {
            bool stay = true;

            while (stay)
            {
                char choice = BookMenu();

                switch (choice)
                {
                    case '1':
                        CreateBook();
                        break;

                    case '2':
                        ReadBooks();
                        break;

                    case '3':
                        UpdateBook();
                        break;

                    case '4':
                        DeleteBook();
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

        public char BookMenu()
        {
            Console.WriteLine("\nCRUD Options");
            Console.WriteLine("1. Create Book");
            Console.WriteLine("2. Read Book Table");
            Console.WriteLine("3. Update Book");
            Console.WriteLine("4. Delete Book");
            Console.WriteLine("5. Go back to Main Menu");
            Console.Write("Select an option (1-5): ");
            return Console.ReadKey(true).KeyChar;
        }

        private void CreateBook()
        {
            Console.WriteLine("Create Book (not implemented)");
        }
       
          private void ReadBooks()
        {
            Console.WriteLine("\n--- Book List ---");

            string connectionstring = "Server=localhost;Port=3306;Uid=root;Pwd=root;Database=Library_Management_System;";
            MySqlConnection connection = new MySqlConnection(connectionstring);
            string query = "SELECT * FROM Book";

            try
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand(query, connection);
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Console.WriteLine(
                        $"ID: {reader["book_ID"]}, " +
                        $"Title: {reader["title"]}, " +
                        $"ISBN: {reader["ISBN"]}, " +
                        $"Price: {reader["price"]}, " +
                        $"Author ID: {reader["author_ID"]}, " +
                        $"Published: {reader["publishing_date"]}, " +
                        $"Status: {reader["book_status"]}, " +
                        $"Genre: {reader["genre"]}");
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

        
        private void UpdateBook()
        {
            Console.WriteLine("Update Book (not implemented)");
        }
        private void DeleteBook()
        {
            Console.WriteLine("Update Book (not implemented)");
        }
    }
}
