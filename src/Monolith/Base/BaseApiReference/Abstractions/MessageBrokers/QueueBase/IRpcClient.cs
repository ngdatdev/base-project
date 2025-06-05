using System;
using System.Threading.Tasks;

namespace BaseApiReference.Abstractions.MessageBrokers.QueueBase;

/// <summary>
/// Represents RPC client
/// </summary>
public interface IRpcClient
{
    /// <summary>
    /// Calls an RPC
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    /// <param name="request"></param>
    /// <param name="queueName"></param>
    /// <param name="timeout"></param>
    /// <returns></returns>
    Task<TResponse> CallAsync<TRequest, TResponse>(
        TRequest request,
        string queueName,
        TimeSpan? timeout = null
    )
        where TRequest : class
        where TResponse : class;
}
