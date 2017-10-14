using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace Assembler
{
    internal class Parser
    {
        public int currentCommand;
        
        private List<string> fileLines = new List<string>();
        private string currentLine;
        private string filepath;
        private int ROMAddress;
        private int labelCounter;
        private SymbolTable symbolTable;

        public Parser(string inFilePath)
        {
            symbolTable = new SymbolTable();
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
            }
            FirstPass();
            currentCommand = 0;
            currentLine = fileLines[currentCommand];
        }

        public bool HasMoreCommands()
        {
            return (currentCommand < fileLines.Count - 1);
        }

        public bool HasCommand()
        {
            return (currentCommand < fileLines.Count);
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
                return HandleASymbol();
            }
            else if (CommandType() == "L_COMMAND")
            {
                return HandleLSymbol();
            }
            throw new Exception("Wrong command type");
        }

        private string HandleASymbol()
        {
            string commandTarget = currentLine.Substring(1, currentLine.Length - 1);
            bool isNumeric = int.TryParse(commandTarget, out int testInt);
            if (isNumeric)
            {
                return commandTarget;
            }
            if (symbolTable.Contains(commandTarget))
            {
                return Convert.ToString(symbolTable.GetAddress(commandTarget));
            }
            symbolTable.AddEntry(commandTarget, labelCounter);
            labelCounter++;
            return Convert.ToString(symbolTable.GetAddress(commandTarget));
        }

        private string HandleLSymbol()
        {
            string commandTarget = currentLine.Substring(1, currentLine.Length - 2);
            if (symbolTable.Contains(commandTarget))
            {
                return Convert.ToString(symbolTable.GetAddress(commandTarget));
            }
            return commandTarget;
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

        public string Comp()
        {
            if (!(CommandType() == "C_COMMAND"))
            {
                throw new Exception("Wrong command type");
            }
            if (Dest() == "null")
            {
                for (int i = 0; i < currentLine.Length - 1; i++)
                {
                    if (currentLine[i] == ';')
                    {
                        if (!CompCorrectness(currentLine.Substring(0, i)))
                        {
                            throw new Exception("Invalid command");
                        }
                        return currentLine.Substring(0, i);
                    }
                }
                if (!CompCorrectness(currentLine))
                {
                    throw new Exception("Invalid command");
                }
                return currentLine;
            }
            for (int i = 0; i < currentLine.Length - 1; i++)
            {
                if (currentLine[i] == '=')
                {
                    for (int j = 0; j < currentLine.Length - 1; j++)
                    {
                        if (currentLine[j] == ';')
                        {
                            if (!CompCorrectness(currentLine.Substring(i+1, j-i-1)))
                            {
                                throw new Exception("Invalid command");
                            }
                            return currentLine.Substring(i+1, j-i-1);
                        }
                    }
                    if (!CompCorrectness(currentLine.Substring(i+1, currentLine.Length-i-1)))
                    {
                        throw new Exception("Invalid command");
                    }
                    return currentLine.Substring(i+1, currentLine.Length-i-1);
                }
            }
            return "null";
        }

        public string Jump()
        {
            if (!(CommandType() == "C_COMMAND"))
            {
                throw new Exception("Wrong command type");
            }
            for (int i = 0; i < currentLine.Length - 1; i++)
            {
                if (currentLine[i] == ';')
                {
                    int semiColonLocation = i;
                    string potentialReturn = currentLine.Substring(semiColonLocation+1, currentLine.Length-semiColonLocation-1);
                    String[] correctReturns = {"JGT", "JEQ", "JGE", "JLT", "JNE", "JLE", "JMP"};
                    if (correctReturns.Any(potentialReturn.Equals))
                    {
                        return potentialReturn;
                    }
                    throw new Exception("Incorrect JMP command");
                }
            }
            return "null";
        }

        public void ResetCounter()
        {
            currentCommand = 0;
            currentLine = fileLines[currentCommand];
        }

        public void printAllLines()
        {
            foreach (string line in fileLines)
            {
                Console.WriteLine(line);
            }
        }

        private bool CompCorrectness(string potentialreturn)
        {
            string[] correctReturns = { "0", "1" , "-1", "D", "A", "!D", "!A", "-D", "-A", "D+1", "A+1", "D-1", "A-1", "D+A", "A+D", "D-A", "A-D", "D&A", "D|A", "M", "!M", "-M", "M+1", "M-1", "D+M", "M+D", "D-M", "M-D", "D&M", "D|M"};
            return correctReturns.Any(potentialreturn.Equals);
        }

        private void FirstPass()
        {
            labelCounter = 16;
            ROMAddress = 0;
            currentCommand = 0;
            currentLine = fileLines[currentCommand];
            while (HasMoreCommands())
            {
                if (currentLine == "(ponggame.0)")
                {

                }
                if (CommandType() == "L_COMMAND")
                {
                    // Check/Add Symbol to the Hash Table
                    if (!symbolTable.Contains(HandleLSymbol()))
                    {
                        symbolTable.AddEntry(HandleLSymbol(), ROMAddress);
                    }
                }
                else
                {
                    ROMAddress++;
                }
                Advance();
            }
        }
    }
}