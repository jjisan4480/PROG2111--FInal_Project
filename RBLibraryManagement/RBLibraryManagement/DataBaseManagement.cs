using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RBLibraryManagement
{
    public class DataBaseManagement
    {
        public void Run()
        {
            bool run = true;    
               while (run)
            {
                ConsoleKeyInfo choice = DatabaseMenu();
                switch(choice.KeyChar)
                {
                    case '1':
                        Member member = new Member();
                        member.Run();
                        break;

                    case '2':
                        Book book = new Book();
                        book.Run();
                        break;

                    case '3':
                        Borrow borrow = new Borrow();
                        borrow.Run();
                        break;

                    case '4':
                        Librarian librarian = new Librarian();
                        librarian.Run();
                        break;

                    case '5':
                        Fine fine = new Fine();
                        fine.Run();
                        break;

                    case '6':
                        Author author = new Author();
                        author.Run();
                        break;
                    default:
                        
                        Console.WriteLine("\nInvalid choice. Try again.\n");
                        
                        break;
                }
            }

        }


        public ConsoleKeyInfo DatabaseMenu()
        {
            Console.WriteLine("\n");
            Console.WriteLine("Database Management Menu\n");
            Console.WriteLine("1. Member Table");
            Console.WriteLine("2. Book Table");
            Console.WriteLine("3. Borrow Table");
            Console.WriteLine("4. Librarian Table");
            Console.WriteLine("5. Fine Table");
            Console.WriteLine("6. Author Table");
            Console.Write("Select an option (1-6): ");
            return Console.ReadKey();
        }

    }
}
