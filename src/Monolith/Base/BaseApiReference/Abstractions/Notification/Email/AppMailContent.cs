﻿namespace BaseApiReference.Abstractions.Notification.Email;

/// <summary>
/// Represent the mail content model.
/// </summary>
public sealed class AppMailContent
{
    public string To { get; set; }

    public string Subject { get; set; }

    public string Body { get; set; }
}
