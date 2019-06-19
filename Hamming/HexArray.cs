namespace Hamming
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    class HexArray
    {
        static string HexSymbols = "0123456789ABCDEF";

        static readonly Dictionary<char, int> HexMap = new Dictionary<char, int>
        {
            ['0'] = 0,
            ['1'] = 1,
            ['2'] = 2,
            ['3'] = 3,
            ['4'] = 4,
            ['5'] = 5,
            ['6'] = 6,
            ['7'] = 7,
            ['8'] = 8,
            ['9'] = 9,
            ['A'] = 10,
            ['B'] = 11,
            ['C'] = 12,
            ['D'] = 13,
            ['E'] = 14,
            ['F'] = 15,
        };

        public HexArray(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Строка должна быть непустой!");
            }

            value = value.ToUpperInvariant();

            if (!value.All(x => HexSymbols.Contains(x)))
            {
                throw new ArgumentException("Строка должна содержать только шестнадцатеричные символы!");
            }

            Array = value.Select(x => HexMap[x]).ToArray();
            String = value;
        }

        public HexArray(int[] array)
        {
            if (array is null)
            {
                throw new ArgumentException();
            }

            if (!array.All(x => x >= 0 && x < 16))
            {
                throw new ArgumentException();
            }

            Array = array;
            String = string.Join("", array.Select(x => HexSymbols[x]));
        }

        public int[] Array { get; }

        public string String { get; }
    }
}