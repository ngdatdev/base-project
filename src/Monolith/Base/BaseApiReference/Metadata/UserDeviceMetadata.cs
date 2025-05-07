using System.Collections.Generic;
using BaseApiReference.Entities;

namespace BaseApiReference.Metadata;

/// <summary>
/// Metadata for UserDevice
/// /summary
public class UserDeviceMetadata : ITableMetadata
{
    public string Name => "UserDevice";
    public string Schema => "dbo";

    public IReadOnlyDictionary<string, string> ColumnMappings => new Dictionary<string, string>
    {
        { nameof(UserDevice.UserId), "UserId" },
        { nameof(UserDevice.DeviceId), "DeviceId" },
        { nameof(UserDevice.DeviceType), "DeviceType" },
        { nameof(UserDevice.DeviceName), "DeviceName" },
        { nameof(UserDevice.LastUsedAt), "LastUsedAt" },
        { nameof(UserDevice.User), "User" }
    };
}