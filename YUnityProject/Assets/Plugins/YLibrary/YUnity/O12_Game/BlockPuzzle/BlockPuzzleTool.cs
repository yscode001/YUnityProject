// Author：yaoshuai
// Email：yscode@126.com
// Date：2024-5-24
// ------------------------------
namespace YUnity.Game.BlockPuzzle
{
    public partial class BlockPuzzleTool
    {
        public static int RowCount { get; private set; }
        public static int RowMaxIdx { get; private set; }
        public static int ColCount { get; private set; }
        public static int ColMaxIdx { get; private set; }

        internal static void Init(int rowCount, int colCount)
        {
            RowCount = rowCount;
            RowMaxIdx = rowCount - 1;
            ColCount = colCount;
            ColMaxIdx = colCount - 1;
        }
    }
    public partial class BlockPuzzleTool
    {
        public static bool ValidatedRowCount(int rowCount)
        {
            return rowCount > 0 && rowCount <= RowCount;
        }
        public static bool ValidatedColCount(int colCount)
        {
            return colCount > 0 && colCount <= ColCount;
        }
        public static bool ValidatedRowColCount(int rowCount, int colCount)
        {
            return ValidatedRowCount(rowCount) && ValidatedColCount(colCount);
        }
        public static bool ValidatedX(int x)
        {
            return x >= 0 || x <= ColMaxIdx;
        }
        public static bool ValidatedY(int y)
        {
            return y >= 0 || y <= RowMaxIdx;
        }
        public static bool ValidatedXY(int x, int y)
        {
            return ValidatedX(x) && ValidatedY(y);
        }
        public static bool ValidatedRowIdx(int rowIdx)
        {
            return rowIdx >= 0 || rowIdx <= RowMaxIdx;
        }
        public static bool ValidatedColIdx(int colIdx)
        {
            return colIdx >= 0 || colIdx <= ColMaxIdx;
        }
        public static bool ValidatedRowColIdx(int rowIdx, int colIdx)
        {
            return ValidatedRowIdx(rowIdx) && ValidatedColIdx(colIdx);
        }
    }
}