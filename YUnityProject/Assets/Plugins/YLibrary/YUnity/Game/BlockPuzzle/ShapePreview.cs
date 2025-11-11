namespace YUnity.Game.BlockPuzzle
{
    public partial class ShapePreview
    {
        public int BlockCount { get; private set; }
        public Coordinate[] BlockArray { get; private set; }
        public FillType[] FillTypeArray { get; private set; }

        public int RowCount { get; private set; }
        public int RowMinIdx { get; private set; }
        public int RowMaxIdx { get; private set; }

        public int ColCount { get; private set; }
        public int ColMinIdx { get; private set; }
        public int ColMaxIdx { get; private set; }

        internal ShapePreview() { }

        private int StartRowIdx = -1;
        private int StartColIdx = -1;
        private Shape Shape = null;
        internal void SetupData(int startRowIdx, int startColIdx, Shape shape)
        {
            if (StartRowIdx != startRowIdx || StartColIdx != startColIdx || Shape != shape)
            {
                // 判断一下，避免重复，提升效率
                StartRowIdx = startRowIdx;
                StartColIdx = startColIdx;
                Shape = shape;

                BlockCount = shape.BlockCount;
                BlockArray = new Coordinate[BlockCount];
                FillTypeArray = new FillType[BlockCount];

                RowCount = shape.RowCount;
                RowMinIdx = startRowIdx;
                RowMaxIdx = startRowIdx + RowCount - 1;

                ColCount = shape.ColCount;
                ColMinIdx = startColIdx;
                ColMaxIdx = startColIdx + ColCount - 1;

                for (int i = 0; i < BlockCount; i++)
                {
                    BlockArray[i] = new Coordinate(startColIdx + shape.BlockArray[i].X, startRowIdx + shape.BlockArray[i].Y);
                    FillTypeArray[i] = shape.FillTypeArray[i];
                }
            }
        }
    }
    public partial class ShapePreview
    {
        public bool BlockContainXY(int x, int y)
        {
            foreach (var coordinate in BlockArray)
            {
                if (coordinate.EqualXY(x, y))
                {
                    return true;
                }
            }
            return false;
        }
        public bool BlockContainRowCol(int rowIdx, int colIdx)
        {
            foreach (var coordinate in BlockArray)
            {
                if (coordinate.EqualRowCol(rowIdx, colIdx))
                {
                    return true;
                }
            }
            return false;
        }
        public bool BlockContainCoordinate(Coordinate coordinate)
        {
            foreach (var block in BlockArray)
            {
                if (coordinate.EqualCoordinate(block))
                {
                    return true;
                }
            }
            return false;
        }
    }
}