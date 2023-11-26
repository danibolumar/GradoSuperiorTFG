using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PoliGest.BackEnd.Servicios
{
    public class ServicioGenerico<T> : IServicioGenerico<T> where T : class
    {
        // Objeto que accede a la capa de acceso a datos creada por Entity Framework
        protected DbContext _entities;
        // Objeto que nos permite acceder a las clases asociadas con las tablas de la base de datos
        protected readonly IDbSet<T> _dbset;

        /*
         * Constructor
         * Paramatro: objeto que nos permite acceder a las clases asociadas a la base de datos
         */
        public ServicioGenerico(DbContext context)
        {
            _entities = context;
            _dbset = context.Set<T>();
        }

        /*
         * Añade la entidad a la base de datos
         * Necesita de un commit para hacer la transacción persistente
         * Parámetro: entidad de tipo genérico
         */
        public T add(T entity)
        {
            return _dbset.Add(entity);
        }
        /*
         * Borra la entidad a la base de datos
         * Necesita de un commit para hacer la transacción persistente
         * Parámetro: entidad de tipo genérico
         */
        public T delete(T entity)
        {
            return _dbset.Remove(entity);
        }
        /*
         * Devuelve una lista con todos los objetos de una tabla de la base de datos
         */
        public IEnumerable<T> getAll()
        {
            return _dbset.AsEnumerable<T>();
        }
        /*
         * Realiza un commit de la cache a la base de datos
         */
        public void save()
        {
            _entities.SaveChanges();
        }
        /*
         * Devuelve un objeto identificado por su id
         * Parametro: identificador
         */
        public T findByID(int id)
        {
            return _dbset.Find(id);
        }
        /*
         * Añade la entidad a la base de datos
         * Necesita de un commit para hacer la transacción persistente
         * Parámetro: entidad de tipo genérico
         */
        public void edit(T entity)
        {
            _entities.Entry(entity).State = EntityState.Modified;
        }

        public IEnumerable<T> findBy(Expression<Func<T, bool>> predicate)
        {
            IEnumerable<T> query = _dbset.Where(predicate).AsEnumerable();
            return query;
        }
    }
}
