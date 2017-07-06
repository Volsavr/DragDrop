using System;
using System.Collections.Generic;

namespace DragDrop.Helpers
{
    internal struct DragAngleSegmentItem
    {
        internal double MinValue { get; set; }
        internal double MaxValue { get; set; }
    }

    internal class DragAngleSegment
    {
        #region Constructor
        internal DragAngleSegment(double minAngle, double maxAngle, bool withInvertedSegment)
        {
            _segmentItems.Add(new DragAngleSegmentItem { MinValue = minAngle, MaxValue = maxAngle });

            if (withInvertedSegment)
            {
                var invertedMinValue = minAngle + 180;
                var invertedMaxValue = maxAngle + 180;

                if(invertedMaxValue <= 360)
                {
                    _segmentItems.Add(new DragAngleSegmentItem { MinValue = invertedMinValue, MaxValue = invertedMaxValue });
                }
                else if(invertedMinValue <= 360)
                {
                    _segmentItems.Add(new DragAngleSegmentItem { MinValue = invertedMinValue, MaxValue = 360 });
                    _segmentItems.Add(new DragAngleSegmentItem { MinValue = 0, MaxValue = invertedMaxValue - 360 });
                }
                else
                {
                    _segmentItems.Add(new DragAngleSegmentItem { MinValue = invertedMinValue - 360, MaxValue = invertedMaxValue - 360 });
                }
            }
        }
        #endregion

        #region Fields
        private List<DragAngleSegmentItem> _segmentItems = new List<DragAngleSegmentItem>();
        #endregion

        internal bool Contains(double angle)
        {
            if (angle > 360)
                throw new ArgumentException("Angle shoul be in [0, 360]");

            foreach(var item in _segmentItems)
            {
                if (angle >= item.MinValue && angle <= item.MaxValue)
                    return true;
            }

            return false;
        }
    }
}
