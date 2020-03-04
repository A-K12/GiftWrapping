using System;
using GiftWrapping.Structures;

namespace GiftWrapping
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var points = new Point[2];
            points[0] = new Point(1, 2, 3);
            points[1] = new Point( 4,5,6 );

            var vector2 = new Point(0, 0, 0);

            var points1 = new Point[3];
            points1[0] = new Point(1, 2, 3);
            points1[1] = new Point(3, 5, 7);
            points1[2] = new Point(1, 3, 4);

            var vector = new Point(3, 0, 1);


            var points4 = new Point[4]
            {
                new Point(1, 1, 0),
                new Point(6, 1, 0),
                new Point(3, 3, 0),
                new Point(3, 3, 6)
            };

            var points6 = new double[3, 3]
            {
                {1, 2, 3},
                {3, 5, 7},
                {1, 3, 4 }
            };
            var matrix = new Matrix(points6);
            var vector3 = new Vector(new double[3] { 3, 0, 1 });
            var expect = new Vector(new double[3] { -4, -13, 11 });

           // var result = LinearEquations.GaussWithChoiceSolveSystem(matrix, vector);

            var faceFinder = new FaceFinder();

            var arra = LinearEquations.GaussWithChoiceSolveSystem(matrix, vector3);
          // var arra = faceFinder.FindFacePoints(points4);
            //var arra = LinearEquations.Calculate(points);
            for (int i = 0; i < arra.Length; i++)
            {
                //Console.Out.Write(arra[i] + " ");
            }
            Console.ReadLine();
        }
    }
}
