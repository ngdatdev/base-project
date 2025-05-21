using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace BaseApiReference.Abstractions.Excel;

/// <summary>
/// Interface for excel handler
/// </summary>
public interface IExcelHandler
{
    /// <summary>
    /// Import excel
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    Task<DataTable> ImportExcelAsync(IFormFile file);

    /// <summary>
    /// Import excel to list
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="file"></param>
    /// <returns></returns>
    Task<List<T>> ImportExcelToListAsync<T>(IFormFile file)
        where T : new();

    /// <summary>
    /// Export to excel data list
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="data"></param>
    /// <param name="worksheetName"></param>
    /// <returns></returns>
    byte[] ExportToExcel<T>(List<T> data, string worksheetName = "Sheet1");

    /// <summary>
    /// Export to excel data table
    /// </summary>
    /// <param name="dataTable"></param>
    /// <param name="worksheetName"></param>
    /// <returns></returns>
    byte[] ExportToExcel(DataTable dataTable, string worksheetName = "Sheet1");

    /// <summary>
    /// Generate excel
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    byte[] GenerateExcelReport(Dictionary<string, object> data);

    /// <summary>
    /// Merge excel
    /// </summary>
    /// <param name="files"></param>
    /// <returns></returns>
    Task<byte[]> MergeExcelFilesAsync(List<IFormFile> files);
}
