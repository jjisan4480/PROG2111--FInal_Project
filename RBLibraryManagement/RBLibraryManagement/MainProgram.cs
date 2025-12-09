using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RBLibraryManagement;

internal class MainProgram
{
    private static void Main(string[] args)
    {
        DataBaseManagement dbm = new DataBaseManagement();
        dbm.Run();
    }
}