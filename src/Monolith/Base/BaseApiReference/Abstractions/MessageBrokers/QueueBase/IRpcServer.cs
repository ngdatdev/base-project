using System;
using System.Threading.Tasks;

namespace BaseApiReference.Abstractions.MessageBrokers.QueueBase;

/// <summary>
/// Represents RPC server
/// </summary>
public interface IRpcServer
{
    /// <summary>
    /// Starts RPC server
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    /// <param name="queueName"></param>
    /// <param name="handler"></param>
    /// <returns></returns>
    Task StartAsync<TRequest, TResponse>(string queueName, Func<TRequest, Task<TResponse>> handler)
        where TRequest : class
        where TResponse : class;
    void Stop();
}
