using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace BaseApiReference.Abstractions.CSV;

/// <summary>
/// Interface for CSV service
/// </summary>
public interface ICsvHandler
{
    /// <summary>
    /// Import from CSV
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="stream"></param>
    /// <returns></returns>
    List<T> ImportFromCsvAsync<T>(Stream stream);

    /// <summary>
    /// Export to CSV
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="records"></param>
    /// <returns></returns>
    string ExportToCsv<T>(IEnumerable<T> records);
}
