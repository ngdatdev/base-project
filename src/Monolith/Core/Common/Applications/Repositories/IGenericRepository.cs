using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Common.Applications.Repositories;

/// <summary>
/// This interface is used to define a generic repository
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IGenericRepository<T>
    where T : class
{
    /// <summary>
    /// This method is used to get an entity by its id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<T> GetByIdAsync(int id);

    /// <summary>
    /// This method is used to get all entities
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<T>> GetAllAsync();

    /// <summary>
    /// This method is used to find entities
    /// </summary>
    /// <param name="expression"></param>
    /// <returns></returns>
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> expression);

    /// <summary>
    /// This method is used to get a single entity
    /// </summary>
    /// <param name="expression"></param>
    /// <returns></returns>
    Task<T> SingleOrDefaultAsync(Expression<Func<T, bool>> expression);

    /// <summary>
    /// This method is used to add an entity
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task AddAsync(T entity);

    /// <summary>
    /// This method is used to add a range of entities
    /// </summary>
    /// <param name="entities"></param>
    /// <returns></returns>
    Task AddRangeAsync(IEnumerable<T> entities);

    /// <summary>
    /// This method is used to update an entity
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task<bool> UpdateAsync(T entity);

    /// <summary>
    /// This method is used to delete an entity
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<bool> DeleteAsync(int id);

    /// <summary>
    /// This method is used to delete an entity
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task<bool> DeleteAsync(T entity);

    /// <summary>
    /// This method is used to count the number of entities
    /// </summary>
    /// <returns></returns>
    Task<int> CountAsync();

    /// <summary>
    /// This method is used to count the number of entities
    /// </summary>
    /// <param name="expression"></param>
    /// <returns></returns>
    Task<int> CountAsync(Expression<Func<T, bool>> expression);

    /// <summary>
    /// This method is used to check if an entity exists
    /// </summary>
    /// <param name="expression"></param>
    /// <returns></returns>
    Task<bool> ExistsAsync(Expression<Func<T, bool>> expression);

    /// <summary>
    /// This method is used to get an entity by its id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    T GetById(int id);

    /// <summary>
    /// This method is used to get all entities
    /// </summary>
    /// <returns></returns>
    IEnumerable<T> GetAll();

    /// <summary>
    /// This method is used to find entities
    /// </summary>
    /// <param name="expression"></param>
    /// <returns></returns>
    IEnumerable<T> Find(Expression<Func<T, bool>> expression);

    /// <summary>
    /// This method is used to get a single entity
    /// </summary>
    /// <param name="expression"></param>
    /// <returns></returns>
    T SingleOrDefault(Expression<Func<T, bool>> expression);

    /// <summary>
    /// This method is used to add an entity
    /// </summary>
    /// <param name="entity"></param>
    void Add(T entity);

    /// <summary>
    /// This method is used to add a range of entities
    /// </summary>
    /// <param name="entities"></param>
    void AddRange(IEnumerable<T> entities);

    /// <summary>
    /// This method is used to update an entity
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    bool Update(T entity);

    /// <summary>
    /// This method is used to delete an entity
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    bool Delete(int id);

    /// <summary>
    /// This method is used to delete an entity
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    bool Delete(T entity);

    /// <summary>
    /// This method is used to count the number of entities
    /// </summary>
    /// <returns></returns>
    int Count();

    /// <summary>
    /// This method is used to count the number of entities
    /// </summary>
    /// <param name="expression"></param>
    /// <returns></returns>
    int Count(Expression<Func<T, bool>> expression);

    /// <summary>
    /// This method is used to check if an entity exists
    /// </summary>
    /// <param name="expression"></param>
    /// <returns></returns>
    bool Exists(Expression<Func<T, bool>> expression);
}
