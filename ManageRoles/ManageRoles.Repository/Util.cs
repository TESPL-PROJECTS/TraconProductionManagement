using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageRoles.Repository
{
    public class Util
    {
        private DataTable Excel_To_DataTable(string pRutaArchivo, int pHojaIndex)
        {
            // --------------------------------- //
            /* REFERENCIAS:
             * NPOI.dll
             * NPOI.OOXML.dll
             * NPOI.OpenXml4Net.dll */
            // --------------------------------- //
            /* USING:
             * using NPOI.SS.UserModel;
             * using NPOI.HSSF.UserModel;
             * using NPOI.XSSF.UserModel; */
            // --------------------------------- //
            DataTable Tabla = null;
            try
            {
                if (System.IO.File.Exists(pRutaArchivo))
                {

                    IWorkbook workbook = null;
                    ISheet worksheet = null;
                    string first_sheet_name = "";

                    using (FileStream FS = new FileStream(pRutaArchivo, FileMode.Open, FileAccess.Read))
                    {
                        workbook = WorkbookFactory.Create(FS);
                        worksheet = workbook.GetSheetAt(pHojaIndex);
                        first_sheet_name = worksheet.SheetName;

                        Tabla = new DataTable(first_sheet_name);
                        Tabla.Rows.Clear();
                        Tabla.Columns.Clear();

                        for (int rowIndex = 0; rowIndex <= worksheet.LastRowNum; rowIndex++)
                        {
                            DataRow NewReg = null;
                            IRow row = worksheet.GetRow(rowIndex);
                            IRow row2 = null;

                            if (row != null)
                            {
                                if (rowIndex > 0) NewReg = Tabla.NewRow();

                                foreach (ICell cell in row.Cells)
                                {
                                    object valorCell = null;
                                    string cellType = "";

                                    if (rowIndex == 0)
                                    {
                                        row2 = worksheet.GetRow(rowIndex + 1);
                                        ICell cell2 = row2.GetCell(cell.ColumnIndex);
                                        switch (cell2.CellType)
                                        {
                                            case CellType.Boolean: cellType = "System.Boolean"; break;
                                            case CellType.String: cellType = "System.String"; break;
                                            case CellType.Numeric:
                                                if (HSSFDateUtil.IsCellDateFormatted(cell2)) { cellType = "System.DateTime"; }
                                                else { cellType = "System.Double"; }
                                                break;
                                            case CellType.Formula:
                                                switch (cell2.CachedFormulaResultType)
                                                {
                                                    case CellType.Boolean: cellType = "System.Boolean"; break;
                                                    case CellType.String: cellType = "System.String"; break;
                                                    case CellType.Numeric:
                                                        if (HSSFDateUtil.IsCellDateFormatted(cell2)) { cellType = "System.DateTime"; }
                                                        else { cellType = "System.Double"; }
                                                        break;
                                                }
                                                break;
                                            default:
                                                cellType = "System.String"; break;
                                        }
                                        
                                        DataColumn codigo = new DataColumn(cell.StringCellValue, System.Type.GetType(cellType));
                                        Tabla.Columns.Add(codigo);
                                    }
                                    else
                                    {
                                        switch (cell.CellType)
                                        {
                                            case CellType.Blank: valorCell = DBNull.Value; break;
                                            case CellType.Boolean: valorCell = cell.BooleanCellValue; break;
                                            case CellType.String: valorCell = cell.StringCellValue; break;
                                            case CellType.Numeric:
                                                if (HSSFDateUtil.IsCellDateFormatted(cell)) { valorCell = cell.DateCellValue; }
                                                else { valorCell = cell.NumericCellValue; }
                                                break;
                                            case CellType.Formula:
                                                switch (cell.CachedFormulaResultType)
                                                {
                                                    case CellType.Blank: valorCell = DBNull.Value; break;
                                                    case CellType.String: valorCell = cell.StringCellValue; break;
                                                    case CellType.Boolean: valorCell = cell.BooleanCellValue; break;
                                                    case CellType.Numeric:
                                                        if (HSSFDateUtil.IsCellDateFormatted(cell)) { valorCell = cell.DateCellValue; }
                                                        else { valorCell = cell.NumericCellValue; }
                                                        break;
                                                }
                                                break;
                                            default: valorCell = cell.StringCellValue; break;
                                        }
                                        NewReg[cell.ColumnIndex] = valorCell;
                                    }
                                }
                            }
                            if (rowIndex > 0) Tabla.Rows.Add(NewReg);
                        }
                        Tabla.AcceptChanges();
                    }
                }
                else
                {
                    throw new Exception("ERROR 404:");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Tabla;
        }
    }
}
