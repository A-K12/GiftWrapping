namespace GiftWrapping.Structures
{
    public class Polyhedron:IPolyhedron
    {
        public IPolyhedron[] Edges { get; set; }
        public int Dim { get; set; }
        public Point[] Boundary { get; set; }
    }
}