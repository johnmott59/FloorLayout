using System;

namespace Edit2DLib
{
    public partial class Edit2DBase
    {


        public int RoundToGrid(int value)
        {
            int GridSizeHalf = (int)Math.Floor((double)GridSize / (double)2);

            int ModX = value % GridSize;
            int DivX = (int)Math.Floor((double)value / (double)GridSize);
            int SnapValue = DivX * GridSize;
            if (ModX > GridSizeHalf) SnapValue += GridSize;

            return SnapValue;
        }


    }
}
