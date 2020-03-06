using System;
using GiftWrapping.LinearEquations;
using GiftWrapping.Structures;

namespace GiftWrapping
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            
            var points6 = new double[2, 3]
            {
                {1, 2, 3},
                {3, 5, 7}
            };
            var matrix = new Matrix(points6);
            var vector3 = new Vector(new double[3] { 3, 0, 1 });
            var expect = new Vector(new double[3] { -4, -13, 11 });


            var solver = new GaussWithChoiceSolveSystem();

            var arra = solver.GetRandomAnswer(matrix, vector3);

            Console.ReadLine();
        }
    }
}
