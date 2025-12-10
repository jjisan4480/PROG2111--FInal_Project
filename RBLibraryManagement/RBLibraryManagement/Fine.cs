//
// FILE               : Fine.cs
// PROJECT            : RBLibraryManagement
// PROGRAMMER		  : Josiah Williams
// FIRST VERSION      : 2025-12-09
// DESCRIPTION        : This class handles CRUD operations for the Fine entity in the library management system.
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
            Console.WriteLine("\n");
            Console.WriteLine("CRUD Options\n");
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
            Console.WriteLine("\n--- Create Fine ---");

            Console.Write("Enter Borrow ID: ");
            int borrowId = int.Parse(Console.ReadLine() ?? "0");

            Console.Write("Enter fine amount: ");
            decimal amount = decimal.Parse(Console.ReadLine() ?? "0");

            Console.Write("Enter due date (yyyy-mm-dd): ");
            DateTime dueDate = DateTime.Parse(Console.ReadLine() ?? "2000-01-01");

            Console.Write("Enter paid date (yyyy-mm-dd) or leave blank: ");
            string? paid = Console.ReadLine();
            object paidDate = string.IsNullOrWhiteSpace(paid) ? DBNull.Value : DateTime.Parse(paid);

            MySqlConnection connection = new MySqlConnection(MainProgram.ConnectionString);

            try
            {
                connection.Open();
                string query = "SELECT * FROM Fines";
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection);

                DataSet ds = new DataSet();
                adapter.Fill(ds, "Fines");
                DataTable? table = ds.Tables["Fines"];
                if (table != null)
                {
                    DataRow row = table.NewRow();
                    row["borrow_ID"] = borrowId;
                    row["fine_amount"] = amount;
                    row["due_date"] = dueDate;
                    row["paid_date"] = paidDate;

                    table.Rows.Add(row);
                } else
                {
                  throw new Exception("Failed to load Fines table.");
                }

                    MySqlCommandBuilder builder = new MySqlCommandBuilder(adapter);
                adapter.Update(ds, "Fines");

                Console.WriteLine("Fine created.");
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

        private void ReadFines()
        {
            Console.WriteLine("\n--- Fines ---");

            MySqlConnection connection = new MySqlConnection(MainProgram.ConnectionString);
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
            Console.WriteLine("\n--- Update Fine ---");

            Console.Write("Enter Fine ID to update: ");
            if (!int.TryParse(Console.ReadLine(), out int id)) { Console.WriteLine("Invalid ID"); return; }

            Console.Write("Enter Date Paid (yyyy-mm-dd) or press Enter if unpaid: ");
            string dateInput = Console.ReadLine() ?? "";

            // Handle null date if they press enter
            object paidDate = string.IsNullOrWhiteSpace(dateInput) ? DBNull.Value : DateTime.Parse(dateInput);

            MySqlConnection connection = new MySqlConnection(MainProgram.ConnectionString);
            try
                {
                    connection.Open();
                    string query = "UPDATE Fines SET paid_date = @date WHERE fine_ID = @id";

                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@date", paidDate);
                    cmd.Parameters.AddWithValue("@id", id);

                    int rows = cmd.ExecuteNonQuery();
                    if (rows > 0) Console.WriteLine("Fine updated successfully.");
                    else Console.WriteLine("Fine ID not found.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            
        }
        private void DeleteFine()
        {
            Console.WriteLine("\n--- Delete Fine Record ---");

            Console.Write("Enter Fine ID to delete: ");
            if (!int.TryParse(Console.ReadLine(), out int id)) { Console.WriteLine("Invalid ID"); return; }

            Console.Write("Are you sure? (y/n): ");
            if (Console.ReadKey().KeyChar != 'y') return;

            using (MySqlConnection connection = new MySqlConnection(MainProgram.ConnectionString))
            {
                try
                {
                    connection.Open();
                    string query = "DELETE FROM Fines WHERE fine_ID = @id";

                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@id", id);

                    int rows = cmd.ExecuteNonQuery();
                    Console.WriteLine();

                    if (rows > 0) Console.WriteLine("Fine record deleted.");
                    else Console.WriteLine("Fine ID not found.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("\nError: " + ex.Message);
                }
            }
        }
    }
}
