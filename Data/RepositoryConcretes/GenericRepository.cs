using Core.RepositoryAbstracts;
using Data.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.RepositoryConcretes
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class, new()
    {
        AppDbContext _context;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
        }

        public void Add(T entity)
        {
            _context.Set<T>().Add(entity);
        }

        public void Commit()
        {
            _context.SaveChanges();
        }

        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public T Get(Func<T, bool>? func = null)
        {
            return func == null ? _context.Set<T>().FirstOrDefault() :
                               _context.Set<T>().Where(func).FirstOrDefault();
        }

        public List<T> GetAll(Func<T, bool>? func = null)
        {
            return func == null ? _context.Set<T>().ToList() :
                               _context.Set<T>().Where(func).ToList();
        }
    }
}
