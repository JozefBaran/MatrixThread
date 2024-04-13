using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MatrixThread
{
    public class Matrix
    {
        public int[,] Data { get; set; }
        public int Rows { get; set; }
        public int Columns { get; set; }

        public Matrix(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
            Data = new int[rows, columns];
        }

        public void GenerateRandomData(int seed)
        {
            Random random = new Random(seed);
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    Data[i, j] = random.Next(1, 10);
                }
            }
        }

        public static Matrix MultiplyUsingThreads(Matrix a, Matrix b, int threadCount)
        {
            if (a.Columns != b.Rows)
            {
                throw new ArgumentException("Liczba kolumn w macierzy musi być równa liczbie wierszy.");
            }

            Matrix result = new Matrix(a.Rows, b.Columns);
            Thread[] threads = new Thread[threadCount];

            int elementsPerThread = (a.Rows * b.Columns) / threadCount;
            int extraElements = (a.Rows * b.Columns) % threadCount;

            int threadIndex = 0;
            for (int i = 0; i < a.Rows; i++)
            {
                for (int j = 0; j < b.Columns; j++)
                {
                    if (threadIndex >= threadCount)
                    {
                        break;
                    }

                    int start = threadIndex * elementsPerThread;
                    int end = (threadIndex + 1) * elementsPerThread;
                    if (threadIndex == threadCount - 1)
                    {
                        end += extraElements;
                    }

                    threads[threadIndex] = new Thread((object obj) =>
                    {
                        var indices = (Tuple<int, int>)obj;
                        int startIndex = indices.Item1;
                        int endIndex = indices.Item2;

                        for (int index = startIndex; index < endIndex; index++)
                        {
                            int row = index / b.Columns;
                            int col = index % b.Columns;
                            for (int k = 0; k < a.Columns; k++)
                            {
                                result.Data[row, col] += a.Data[row, k] * b.Data[k, col];
                            }
                        }
                    });

                    threads[threadIndex].Start(new Tuple<int, int>(start, end));
                    threadIndex++;
                }
            }

            foreach (var thread in threads)
            {
                thread?.Join();
            }

            return result;
        }

        public static Matrix MultiplyUsingParallel(Matrix a, Matrix b, int threadCount)
        {
            if (a.Columns != b.Rows)
            {
                throw new ArgumentException("Liczba kolumn w macierzy musi byc rowna liczbie wierszy.");
            }

            Matrix result = new Matrix(a.Rows, b.Columns);

            Parallel.For(0, a.Rows, new ParallelOptions { MaxDegreeOfParallelism = threadCount }, i =>
            {
                for (int j = 0; j < b.Columns; j++)
                {
                    for (int k = 0; k < a.Columns; k++)
                    {
                        result.Data[i, j] += a.Data[i, k] * b.Data[k, j];
                    }
                }
            });

            return result;
        }

        public void Print()
        {
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    Console.Write($"{Data[i, j]} ");
                }
                Console.WriteLine();
            }
        }
    }
}