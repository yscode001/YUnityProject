namespace YUnity.Game.BlockPuzzle
{
    /// <summary>
    /// 坐标
    /// </summary>
    public struct Coordinate
    {
        public int X { get; private set; }
        public int ColIdx { get; private set; }
        public int Y { get; private set; }
        public int RowIdx { get; private set; }

        public Coordinate(int x, int y)
        {
            X = ColIdx = x;
            Y = RowIdx = y;
        }

        public void SetupXY(int x, int y)
        {
            X = ColIdx = x;
            Y = RowIdx = y;
        }
        public void SetupRowCol(int rowIdx, int colIdx)
        {
            X = ColIdx = colIdx;
            Y = RowIdx = rowIdx;
        }

        public bool EqualX(int x)
        {
            return X == x;
        }
        public bool EqualY(int y)
        {
            return Y == y;
        }
        public bool EqualXY(int x, int y)
        {
            return X == x && Y == y;
        }
        public bool EqualRow(int rowIdx)
        {
            return RowIdx == rowIdx;
        }
        public bool EqualCol(int colIdx)
        {
            return ColIdx == colIdx;
        }
        public bool EqualRowCol(int rowIdx, int colIdx)
        {
            return RowIdx == rowIdx && ColIdx == colIdx;
        }
        public bool EqualCoordinate(Coordinate other)
        {
            return X == other.X && Y == other.Y;
        }
    }
}