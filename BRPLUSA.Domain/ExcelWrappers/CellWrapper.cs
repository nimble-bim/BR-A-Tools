using System;
using System.Collections.Generic;

namespace BRPLUSA.Domain.ExcelWrappers
{
    public class CellWrapper
    {
        public double ColumnWidth { get; set; }
        public double RowHeight { get; set; }
        public string Data { get; set; }
        /// <summary>
        /// Always in Column | Row syntax
        /// </summary>
        public Tuple<int, int> Position { get; set; }
    }

    //public class ColumnWrapper
    //{
    //    public float Width { get; set; }
    //    public CellWrapper[] Data { get; set; }

    //    public ColumnWrapper(params CellWrapper[] cells)
    //    {
    //        Data = cells;
    //    }

    //    public ColumnWrapper() { }
    //}

    public class TableWrapper
    {
        public CellWrapper[][] Data { get; set; }

        public TableWrapper(int columns, int rows)
        {
            Initialize(columns, rows);
        }

        private void Initialize(int columns, int rows)
        {
            Data = new CellWrapper[columns][];

            for (int c = 0; c < Data.Length; c++)
            {
                Data[c] = new CellWrapper[rows];
            }
        }

        //public TableWrapper()
        //{
        //    Data = new List<ColumnWrapper>();
        //}
        //public SheetWrapper(params ColumnWrapper[] columns)
        //{
        //    Data = columns;
        //}
    }
}
