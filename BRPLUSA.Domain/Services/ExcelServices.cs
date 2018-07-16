using BRPLUSA.Domain.ExcelWrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;


namespace BRPLUSA.Domain.Services
{
    public static class ExcelServices
    {
        static Excel.Application app = null;
        static Excel.Workbooks books = null;
        static Excel.Workbook book = null;
        static Excel.Worksheet sheet = null;
        static Excel.Worksheets sheets = null;
        static Excel.Range range = null;
        static Excel.Range rows = null;
        static Excel.Range columns = null;
        static Excel.Range cellData = null;

        public static string[][] GetSheetData(string fileName, int sheetIndex)
        {
            try
            {
                app = new Excel.Application();
                books = app.Workbooks;
                book = books.Open(fileName);
                sheet = GetSheet(sheetIndex, book);
                range = sheet.UsedRange;
                rows = range.Rows;
                columns = range.Columns;
                cellData = range.Cells;

                //var data = CreateSheetDataWrapper(cellData);
                //return data;
                throw new Exception();
            }

            catch(Exception e)
            {

            }

            finally
            {
                DisposeAll();
            }

            return null;
        }

        public static string[][] GetSheetData(string fileName, string sheetName)
        {
            try
            {
                app = new Excel.Application();
                books = app.Workbooks;
                book = books.Open(fileName);
                sheets = (Excel.Worksheets)book.Sheets;
                sheet = sheets[sheetName];
                range = sheet.UsedRange;
                rows = range.Rows;
                columns = range.Columns;
                cellData = range.Cells;

                CreateSheetDataWrapper(cellData);
            }

            catch (Exception e)
            {

            }

            finally
            {
                DisposeAll();
            }

            return null;
        }

        public static void CreateSheetDataWrapper(Excel.Range cells)
        {
            var table = new TableWrapper(columns.Count, rows.Count);

            for (int i = 1; i < columns.Count; i++)
            {
                for (int j = 1; j < rows.Count; j++)
                {
                    var cell = ((Excel.Range) cells.Item[j,i]);

                    var wrapper = new CellWrapper
                    {
                        ColumnWidth = cell.ColumnWidth,
                        RowHeight = cell.RowHeight,
                        Position = new Tuple<int, int>(i, j),
                        Data = cell.Value2
                    };

                    var value = cell.Value2;
                    var cWidth = cell.ColumnWidth;
                    var rHeight = cell.RowHeight;
                    
                    Console.WriteLine($"Cell location is: {i},{j}");
                    Console.WriteLine($"Cell value is: {value}");
                    Console.WriteLine($"Cell Width is: {cWidth}");
                    Console.WriteLine($"Cell height is: {rHeight}");
                    Console.WriteLine();

                    table.Data[i][j] = wrapper;
                }
            }
        }

        public static int GetRowCount(string fileName, string sheetName)
        {
            int count = 0;

            try
            {
                app = new Excel.Application();
                books = app.Workbooks;
                book = books.Open(fileName);
                sheet = GetSheet(sheetName, book);
                range = sheet.UsedRange;
                rows = range.Rows;
                count = rows.Count;
            }

            catch (Exception e)
            {

            }

            finally
            {
                DisposeAll();
            }

            return count;
        }

        public static int GetRowCount(string fileName, int sheetIndex)
        {
            int count = 0;

            try
            {
                app = new Excel.Application();
                books = app.Workbooks;
                book = books.Open(fileName);
                sheet = GetSheet(sheetIndex, book);
                range = sheet.UsedRange;
                rows = range.Rows;
                count = rows.Count;
            }

            catch (Exception e)
            {

            }

            finally
            {
                DisposeAll();
            }

            return count;
        }

        public static int GetColumnCount(Excel.Worksheet sheet)
        {
            return sheet.UsedRange.Columns.Count;
        }

        public static Excel.Worksheet GetSheet(int index, Excel.Workbook book)
        {
            Excel.Worksheet req = null;
            var sheets = book.Sheets;
            var count = sheets.Count;

            if (index > count)
                throw new Exception("Sheet is not in book - out of range");

            var iter = sheets.GetEnumerator();
            var num = 0;
            while (iter.MoveNext())
            {
                if (num != index)
                {
                    num++;
                    continue;
                }

                var item = iter.Current;
                req = (Excel.Worksheet)item;
                
            }

            if(book != null)
                Marshal.ReleaseComObject(book);

            if(sheets != null)
                Marshal.ReleaseComObject(sheets);

            return req;
        }

        public static Excel.Worksheet GetSheet(string sheetName, Excel.Workbook book)
        {
            sheets = (Excel.Worksheets) book.Sheets;
            sheet = book.Sheets[sheetName];

            if (sheets != null)
                Marshal.ReleaseComObject(sheets);

            return sheet;
        }

        public static void DisposeAll()
        {
            if (app != null)
            {
                try
                {
                    app.Quit();
                }
                catch (Exception e)
                {

                }

                Marshal.ReleaseComObject(app);
            }

            if (books != null)
                Marshal.ReleaseComObject(books);

            if (book != null)
                Marshal.ReleaseComObject(book);

            if (sheet != null)
                Marshal.ReleaseComObject(sheet);

            if (range != null)
                Marshal.ReleaseComObject(range);

            if (rows != null)
                Marshal.ReleaseComObject(rows);

            if (columns != null)
                Marshal.ReleaseComObject(columns);
        }
    }
}
