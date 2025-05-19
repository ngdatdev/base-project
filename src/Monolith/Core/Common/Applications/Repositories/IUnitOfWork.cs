using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Applications.Repositories;

/// <summary>
/// This interface is used to define a unit of work
/// </summary>
public interface IUnitOfWork : IDisposable
{
    /// <summary>
    /// This method is used to define a repository
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    IGenericRepository<T> Repository<T>()
        where T : class;

    /// <summary>
    /// This method is used to save changes
    /// </summary>
    /// <returns></returns>
    Task<int> SaveChangesAsync();

    /// <summary>
    /// This method is used to save changes
    /// </summary>
    /// <returns></returns>
    int SaveChanges();
    Task BeginTransactionAsync();

    /// <summary>
    /// This method is used to save changes
    /// </summary>
    /// <returns></returns>
    Task CommitTransactionAsync();

    /// <summary>
    /// This method is used to save changes
    /// </summary>
    /// <returns></returns>
    Task RollbackTransactionAsync();
}
