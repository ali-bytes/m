using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WoodStore.Domain.Models;

namespace WoodStore.Domain.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private DbContext _context;
        protected DbSet<T> _set;
        const string ConnectionStringName = "StoresDbContext";

        public Repository()
            : this(new StoresDbContext())
        {

        }


        public Repository(StoresDbContext context)
        {
            //string connectionString = ConfigurationManager
            //             .ConnectionStrings[ConnectionStringName]
            //             .ConnectionString;

            //_context = new DbContext(connectionString);
            _context = context;
            _set = _context.Set<T>();
        }
        public virtual IEnumerable<T> Get(
           Expression<Func<T, bool>> filter = null,
           Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
           string includeProperties = "")
        {
            IQueryable<T> query = _set;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }
        public IQueryable<T> GetAll()
        {
            return _set;
        }

        public IQueryable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return _set.Where(predicate);
        }

        public T FindById(int id)
        {
            return _set.Find(id);
        }
        public T FindById(string id)
        {
            return _set.Find(id);
        }
        public bool Add(T newEntity)
        {
            bool isValid = false;
            try
            {
                DbEntityValidationResult validationResult = _context.Entry(newEntity).GetValidationResult();

                if (validationResult.IsValid)
                {
                    _context.Set<T>().Add(newEntity);
                    isValid = true;
                }
            }
            catch { }

            return isValid;

        }



        public void Remove(T entity)
        {
            _context.Entry(entity).State = EntityState.Deleted;
        }
        public void RemoveAll(IEnumerable<T> entities)
        {
            foreach (var e in entities)
            {
                _context.Entry(e).State = EntityState.Deleted;

            }
        }
        public void Update(T entity)
        {

            _context.Entry(entity).State = EntityState.Modified;

        }
        public void Update(T item, int id)
        {
            var entity = _set.Find(id);
            if (entity == null)
            {
                return;
            }

            _context.Entry(entity).CurrentValues.SetValues(item);

        }

    }
}
