using System;

namespace YUnity.Game.BlockPuzzle
{
    public partial class Shape
    {
        public int RowCount { get; private set; }
        public int ColCount { get; private set; }
        public int BlockCount { get; private set; }
        public Coordinate[] BlockArray { get; private set; }
        public FillType[] FillTypeArray { get; private set; }

        public int PutonRowMaxIdx { get; private set; }
        public int PutonYMax { get; private set; }
        public int PutonColMaxIdx { get; private set; }
        public int PutonXMax { get; private set; }

        private readonly ShapePreview ShapePreview = new ShapePreview();

        public Shape(Coordinate[] blockArray, FillType[] fillTypeArray)
        {
            if (blockArray == null || fillTypeArray == null || blockArray.Length == 0 || fillTypeArray.Length == 0 || blockArray.Length != fillTypeArray.Length)
            {
                throw new Exception("error");
            }
            RowCount = ColCount = BlockCount = 0;
            BlockArray = null;
            FillTypeArray = null;
            PutonRowMaxIdx = PutonYMax = PutonColMaxIdx = PutonXMax = 0;
            Init(blockArray);
            SetupFillTypes(fillTypeArray);
        }

        private void Init(Coordinate[] blockArray)
        {
            BlockCount = blockArray.Length;
            BlockArray = new Coordinate[BlockCount];
            int minX = int.MaxValue;
            int maxX = int.MinValue;
            int minY = int.MaxValue;
            int maxY = int.MinValue;

            for (int i = 0; i < BlockCount; i++)
            {
                Coordinate coordinate = blockArray[i];
                BlockArray[i] = coordinate;

                if (minX > coordinate.X) { minX = coordinate.X; }
                if (maxX < coordinate.X) { maxX = coordinate.X; }
                if (minY > coordinate.Y) { minY = coordinate.Y; }
                if (maxY < coordinate.Y) { maxY = coordinate.Y; }
            }

            RowCount = maxY - minY + 1;
            ColCount = maxX - minX + 1;
            PutonYMax = PutonRowMaxIdx = ChessBoard.RowCount - RowCount;
            PutonXMax = PutonColMaxIdx = ChessBoard.ColCount - ColCount;
        }

        public void SetupFillTypes(FillType[] fillTypeArray)
        {
            if (fillTypeArray == null || fillTypeArray.Length != BlockArray.Length)
            {
                throw new Exception("error");
            }
            FillTypeArray = fillTypeArray;
        }
    }
    public partial class Shape
    {
        public ShapePreview PreviewRowCol(int startRowIdx, int startColIdx)
        {
            ShapePreview.SetupData(startRowIdx, startColIdx, this);
            return ShapePreview;
        }
        public ShapePreview PreviewXY(int startX, int startY)
        {
            ShapePreview.SetupData(startY, startX, this);
            return ShapePreview;
        }
    }
    public partial class Shape
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