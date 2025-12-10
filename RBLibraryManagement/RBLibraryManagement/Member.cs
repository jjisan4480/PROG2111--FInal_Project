//
// FILE               : Member.cs
// PROJECT            : RBLibraryManagement
// PROGRAMMER		  : Josiah Williams, Jobair Ahmed Jisan
// FIRST VERSION      : 2025-12-09
// DESCRIPTION        : This class handles CRUD operations for the Member entity in the library management system.
// 

using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Org.BouncyCastle.Math.EC.ECCurve;


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
            Console.WriteLine("\n");
            Console.WriteLine("CRUD Options\n");
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

            string first = "";
            while (string.IsNullOrWhiteSpace(first))
            {
                Console.Write("Enter first name: ");
                first = Console.ReadLine() ?? "";
            }

            string last = "";
            while (string.IsNullOrWhiteSpace(last))
            {
                Console.Write("Enter last name: ");
                last = Console.ReadLine() ?? "";
            }

            string email = "";
            while (string.IsNullOrWhiteSpace(email))
            {
                Console.Write("Enter email: ");
                email = Console.ReadLine() ?? "";
            }

            string phone = "";
            while (string.IsNullOrWhiteSpace(phone))
            {
                Console.Write("Enter phone: ");
                phone = Console.ReadLine() ?? "";
            }

            DateTime date;
            while (true)
            {
                Console.Write("Enter membership date (yyyy-mm-dd): ");
                if (DateTime.TryParse(Console.ReadLine(), out date)) break;
                Console.WriteLine("Invalid date format. Please use yyyy-mm-dd.");
            }

            MySqlConnection connection = new MySqlConnection(MainProgram.ConnectionString);

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
                    newRow["date"] = date;
                    
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

            MySqlConnection connection = new MySqlConnection(MainProgram.ConnectionString);
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
            Console.WriteLine("\n--- Update Member Contact Info ---");

            Console.Write("Enter Member ID to update: ");
            if (!int.TryParse(Console.ReadLine(), out int id)) { Console.WriteLine("Invalid ID"); return; }

            Console.Write("Enter new Email: ");
            string email = Console.ReadLine() ?? "";

            Console.Write("Enter new Phone: ");
            string phone = Console.ReadLine() ?? "";

            MySqlConnection connection = new MySqlConnection(MainProgram.ConnectionString);
            try
                {
                    connection.Open();
                    string query = "UPDATE Member SET email = @email, phone = @phone WHERE member_ID = @id";

                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.Parameters.AddWithValue("@phone", phone);
                    cmd.Parameters.AddWithValue("@id", id);

                    int rows = cmd.ExecuteNonQuery();
                    if (rows > 0) Console.WriteLine("Member updated successfully.");
                    else Console.WriteLine("Member ID not found.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            
        }
        private void DeleteMember()
        {
            Console.WriteLine("\n--- Delete Member ---");

            Console.Write("Enter Member ID to delete: ");
            if (!int.TryParse(Console.ReadLine(), out int id)) { Console.WriteLine("Invalid ID"); return; }

            Console.Write("Are you sure? (y/n): ");
            if (Console.ReadKey().KeyChar != 'y') return;

            using (MySqlConnection connection = new MySqlConnection(MainProgram.ConnectionString))
            {
                try
                {
                    connection.Open();
                    string query = "DELETE FROM Member WHERE member_ID = @id";

                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@id", id);

                    int rows = cmd.ExecuteNonQuery();
                    Console.WriteLine();

                    if (rows > 0) Console.WriteLine("Member deleted successfully.");
                    else Console.WriteLine("Member ID not found.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("\nError: " + ex.Message);
                    Console.WriteLine("Note: You cannot delete a member if they have active loans or fines.");
                }
            }
        }
    }
}
