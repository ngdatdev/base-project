using System;

namespace BaseApiReference.Abstractions.IdGenerator;

/// <summary>
/// DecodeId is used to decode the Id generated.
/// </summary>
public class DecodeIdModel
{
    public uint MachineId { get; set; }

    public DateTimeOffset CreatedTimestamp { get; set; }

    public long Sequence { get; set; }
}
