using System.Collections;
using System;
using System.IO;

namespace Assembler
{
    public class SymbolTable
    {
        private Hashtable symbolTable;

        public SymbolTable()
        {
            symbolTable = new Hashtable();
            AddEntry("SP", 0);
            AddEntry("LCL", 1);
            AddEntry("ARG", 2);
            AddEntry("THIS", 3);
            AddEntry("THAT", 4);
            AddEntry("R0", 0);
            AddEntry("R1", 1);
            AddEntry("R2", 2);
            AddEntry("R3", 3);
            AddEntry("R4", 4);
            AddEntry("R5", 5);
            AddEntry("R6", 6);
            AddEntry("R7", 7);
            AddEntry("R8", 8);
            AddEntry("R9", 9);
            AddEntry("R10", 10);
            AddEntry("R11", 11);
            AddEntry("R12", 12);
            AddEntry("R13", 13);
            AddEntry("R14", 14);
            AddEntry("R15", 15);
            AddEntry("SCREEN", 16384);
            AddEntry("KBD", 24576);
        }
        
        public void AddEntry(string symbol, int address)
        {
            try
            {
                symbolTable.Add(symbol, address);
            }
            catch(ArgumentException)
            {

            }
            
        }

        public bool Contains(string symbol)
        {
            return symbolTable.Contains(symbol);
        }

        public int GetAddress(string symbol)
        {
            return Convert.ToInt16(symbolTable[symbol]);
        }
    }
}