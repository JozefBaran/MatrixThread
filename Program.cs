using System;
using System.Diagnostics;

namespace MatrixThread
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Podaj rozmiar macierzy: ");
            int size = int.Parse(Console.ReadLine());
            Console.WriteLine("Podaj seed dla macierzy A: ");
            int seedA = int.Parse(Console.ReadLine());
            Console.WriteLine("Podaj seed dla macierzy B: ");
            int seedB = int.Parse(Console.ReadLine());
            Console.WriteLine("Podaj liczbę wątków: ");
            int threadCount = int.Parse(Console.ReadLine());

            Matrix matrixA = new Matrix(size, size);
            Matrix matrixB = new Matrix(size, size);

            matrixA.GenerateRandomData(seedA);
            matrixB.GenerateRandomData(seedB);

            /*
            Console.WriteLine("Macierz A:");
            matrixA.Print();
            Console.WriteLine("Macierz B:");
            matrixB.Print();
            */

            Console.WriteLine("Wybierz metodę mnożenia macierzy:");
            Console.WriteLine("1. Wielowątkowe mnożenie z wykorzystaniem klasy Thread");
            Console.WriteLine("2. Wielowątkowe mnożenie z wykorzystaniem biblioteki Parallel");
            int choice = int.Parse(Console.ReadLine());

            Stopwatch stopwatch = Stopwatch.StartNew();
            Matrix result;
            switch (choice)
            {
                case 1:
                    result = Matrix.MultiplyUsingThreads(matrixA, matrixB, threadCount);
                    break;
                case 2:
                    result = Matrix.MultiplyUsingParallel(matrixA, matrixB, threadCount);
                    break;
                default:
                    Console.WriteLine("Nieprawidłowy wybór metody.");
                    return;
            }
            stopwatch.Stop();

            //Console.WriteLine("Wynik:");
           // result.Print();

            Console.WriteLine($"Czas wykonania: {stopwatch.ElapsedMilliseconds} ms");
        }
    }
}
