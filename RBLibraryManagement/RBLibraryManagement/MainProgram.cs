//
// FILE          : MainProgram.cs
// PROJECT       : RBLibraryManagement
// PROGRAMMER    : Josiah Williams, Jobair Ahmed Jisan
// FIRST VERSION : 2025-12-09
// DESCRIPTION   : This is the entry point of the application. It initializes the database configuration
//                 and launches the main database management menu.
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RBLibraryManagement;

internal class MainProgram
{
    public static string ConnectionString = "Server=localhost;Port=3306;Uid=root;Pwd=Jom@nEngineer2002;Database=Library_Management_System;";
    private static void Main(string[] args)
    {
        DataBaseManagement dbm = new DataBaseManagement();
        dbm.Run();
    }
}