using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Globalization;
using System.IO;
using System.Linq;
using BaseApiReference.Abstractions.CSV;
using CsvHelper;

namespace Infrastructure.CSV;

/// <summary>
/// CSV Handler
/// </summary>
public class CsvHandler : ICsvHandler
{
    public List<T> ImportFromCsvAsync<T>(Stream stream)
    {
        using var reader = new StreamReader(stream);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

        var records = csv.GetRecords<T>();
        return records.ToList();
    }

    public string ExportToCsv<T>(IEnumerable<T> records)
    {
        using var writer = new StringWriter();
        using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);

        csv.WriteRecords(records);
        return writer.ToString();
    }
}
