using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace Infrastructure.Excel;

/// <summary>
/// Excel utility class providing common Excel operations for ASP.NET Web API
/// </summary>
public class ExcelUtils
{
    static ExcelUtils()
    {
        // EPPlus requires this license setting
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // For non-commercial use
        // Use LicenseContext.Commercial for commercial applications with a proper license
    }

    #region Read Excel

    /// <summary>
    /// Read Excel file and convert to DataTable
    /// </summary>
    public static async Task<DataTable> ReadExcelToDataTableAsync(
        IFormFile file,
        int worksheetIndex = 0,
        bool hasHeader = true
    )
    {
        var dataTable = new DataTable();

        using (var stream = new MemoryStream())
        {
            await file.CopyToAsync(stream);
            using (var package = new ExcelPackage(stream))
            {
                // Get the first worksheet or the specified worksheet
                var worksheet = package.Workbook.Worksheets[worksheetIndex];

                // Get dimensions of the worksheet
                var startRow = hasHeader ? 2 : 1;
                var endRow = worksheet.Dimension.End.Row;
                var startCol = worksheet.Dimension.Start.Column;
                var endCol = worksheet.Dimension.End.Column;

                // Create columns in the DataTable
                if (hasHeader)
                {
                    for (int col = startCol; col <= endCol; col++)
                    {
                        var columnName =
                            worksheet.Cells[1, col].Value?.ToString() ?? $"Column{col}";
                        dataTable.Columns.Add(columnName);
                    }
                }
                else
                {
                    for (int col = startCol; col <= endCol; col++)
                    {
                        dataTable.Columns.Add($"Column{col}");
                    }
                }

                // Add rows to the DataTable
                for (int row = startRow; row <= endRow; row++)
                {
                    var dataRow = dataTable.NewRow();
                    for (int col = startCol; col <= endCol; col++)
                    {
                        dataRow[col - startCol] = worksheet.Cells[row, col].Value ?? DBNull.Value;
                    }
                    dataTable.Rows.Add(dataRow);
                }
            }
        }

        return dataTable;
    }

    /// <summary>
    /// Read Excel file and convert to a List of type T
    /// </summary>
    public static async Task<List<T>> ReadExcelToListAsync<T>(
        IFormFile file,
        int worksheetIndex = 0
    )
        where T : new()
    {
        var result = new List<T>();
        var properties = typeof(T).GetProperties();

        using (var stream = new MemoryStream())
        {
            await file.CopyToAsync(stream);
            using (var package = new ExcelPackage(stream))
            {
                var worksheet = package.Workbook.Worksheets[worksheetIndex];

                // Assume first row is header
                var headerRow = 1;
                var startRow = 2;
                var endRow = worksheet.Dimension.End.Row;
                var startCol = worksheet.Dimension.Start.Column;
                var endCol = worksheet.Dimension.End.Column;

                // Create a dictionary to map headers to properties
                var headerMap = new Dictionary<string, int>();
                for (int col = startCol; col <= endCol; col++)
                {
                    var header = worksheet.Cells[headerRow, col].Value?.ToString();
                    if (!string.IsNullOrEmpty(header))
                    {
                        headerMap[header] = col;
                    }
                }

                // Process rows
                for (int row = startRow; row <= endRow; row++)
                {
                    var item = new T();

                    foreach (var prop in properties)
                    {
                        // Try to find the column matching the property name
                        if (headerMap.TryGetValue(prop.Name, out int col))
                        {
                            var value = worksheet.Cells[row, col].Value;
                            if (value != null)
                            {
                                // Convert the value to the property type
                                prop.SetValue(item, Convert.ChangeType(value, prop.PropertyType));
                            }
                        }
                    }

                    result.Add(item);
                }
            }
        }

        return result;
    }

    /// <summary>
    /// Validate Excel file basic properties
    /// </summary>
    public static (bool IsValid, string ErrorMessage) ValidateExcelFile(
        IFormFile file,
        long maxSizeInBytes = 10485760
    )
    {
        if (file == null || file.Length == 0)
            return (false, "No file uploaded");

        if (file.Length > maxSizeInBytes)
            return (false, $"File size exceeds the limit of {maxSizeInBytes / 1048576} MB");

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (extension != ".xlsx" && extension != ".xls")
            return (false, "File must be Excel format (.xlsx or .xls)");

        return (true, string.Empty);
    }

    #endregion

    #region Write Excel

    /// <summary>
    /// Create Excel file from DataTable
    /// </summary>
    public static byte[] CreateExcelFromDataTable(
        DataTable dataTable,
        string worksheetName = "Sheet1"
    )
    {
        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add(worksheetName);

            // Add headers
            for (int i = 0; i < dataTable.Columns.Count; i++)
            {
                worksheet.Cells[1, i + 1].Value = dataTable.Columns[i].ColumnName;
                worksheet.Cells[1, i + 1].Style.Font.Bold = true;
                worksheet.Cells[1, i + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[1, i + 1].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
            }

            // Add data
            for (int row = 0; row < dataTable.Rows.Count; row++)
            {
                for (int col = 0; col < dataTable.Columns.Count; col++)
                {
                    worksheet.Cells[row + 2, col + 1].Value = dataTable.Rows[row][col];
                }
            }

            // Auto fit columns
            worksheet.Cells.AutoFitColumns();

            return package.GetAsByteArray();
        }
    }

    /// <summary>
    /// Create Excel file from a List of objects
    /// </summary>
    public static byte[] CreateExcelFromList<T>(List<T> data, string worksheetName = "Sheet1")
    {
        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add(worksheetName);

            // Get properties of T
            var properties = typeof(T).GetProperties();

            // Add headers
            for (int i = 0; i < properties.Length; i++)
            {
                worksheet.Cells[1, i + 1].Value = properties[i].Name;
                worksheet.Cells[1, i + 1].Style.Font.Bold = true;
                worksheet.Cells[1, i + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[1, i + 1].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
            }

            // Add data
            for (int row = 0; row < data.Count; row++)
            {
                for (int col = 0; col < properties.Length; col++)
                {
                    worksheet.Cells[row + 2, col + 1].Value = properties[col].GetValue(data[row]);
                }
            }

            // Auto fit columns
            worksheet.Cells.AutoFitColumns();

            return package.GetAsByteArray();
        }
    }

    /// <summary>
    /// Create Excel file with multiple worksheets
    /// </summary>
    public static byte[] CreateExcelWithMultipleWorksheets(Dictionary<string, DataTable> worksheets)
    {
        using (var package = new ExcelPackage())
        {
            foreach (var item in worksheets)
            {
                var worksheet = package.Workbook.Worksheets.Add(item.Key);
                var dataTable = item.Value;

                // Add headers
                for (int i = 0; i < dataTable.Columns.Count; i++)
                {
                    worksheet.Cells[1, i + 1].Value = dataTable.Columns[i].ColumnName;
                    worksheet.Cells[1, i + 1].Style.Font.Bold = true;
                    worksheet.Cells[1, i + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[1, i + 1].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                }

                // Add data
                for (int row = 0; row < dataTable.Rows.Count; row++)
                {
                    for (int col = 0; col < dataTable.Columns.Count; col++)
                    {
                        worksheet.Cells[row + 2, col + 1].Value = dataTable.Rows[row][col];
                    }
                }

                // Auto fit columns
                worksheet.Cells.AutoFitColumns();
            }

            return package.GetAsByteArray();
        }
    }

    /// <summary>
    /// Create an Excel template with predefined structure
    /// </summary>
    public static byte[] CreateExcelTemplate(string[] headers, string worksheetName = "Template")
    {
        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add(worksheetName);

            // Add headers
            for (int i = 0; i < headers.Length; i++)
            {
                worksheet.Cells[1, i + 1].Value = headers[i];
                worksheet.Cells[1, i + 1].Style.Font.Bold = true;
                worksheet.Cells[1, i + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[1, i + 1].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
            }

            // Auto fit columns
            worksheet.Cells.AutoFitColumns();

            return package.GetAsByteArray();
        }
    }

    #endregion

    #region Helper Methods

    /// <summary>
    /// Get Excel file as FileContentResult for download
    /// </summary>
    public static FileContentResult GetExcelFileResult(
        byte[] fileContent,
        string fileName = "export.xlsx"
    )
    {
        return new FileContentResult(
            fileContent,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        )
        {
            FileDownloadName = fileName
        };
    }

    /// <summary>
    /// Apply styling to a worksheet
    /// </summary>
    public static void ApplyExcelStyling(ExcelWorksheet worksheet)
    {
        // Style header row
        var headerRange = worksheet.Cells[1, 1, 1, worksheet.Dimension.End.Column];
        headerRange.Style.Font.Bold = true;
        headerRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
        headerRange.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
        headerRange.Style.Border.Bottom.Style = ExcelBorderStyle.Medium;

        // Style data rows
        var dataRange = worksheet.Cells[
            2,
            1,
            worksheet.Dimension.End.Row,
            worksheet.Dimension.End.Column
        ];
        dataRange.Style.Border.Top.Style = ExcelBorderStyle.Thin;
        dataRange.Style.Border.Left.Style = ExcelBorderStyle.Thin;
        dataRange.Style.Border.Right.Style = ExcelBorderStyle.Thin;
        dataRange.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

        // Auto fit columns
        worksheet.Cells.AutoFitColumns();
    }

    /// <summary>
    /// Set column data type formatting
    /// </summary>
    public static void SetColumnDataFormat(
        ExcelWorksheet worksheet,
        int columnIndex,
        string dataFormat
    )
    {
        for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
        {
            worksheet.Cells[row, columnIndex].Style.Numberformat.Format = dataFormat;
        }
    }

    #endregion
}
