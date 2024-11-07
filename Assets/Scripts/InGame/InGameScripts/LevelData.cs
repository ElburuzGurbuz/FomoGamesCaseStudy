using System;

namespace ColorBlockData 
{
    [Serializable]
    public class LevelData
    {
        public int MoveLimit;
        public int RowCount;
        public int ColCount;
        public CellInfo[] CellInfo;
        public MovableInfo[] MovableInfo;
        public ExitInfo[] ExitInfo;
    }
}

