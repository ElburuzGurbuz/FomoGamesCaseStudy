using System;

namespace ColorBlockData
{
    [Serializable]
    public class MovableInfo
    {
        public int Row;
        public int Col;
        public int[] Direction;
        public int Length;
        public int Colors;
    }
}

