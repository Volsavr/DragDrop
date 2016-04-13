using System;
using System.Windows;

namespace DragDrop
{
    internal static class CollisionHelper
    {
        internal static bool CheckRectsCollision(Rect rectA, Rect rectB)
        {
            return rectA.IntersectsWith(rectB);
        }

        internal static bool CheckEllipsesCollision(Rect rectA, Rect rectB)
        {
            Point centerA = new Point(rectA.X + rectA.Width / 2, rectA.Y + rectA.Height / 2);
            Point centerB = new Point(rectB.X + rectB.Width / 2, rectB.Y + rectB.Height / 2);

            double tangens = (centerB.Y - centerA.Y)/(centerB.X - centerA.X);
            double cosinus = Math.Sqrt(1/(tangens*tangens + 1));
            double sinus = Math.Sqrt(1 - cosinus*cosinus);

            double radius1 = Math.Abs(rectA.Width - rectA.X)*Math.Abs(rectA.Height - rectA.Y)/
                             Math.Sqrt((rectA.Height - rectA.Y)*(rectA.Height - rectA.Y)*cosinus*cosinus +
                                       (rectA.Width - rectA.X)*(rectA.Width - rectA.X)*sinus*sinus);
            double radius2 = Math.Abs(rectB.Width - rectB.X) * Math.Abs(rectB.Height - rectB.Y) /
                              Math.Sqrt((rectB.Height - rectB.Y) * (rectB.Height - rectB.Y) * cosinus * cosinus +
                                        (rectB.Width - rectB.X) * (rectB.Width - rectB.X) * sinus * sinus);

            double delta = Math.Sqrt((rectB.X - rectA.X)*(rectB.X - rectA.X) + (rectB.Y - rectA.Y)*(rectB.Y - rectA.Y));

            if (delta <= radius1 + radius2)
                return true;

            return false;
        }

        internal static bool CheckPointCircleCollision(Point point, Point circleCenter, Double circleRadius)
        {
            if (circleRadius <= 0 )
                return false;

            var dx = circleCenter.X - point.X;
            var dy = circleCenter.Y - point.Y;
            return dx*dx + dy*dy <= circleRadius*circleRadius;
        }

        internal static bool CheckPointRectCollision(Point point, Rect rect)
        {
            if ((point.X >= rect.X && point.X <= rect.X + rect.Width) && (point.Y >= rect.Y && point.Y <= rect.Y + rect.Height))
                return true;

            return false;
        } 

    }
}
