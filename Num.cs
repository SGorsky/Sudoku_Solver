using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku_Solver
{
    class Num
    {
        public List<byte> possibilities = new List<byte>();

        byte value;
        public byte Value
        {
            get { return value; }
            set { this.value = value; }
        }

        public Num(string input)
        {
            if (input == String.Empty || input == "0")
            {
                value = 0;
                possibilities = new List<byte>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            }
            else
            {
                value = Convert.ToByte(input);
            }
        }

        public override string ToString()
        {
            if (value != 0)
            {
                return value.ToString();
            }
            else
            {
                return String.Empty;
            }
        }
    }
}
