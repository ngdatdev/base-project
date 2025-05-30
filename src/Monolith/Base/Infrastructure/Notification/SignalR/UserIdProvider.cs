﻿using Microsoft.AspNetCore.SignalR;

namespace Infrastructure.Notification.SignalR;

/// <summary>
/// Custom Provider implementation of <see cref="IUserIdProvider"/>
/// </summary>
public class UserIdProvider : IUserIdProvider
{
    public string GetUserId(HubConnectionContext connection)
    {
        return connection.User?.FindFirst("sub")?.Value;
    }
}
