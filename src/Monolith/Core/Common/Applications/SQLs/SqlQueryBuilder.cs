using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaseApiReference.Metadata;

namespace Common.Applications.SQLs;

/// <summary>
/// Sql query builder for entity
/// </summary>
/// <typeparam name="T"></typeparam>
public class SqlQueryBuilder<T>
    where T : class
{
    private readonly ITableMetadata _metadata;
    private readonly string _fullTableName;

    public SqlQueryBuilder(ITableMetadata metadata)
    {
        _metadata = metadata;
        _fullTableName = string.IsNullOrEmpty(_metadata.Schema)
            ? $"[{_metadata.Name}]"
            : $"[{_metadata.Schema}].[{_metadata.Name}]";
    }

    public string BuildSelectAll()
    {
        var columns = string.Join(", ", _metadata.ColumnMappings.Values.Select(col => $"[{col}]"));
        return $"SELECT {columns} FROM {_fullTableName}";
    }

    public string BuildSelectById(string idColumn = "Id")
    {
        var columns = string.Join(", ", _metadata.ColumnMappings.Values.Select(col => $"[{col}]"));
        return $"SELECT {columns} FROM {_fullTableName} WHERE [{idColumn}] = @Id";
    }

    public string BuildInsert(IEnumerable<string> excludeColumns = null)
    {
        var excludeSet = new HashSet<string>(excludeColumns ?? Enumerable.Empty<string>());
        var filteredColumns = _metadata
            .ColumnMappings.Values.Where(col => !excludeSet.Contains(col))
            .ToList();

        var columns = string.Join(", ", filteredColumns.Select(col => $"[{col}]"));
        var parameters = string.Join(", ", filteredColumns.Select(col => $"@{col}"));

        return $"INSERT INTO {_fullTableName} ({columns}) VALUES ({parameters})";
    }

    public string BuildUpdate(string idColumn = "Id", IEnumerable<string> excludeColumns = null)
    {
        var excludeSet = new HashSet<string>(
            (excludeColumns ?? Enumerable.Empty<string>()).Concat(new[] { idColumn })
        );
        var updateColumns = _metadata
            .ColumnMappings.Values.Where(col => !excludeSet.Contains(col))
            .Select(col => $"[{col}] = @{col}")
            .ToList();

        var setClause = string.Join(", ", updateColumns);
        return $"UPDATE {_fullTableName} SET {setClause} WHERE [{idColumn}] = @{idColumn}";
    }

    public string BuildDelete(string idColumn = "Id")
    {
        return $"DELETE FROM {_fullTableName} WHERE [{idColumn}] = @{idColumn}";
    }

    public string BuildSelectWhere(string whereClause)
    {
        var columns = string.Join(", ", _metadata.ColumnMappings.Values.Select(col => $"[{col}]"));
        return $"SELECT {columns} FROM {_fullTableName} WHERE {whereClause}";
    }

    public string BuildCount(string whereClause = null)
    {
        var sql = $"SELECT COUNT(*) FROM {_fullTableName}";
        if (!string.IsNullOrEmpty(whereClause))
            sql += $" WHERE {whereClause}";
        return sql;
    }

    public string BuildPaginated(string orderBy, string whereClause = null)
    {
        var columns = string.Join(", ", _metadata.ColumnMappings.Values.Select(col => $"[{col}]"));
        var sql = $"SELECT {columns} FROM {_fullTableName}";

        if (!string.IsNullOrEmpty(whereClause))
            sql += $" WHERE {whereClause}";

        sql += $" ORDER BY {orderBy} OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";
        return sql;
    }

    // Mapping property name sang column name
    public string GetColumnName(string propertyName)
    {
        return _metadata.ColumnMappings.TryGetValue(propertyName, out var columnName)
            ? columnName
            : propertyName;
    }
}
