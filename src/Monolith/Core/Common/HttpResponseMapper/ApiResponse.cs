using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;

namespace Common.HttpResponseMapper;

/// <summary>
/// Represents an API response pattern.
/// </summary>
public sealed class ApiResponse
{
    [JsonIgnore]
    public int HttpCode { get; set; } = StatusCodes.Status200OK;

    public bool IsSuccess => HttpCode >= 200 && HttpCode < 400;

    public string Message { get; init; } = string.Empty;

    public DateTime ResponseTime { get; init; } =
        TimeZoneInfo.ConvertTimeFromUtc(
            dateTime: DateTime.UtcNow,
            destinationTimeZone: TimeZoneInfo.FindSystemTimeZoneById(id: "SE Asia Standard Time")
        );

    public object Body { get; init; } = new();

    public IEnumerable<string> DetailErrors { get; init; } = [];
}
