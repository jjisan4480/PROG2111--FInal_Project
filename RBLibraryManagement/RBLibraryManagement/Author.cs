//
// FILE               : Author.cs
// PROJECT            : RBLibraryManagement
// PROGRAMMER		  : Josiah Williams
// FIRST VERSION      : 2025-12-09
// DESCRIPTION        : This class handles CRUD operations for the Author entity in the library management system.
//   
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RBLibraryManagement
{
    internal class Author
    {
        string? authFirstName;
        string? authLastName;
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
            Console.WriteLine("\n--- Create Author ---");

            GetInfo();
            string connString = "Server=localhost;Port=3306;Uid=root;Pwd=root;Database=Library_Management_System;";
            MySqlConnection connection = new MySqlConnection(connString);

            try
            {
                connection.Open();
                string query = "SELECT * FROM Author";
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection);

                DataSet ds = new DataSet();
                adapter.Fill(ds, "Author");
                DataTable? table = ds.Tables["Author"];
                if (table != null)
                {
                    DataRow row = table.NewRow();
                    row["first_name"] = authFirstName;
                    row["last_name"] = authLastName;

                    table.Rows.Add(row);
                } else
                {
                    throw new Exception("Failed to load Author table");
                }
                    MySqlCommandBuilder builder = new MySqlCommandBuilder(adapter);
                adapter.Update(ds, "Author");

                Console.WriteLine("Author successfully created.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
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

        private void GetInfo()
        {
            bool valid = false;
            while (!valid)
            {
                Console.Write("Enter first name: ");
                string? first = Console.ReadLine();
                if(!string.IsNullOrWhiteSpace(first) && first.Length < 50 )
                {
                    authFirstName = first;
                    valid = true;
                }
                else
                {
                    Console.WriteLine("First name cannot be empty. Please try again.");
                }
            }
            valid = false;
           while(!valid)
            {
                Console.Write("Enter last name: ");
                string? last = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(last) && last.Length < 50)
                {
                    authLastName = last;
                    valid = true;
                }
                else
                {
                    Console.WriteLine("Last name cannot be empty. Please try again.");
                }
            }
         
        }
    }
}
