﻿using System.Collections.Generic;
using GiftWrapping.Structures;

namespace GiftWrapping
{
    public class GiftWrappingAlgorithm
    {
        private double tolerance;
        public GiftWrappingAlgorithm(Point[] points, double tolerance)
        {
            this.points = points;
            faces = new List<IConvexFace>();
            unprocessedFaces = new Queue<IConvexFace>();
        }

        private Point[] points;

        private IList<IConvexFace> faces;

        private Queue<IConvexFace> unprocessedFaces;

        public ConvexFace Create(Point points)
        {
            
            FindFirstFace();

            return new ConvexFace();

        }

        private void FindFirstFace()
        {
            Point mainPoint;
            Vector normal;

        }
    }
}