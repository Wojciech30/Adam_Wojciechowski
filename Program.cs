﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Lab1_VychMat_Console
{
    class Matrix
    {
        static void Main(string[] args)
        {
            const int MAXSIZE = 20;

            Console.WriteLine("Choose the option:\n1.Enter from keyboard\n2.Read from file\n3.Random numbers");
            int answer=0;
            try {
                answer = Int32.Parse(Console.ReadLine());
            }
            catch(FormatException)
            {
                bool mist = true;
                do
                {
                    int corrected = MistakeInInt(1, 3);
                    if (corrected > 0 && corrected < 4)
                    {
                        answer = corrected;
                        mist = false;
                    }
                } while (mist == true);
            }

            int size=0;
            double[,] matrix=null;
            double precision = 0;



            switch (answer)
            {
                case 1:
                    Console.WriteLine("Enter the number of variables: ");
                    size = CheckSize(2,MAXSIZE);

                    Console.WriteLine("Enter the matrix: ");
                    matrix = InputMatrix(size);
                    OutputMatrix(matrix, size);
                    //Console.ReadKey();

                    Console.WriteLine("Enter the value of precision: ");
                    precision = GetPrecision();
                    Console.WriteLine("Precision: " + precision);
                    break;

                case 2:
                    Console.WriteLine("Enter the name of the file/directory");
                    string fname = Console.ReadLine();
                    if (File.Exists(fname) == false)
                    {
                        bool mist = true;
                        do
                        {
                            string corrected = MistakeInStr();
                            if (File.Exists(corrected) == true)
                            {
                                mist = false;
                            }
                        } while (mist == true);
                    }
                    precision = PrecFromFile(fname);
                    Console.WriteLine("Precision: " + precision);

                    size = CountLinesInFile(fname);
                    Console.WriteLine("Size of matrix: " + size);

                    matrix = ReadMatrixFromFile(fname, size);// 
                    OutputMatrix(matrix, size);
                    break;
                case 3:
                    Console.WriteLine("Enter the number of variables: ");
                    Random r = new Random();
                    size = r.Next(2, 20); //random size
                    Console.WriteLine("Size: " + size);
                    matrix = RandomNums(size);
                    OutputMatrix(matrix, size);
                    break;

            }

            bool iterable = IsDiag(matrix, size);   //проверяем на диагональное преобладание 
            if (iterable == false)
            {
                Console.WriteLine("not of the diagonal view");
                matrix = TryDiag(matrix, size);
                OutputMatrix(matrix, size);
            }

            IterationResult res = new IterationResult();
            res = Iteration(size, matrix, precision);
            for(int i=0; i< size; i++)
            {
                Console.WriteLine("X{0} = {1}\t +- \t{2}",i, res.x[i].ToString("N5"), res.err[i].ToString("N10"));
                
            }
            Console.WriteLine("Num of iterations: " + res.iter.ToString());
            Console.ReadKey();

        }

        static string MistakeInStr()
        {
            Console.WriteLine("File doesn't exist. Enter another value (y/n)");
            string answer = Console.ReadLine();
            string corrected="";
            switch (answer)
            {
                case "y":
                    Console.WriteLine("Enter the new value");
                    corrected = Console.ReadLine();
                    break;
                case "n":
                    Console.WriteLine("Exiting");
                    Console.ReadKey();
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Incorrect value entered. Exiting. Press any key to exit");
                    Environment.Exit(-1);
                    break;
            }
            return corrected;
        }//REDO

        static double[,] TryDiag(double[,] oldMatr, int size)
        {
            double[,] newMatr = new double[size, size+1];
            int[] max = new int[size];
            double[] sum = new double[size];

            Great gr;
            bool oneNotEqual = false;
            //ищем максимальный элемент в каждой строке
            for (int i = 0; i < size; i++)
            {
                gr=FindGreatest(oldMatr, size, i);
                max[i] = gr.j_max;
                if (gr.notEqual == true)
                {
                    oneNotEqual = gr.notEqual;
                };
                //Console.WriteLine("Greatest in line #{0} == {1}", i, max[i]);
            }

            bool possibToDiag=PossibToDiag(max);
            if (possibToDiag == false || oneNotEqual==false)
            {
                Console.WriteLine("Impossible to diagonal view. Press any key to exit");
                Console.ReadKey();
                Environment.Exit(0);
            }
            else
            {
                for(int i=0; i< size; i++)
                {
                    for (int j=0; j<size+1; j++)
                    {
                        newMatr[max[i], j] = oldMatr[i, j];
                    }
                }
            }


            return newMatr;
        }

        static bool PossibToDiag(int[] a)
        {
           
            bool res=false;
            for (int i=0; i<a.Length ; i++)
            {
                if (a[i] != -1)
                {
                    res = true;
                }
                else
                {
                    res = false;
                }
            }

            for (int i=0; i< a.Length; ++i)
            {
                res = true;
                for (int j=i+1; j<a.Length-1; ++j)
                {
                    if (a[j] == a[i])
                    {
                        res = false;
                        break;
                    }
                }
                if (res == false)
                {
                    break;
                }
            }

            return res;
        }

        struct Great
        {
            public bool notEqual;
            public int j_max;
        }

        static Great FindGreatest(double[,] matr, int size, int i_row)
        {
            Great find;
            find.notEqual = false;// хотя бы один элемент больше
            double sum = Math.Abs(matr[i_row, 0]);
            find.j_max = 0;
            for (int j = 1; j < size; ++j)
            {
                sum += Math.Abs(matr[i_row, j]);
                if (Math.Abs(matr[i_row, j]) >= Math.Abs(matr[i_row, find.j_max]))
                {
                    find.j_max = j;
                }
                if (Math.Abs(matr[i_row, find.j_max]) < sum - Math.Abs(matr[i_row, find.j_max]))
                {
                    find.j_max = -1;
                }
                if (Math.Abs(matr[i_row, find.j_max]) > sum - Math.Abs(matr[i_row, find.j_max]))
                {
                    find.notEqual = true;
                }

            }
            
            return find;
        }
        // запоминаем индекс максимального числа в строке

        static double PrecFromFile(string filename)
        {
            string line1;
            using (StreamReader reader = new StreamReader(filename))
            {
                line1 = reader.ReadLine();
                //Console.WriteLine("FirstLine" + line1); debugging
            }
            //string line1 = File.ReadLines(filename).Take(0).First(); // gets the first line from file.
            double res = 0;
            try
            {
                res = Convert.ToDouble(line1, new System.Globalization.CultureInfo("en-US"));
            }
            catch (FormatException)
            {
                res = MistakeInPecFromFile();
            }
            
            return res;
        }   //считываем точность матрицы из файла (первая строка)

        static double MistakeInPecFromFile()
        {
            bool mistake = true;
            double prec = 0;
            do
            {
                Console.WriteLine("Impossible to convert to double.\n" +
                    "1. Try another file\n2. Enter the precision from keyboard");
                int answ = CheckSize(1, 2);
                switch (answ)
                {
                    case 1:
                        Console.WriteLine("Enter the name of the file: ");
                        string fname = Console.ReadLine();
                        bool mist = false;
                        do
                        {
                            mist = true;
                            if (File.Exists(fname) == true)
                            {
                                mist = false;
                                break;
                            }
                            else
                            {
                                mist = true;
                                fname = MistakeInStr();
                                if (File.Exists(fname) == true)
                                {
                                    mist = false;
                                }
                            }
                        } while (mist == true);
                        prec = PrecFromFile(fname);
                        break;

                    case 2:
                        prec = GetPrecision();
                        break;
                }

            } while (mistake == true);

            return prec;
        }//TODO impossible to convert to double

        static int CountLinesInFile(string f)
        {
            int count = 0;
            using (StreamReader r = new StreamReader(f))
            {
                string line;
                while ((line = r.ReadLine()) != null)
                {
                    count++;
                }
            }
            return count-1;
        }

        static double[,] ReadMatrixFromFile(string filename, int size)
        {
            string[] input = File.ReadLines(filename).Skip(1).Take(size).ToArray();
            double[,] result = new double[size, size+1];

            int i = 0;
            foreach (var row in input)
            {
                int j = 0;
                foreach (var col in row.Trim().Split(' '))
                {
                    result[i, j] = Convert.ToDouble(col.Trim());
                    ++j;
                }
                ++i;
            }
            
            return result;
        }//считываем матрицу из файла

        static double[,] RandomNums(int s)//REDO
        {
            double[,] matrix = new double[s, s+1];
            Random random = new Random();
            for (int i = 0; i < s; i++)
            {
                for (int j = 0; j < s+1; j++)
                {
                    matrix[i, j] = random.Next(-20, 50);
                }
            }
            return matrix;
        }   //задаем матрицу случайных коэффициентов

        struct IterationResult
        {
            public int iter;
            public double[] x;
            public double[] err;
           
        }

        static IterationResult Iteration(int size, double[,] matrix /*, double[] prevMatrix*/, double precision)
        {
            IterationResult tmp;

            double[] last = new double[size]; //столбец неизвестных предыдущей итерации
            double[] now = new double[size]; //стоблец неизвестных текущей итерации
            double[] err = new double[size]; //столбец погрешностей
            tmp.iter = 0;
            while (true)
            {
                tmp.iter++; //итерация +1
              
                for (int i = 0; i < size; i++) //подсчет нового столбца неизвестных
                {
                    double x = matrix[i, size];
                    for (int j = 0; j < size; j++)
                    {
                        if (!(i == j)) x -= matrix[i, j] * now[j];
                    }
                    now[i] =x / matrix[i, i];
                }

                double ac = double.MinValue; //для определения достижения заданной точности
                for (int i = 0; i < size; i++)  //подсчет столбца погрешностей
                {
                    err[i] = Math.Abs(now[i] - last[i]); //заполняем столбец погрешностей
                    if (err[i] > ac) ac = err[i];
                }

                if (ac < precision ) break; // по достижению введенной пользователем точности

                for (int i = 0; i < size; i++)
                {
                    last[i] = now[i];
                }
            }
            tmp.x = now;
            tmp.err = err;

            return tmp;
        }//сама итерация
    
        static int CheckSize(int min, int max)
        {
            int nSize = 0;
            try
            { 
                nSize = Convert.ToInt32(Console.ReadLine());
                bool mistake = false;
                do {
                    if (nSize < min || nSize > max)
                    {
                        mistake = true;
                        int corrected = MistakeInInt(min,max);
                        if (corrected > min && corrected < max+1)
                        {
                            nSize = corrected;
                            mistake = false;
                        }
                    }
                }
                while (mistake == true);//TODO
            }
            catch (FormatException)
            {
                Console.WriteLine("The value must be int");
                bool mistake = false;
                do
                {
                    //if (nSize < 1 || nSize > 20)
                    //{
                        mistake = true;
                        int corrected = MistakeInInt(0,max);
                        if (corrected > min && corrected < max+1)
                        {
                            nSize = corrected;
                            mistake = false;
                        }
                    //}
                }
                while (mistake == true);
            } 
            return nSize;
        }   //считывание размера матрицы и проверка возвращаемого значения

        static double[,] InputMatrix(int size)
        {
            double[,] matrix = new double[size, size+1];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size+1; j++)
                {
                    bool mistake = true;
                    do
                    {
                        try
                        {
                            matrix[i, j] = Convert.ToDouble(Console.ReadLine());
                            mistake = false;
                        }
                        catch (FormatException)
                        {
                            mistake = true;
                            double corrected = MistakeInDouble(Double.MinValue, Double.MaxValue);
                        }
                    } while (mistake == true);
                }
            }
            return matrix;
        }   //ввод матрицы

        static void OutputMatrix(double[,] matrix, int size) {
            Console.WriteLine("Matrix");
            for (int i = 0; i < size; i++)
            {
                //Console.Write("{0}) ", i);
                
                for (int j = 0; j < size+1; j++)
                {
                    //Console.WriteLine("Element({0},{1})={2}", i, j, matrix[i, j]);
                    Console.Write("{0} ", matrix[i, j]);
                }
                Console.WriteLine();
            }
        }   //вывод матрицы на экран

        static int MistakeInInt(int min, int max)
        {
            int correctedNum = 0;
            Console.WriteLine("Value is incorrect. Do you want to enter anoher value? (y/n)");
            string answer = Console.ReadLine();
            switch (answer)
            {
                case "y":
                    Console.WriteLine("Enter the new value");
                    try {
                        correctedNum = Convert.ToInt32(Console.ReadLine());
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("Must be an int");
                    }
                    break;
                case "n":
                    Console.WriteLine("Exiting");
                    Console.ReadKey();
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Incorrect value entered. Exiting. Press any key to exit");
                    Environment.Exit(-1);
                    break;
            }
            if (correctedNum>min&& correctedNum < max)
            {
                return correctedNum;
            }
            else
            {
                return -1;
            }
        }   //обработка ошибки целочисленного числа

        static double GetPrecision()
        {
            bool mistake = false;
            double res = 0;
            try {
                res = Convert.ToDouble(Console.ReadLine());
                do
                {

                    if (res > 1 || res < 0)
                    {
                        mistake = true;
                        double corrected = MistakeInDouble(0, 1);
                        if (corrected > 0 && corrected < 1)
                        {
                            res = corrected;
                            mistake = false;
                        }
                    }
                    else
                    {
                        mistake = true;
                        break;
                    }
                }
                while (mistake == true);//TODO
            }
            catch (FormatException)
            {
                Console.WriteLine("Value must be double");
                do
                {
                    mistake = true;
                    double corrected = MistakeInDouble(0, 1);
                    if (corrected > 0 && corrected < 1)
                    {
                       res = corrected;
                        mistake = false;
                    }
                    
                    else
                    {
                        break;
                    }
                }
                while (mistake == true);
            }
            return res;
        }   //ввод и проверка адекватности сего действия по отношению к точности вычисления

        static double MistakeInDouble(double min, double max)
        {
            double correctedNum = 0;
            Console.WriteLine("Value is incorrect. Do you want to enter anoher value? (y/n)");
            string answer = Console.ReadLine();
            switch (answer)
            {
                case "y":
                    Console.WriteLine("Enter the new value");
                    double.TryParse(Console.ReadLine(), out correctedNum);
                    break;
                case "n":
                    Console.WriteLine("Exiting");
                    Console.ReadKey();
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Incorrect value entered. Exiting. Press any key to exit");
                    Environment.Exit(-1);
                    break;
            }
            if (correctedNum > 0 && correctedNum < 1)
            {
                return correctedNum;
            }
            else
            {
                return -1;
            }
        }   //обработка ошибки числа типа double

        static bool IsDiag(double[,] iterableMatrix, int size)
        {
            bool testIter = false;
            for (int i = 0; i < size; i++)
            {
                double sum = 0;
                for (int j = 0; j < size; j++)
                {
                    if (j != i)
                    {
                        sum += Math.Abs(iterableMatrix[i, j]);
                    }
                }
                if (Math.Abs(iterableMatrix[i, i]) > sum)
                {
                    testIter = true;
                    sum = 0;
                }
                else
                {
                    testIter = false;
                    break;
                }

            }
            return testIter;
        }   //проверка на диагональное преобладание

    }
}
