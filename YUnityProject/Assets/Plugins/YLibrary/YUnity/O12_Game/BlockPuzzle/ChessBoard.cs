// Author：yaoshuai
// Email：yscode@126.com
// Date：2024-5-24
// ------------------------------
using System.Collections.Generic;

namespace YUnity.Game.BlockPuzzle
{
    #region 属性定义与初始化
    public partial class ChessBoard
    {
        public static int RowCount { get; private set; }
        public static int RowMaxIdx { get; private set; }
        public static int ColCount { get; private set; }
        public static int ColMaxIdx { get; private set; }

        public static int[] RowIdxArray { get; private set; }
        public static int[] ColIdxArray { get; private set; }

        public static (int x, int y)[] SubScriptXY { get; private set; }
        public static (int rowIdx, int colIdx)[] SubScriptRowCol { get; private set; }

        public static Block[,] Panel { get; private set; }

        public static void Init(int rowCount, int colCount)
        {
            if (rowCount <= 0 || colCount <= 0)
            {
                throw new System.Exception("error");
            }

            BlockPuzzleTool.Init(rowCount, colCount);

            RowCount = rowCount;
            RowMaxIdx = rowCount - 1;
            ColCount = colCount;
            ColMaxIdx = colCount - 1;

            if (RowIdxArray == null || RowIdxArray.Length != rowCount)
            {
                RowIdxArray = new int[rowCount];
                for (int rowIdx = 0; rowIdx <= RowMaxIdx; rowIdx++)
                {
                    RowIdxArray[rowIdx] = rowIdx;
                }
            }
            if (ColIdxArray == null || ColIdxArray.Length != colCount)
            {
                ColIdxArray = new int[colCount];
                for (int colIdx = 0; colIdx <= ColMaxIdx; colIdx++)
                {
                    ColIdxArray[colIdx] = colIdx;
                }
            }

            if (SubScriptXY == null || SubScriptXY.Length != rowCount * colCount)
            {
                SubScriptXY = new (int x, int y)[rowCount * colCount];
                for (int rowIdx = 0; rowIdx <= RowMaxIdx; rowIdx++)
                {
                    for (int colIdx = 0; colIdx <= ColMaxIdx; colIdx++)
                    {
                        SubScriptXY[colIdx + rowIdx * colCount] = (colIdx, rowIdx);
                    }
                }
            }
            if (SubScriptRowCol == null || SubScriptRowCol.Length != rowCount * colCount)
            {
                SubScriptRowCol = new (int rowIdx, int colIdx)[rowCount * colCount];
                for (int rowIdx = 0; rowIdx <= RowMaxIdx; rowIdx++)
                {
                    for (int colIdx = 0; colIdx <= ColMaxIdx; colIdx++)
                    {
                        SubScriptRowCol[colIdx + rowIdx * colCount] = (rowIdx, colIdx);
                    }
                }
            }

            if (Panel == null || Panel.GetLength(0) * Panel.GetLength(1) != rowCount * colCount)
            {
                Panel = new Block[rowCount, colCount];
                for (int rowIdx = 0; rowIdx <= RowMaxIdx; rowIdx++)
                {
                    for (int colIdx = 0; colIdx <= ColMaxIdx; colIdx++)
                    {
                        Panel[rowIdx, colIdx] = new Block();
                    }
                }
            }
            for (int rowIdx = 0; rowIdx <= RowMaxIdx; rowIdx++)
            {
                for (int colIdx = 0; colIdx <= ColMaxIdx; colIdx++)
                {
                    Panel[rowIdx, colIdx].Init(rowIdx, colIdx);
                }
            }
        }
    }
    #endregion
    #region 获取与设置Block
    public partial class ChessBoard
    {
        public static Block GetBlock(int rowIdx, int colIdx)
        {
            return Panel[rowIdx, colIdx];
        }
        public static Block[] GetBlocks()
        {
            Block[] data = new Block[RowCount * ColCount];
            foreach (var rowIdx in RowIdxArray)
            {
                foreach (var colIdx in ColIdxArray)
                {
                    data[colIdx + rowIdx * ColCount] = Panel[rowIdx, colIdx];
                }
            }
            return data;
        }
        public static List<Block> GetBlocks(bool isFilled)
        {
            List<Block> blocks = new List<Block>();
            foreach (var block in Panel)
            {
                if (block.IsFilled == isFilled && !blocks.Contains(block))
                {
                    blocks.Add(block);
                }
            }
            return blocks;
        }
        public static Block[] GetRow(int rowIdx)
        {
            Block[] data = new Block[ColCount];
            foreach (var colIdx in ColIdxArray)
            {
                data[colIdx] = Panel[rowIdx, colIdx];
            }
            return data;
        }
        public static List<Block> GetRow(int rowIdx, ShapePreview except)
        {
            List<Block> data = new List<Block>();
            foreach (var colIdx in ColIdxArray)
            {
                if (except == null || !except.BlockContainRowCol(rowIdx, colIdx))
                {
                    data.Add(Panel[rowIdx, colIdx]);
                }
            }
            return data;
        }
        public static Block[] GetCol(int colIdx)
        {
            Block[] data = new Block[RowCount];
            foreach (var rowIdx in RowIdxArray)
            {
                data[rowIdx] = Panel[rowIdx, colIdx];
            }
            return data;
        }
        public static List<Block> GetCol(int colIdx, ShapePreview except)
        {
            List<Block> data = new List<Block>();
            foreach (var rowIdx in RowIdxArray)
            {
                if (except == null || !except.BlockContainRowCol(rowIdx, colIdx))
                {
                    data.Add(Panel[rowIdx, colIdx]);
                }
            }
            return data;
        }
    }
    public partial class ChessBoard
    {
        public static bool IsEnable(int rowIdx, int colIdx)
        {
            return Panel[rowIdx, colIdx].IsEnable;
        }
        public static bool IsDisable(int rowIdx, int colIdx)
        {
            return Panel[rowIdx, colIdx].IsDisable;
        }
        public static bool IsEmpty(int rowIdx, int colIdx)
        {
            return Panel[rowIdx, colIdx].IsEmpty;
        }
        public static bool IsFilled(int rowIdx, int colIdx)
        {
            return Panel[rowIdx, colIdx].IsFilled;
        }
    }
    public partial class ChessBoard
    {
        public static void SetupEnableState(int rowIdx, int colIdx, bool enable)
        {
            Panel[rowIdx, colIdx].SetupEnableState(enable);
        }
        public static void SetupHP(int rowIdx, int colIdx, uint hp)
        {
            Panel[rowIdx, colIdx].SetupHP(hp);
        }
        public static void SetupFillType(int rowIdx, int colIdx, FillType fillType)
        {
            Panel[rowIdx, colIdx].SetupFillType(fillType);
        }
        public static void SetupFillState(int rowIdx, int colIdx, FillState fillState)
        {
            Panel[rowIdx, colIdx].SetupFillState(fillState);
        }
        public static void SetupFillData(int rowIdx, int colIdx, uint hp, FillType fillType, FillState fillState)
        {
            Panel[rowIdx, colIdx].SetupFillData(hp, fillType, fillState);
        }
    }
    #endregion
    #region 填充进度
    public partial class ChessBoard
    {
        private static FillProgress GetFillProgressType(Block[] blocks)
        {
            if (blocks == null || blocks.Length == 0)
            {
                return FillProgress.Empty;
            }
            else
            {
                int totalCount = 0;
                int emptyCount = 0;
                foreach (var block in blocks)
                {
                    if (block.IsEnable)
                    {
                        totalCount += 1;
                        if (block.IsEmpty)
                        {
                            emptyCount += 1;
                        }
                    }
                }
                if (totalCount == 0 || emptyCount >= totalCount)
                {
                    return FillProgress.Empty;
                }
                if (emptyCount <= 0)
                {
                    return FillProgress.Full;
                }
                return FillProgress.Part;
            }
        }
        public static FillProgress GetRowFillProgressType(int rowIdx)
        {
            return GetFillProgressType(GetRow(rowIdx));
        }
        public static FillProgress GetRowFillProgressType(int rowIdx, ShapePreview except)
        {
            List<Block> blocks = GetRow(rowIdx, except);
            if (blocks == null || blocks.Count == 0)
            {
                return FillProgress.Empty;
            }
            else
            {
                return GetFillProgressType(blocks.ToArray());
            }
        }
        public static FillProgress GetColFillProgressType(int colIdx)
        {
            return GetFillProgressType(GetCol(colIdx));
        }
        public static FillProgress GetColFillProgressType(int colIdx, ShapePreview except)
        {
            List<Block> blocks = GetCol(colIdx, except);
            if (blocks == null || blocks.Count == 0)
            {
                return FillProgress.Empty;
            }
            else
            {
                return GetFillProgressType(blocks.ToArray());
            }
        }

        public static List<int> GetRowIdxList(FillProgress fillProgressType)
        {
            List<int> list = new List<int>();
            foreach (var rowIdx in RowIdxArray)
            {
                if (GetRowFillProgressType(rowIdx) == fillProgressType && !list.Contains(rowIdx))
                {
                    list.Add(rowIdx);
                }
            }
            return list;
        }
        public static List<int> GetRowIdxList(FillProgress fillProgressType, ShapePreview except)
        {
            List<int> list = new List<int>();
            foreach (var rowIdx in RowIdxArray)
            {
                if (GetRowFillProgressType(rowIdx, except) == fillProgressType && !list.Contains(rowIdx))
                {
                    list.Add(rowIdx);
                }
            }
            return list;
        }
        public static List<int> GetColIdxList(FillProgress fillProgressType)
        {
            List<int> list = new List<int>();
            foreach (var colIdx in ColIdxArray)
            {
                if (GetColFillProgressType(colIdx) == fillProgressType && !list.Contains(colIdx))
                {
                    list.Add(colIdx);
                }
            }
            return list;
        }
        public static List<int> GetColIdxList(FillProgress fillProgressType, ShapePreview except)
        {
            List<int> list = new List<int>();
            foreach (var colIdx in ColIdxArray)
            {
                if (GetColFillProgressType(colIdx, except) == fillProgressType && !list.Contains(colIdx))
                {
                    list.Add(colIdx);
                }
            }
            return list;
        }
        public static (List<int> rowIdxList, List<int> colIdxList) GetRowColIdxList(FillProgress fillProgressType)
        {
            List<int> rowList = new List<int>();
            foreach (var rowIdx in RowIdxArray)
            {
                if (GetRowFillProgressType(rowIdx) == fillProgressType && !rowList.Contains(rowIdx))
                {
                    rowList.Add(rowIdx);
                }
            }
            List<int> colList = new List<int>();
            foreach (var colIdx in ColIdxArray)
            {
                if (GetColFillProgressType(colIdx) == fillProgressType && !colList.Contains(colIdx))
                {
                    colList.Add(colIdx);
                }
            }
            return (rowList, colList);
        }
        public static (List<int> rowIdxList, List<int> colIdxList) GetRowColIdxList(FillProgress fillProgressType, ShapePreview except)
        {
            List<int> rowList = new List<int>();
            foreach (var rowIdx in RowIdxArray)
            {
                if (GetRowFillProgressType(rowIdx, except) == fillProgressType && !rowList.Contains(rowIdx))
                {
                    rowList.Add(rowIdx);
                }
            }
            List<int> colList = new List<int>();
            foreach (var colIdx in ColIdxArray)
            {
                if (GetColFillProgressType(colIdx, except) == fillProgressType && !colList.Contains(colIdx))
                {
                    colList.Add(colIdx);
                }
            }
            return (rowList, colList);
        }
    }
    #endregion
    #region 消除
    public partial class ChessBoard
    {
        public static bool IsCanEliminate => GetRowIdxList(FillProgress.Full).Count > 0 || GetColIdxList(FillProgress.Full).Count > 0;

        public static (List<int> rowIdxList, List<int> colIdxList, List<(int rowIdx, int colIdx, FillType)> blocks) Eliminate()
        {
            List<int> rows = GetRowIdxList(FillProgress.Full);
            List<int> cols = GetColIdxList(FillProgress.Full);
            List<(int rowIdx, int colIdx, FillType)> blocks = new List<(int rowIdx, int colIdx, FillType)>();
            foreach (var (rowIdx, colIdx) in SubScriptRowCol)
            {
                if (rows.Contains(rowIdx) || cols.Contains(colIdx))
                {
                    var block = GetBlock(rowIdx, colIdx);
                    blocks.Add((rowIdx, colIdx, block.FillType));
                    block.Eliminate();
                }
            }
            return (rows, cols, blocks);
        }
    }
    #endregion
}