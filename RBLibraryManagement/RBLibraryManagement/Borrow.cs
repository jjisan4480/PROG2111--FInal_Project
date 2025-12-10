//
// FILE               : Borrow.cs
// PROJECT            : RBLibraryManagement
// PROGRAMMER		  : Josiah Williams
// FIRST VERSION      : 2025-12-09
// DESCRIPTION        : This class handles CRUD operations for the Borrow entity in the library management system.
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
            Console.WriteLine("\n");
            Console.WriteLine("CRUD Options\n");
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
            Console.WriteLine("\n--- Create Borrow Record ---");

            Console.Write("Enter Book ID: ");
            int bookId = int.Parse(Console.ReadLine() ?? "0");

            Console.Write("Enter Member ID: ");
            int memberId = int.Parse(Console.ReadLine() ?? "0");

            Console.Write("Enter Librarian ID: ");
            int libId = int.Parse(Console.ReadLine() ?? "0");

            Console.Write("Enter borrow date (yyyy-mm-dd): ");
            DateTime borrowDate = DateTime.Parse(Console.ReadLine() ?? "2000-01-01");

            Console.Write("Enter return date (yyyy-mm-dd): ");
            DateTime returnDate = DateTime.Parse(Console.ReadLine() ?? "2000-01-01");

            MySqlConnection connection = new MySqlConnection(MainProgram.ConnectionString);

            try
            {
                connection.Open();
                string query = "SELECT * FROM Borrow";
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection);

                DataSet ds = new DataSet();
                adapter.Fill(ds, "Borrow");
                DataTable? table = ds.Tables["Borrow"];
                if (table != null)
                {
                    DataRow row = table.NewRow();
                    row["book_ID"] = bookId;
                    row["member_ID"] = memberId;
                    row["issuedByLibrarian"] = libId;
                    row["borrowing_date"] = borrowDate;
                    row["return_date"] = returnDate;

                    table.Rows.Add(row);
                } else
                {
                    throw new Exception("Failed to load Borrow table");
                }

                    MySqlCommandBuilder builder = new MySqlCommandBuilder(adapter);
                adapter.Update(ds, "Borrow");

                Console.WriteLine("Borrow record added.");
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

        
      
           private void ReadBorrows()
        {
            Console.WriteLine("\n--- Borrow Records ---");

            MySqlConnection connection = new MySqlConnection(MainProgram.ConnectionString);
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
            Console.WriteLine("\n--- Update Borrow Record (Return Book) ---");

            Console.Write("Enter Borrow ID to update: ");
            if (!int.TryParse(Console.ReadLine(), out int id)) { Console.WriteLine("Invalid ID"); return; }

            Console.Write("Enter Actual Return Date (yyyy-mm-dd): ");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime returnDate))
            {
                Console.WriteLine("Invalid Date");
                return;
            }

            MySqlConnection connection = new MySqlConnection(MainProgram.ConnectionString);
            try
                {
                    connection.Open();
                    string query = "UPDATE Borrow SET return_date = @date WHERE borrow_ID = @id";

                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@date", returnDate);
                    cmd.Parameters.AddWithValue("@id", id);

                    int rows = cmd.ExecuteNonQuery();
                    if (rows > 0) Console.WriteLine("Borrow record updated.");
                    else Console.WriteLine("Borrow ID not found.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            
        }
        
    }
}
