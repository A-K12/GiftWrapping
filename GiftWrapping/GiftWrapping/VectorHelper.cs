using System;
using System.Dynamic;
using System.Linq;
using GiftWrapping.Structures;

namespace GiftWrapping
{
    public static class VectorHelper
    {
        public static Vector CreateVector(Vector startVector, Vector endVector)
        {
            //var vertor = new Vector(startVector.Dim);
            //for (int i = 0; i < startVector.Dim; i++)
            //{
            //    vertor[i] = startVector[i] -endVector[i];
            //}

            //return vertor;
           
            return new Vector(1);
        }


        public static double GetCosVectors(Vector vector1, Vector vector2)
        {
            //var length1 = GetVectorLength(vector1);
            //var length2 = GetVectorLength(vector2);
            //var scalar = GetScalarProduct(vector1, vector2);

           // return scalar / (length1 * length2);
           return -4;
        }

    }
}