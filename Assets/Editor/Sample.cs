using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CreationStates
{
    class Program
    {
        static void Main(string[] args)
        {
            var projectFolder = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            String categoryPath = Path.Combine(projectFolder, @"config\categories.json");
            String configPath = Path.Combine(projectFolder, @"config\data.json");
            Database db = new Database(categoryPath, configPath); 
            Console.ReadLine();
        } 
    }
}
