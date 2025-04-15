using System;

namespace BaseApiReference.Base;

/// <summary>
/// Interface for tracking entity changes, including versioning and timestamps.
/// </summary>
public interface ITrackable
{
    /// <summary>
    /// A byte array representing the row version for concurrency control.
    /// Used to track changes and prevent conflicting updates in a database.
    /// </summary>
    byte[] RowVersion { get; set; }

    /// <summary>
    /// The timestamp indicating when the entity was initially created.
    /// </summary>
    DateTime CreatedDateTime { get; set; }

    /// <summary>
    /// The timestamp indicating when the entity was last updated.
    /// </summary>
    DateTime? UpdatedDateTime { get; set; }
}
