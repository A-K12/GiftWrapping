namespace GiftWrapping.Structures
{
    public interface IPolyhedron
    {
        IPolyhedron[] Edges { get; set; }
        int Dim { get; set; }

        IPoint[] Boundary { get; set; }
    }
}