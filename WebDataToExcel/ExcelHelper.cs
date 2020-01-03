using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WebDataToExcel
{
    public static class ExcelHelper
    {
        public static void ExportToExcel(DataTable dt)
        {
            ExportToExcelData(dt);
        }

        public static DataTable ToDataTable(this DataGridView myDGV)
        {
            DataTable dt = new DataTable();
            for (int i = 0; i < myDGV.ColumnCount; i++)
            {
                dt.Columns.Add(myDGV.Columns[i].HeaderText);
            }
            //写入数值
            for (int r = 0; r < myDGV.Rows.Count; r++)
            {
                List<object> values = new List<object>();
                for (int i = 0; i < myDGV.ColumnCount; i++)
                {
                    values.Add(myDGV.Rows[r].Cells[i].Value);
                }
                dt.Rows.Add(values.ToArray());
            }
            return dt;
        }
        #region 导出
        /// <summary>
        /// 数据导出
        /// </summary>
        /// <param name="data"></param>
        /// <param name="sheetName"></param>
        public static void ExportToExcelData(this DataTable data)
        {
            ExportToExcel(data, "Sheet1");
        }
        /// <summary>
        /// 数据导出
        /// </summary>
        /// <param name="data"></param>
        /// <param name="sheetName"></param>
        public static void ExportToExcel(this DataTable data, string sheetName)
        {
            SaveFileDialog fileDialog = new SaveFileDialog();
            //fileDialog.Filter = "Excel(97-2003)|*.xls|Excel(2007-2013)|*.xlsx";
            fileDialog.Filter = "Excel|*.xls|Excel|*.xlsx";
            if (fileDialog.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
            {
                return;
            }
            IWorkbook workbook = new XSSFWorkbook();
            ISheet sheet = workbook.CreateSheet(sheetName);
            IRow rowHead = sheet.CreateRow(0);

            //填写表头
            for (int i = 0; i < data.Columns.Count; i++)
            {
                rowHead.CreateCell(i, CellType.String).SetCellValue(data.Columns[i].ColumnName.ToString());
            }
            //填写内容
            for (int i = 0; i < data.Rows.Count; i++)
            {
                IRow row = sheet.CreateRow(i + 1);
                for (int j = 0; j < data.Columns.Count; j++)
                {
                    row.CreateCell(j, CellType.String).SetCellValue(data.Rows[i][j].ToString());
                }
            }

            for (int i = 0; i < data.Columns.Count; i++)
            {
                sheet.AutoSizeColumn(i);
            }

            using (FileStream stream = File.OpenWrite(fileDialog.FileName))
            {
                workbook.Write(stream);
                stream.Close();
            }
            MessageBox.Show("导出数据成功!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            GC.Collect();
        }
        #endregion

        #region 导入
        /// <summary>
        /// 导入的文件名
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static DataSet ExcelToDataSet(string fileName)
        {
            return ExcelToDataSet(fileName, true);
        }
        /// <summary>
        /// 返回dataset
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="firstRowAsHeader"></param>
        /// <returns></returns>
        public static DataSet ExcelToDataSet(string fileName, bool firstRowAsHeader)
        {
            int sheetCount = 0;
            return ExcelToDataSet(fileName, firstRowAsHeader, out sheetCount);
        }
        /// <summary>
        /// 返回dataset
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="firstRowAsHeader">文件头</param>
        /// <param name="sheetCount">内容</param>
        /// <returns></returns>
        public static DataSet ExcelToDataSet(string fileName, bool firstRowAsHeader, out int sheetCount)
        {
            using (DataSet ds = new DataSet())
            {
                using (FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                {
                    IWorkbook workbook = WorkbookFactory.Create(fileStream);
                    IFormulaEvaluator evaluator = WorkbookFactory.CreateFormulaEvaluator(workbook);

                    sheetCount = workbook.NumberOfSheets;

                    for (int i = 0; i < sheetCount; ++i)
                    {
                        ISheet sheet = workbook.GetSheetAt(i);
                        DataTable dt = ExcelToDataTable(sheet, evaluator, firstRowAsHeader);
                        ds.Tables.Add(dt);
                    }
                    return ds;
                }
            }
        }
        /// <summary>
        /// 返回DataTable
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="evaluator"></param>
        /// <param name="firstRowAsHeader"></param>
        /// <returns></returns>
        private static DataTable ExcelToDataTable(ISheet sheet, IFormulaEvaluator evaluator, bool firstRowAsHeader)
        {
            if (firstRowAsHeader)
            {
                return ExcelToDataTableFirstRowAsHeader(sheet, evaluator);
            }
            else
            {
                return ExcelToDataTable(sheet, evaluator);
            }
        }
        private static DataTable ExcelToDataTableFirstRowAsHeader(ISheet sheet, IFormulaEvaluator evaluator)
        {
            try
            {
                using (DataTable dt = new DataTable())
                {
                    IRow firstRow = sheet.GetRow(0);
                    int cellCount = GetCellCount(sheet);

                    for (int i = 0; i < cellCount; i++)
                    {
                        if (firstRow.GetCell(i) != null)
                        {
                            dt.Columns.Add(firstRow.GetCell(i).StringCellValue ?? string.Format("F{0}", i + 1), typeof(string));
                        }
                        else
                        {
                            dt.Columns.Add(string.Format("F{0}", i + 1), typeof(string));
                        }
                    }

                    for (int i = 1; i <= sheet.LastRowNum; i++)
                    {
                        IRow row = sheet.GetRow(i);
                        DataRow dr = dt.NewRow();
                        FillDataRowByRow(row, evaluator, ref dr);
                        dt.Rows.Add(dr);
                    }

                    dt.TableName = sheet.SheetName;
                    return dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }
        private static DataTable ExcelToDataTable(ISheet sheet, IFormulaEvaluator evaluator)
        {
            using (DataTable dt = new DataTable())
            {
                if (sheet.LastRowNum != 0)
                {
                    int cellCount = GetCellCount(sheet);

                    for (int i = 0; i < cellCount; i++)
                    {
                        dt.Columns.Add(string.Format("F{0}", i), typeof(string));
                    }

                    for (int i = 0; i < sheet.FirstRowNum; ++i)
                    {
                        DataRow dr = dt.NewRow();
                        dt.Rows.Add(dr);
                    }

                    for (int i = sheet.FirstRowNum; i <= sheet.LastRowNum; i++)
                    {
                        IRow row = sheet.GetRow(i);
                        DataRow dr = dt.NewRow();
                        FillDataRowByRow(row, evaluator, ref dr);
                        dt.Rows.Add(dr);
                    }
                }

                dt.TableName = sheet.SheetName;
                return dt;
            }
        }
        /// <summary>
        /// 填充数据
        /// </summary>
        /// <param name="row"></param>
        /// <param name="evaluator"></param>
        /// <param name="dr"></param>
        private static void FillDataRowByRow(IRow row, IFormulaEvaluator evaluator, ref DataRow dr)
        {
            if (row != null)
            {
                for (int j = 0; j < dr.Table.Columns.Count; j++)
                {
                    ICell cell = row.GetCell(j);

                    if (cell != null)
                    {
                        switch (cell.CellType)
                        {
                            case CellType.Blank:
                                {
                                    dr[j] = DBNull.Value;
                                    break;
                                }
                            case CellType.Boolean:
                                {
                                    dr[j] = cell.BooleanCellValue;
                                    break;
                                }
                            case CellType.Numeric:
                                {
                                    if (DateUtil.IsCellDateFormatted(cell))
                                    {
                                        dr[j] = cell.DateCellValue;
                                    }
                                    else
                                    {
                                        dr[j] = cell.NumericCellValue;
                                    }
                                    break;
                                }
                            case CellType.String:
                                {
                                    dr[j] = cell.StringCellValue;
                                    break;
                                }
                            case CellType.Error:
                                {
                                    dr[j] = cell.ErrorCellValue;
                                    break;
                                }
                            case CellType.Formula:
                                {
                                    cell = evaluator.EvaluateInCell(cell) as HSSFCell;
                                    dr[j] = cell.ToString();
                                    break;
                                }
                            default:
                                throw new NotSupportedException(string.Format("Unsupported format type:{0}", cell.CellType));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 获取单元格
        /// </summary>
        /// <param name="sheet"></param>
        /// <returns></returns>
        private static int GetCellCount(ISheet sheet)
        {
            int firstRowNum = sheet.FirstRowNum;

            int cellCount = 0;

            for (int i = sheet.FirstRowNum; i <= sheet.LastRowNum; ++i)
            {
                IRow row = sheet.GetRow(i);

                if (row != null && row.LastCellNum > cellCount)
                {
                    cellCount = row.LastCellNum;
                }
            }
            return cellCount;
        }


        #endregion

        #region 转换

        /// <summary>
        /// DataTable转成List
        /// </summary>
        public static List<T> ToDataList<T>(this DataTable dt)
        {
            var list = new List<T>();
            var plist = new List<PropertyInfo>(typeof(T).GetProperties());

            if (dt == null || dt.Rows.Count == 0)
            {
                return null;
            }

            foreach (DataRow item in dt.Rows)
            {
                T s = Activator.CreateInstance<T>();
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    PropertyInfo info = plist.Find(p => p.Name == dt.Columns[i].ColumnName);
                    if (info != null)
                    {
                        try
                        {
                            if (!Convert.IsDBNull(item[i]))
                            {
                                object v = null;
                                if (info.PropertyType.ToString().Contains("System.Nullable"))
                                {
                                    v = Convert.ChangeType(item[i], Nullable.GetUnderlyingType(info.PropertyType));
                                }
                                else
                                {
                                    v = Convert.ChangeType(item[i], info.PropertyType);
                                }
                                info.SetValue(s, v, null);
                            }
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("字段[" + info.Name + "]转换出错," + ex.Message);
                        }
                    }
                }
                list.Add(s);
            }
            return list;
        }

        /// <summary>
        /// DataTable转成实体对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static T ToDataEntity<T>(this DataTable dt)
        {
            T s = Activator.CreateInstance<T>();
            if (dt == null || dt.Rows.Count == 0)
            {
                return default(T);
            }
            var plist = new List<PropertyInfo>(typeof(T).GetProperties());
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                PropertyInfo info = plist.Find(p => p.Name == dt.Columns[i].ColumnName);
                if (info != null)
                {
                    try
                    {
                        if (!Convert.IsDBNull(dt.Rows[0][i]))
                        {
                            object v = null;
                            if (info.PropertyType.ToString().Contains("System.Nullable"))
                            {
                                v = Convert.ChangeType(dt.Rows[0][i], Nullable.GetUnderlyingType(info.PropertyType));
                            }
                            else
                            {
                                v = Convert.ChangeType(dt.Rows[0][i], info.PropertyType);
                            }
                            info.SetValue(s, v, null);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("字段[" + info.Name + "]转换出错," + ex.Message);
                    }
                }
            }
            return s;
        }

        /// <summary>
        /// List转成DataTable
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entities">实体集合</param>
        public static DataTable ToDataTable<T>(List<T> entities)
        {
            if (entities == null || entities.Count == 0)
            {
                return null;
            }

            var result = CreateTable<T>();
            FillData(result, entities);
            return result;
        }

        /// <summary>
        /// 创建表
        /// </summary>
        private static DataTable CreateTable<T>()
        {
            var result = new DataTable();
            var type = typeof(T);
            foreach (var property in type.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance))
            {
                var propertyType = property.PropertyType;
                if ((propertyType.IsGenericType) && (propertyType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                    propertyType = propertyType.GetGenericArguments()[0];
                result.Columns.Add(property.Name, propertyType);
            }
            return result;
        }

        /// <summary>
        /// 填充数据
        /// </summary>
        private static void FillData<T>(DataTable dt, IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                dt.Rows.Add(CreateRow(dt, entity));
            }
        }

        /// <summary>
        /// 创建行
        /// </summary>
        private static DataRow CreateRow<T>(DataTable dt, T entity)
        {
            DataRow row = dt.NewRow();
            var type = typeof(T);
            foreach (var property in type.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance))
            {
                row[property.Name] = property.GetValue(entity) ?? DBNull.Value;
            }
            return row;
        }
        #endregion

    }


}

