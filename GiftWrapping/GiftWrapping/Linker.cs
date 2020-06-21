using System.Collections.Generic;
using GiftWrapping.Structures;

namespace GiftWrapping
{
    public class Linker
    {
        List<IFace> faces = new List<IFace>();
        Dictionary<Hyperplane, IFace> processedPlanes;

        public Linker(Hyperplane firstPlane)
        {
            faces = new List<IFace>();
            processedPlanes = new Dictionary<Hyperplane, IFace>(new HyperplaneComparer());
            Face firstFace = new Face(firstPlane);
            processedPlanes.Add(firstPlane, firstFace);
            faces.Add(firstFace);
        }
    }
}