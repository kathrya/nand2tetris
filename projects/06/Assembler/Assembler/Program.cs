using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assembler
{
    class Program
    {
        private static string inFilePath;

        static void Main(string[] args)
        {
            inFilePath = @"C:\Users\Catherine-PC\Documents\GitHub\nand2tetris\projects\06\max\MaxL.asm";
            Parser parser = new Parser(inFilePath);
            parser.printAllLines();
            Console.ReadLine();
        }
    }
}
