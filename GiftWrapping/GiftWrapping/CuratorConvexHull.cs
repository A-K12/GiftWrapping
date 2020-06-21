using System.Collections.Generic;
using GiftWrapping.Structures;

namespace GiftWrapping
{
    public class CuratorConvexHull
    {
        private readonly int _dim;
        private readonly List<IFace> _faces;
        private readonly Dictionary<Hyperplane, IFace> _processedPlanes;

        public CuratorConvexHull(Hyperplane firstPlane)
        {
            _faces = new List<IFace>();
            _processedPlanes = new Dictionary<Hyperplane, IFace>(new HyperplaneComparer());
            _dim = firstPlane.Dimension;
            AddNewFace(new Face(firstPlane));
        }

        private void AddNewFace(IFace face)
        {
            _processedPlanes.Add(face.Hyperplane, face);
            _faces.Add(face);
        }

        public IEnumerable<IFace> GetEmptyFaces()
        {
            for (int i = 0; i < _faces.Count; i++)
            {
                yield return _faces[i];
            }
        }

        public void AddNewPlane(Hyperplane plane, IFace parentFace)
        {
            if (_processedPlanes.TryGetValue(plane, out IFace face))
            {
                face.AdjacentCells.Add(parentFace);
                return;
            }
            Face newFace = new Face(plane);
            newFace.AdjacentCells.Add(parentFace);
            AddNewFace(newFace);
        }

        public IConvexHull GetConvexHull()
        {
            ConvexHull convexHull = new ConvexHull(_dim);
            _faces.ForEach((face => convexHull.AddInnerCell(face)));

            return convexHull;
        }
    }
}