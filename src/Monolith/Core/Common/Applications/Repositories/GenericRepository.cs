using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Persistence.PostgreSQL.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Common.Applications.Repositories;

/// <summary>
/// This implementation is used to define a generic repository
/// </summary>
/// <typeparam name="T"></typeparam>
public class GenericRepository<T> : IGenericRepository<T>
    where T : class
{
    protected readonly AppDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public GenericRepository(AppDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<T> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> expression)
    {
        return await _dbSet.Where(expression).ToListAsync();
    }

    public async Task<T> SingleOrDefaultAsync(Expression<Func<T, bool>> expression)
    {
        return await _dbSet.SingleOrDefaultAsync(expression);
    }

    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public async Task AddRangeAsync(IEnumerable<T> entities)
    {
        await _dbSet.AddRangeAsync(entities);
    }

    public async Task<bool> UpdateAsync(T entity)
    {
        try
        {
            _dbSet.Update(entity);
            return await Task.FromResult(true);
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await GetByIdAsync(id);
        if (entity == null)
            return false;

        _dbSet.Remove(entity);
        return true;
    }

    public async Task<bool> DeleteAsync(T entity)
    {
        try
        {
            _dbSet.Remove(entity);
            return await Task.FromResult(true);
        }
        catch
        {
            return false;
        }
    }

    public async Task<int> CountAsync()
    {
        return await _dbSet.CountAsync();
    }

    public async Task<int> CountAsync(Expression<Func<T, bool>> expression)
    {
        return await _dbSet.CountAsync(expression);
    }

    public async Task<bool> ExistsAsync(Expression<Func<T, bool>> expression)
    {
        return await _dbSet.AnyAsync(expression);
    }

    public T GetById(int id)
    {
        return _dbSet.Find(id);
    }

    public IEnumerable<T> GetAll()
    {
        return _dbSet.ToList();
    }

    public IEnumerable<T> Find(Expression<Func<T, bool>> expression)
    {
        return _dbSet.Where(expression).ToList();
    }

    public T SingleOrDefault(Expression<Func<T, bool>> expression)
    {
        return _dbSet.SingleOrDefault(expression);
    }

    public void Add(T entity)
    {
        _dbSet.Add(entity);
    }

    public void AddRange(IEnumerable<T> entities)
    {
        _dbSet.AddRange(entities);
    }

    public bool Update(T entity)
    {
        try
        {
            _dbSet.Update(entity);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public bool Delete(int id)
    {
        var entity = GetById(id);
        if (entity == null)
            return false;

        _dbSet.Remove(entity);
        return true;
    }

    public bool Delete(T entity)
    {
        try
        {
            _dbSet.Remove(entity);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public int Count()
    {
        return _dbSet.Count();
    }

    public int Count(Expression<Func<T, bool>> expression)
    {
        return _dbSet.Count(expression);
    }

    public bool Exists(Expression<Func<T, bool>> expression)
    {
        return _dbSet.Any(expression);
    }
}
