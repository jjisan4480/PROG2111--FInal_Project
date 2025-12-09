//
// FILE               : Author.cs
// PROJECT            : RBLibraryManagement
// PROGRAMMER		  : Josiah Williams
// FIRST VERSION      : 2025-12-09
// DESCRIPTION        : This class handles CRUD operations for the Author entity in the library management system.
//   
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace RBLibraryManagement
{
    internal class Author
    {
        public void Run()
        {
            bool stay = true;

            while (stay)
            {
                char choice = AuthorMenu();

                switch (choice)
                {
                    case '1':
                        CreateAuthor();
                        break;

                    case '2':
                        ReadAuthors();
                        break;

                    case '3':
                        UpdateAuthor();
                        break;

                    case '4':
                        DeleteAuthor();
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

        public char AuthorMenu()
        {
            Console.WriteLine("\nCRUD Options");
            Console.WriteLine("1. Create Author");
            Console.WriteLine("2. Read Author Table");
            Console.WriteLine("3. Update Author");
            Console.WriteLine("4. Delete Author");
            Console.WriteLine("5. Go back to Main Menu");
            Console.Write("Select an option (1-5): ");
            return Console.ReadKey(true).KeyChar;
        }

        private void CreateAuthor()
        {
            Console.WriteLine("Create Author (not implemented)");
        }
       
        private void ReadAuthors()
        {
            Console.WriteLine("\n--- Author List ---");

            string connectionstring = "Server=localhost;Port=3306;Uid=root;Pwd=root;Database=Library_Management_System;";
            MySqlConnection connection = new MySqlConnection(connectionstring);
            string query = "SELECT * FROM Author";

            try
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand(query, connection);
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Console.WriteLine(
                        $"ID: {reader["author_id"]}, " +
                        $"Name: {reader["first_name"]} {reader["last_name"]}"
                    );
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

        
        private void UpdateAuthor() { 
            Console.WriteLine("Update Author (not implemented)");
        } 
        private void DeleteAuthor()
        {
           Console.WriteLine("Update Author (not implemented)");
        }
    }
}
