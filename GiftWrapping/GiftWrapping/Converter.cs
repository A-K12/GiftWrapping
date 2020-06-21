using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GiftWrapping.Structures;

namespace GiftWrapping
{
    public static class Converter
    {
        public static void ConvertToOff(this ConvexHull ch, string path, string name)
        {
            if (ch.Dimension != 3)
            {
                throw new ArgumentException("Wrong dimension");
            }
            string writePath = Path.Combine(path, name + ".off");
            List<PlanePoint> points = ch.GetPoints().ToList();
            int countPoints = points.Count;
            int countFaces = ch.Cells.Count();
            using StreamWriter sw = new StreamWriter(writePath, false, System.Text.Encoding.Default);
            sw.WriteLine("OFF");
            sw.WriteLine(countPoints.ToString() + " " + countFaces.ToString() + " 0");
            foreach (PlanePoint planePoint in points)
            {
                sw.WriteLine(planePoint.GetPoint(3).ToString());
            }
            foreach (ICell cell in ch.Cells)
            {
                ICollection<PlanePoint> facePoints = cell.GetPoints();
                IEnumerable<int> indexes = facePoints.Select(point => points.IndexOf(point));
                string strFace = String.Join(" ", indexes);
                sw.WriteLine(facePoints.Count+" "+strFace);
            }

        }

        public static void ConvertToTxt(this IEnumerable<PlanePoint> ch, string path, string name)
        {
            string writePath = Path.Combine(path, name + ".txt");
            using StreamWriter sw = new StreamWriter(writePath, false, System.Text.Encoding.Default);
            foreach (PlanePoint points in ch)
            {
                sw.WriteLine(points.ToString());
            }
        }
    }
}