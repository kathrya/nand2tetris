using System;
using System.IO;
using System.Collections.Generic;


namespace Assembler
{
    internal class Parser
    {
        public int currentCommand;

        private List<string> fileLines = new List<string>();
        private string currentLine;
        private string filepath;

        public Parser(string inFilePath)
        {
            filepath = inFilePath;
            string[] fileLineArray = File.ReadAllLines(inFilePath);
            foreach (string line in fileLineArray)
            {
                string newLine = line;
                for (int i = 0; i < line.Length - 1; i++)
                {
                   if (((line[i] == line[i+1]) && (line[i] == '/')))
                   {
                        newLine = newLine.Substring(0, i);
                   }
                   
                }
                newLine = newLine.Replace(" ", String.Empty);
                if (newLine.Length > 0)
                {
                    fileLines.Add(newLine);
                }
                currentCommand = 0;
            }
        }

        public bool HasMoreCommands()
        {
            return (currentCommand < fileLines.Count - 1);
        }

        public void Advance()
        {
            if (!HasMoreCommands())
            {
                throw new Exception("No more commands!");
            }
            currentCommand++;
            currentLine = fileLines[currentCommand];
        }

        public string CommandType()
        {
            if (currentLine[0] == '@')
            {
                return "A_COMMAND";
            }
            if (currentLine[0] == '(')
            {
                return "L_COMMAND";
            }
            return "C_COMMAND";
        }

        public string Symbol()
        {
            if (CommandType() == "A_COMMAND")
            {
                return currentLine.Substring(1, currentLine.Length-1);
            }
            else if (CommandType() == "L_COMMAND")
            {
                return currentLine.Substring(1, currentLine.Length - 2);
            }
            throw new Exception("Wrong command type");
        }

        public string Dest()
        {
            if (!(CommandType() == "C_COMMAND"))
            {
                throw new Exception("Wrong command type");
            }
            for (int i = 0; i < currentLine.Length - 1; i++)
            {
                if (currentLine[i] == '=')
                {
                    int equalsLocation = i;
                    string potentialReturn = currentLine.Substring(0, equalsLocation);
                    if ((potentialReturn == "M") || (potentialReturn == "D") || (potentialReturn == "MD") || (potentialReturn == "A") || (potentialReturn == "AM") || (potentialReturn == "AD") || (potentialReturn == "AMD"))
                    {
                        return potentialReturn;
                    }
                    throw new Exception("Incorrect destination");
                }
            }
            return "null";
        }

        public void printAllLines()
        {
            foreach (string line in fileLines)
            {
                Console.WriteLine(line);
            }
        }
    }
}