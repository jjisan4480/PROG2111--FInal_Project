//
// FILE               : Book.cs
// PROJECT            : RBLibraryManagement
// PROGRAMMER		  : Josiah Williams, Jobair Ahmed Jisan
// FIRST VERSION      : 2025-12-09
// DESCRIPTION        : This class handles CRUD operations for the Book entity in the library management system.
// 
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Relational;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Org.BouncyCastle.Math.EC.ECCurve;

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
            Console.WriteLine("\n");
            Console.WriteLine("CRUD Options\n");
            Console.WriteLine("1. Create Book");
            Console.WriteLine("2. Read Book Table");
            Console.WriteLine("3. Update Book");
            Console.WriteLine("4. Delete Book");
            Console.WriteLine("5. Go back to Main Menu");
            Console.Write("Select an option (1-5): ");
            return Console.ReadKey(true).KeyChar;
        }

        //
        // METHOD      : CreateBook
        // DESCRIPTION : Collects user input for a new book (Title, ISBN, Price, etc.), validates the data,
        //               and inserts a new record into the 'Book' database table.
        // PARAMETERS  : None
        // RETURNS     : void
        //
        private void CreateBook()
        {
            Console.WriteLine("\n--- Create Book ---");

            string title = "";
            while (string.IsNullOrWhiteSpace(title))
            {
                Console.Write("Enter title: ");
                title = Console.ReadLine() ?? "";
            }

            int isbn;
            while (true)
            {
                Console.Write("Enter ISBN (numbers only): ");
                if (int.TryParse(Console.ReadLine(), out isbn)) break;
                Console.WriteLine("Invalid input. Please enter a valid number for ISBN.");
            }

            decimal price;
            while (true)
            {
                Console.Write("Enter price: ");
                if (decimal.TryParse(Console.ReadLine(), out price)) break;
                Console.WriteLine("Invalid price. Please enter a decimal value (e.g. 19.99).");
            }

            int authorId;
            while (true)
            {
                Console.Write("Enter Author ID: ");
                if (int.TryParse(Console.ReadLine(), out authorId)) break;
                Console.WriteLine("Invalid ID. Please enter a number.");
            }

            DateTime pubDate;
            while (true)
            {
                Console.Write("Enter publishing date (yyyy-mm-dd): ");
                string input = Console.ReadLine() ?? "";
                // If empty, default to year 2000 
                if (string.IsNullOrWhiteSpace(input)) { pubDate = new DateTime(2000, 1, 1); break; }

                if (DateTime.TryParse(input, out pubDate)) break;
                Console.WriteLine("Invalid date. Format: yyyy-mm-dd");
            }

            bool status;
            while (true)
            {
                Console.Write("Is the book available?: ");
                if (bool.TryParse(Console.ReadLine(), out status)) break;
                Console.WriteLine("Please type '0' or '1'.");
            }

            string genre = "";
            while (string.IsNullOrWhiteSpace(genre))
            {
                Console.Write("Enter genre: ");
                genre = Console.ReadLine() ?? "";
            }

            MySqlConnection connection = new MySqlConnection(MainProgram.ConnectionString);

            try
            {
                connection.Open();
                string query = "SELECT * FROM Book";
                MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection);

                DataSet ds = new DataSet();
                adapter.Fill(ds, "Book");
               
                DataTable? table = ds.Tables["Book"];
                if (table != null)
                {
                    DataRow row = table.NewRow();
                    row["title"] = title;
                    row["ISBN"] = isbn;
                    row["price"] = price;
                    row["author_ID"] = authorId;
                    row["publishing_date"] = pubDate;
                    row["book_status"] = status;
                    row["genre"] = genre;

                    table.Rows.Add(row);
                }
                else
                {
                    throw new Exception("Failed to load Books table");
                }

                    MySqlCommandBuilder builder = new MySqlCommandBuilder(adapter);
                adapter.Update(ds, "Book");

                Console.WriteLine("Book created successfully.");
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


        //
        // METHOD      : ReadBooks
        // DESCRIPTION : Connects to the database, retrieves all records from the 'Book' table,
        //               and displays them to the console in a readable format.
        // PARAMETERS  : None
        // RETURNS     : void
        //
        private void ReadBooks()
        {
            Console.WriteLine("\n--- Book List ---");

            MySqlConnection connection = new MySqlConnection(MainProgram.ConnectionString);
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


        //
        // METHOD      : UpdateBook
        // DESCRIPTION : Prompts the user for a Book ID and new details (Price, Status), then updates
        //               the corresponding record in the database using a parameterized SQL query.
        // PARAMETERS  : None
        // RETURNS     : void
        //
        private void UpdateBook()
        {
            Console.WriteLine("\n--- Update Book ---");

            Console.Write("Enter Book ID to update: ");
            if (!int.TryParse(Console.ReadLine(), out int id)) { Console.WriteLine("Invalid ID"); return; }

            Console.Write("Enter new Price: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal price)) { Console.WriteLine("Invalid Price"); return; }

            Console.Write("Is the book available? (true/false): ");
            if (!bool.TryParse(Console.ReadLine(), out bool status)) { Console.WriteLine("Invalid Status"); return; }

            MySqlConnection connection = new MySqlConnection(MainProgram.ConnectionString);
            try
                {
                    connection.Open();
                    string query = "UPDATE Book SET price = @price, book_status = @status WHERE book_ID = @id";

                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@price", price);
                    cmd.Parameters.AddWithValue("@status", status);
                    cmd.Parameters.AddWithValue("@id", id);

                    int rows = cmd.ExecuteNonQuery();
                    if (rows > 0) Console.WriteLine("Book updated successfully.");
                    else Console.WriteLine("Book ID not found.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            
        }

        //
        // METHOD      : DeleteBook
        // DESCRIPTION : Prompts the user for a Book ID and confirms deletion. If confirmed, it removes
        //               the record from the database.
        // PARAMETERS  : None
        // RETURNS     : void
        //
        private void DeleteBook()
        {
            Console.WriteLine("\n--- Delete Book ---");

            Console.Write("Enter Book ID to delete: ");
            if (!int.TryParse(Console.ReadLine(), out int id)) { Console.WriteLine("Invalid ID"); return; }

            Console.Write("Are you sure? (y/n): ");
            if (Console.ReadKey().KeyChar != 'y') return;

            using (MySqlConnection connection = new MySqlConnection(MainProgram.ConnectionString))
            {
                try
                {
                    connection.Open();
                    string query = "DELETE FROM Book WHERE book_ID = @id";

                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@id", id);

                    int rows = cmd.ExecuteNonQuery();
                    Console.WriteLine();

                    if (rows > 0) Console.WriteLine("Book deleted successfully.");
                    else Console.WriteLine("Book ID not found.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("\nError: " + ex.Message);
                }
            }
        }
    }
}
