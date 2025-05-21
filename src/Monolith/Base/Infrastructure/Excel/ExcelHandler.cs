using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseApiReference.Abstractions.Excel;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace Infrastructure.Excel;

/// <summary>
/// Excel handler
/// </summary>
internal class ExcelHandler : IExcelHandler
{
    public async Task<DataTable> ImportExcelAsync(IFormFile file)
    {
        if (file == null || file.Length == 0)
            throw new ArgumentException("File is empty");

        return await ExcelUtils.ReadExcelToDataTableAsync(file);
    }

    public async Task<List<T>> ImportExcelToListAsync<T>(IFormFile file)
        where T : new()
    {
        if (file == null || file.Length == 0)
            throw new ArgumentException("File is empty");

        return await ExcelUtils.ReadExcelToListAsync<T>(file);
    }

    public byte[] ExportToExcel<T>(List<T> data, string worksheetName = "Sheet1")
    {
        if (data == null || !data.Any())
            throw new ArgumentException("No data to export");

        return ExcelUtils.CreateExcelFromList(data, worksheetName);
    }

    public byte[] ExportToExcel(DataTable dataTable, string worksheetName = "Sheet1")
    {
        if (dataTable == null || dataTable.Rows.Count == 0)
            throw new ArgumentException("No data to export");

        return ExcelUtils.CreateExcelFromDataTable(dataTable, worksheetName);
    }

    public byte[] GenerateExcelReport(Dictionary<string, object> data)
    {
        if (data == null || !data.Any())
            throw new ArgumentException("No data to generate report");

        using (var package = new ExcelPackage())
        {
            foreach (var item in data)
            {
                var worksheetName = item.Key;
                var worksheetData = item.Value;

                if (worksheetData is DataTable dataTable)
                {
                    var worksheet = package.Workbook.Worksheets.Add(worksheetName);

                    // Add headers
                    for (int i = 0; i < dataTable.Columns.Count; i++)
                    {
                        worksheet.Cells[1, i + 1].Value = dataTable.Columns[i].ColumnName;
                    }

                    // Add data
                    for (int row = 0; row < dataTable.Rows.Count; row++)
                    {
                        for (int col = 0; col < dataTable.Columns.Count; col++)
                        {
                            worksheet.Cells[row + 2, col + 1].Value = dataTable.Rows[row][col];
                        }
                    }

                    ExcelUtils.ApplyExcelStyling(worksheet);
                }
                else if (worksheetData is IEnumerable<object> listData)
                {
                    var worksheet = package.Workbook.Worksheets.Add(worksheetName);

                    // Try to get first item to determine properties
                    var firstItem = listData.FirstOrDefault();
                    if (firstItem != null)
                    {
                        var properties = firstItem.GetType().GetProperties();

                        // Add headers
                        for (int i = 0; i < properties.Length; i++)
                        {
                            worksheet.Cells[1, i + 1].Value = properties[i].Name;
                        }

                        // Add data
                        int row = 2;
                        foreach (var dataItem in listData)
                        {
                            for (int col = 0; col < properties.Length; col++)
                            {
                                worksheet.Cells[row, col + 1].Value = properties[col]
                                    .GetValue(dataItem);
                            }
                            row++;
                        }

                        ExcelUtils.ApplyExcelStyling(worksheet);
                    }
                }
            }

            return package.GetAsByteArray();
        }
    }

    public async Task<byte[]> MergeExcelFilesAsync(List<IFormFile> files)
    {
        if (files == null || !files.Any())
            throw new ArgumentException("No files to merge");

        using (var package = new ExcelPackage())
        {
            foreach (var file in files)
            {
                using (var stream = new MemoryStream())
                {
                    await file.CopyToAsync(stream);
                    stream.Position = 0;

                    using (var sourcePackage = new ExcelPackage(stream))
                    {
                        foreach (var sourceWorksheet in sourcePackage.Workbook.Worksheets)
                        {
                            // Create a new worksheet with the same name in the target package
                            // If a worksheet with the same name exists, append a number
                            string newWorksheetName = sourceWorksheet.Name;
                            int counter = 1;

                            while (
                                package.Workbook.Worksheets.Any(ws =>
                                    ws.Name.Equals(
                                        newWorksheetName,
                                        StringComparison.OrdinalIgnoreCase
                                    )
                                )
                            )
                            {
                                newWorksheetName = $"{sourceWorksheet.Name}_{counter++}";
                            }

                            var targetWorksheet = package.Workbook.Worksheets.Add(newWorksheetName);

                            // Copy the source worksheet to the target worksheet
                            var sourceRange = sourceWorksheet.Cells[
                                1,
                                1,
                                sourceWorksheet.Dimension.End.Row,
                                sourceWorksheet.Dimension.End.Column
                            ];
                            var targetRange = targetWorksheet.Cells[
                                1,
                                1,
                                sourceWorksheet.Dimension.End.Row,
                                sourceWorksheet.Dimension.End.Column
                            ];

                            // Copy values
                            targetRange.Value = sourceRange.Value;

                            // Copy formatting
                            targetRange.Style.Font.SetFromFont(
                                sourceRange.Style.Font.Name,
                                sourceRange.Style.Font.Size
                            );

                            // Try to copy more style properties
                            for (int row = 1; row <= sourceWorksheet.Dimension.End.Row; row++)
                            {
                                for (
                                    int col = 1;
                                    col <= sourceWorksheet.Dimension.End.Column;
                                    col++
                                )
                                {
                                    targetWorksheet.Cells[row, col].Style.Font.Bold =
                                        sourceWorksheet.Cells[row, col].Style.Font.Bold;
                                    targetWorksheet.Cells[row, col].Style.Border.Top.Style =
                                        sourceWorksheet.Cells[row, col].Style.Border.Top.Style;
                                    targetWorksheet.Cells[row, col].Style.Border.Bottom.Style =
                                        sourceWorksheet.Cells[row, col].Style.Border.Bottom.Style;
                                    targetWorksheet.Cells[row, col].Style.Border.Left.Style =
                                        sourceWorksheet.Cells[row, col].Style.Border.Left.Style;
                                    targetWorksheet.Cells[row, col].Style.Border.Right.Style =
                                        sourceWorksheet.Cells[row, col].Style.Border.Right.Style;

                                    if (
                                        sourceWorksheet.Cells[row, col].Style.Fill.PatternType
                                        != ExcelFillStyle.None
                                    )
                                    {
                                        targetWorksheet.Cells[row, col].Style.Fill.PatternType =
                                            sourceWorksheet.Cells[row, col].Style.Fill.PatternType;
                                    }
                                }
                            }

                            // Apply auto fit
                            targetWorksheet.Cells.AutoFitColumns();
                        }
                    }
                }
            }

            return package.GetAsByteArray();
        }
    }
}
