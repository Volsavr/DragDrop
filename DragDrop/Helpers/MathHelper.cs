using System;
using System.Windows;

namespace DragDrop.Helpers
{
    internal static class MathHelper
    {
        internal static double GetLengthOfVector(Vector a)
        {
            return Math.Sqrt(Math.Pow(a.X, 2) + Math.Pow(a.Y, 2));
        }

        internal static Vector GetVectorPoint(Point a, Point b)
        {
            return new Vector(b.X - a.X, b.Y - a.Y);
        }

        internal static double GetCosOfAngleBetweenVectors(Vector a, Vector b)
        {
            return (a.X * b.X + a.Y * b.Y) / (GetLengthOfVector(a) * GetLengthOfVector(b));
        }

        internal static double RadiansToGradus(double radians)
        {
            return radians * 180 / Math.PI;
        }
    }
}
