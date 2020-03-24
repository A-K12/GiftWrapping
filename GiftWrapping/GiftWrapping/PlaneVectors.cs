using System.Collections.Generic;
using GiftWrapping.Structures;

namespace GiftWrapping
{
    public class PlaneVectors
    {
        public Vector[] Vectors { get; }

        private bool[] _map;

        private int _dim;
        public PlaneVectors(int dim)
        {
            _dim = dim;
            Vectors = new Vector[dim-1];
            _map = new bool[dim-1];
            Init();
        }

        private void Init()
        {
            for (int i = 0; i < Vectors.Length ; i++)
            {
                double[] cells = new double[_dim];
                cells[i+1] = 1;
                Vectors[i] = new Vector(cells);
            }
        }

        public Vector[] SetVectorAndGet(Vector vector)
        {
            Vector[] vectors = new Vector[_dim-1];
            for (int i = 0; i < Vectors.Length; i++)
            {
                vectors[i] = Vectors[i];
            }
            for (int i = 0; i < Vectors.Length; i++)
            {
                
                if (!Vector.AreParallel(Vectors[i], vector)) continue;
                if (!_map[i])
                {
                    vectors[i] = vector;
                }
                else
                {
                    SetInEmptyPlace(vector);
                }
            }

            return vectors;
        }

        public void SetVector(Vector vector)
        {
            for (int i = 0; i < Vectors.Length; i++)
            {
                if (!Vector.AreParallel(Vectors[i], vector)) continue;
                if (!_map[i])
                {
                    Vectors[i] = vector;
                }
                else
                {
                    SetInEmptyPlace(vector);
                }
            }
        }

        private void SetInEmptyPlace(Vector vector)
        {
            for (int i = 0; i < _map.Length; i++)
            {
                if (_map[i]) continue;
                Vectors[i] = vector;
                return;
            }
        }




    }
}