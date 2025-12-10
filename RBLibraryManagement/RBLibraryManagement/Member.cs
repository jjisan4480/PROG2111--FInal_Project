//
// FILE               : Member.cs
// PROJECT            : RBLibraryManagement
// PROGRAMMER		  : Josiah Williams
// FIRST VERSION      : 2025-12-09
// DESCRIPTION        : This class handles CRUD operations for the Member entity in the library management system.
// 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;


namespace RBLibraryManagement
{
    internal class Member
    {

       
        public void Run()
        {
            bool stay = true;

            while (stay)
            {
                char choice = MemberMenu();

                switch (choice)
                {
                    case '1':
                        CreateMember();
                        break;

                    case '2':
                        ReadMembers();
                        break;

                    case '3':
                        UpdateMember();
                        break;

                    case '4':
                        DeleteMember();
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

        public char MemberMenu()
        {
            Console.WriteLine("\nCRUD Options");
            Console.WriteLine("1. Create Member");
            Console.WriteLine("2. Read Member Table");
            Console.WriteLine("3. Update Member");
            Console.WriteLine("4. Delete Member");
            Console.WriteLine("5. Go back to Main Menu");
            Console.Write("Select an option (1-5): ");
            return Console.ReadKey(true).KeyChar;
        }

        private void CreateMember()
        {
            Console.WriteLine("\n--- Create Member ---");

            Console.Write("Enter first name: ");
            string? first = Console.ReadLine();

            Console.Write("Enter last name: ");
            string? last = Console.ReadLine();

            Console.Write("Enter email: ");
            string? email = Console.ReadLine();

            Console.Write("Enter phone: ");
            string? phone = Console.ReadLine();

            Console.Write("Enter membership date (yyyy-mm-dd): ");
            string? date = Console.ReadLine();

            string connString = "Server=localhost;Port=3306;Uid=root;Pwd=root;Database=Library_Management_System;";
            MySqlConnection connection = new MySqlConnection(connString);

            try
            {
                connection.Open();

                // 1. Load Member table into DataSet
                string query = "SELECT * FROM Member";
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection);

                DataSet ds = new DataSet();
                adapter.Fill(ds, "Member");

                DataTable? table = ds.Tables["Member"];
                if(table != null)
                {
                    DataRow newRow = table.NewRow();
                    newRow["first_name"] = first;
                    newRow["last_name"] = last;
                    newRow["email"] = email;
                    newRow["phone"] = phone;
                    if (!DateTime.TryParse(date, out DateTime parsedDate))
                    {
                        throw new Exception("Invalid date format. Use yyyy-mm-dd.");
                    }
                    table.Rows.Add(newRow);

                }
                else
                {
                 throw new Exception("Failed to load Member table.");
                }




                    MySqlCommandBuilder builder = new MySqlCommandBuilder(adapter);


                adapter.Update(ds, "Member");

                Console.WriteLine("Member successfully created!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                    //Console.WriteLine("Connection closed.");
                }
            }

        }
        private void ReadMembers()
        {
            Console.WriteLine("\n--- Member List ---");

            string connectionstring = "Server=localhost;Port=3306;Uid=root;Pwd=root;Database=Library_Management_System;";
            MySqlConnection connection = new MySqlConnection(connectionstring);
            string query = "SELECT * FROM Member";

            try
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand(query, connection);
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Console.WriteLine(
                        $"ID: {reader["member_ID"]}, " +
                        $"Name: {reader["first_name"]} {reader["last_name"]}, " +
                        $"Email: {reader["email"]}, " +
                        $"Phone: {reader["phone"]}, " +
                        $"Membership Date: {reader["membership_date"]}"
                    );
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                    //Console.WriteLine("Connection closed.");
                }
            }
        }

        private void UpdateMember()
        {
            Console.WriteLine("Update Member (not implemented)");
        }
        private void DeleteMember()
        {
            Console.WriteLine("Update Member (not implemented)");
        }
    }
}
