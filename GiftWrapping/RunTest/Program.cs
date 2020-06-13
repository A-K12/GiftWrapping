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
            PlanePoint[] points1 = PointsReader.MakeVertices(10);
            //Read(@"D:\1.txt");
            //
            // List<PlanePoint> points = new List<PlanePoint> {
            //     new PlanePoint(new double[]{1, 1, 1, 1}),
            //     new PlanePoint(new double[]{5, 1, 1, 1}),
            //     new PlanePoint(new double[]{1, 5, 1, 1}),
            //     new PlanePoint(new double[]{5, 5, 1,1}),
            //     new PlanePoint(new double[]{1, 1, 5,1}),
            //     new PlanePoint(new double[]{5, 1, 5,1}),
            //     new PlanePoint(new double[]{1, 5, 5,1}),
            //     new PlanePoint(new double[]{5, 5, 5,1}),
            //     new PlanePoint(new double[]{1, 1, 1, 5}),
            //     new PlanePoint(new double[]{5, 1, 1, 5}),
            //     new PlanePoint(new double[]{1, 5, 1, 5}),
            //     new PlanePoint(new double[]{5, 5, 1,5}),
            //     new PlanePoint(new double[]{1, 1, 5,5}),
            //     new PlanePoint(new double[]{5, 1, 5,5}),
            //     new PlanePoint(new double[]{1, 5, 5,5}),
            //     new PlanePoint(new double[]{5, 5, 5,5}),
            //
            // };

            //Random r = new Random();
            //List<PlanePoint> points = new List<PlanePoint>();
            //for (int i = 0; i < 10; i++)
            //{
            //    points.Add(new PlanePoint(new double[] { r.NextDouble(), r.NextDouble(), r.NextDouble() }));
            //}

            // PlanePoint[] points = new PlanePoint[] {
            //      new PlanePoint(new double[]{1, 1, 1, 1,1}),
            //      new PlanePoint(new double[]{5, 1, 1, 1,1}),
            //      new PlanePoint(new double[]{1, 5, 1, 1,1}),
            //      new PlanePoint(new double[]{5, 5, 1,1,1}),
            //      new PlanePoint(new double[]{1, 1, 5,1,1}),
            //      new PlanePoint(new double[]{5, 1, 5,1,1}),
            //      new PlanePoint(new double[]{1, 5, 5,1,1}),
            //      new PlanePoint(new double[]{5, 5, 5,1,1}),
            //      new PlanePoint(new double[]{1, 1, 1, 5,1}),
            //      new PlanePoint(new double[]{5, 1, 1, 5,1}),
            //      new PlanePoint(new double[]{1, 5, 1, 5,1}),
            //      new PlanePoint(new double[]{5, 5, 1,5,1}),
            //      new PlanePoint(new double[]{1, 1, 5,5,1}),
            //      new PlanePoint(new double[]{5, 1, 5,5,1}),
            //      new PlanePoint(new double[]{1, 5, 5,5,1}),
            //      new PlanePoint(new double[]{5, 5, 5,5,1}),
            //      new PlanePoint(new double[]{1, 1, 1, 1,5}),
            //      new PlanePoint(new double[]{5, 1, 1, 1,5}),
            //      new PlanePoint(new double[]{1, 5, 1, 1,5}),
            //      new PlanePoint(new double[]{5, 5, 1,1,5}),
            //      new PlanePoint(new double[]{1, 1, 5,1,5}),
            //      new PlanePoint(new double[]{5, 1, 5,1,5}),
            //      new PlanePoint(new double[]{1, 5, 5,1,5}),
            //      new PlanePoint(new double[]{5, 5, 5,1,5}),
            //      new PlanePoint(new double[]{1, 1, 1, 5,5}),
            //      new PlanePoint(new double[]{5, 1, 1, 5,5}),
            //      new PlanePoint(new double[]{1, 5, 1, 5,5}),
            //      new PlanePoint(new double[]{5, 5, 1,5,5}),
            //      new PlanePoint(new double[]{1, 1, 5,5,5}),
            //      new PlanePoint(new double[]{5, 1, 5,5,5}),
            //      new PlanePoint(new double[]{1, 5, 5,5,5}),
            //      new PlanePoint(new double[]{5, 5, 5,5,5}),
            //
            //  };
            //Point min = points1.Min();
            //min = -min;
            //for (int i = 0; i < points1.Length; i++)
            //{
            //    points1[i] = new PlanePoint(((Point)points1[i]) + min);
            //}

            GiftWrappingAlgorithm giftWrapping = new GiftWrappingAlgorithm(points1);
            Stopwatch sp = new Stopwatch();
            // IFace result = giftWrapping.Create();
            Console.Out.WriteLine("Start");
            sp.Start();
            IFace result1 = giftWrapping.Create();
            sp.Stop();
            
            Console.Out.WriteLine("Stop");
            ((ConvexHull)result1).Convert(@"D:\", "dode21");
            Console.Out.WriteLine("sp = {0}", sp.ElapsedMilliseconds);
            
        }
    }
}
