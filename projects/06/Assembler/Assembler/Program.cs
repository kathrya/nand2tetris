using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Assembler
{
    class Program
    {
        private static string FilePath;
        private static string inFile;
        private static string inFilePathFull;
        private static string outFile;
        private static string outFilePathFull;
        private static List<String> outputLines;
        private static int workingLine;
        private static int ROMLine;
        private static Parser parser;
        private static Code code;

        static void Main(string[] args)
        {
            FilePath = @"C:\Users\Catherine-PC\Documents\GitHub\nand2tetris\projects\06\pong\";
            inFile = "Pong.asm";
            outFile = "Pong1.hack";
            inFilePathFull = FilePath+inFile;
            outFilePathFull = FilePath + outFile;
            parser = new Parser(inFilePathFull);
            code = new Code();
            workingLine = 0;
            ROMLine = 0;
            outputLines = new List<string>();
            while (parser.HasMoreCommands())
            {
                ProcessCommands();
                workingLine++;
                parser.Advance();
            }
            // Still need to run the last command
            ProcessCommands();
            File.WriteAllLines(outFilePathFull, outputLines);
            Console.ReadLine();
        }

        private static void ProcessCommands()
        {
            if (parser.CommandType() == "C_COMMAND")
            {
                outputLines.Add("");
                outputLines[ROMLine] = outputLines[ROMLine] + "111";
                outputLines[ROMLine] = outputLines[ROMLine] + code.Comp(parser.Comp());
                outputLines[ROMLine] = outputLines[ROMLine] + code.Dest(parser.Dest());
                outputLines[ROMLine] = outputLines[ROMLine] + code.Jump(parser.Jump());
                ROMLine++;
            }
            if (parser.CommandType() == "A_COMMAND")
            {
                outputLines.Add("");
                int ATargetInt = Convert.ToInt16(parser.Symbol());
                string ATargetStr = Convert.ToString(ATargetInt, 2).PadLeft(16, '0');
                outputLines[ROMLine] = outputLines[ROMLine] + ATargetStr;
                ROMLine++;
            }
        }
    }
}
