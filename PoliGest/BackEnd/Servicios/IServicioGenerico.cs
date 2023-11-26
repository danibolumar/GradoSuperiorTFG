using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PoliGest.BackEnd.Servicios
{
    interface IServicioGenerico<T> where T : class
    {
        /*
         * Obtiene todos los objetos de una determinada entidad
         */
        IEnumerable<T> getAll();
        /*
         * Busca elementos según la expresión o predicado pasado como parámetro
         */
        IEnumerable<T> findBy(Expression<Func<T, bool>> predicate);
        /*
         * Busca un objeto por su identificador
         */
        T findByID(int id);
        /*
         * Inserta un objeto nuevo en la base de datos
         * Luego se debe de realizar un commit para que se hagan persistentes los cambios
         */
        T add(T entity);
        /*
         * Borra un objeto de la base de datos en función de su id
         * Luego se debe de realizar un commit para que se hagan persistentes los cambios
         */
        T delete(T entity);
        /*
         * Edita un objeto de la base de datos
         * Luego se debe de realizar un commit para que se hagan persistentes los cambios
         */
        void edit(T entity);

        /*
         * Realiza un commit para que los cambios se hagan persistentes
         */
        void save();
    }
}
