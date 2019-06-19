namespace Hamming
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Security.Principal;

    static class RSCoder
    {

        const int Generator = 2;


        const int GaluaSize = 16;


        const int K = 7;


        const int N = 15;


        const int R = N - K;

        const int GaluaZero = int.MinValue;

        /// <summary>
        ///     Соответствие кодовых символов степени элемента поля Галуа
        /// </summary>
        static readonly Dictionary<int, int> CodeToPowerMap = new Dictionary<int, int>
        {
            [0b0000] = GaluaZero,
            [0b1000] = 0,
            [0b0100] = 1,
            [0b0010] = 2,
            [0b0001] = 3,
            [0b1100] = 4,
            [0b0110] = 5,
            [0b0011] = 6,
            [0b1101] = 7,
            [0b1010] = 8,
            [0b0101] = 9,
            [0b1110] = 10,
            [0b0111] = 11,
            [0b1111] = 12,
            [0b1011] = 13,
            [0b1001] = 14,
        };

        /// <summary>
        ///     Соответствие кодовых символов степени элемента поля Галуа
        /// </summary>
        static readonly Dictionary<int, int> PowerToCodeMap = new Dictionary<int, int>
        {
            [GaluaZero] = 0b0000,
            [0] = 0b1000,
            [1] = 0b0100,
            [2] = 0b0010,
            [3] = 0b0001,
            [4] = 0b1100,
            [5] = 0b0110,
            [6] = 0b0011,
            [7] = 0b1101,
            [8] = 0b1010,
            [9] = 0b0101,
            [10] = 0b1110,
            [11] = 0b0111,
            [12] = 0b1111,
            [13] = 0b1011,
            [14] = 0b1001,
        };

        public static readonly int[,] AddTable;
        public static readonly int[,] MultTable;
        public static readonly int[,] DivTable;
        public static readonly int[,] GeneratorMatrix;
        public static readonly int[,] ParityCheckMatrix;
        public static readonly int[,] DecoderMatrix;
        public static readonly int[,] EncoderMatrix;

        static RSCoder()
        {
            AddTable = new int[GaluaSize - 1, GaluaSize - 1];
            MultTable = new int[GaluaSize - 1, GaluaSize - 1];
            DivTable = new int[GaluaSize - 1, GaluaSize - 1];

            for (var i = 0; i < GaluaSize - 1; i++)
            {
                for (var j = 0; j < GaluaSize - 1; j++)
                {
                    var firstCode = PowerToCodeMap[i];
                    var secondCode = PowerToCodeMap[j];
                    var addResult = firstCode ^ secondCode;
                    var addPower = CodeToPowerMap[addResult];
                    AddTable[i, j] = addPower;

                    var multPower = (i + j) % (GaluaSize - 1);
                    MultTable[i, j] = multPower;

                    var divPower = i - j;
                    if (i - j < 0)
                    {
                        divPower += GaluaSize - 1;
                    }

                    divPower %= GaluaSize - 1;

                    DivTable[i, j] = divPower;
                }
            }

            GeneratorMatrix = new int[K, N];
            for (var k = 0; k < K; k++)
            {
                for (var n = 0; n < N; n++)
                {
                    var resultPower = (k * n) % (GaluaSize - 1);
                    GeneratorMatrix[k, n] = resultPower;
                }
            }

            ParityCheckMatrix = new int[R, N];
            for (var r = 0; r < R; r++)
            {
                for (var n = 0; n < N; n++)
                {
                    var power = (GaluaSize - 1) - (r + K) * n % (GaluaSize - 1);
                    power = power % (GaluaSize - 1);
                    ParityCheckMatrix[r, n] = power;
                }
            }

            DecoderMatrix = new int[N, N];
            for (var i = 0; i < N; i++)
            {
                for (var j = 0; j < N; j++)
                {
                    var power = (GaluaSize - 1) - i * j % (GaluaSize - 1);
                    power = power % (GaluaSize - 1);
                    DecoderMatrix[i, j] = power;
                }
            }

            EncoderMatrix = new int[N, N];
            for (var i = 0; i < N; i++)
            {
                for (var j = 0; j < N; j++)
                {
                    var resultPower = (i * j) % (GaluaSize - 1);
                    EncoderMatrix[i, j] = resultPower;
                }
            }

            using (var f = File.OpenWrite(Environment.CurrentDirectory+"\\add.txt"))
            {
                using (TextWriter writer = new StreamWriter(f))
                {
                    for (var i = 0; i < GaluaSize - 1; i++)
                    {
                        for (var j = 0; j < GaluaSize - 1; j++)
                        {
                            writer.Write("{0,12}", AddTable[i, j]);
                        }

                        writer.WriteLine();
                    }

                    writer.Close();
                }
            }

            using (var f = File.OpenWrite(Environment.CurrentDirectory + "\\mul.txt"))
            {
                using (TextWriter writer = new StreamWriter(f))
                {
                    for (var i = 0; i < GaluaSize - 1; i++)
                    {
                        for (var j = 0; j < GaluaSize - 1; j++)
                        {
                            writer.Write("{0,3}", MultTable[i, j]);
                        }

                        writer.WriteLine();
                    }

                    writer.Close();
                }
            }

            using (var f = File.OpenWrite(Environment.CurrentDirectory + "\\generator.txt"))
            {
                using (TextWriter writer = new StreamWriter(f))
                {
                    for (var i = 0; i < K; i++)
                    {
                        for (var j = 0; j < N; j++)
                        {
                            writer.Write("{0,3}", GeneratorMatrix[i, j]);
                        }

                        writer.WriteLine();
                    }

                    writer.Close();
                }
            }

            using (var f = File.OpenWrite(Environment.CurrentDirectory + "\\check.txt"))
            {
                using (TextWriter writer = new StreamWriter(f))
                {
                    for (var i = 0; i < R; i++)
                    {
                        for (var j = 0; j < N; j++)
                        {
                            writer.Write("{0,3}", ParityCheckMatrix[i, j]);
                        }

                        writer.WriteLine();
                    }

                    writer.Close();
                }
            }

            using (var f = File.OpenWrite(Environment.CurrentDirectory + "\\decoder.txt"))
            {
                using (TextWriter writer = new StreamWriter(f))
                {
                    for (var i = 0; i < N; i++)
                    {
                        for (var j = 0; j < N; j++)
                        {
                            writer.Write("{0,3}", DecoderMatrix[i, j]);
                        }

                        writer.WriteLine();
                    }

                    writer.Close();
                }
            }
        }

        public static int[] Encode(int[] source)
        {
            if (source.Length != K)
            {
                throw new ArgumentException($"Длина сообщения должна быть равна {K}!");
            }

            var galuaSource = source.Select(x => CodeToPowerMap[x]).ToArray();

            var result = new int[N];

            for (var n = 0; n < N; n++)
            {
                result[n] = GaluaZero;

                for (var k = 0; k < K; k++)
                {
                    var fieldElement = galuaSource[k];
                    var current = Mult(fieldElement, GeneratorMatrix[k, n]);
                    result[n] = Add(result[n], current);
                }
            }

            var encoded = result.Select(x => PowerToCodeMap[x]).ToArray();
            return encoded;
        }

        public static int[] Decode(int[] code, bool[] erased)
        {
            if (code.Length != N)
            {
                throw new ArgumentException($"Длина закодированного сообщения должна быть равна {N}!");
            }

            if (erased.Length != N)
            {
                throw new ArgumentException($"Длина массива признаков стёртости должна быть равна {N}!");
            }

            var galuaCode = code.Select(x => CodeToPowerMap[x]).ToArray();

            // Если есть стёртые символы - пытаемся их восстановить
            if (erased.Any(x => x))
            {
                // Количество стёртых символов
                var erasedCount = erased.Count(x => x);
                if (erasedCount > R)
                {
                    throw new ArgumentException($"Невозможно восстановить более {R} стёртых символов!");
                }

                var erasedIndices = new List<int>();
                for (var i = 0; i < N; i++)
                {
                    if (erased[i])
                    {
                        erasedIndices.Add(i);
                    }
                }

                var solveMatrix = new int[erasedCount, erasedCount + 1];
                for (var i = 0; i < erasedCount; i++)
                {
                    var remainder = GaluaZero;
                    var j = 0;
                    for (var k = 0; k < N; k++)
                    {
                        if (erasedIndices.Contains(k))
                        {
                            solveMatrix[i, j++] = ParityCheckMatrix[i, k];
                        }
                        else
                        {
                            remainder = Add(remainder, Mult(ParityCheckMatrix[i, k], galuaCode[k]));
                        }
                    }

                    solveMatrix[i, j] = remainder;
                }

                var recovered = GaussSolve(solveMatrix);

                // Восстанавливаем стёртые значения
                for (int i = 0; i < erasedCount; i++)
                {
                    galuaCode[erasedIndices[i]] = recovered[i];
                }
            }

            // Восстановили стёртые символы, ищем ошибки

            // Синдром
            var syndrome = new int[R];
            for (var r = 0; r < R; r++)
            {
                syndrome[r] = GaluaZero;

                for (var n = 0; n < N; n++)
                {
                    var current = Mult(galuaCode[n], ParityCheckMatrix[r, n]);
                    syndrome[r] = Add(syndrome[r], current);
                }
            }

            // Если ошибок нет - декодируем сообщение
            if (syndrome.Any(x => x != GaluaZero))
            {
                var errorPalindrome = new[] { 0, GaluaZero, GaluaZero, GaluaZero, GaluaZero };


                var teplitz = new int[errorPalindrome.Length, errorPalindrome.Length];

                for (int i = 0; i < errorPalindrome.Length; i++)
                {
                    for (int j = 0; j < errorPalindrome.Length; j++)
                    {
                        var number = errorPalindrome.Length - 1 - i + j;
                        if (number < syndrome.Length)
                        {
                            teplitz[i, j] = syndrome[number];
                        }
                        else
                        {
                            teplitz[i, j] = GaluaZero;
                        }
                    }
                }

                var gaussMatrix = new int[R / 2, R / 2 + 1];
                for (int i = 0; i < R / 2; i++)
                {
                    for (int j = 0; j < R / 2; j++)
                    {
                        gaussMatrix[i, j] = teplitz[j + 1, i];
                    }

                    // Свободный член
                    gaussMatrix[i, R / 2] = teplitz[0, i];
                }

                var errorPolyCoeffs = GaussSolve(gaussMatrix);

                // Заносим значения в полиндром, описывающий позиции ошибок
                for (int i = 0; i < R / 2; i++)
                {
                    errorPalindrome[i + 1] = errorPolyCoeffs[i];
                }

                int ErrorLocator(int power)
                {
                    var res = 0;

                    for (int i = 1; i < errorPalindrome.Length; i++)
                    {
                        // Вычисляем степень
                        var current = power;
                        for (int j = 0; j < i - 1; j++)
                        {
                            current = Mult(current, power);
                        }

                        // Умножаем на коэффициент
                        var value = Mult(errorPalindrome[i], current);
                        res = Add(res, value);
                    }

                    return res;
                }

                // Ищем позиции ошибок - методом перебора
                var errorPositions = new List<int>();

                for (int i = 0; i < GaluaSize - 1; i++)
                {
                    var errorPalindromeValue = ErrorLocator(i);

                    // Нашли корень
                    if (errorPalindromeValue == GaluaZero)
                    {
                        errorPositions.Add(i);
                    }
                }

                // Имеем позиции ошибок в виде степени элемента поля Галуа, т. е. 0 - ошибка в первом символе, 14 - в последнем
                var errorsCount = errorPositions.Count;
                /*
                int SyndromePoly(int x)
                {
                    int res = GaluaZero;

                    var elem = 0;

                    for (int i = 0; i < R; i++)
                    {
                        res = Add(res, Mult(syndrome[i], elem));
                        elem = Mult(elem, x);
                    }

                    return res;
                }

                var syndromeMatrix = new int[errorsCount, errorsCount];
                for (int i = 0; i < errorsCount; i++)
                {
                    for (int j = 0; j < errorsCount; j++)
                    {
                        if (i > j)
                        {
                            syndromeMatrix[i, j] = GaluaZero;
                            continue;
                        }

                        var index = j - i;
                        syndromeMatrix[i, j] = syndrome[index];
                    }
                }

                // Оценка ошибок
                var errorsEvaluate = new int[errorsCount];
                for (int i = 0; i < errorsCount; i++)
                {

                }
                */
                /*
                int ErrorEvaluator(int x)
                {
                    var res = Mult(ErrorLocator(x), SyndromePoly(x));
                }*/

                // Строим матрицу для поиска значений ошибок
                var errorValuesMatrix = new int[errorsCount, errorsCount + 1];
                for (int i = 0; i < errorsCount; i++)
                {
                    for (int j = 0; j < errorsCount; j++)
                    {
                        var elem = errorPositions[j];
                        for (int k = 0; k < i; k++)
                        {
                            elem = Mult(elem, errorPositions[j]);
                        }

                        errorValuesMatrix[i, j] = elem;
                    }

                    errorValuesMatrix[i, errorsCount] = syndrome[i];
                }

                // Находим значения ошибок
                var errorValues = GaussSolve(errorValuesMatrix);

                // Исправляем кодовое слово
                for (int i = 0; i < errorsCount; i++)
                {
                    galuaCode[errorPositions[i]] = Add(galuaCode[errorPositions[i]], errorValues[i]);
                }

                /*
                // Строим систему уравнений, чтобы найти значения
                var valuesMatrix = new int[errorPositions.Count, errorPositions.Count + 1];
                for (int i = 0; i < errorPositions.Count; i++)
                {
                    for (int j = 0; j < errorPositions.Count; j++)
                    {
                        if (i > j)
                        {
                            valuesMatrix[i, j] = GaluaZero;
                            continue;
                        }

                        var inverseColumn = errorPositions.Count - j;
                        var palindromeCoeffIndex = i + inverseColumn;
                        valuesMatrix[i, j] = errorPalindrome[palindromeCoeffIndex];
                    }

                    var freeMember = GaluaZero;
                    for (int j = 0, k = i; j <= i && k >= 0; j++, k--)
                    {
                        var current = Mult(syndrome[j], errorPalindrome[k]);
                        freeMember = Add(freeMember, current);
                    }

                    valuesMatrix[i, errorPositions.Count] = freeMember;
                }

                // Находим значения для коррекции
                var recoveredValues = GaussSolve(valuesMatrix);

                // Теперь у нас есть позиции ошибок и значения для коррекции
                // Объединяем эти значения с синдромом ошибки и кодируем
                var valuesToEncode = recoveredValues.Concat(syndrome).ToArray();

                var encodedValues = new int[N];

                for (var n = 0; n < N; n++)
                {
                    encodedValues[n] = GaluaZero;

                    for (var k = 0; k < valuesToEncode.Length; k++)
                    {
                        var fieldElement = valuesToEncode[k];
                        var current = Mult(fieldElement, EncoderMatrix[k, n]);
                        encodedValues[n] = Add(encodedValues[n], current);
                    }
                }

                // Вычитаем эти значения из кодового слова
                for (int i = 0; i < encodedValues.Length; i++)
                {
                    galuaCode[i] = Add(galuaCode[i], encodedValues[i]);
                }*/
            }

            var result = new int[K];

            for (var i = 0; i < K; i++)
            {
                result[i] = GaluaZero;

                for (var j = 0; j < N; j++)
                {
                    var current = Mult(galuaCode[j], DecoderMatrix[i, j]);
                    result[i] = Add(result[i], current);
                }
            }

            var decoded = result.Select(x => PowerToCodeMap[x]).ToArray();
            return decoded;
        }

        /// <summary>
        ///     Решает СЛАУ методом Гаусса
        /// </summary>
        static int[] GaussSolve(int[,] matrix)
        {
            var rootsCount = matrix.GetLength(0);
            if (matrix.GetLength(1) != rootsCount + 1)
            {
                throw new ArgumentException("Неправильно сформирована матрица!");
            }

            var solution = new int[rootsCount];

            // Решаем систему методом Гаусса
            // Прямой ход
            for (int i = 0; i < rootsCount - 1; i++)
            {
                var elem = matrix[i, i];
                for (int j = i + 1; j < rootsCount; j++)
                {
                    var coeff = Div(matrix[j, i], elem);
                    for (int k = i; k <= rootsCount; k++)
                    {
                        var adjusted = Mult(coeff, matrix[i, k]);

                        matrix[j, k] = Add(matrix[j, k], adjusted);
                    }
                }
            }

            for (int i = rootsCount - 1; i >= 0; i--)
            {
                solution[i] = Div(matrix[i, rootsCount], matrix[i, i]);

                for (int c = rootsCount - 1; c > i; c--)
                {
                    solution[i] = Add(solution[i], Div(Mult(matrix[i, c], solution[c]), matrix[i, i]));
                }
            }

            return solution;
        }

        public static int Add(int a, int b)
        {
            if (a == GaluaZero)
            {
                return b;
            }

            if (b == GaluaZero)
            {
                return a;
            }

            return AddTable[a, b];
        }

        public static int Mult(int a, int b)
        {
            if (a == GaluaZero || b == GaluaZero)
            {
                return GaluaZero;
            }

            return MultTable[a, b];
        }

        public static int Div(int a, int b)
        {
            if (a == GaluaZero)
            {
                return GaluaZero;
            }

            if (b == GaluaZero)
            {
                throw new ArgumentException("На 0 делить нельзя!");
            }

            return DivTable[a, b];
        }
    }
}