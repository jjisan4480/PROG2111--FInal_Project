//
// FILE               : Librarian.cs
// PROJECT            : RBLibraryManagement
// PROGRAMMER		  : Josiah Williams
// FIRST VERSION      : 2025-12-09
// DESCRIPTION        : This class handles CRUD operations for the Librarian entity in the library management system.
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
    internal class Librarian
    {
        public void Run()
        {
            bool stay = true;

            while (stay)
            {
                char choice = LibrarianMenu();

                switch (choice)
                {
                    case '1':
                        CreateLibrarian();
                        break;

                    case '2':
                        ReadLibrarians();
                        break;

                    case '3':
                        UpdateLibrarian();
                        break;

                    case '4':
                        DeleteLibrarian();
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

        public char LibrarianMenu()
        {
            Console.WriteLine("\nCRUD Options");
            Console.WriteLine("1. Create Librarian");
            Console.WriteLine("2. Read Librarian Table");
            Console.WriteLine("3. Update Librarian");
            Console.WriteLine("4. Delete Librarian");
            Console.WriteLine("5. Go back to Main Menu");
            Console.Write("Select an option (1-5): ");
            return Console.ReadKey(true).KeyChar;
        }

       
           private void CreateLibrarian()
        {
            Console.WriteLine("\n--- Create Librarian ---");

            Console.Write("Enter first name: ");
            string first = Console.ReadLine();

            Console.Write("Enter last name: ");
            string last = Console.ReadLine();

            Console.Write("Enter phone number: ");
            string phone = Console.ReadLine();

            string connString = "Server=localhost;Port=3306;Uid=root;Pwd=root;Database=Library_Management_System;";
            MySqlConnection connection = new MySqlConnection(connString);
            try
            {
                connection.Open();
                string query = "SELECT * FROM Librarian";
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection);

                DataSet ds = new DataSet();
                adapter.Fill(ds, "Librarian");
                DataTable table = ds.Tables["Librarian"];

                DataRow newRow = table.NewRow();
                newRow["first_name"] = first;
                newRow["last_name"] = last;
                newRow["phone_number"] = phone;

                table.Rows.Add(newRow);

                MySqlCommandBuilder builder = new MySqlCommandBuilder(adapter);
                adapter.Update(ds, "Librarian");

                Console.WriteLine("Librarian created.");
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

        
        private void ReadLibrarians()
        {
            Console.WriteLine("\n--- Librarian List ---");

            string connectionstring = "Server=localhost;Port=3306;Uid=root;Pwd=root;Database=Library_Management_System;";
            MySqlConnection connection = new MySqlConnection(connectionstring);
            string query = "SELECT * FROM Librarian";

            try
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand(query, connection);
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Console.WriteLine(
                        $"ID: {reader["librarian_ID"]}, " +
                        $"Name: {reader["first_name"]} {reader["last_name"]}, " +
                        $"Phone: {reader["phone_number"]}"
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
               // Console.WriteLine("Connection closed");
            }
        }


        private void UpdateLibrarian()
        {
            Console.WriteLine("Update Librarian (not implemented)");
        }
        private void DeleteLibrarian()
        {
            Console.WriteLine("Update Librarian (not implemented)");
        }
    }
}
