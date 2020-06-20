using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using GiftWrapping;
using GiftWrapping.Structures;

namespace RunTest
{
    class Program
    {
        static void Main(string[] args)
        {
            List<PlanePoint> points = new List<PlanePoint> {
                new PlanePoint(new double[]{0, 0, 0, 0}),
                new PlanePoint(new double[]{ 100, 0, 0, 0}),
                new PlanePoint(new double[]{100, 100, 0, 0}),
                new PlanePoint(new double[]{0, 100, 0, 0}),
                new PlanePoint(new double[]{0, 0, 100, 0}),
                new PlanePoint(new double[]{ 100, 0, 100, 0}),
                new PlanePoint(new double[]{100, 100, 100, 0}),
                new PlanePoint(new double[]{0, 100, 100, 0}),
                new PlanePoint(new double[]{0, 0, 0, 100}),
                new PlanePoint(new double[]{ 100, 0, 0,100}),
                new PlanePoint(new double[]{100, 100, 0, 100}),
                new PlanePoint(new double[]{0, 100, 0, 100}),
                new PlanePoint(new double[]{0, 0, 100, 100}),
                new PlanePoint(new double[]{ 100, 0, 100, 100}),
                new PlanePoint(new double[]{100, 100, 100, 100}),
                new PlanePoint(new double[]{0, 100, 100, 100}),
            };


            Random r = new Random();
            
            for (int i = 0; i < 1000; i++)
            {
                points.Add(new PlanePoint(new double[] { r.Next(1, 80), r.Next(1, 80), r.Next(1, 80), r.Next(1, 80) })); 
            }

            GiftWrappingAlgorithm giftWrapping = new GiftWrappingAlgorithm(points);
            Stopwatch sp = new Stopwatch();
            Console.Out.WriteLine("Start");
            sp.Start();
            IFace result1 = giftWrapping.Create();
            sp.Stop();
            
             Console.Out.WriteLine("Stop");
            // ((ConvexHull)result1).Convert(@"D:\", "dode21");
            Console.Out.WriteLine("ms = {0}", sp.ElapsedMilliseconds);
            Console.Out.WriteLine("t = {0}", sp.ElapsedTicks);
        }
    }
}
