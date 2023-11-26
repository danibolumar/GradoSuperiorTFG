using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoliGest.BackEnd.Servicios
{
    public class ServicioSQL
    {
        // Variable que almacena la conexión con la base de datos
        private MySqlConnection con;
        /// <summary>
        /// Propiedad pública que devuelve la conexión con la base de datos
        /// </summary>
        public MySqlConnection conexion { get { return con; } }
        /// <summary>
        /// Constructor de la clase
        /// Realiza la conexión con la base de datos
        /// </summary>
        public ServicioSQL()
        {
            con = new MySqlConnection(GetConnectionStringByProvider("MySQL"));
            con.Open();
        }
        /// <summary>
        /// Devuelve la cadena de conexión correspondiente
        /// La busca en el fichero de configuración App.config
        /// </summary>
        /// <param name="providerName">Nos indica el proveedor de la base de datos</param>
 /// <returns></returns>
        private static string GetConnectionStringByProvider(string providerName)
        {
            // Retorna null si hay fallo
            string returnValue = null;
            // Obtenemos la cadena de conexión del fichero de configuración App.config
            ConnectionStringSettingsCollection settings =
           ConfigurationManager.ConnectionStrings;
            // Si encontramos cadenas de configuración en el fichero App.config
            if (settings != null)
            {
                // Recorremos todas las cadenas de conexión del fichero
                foreach (ConnectionStringSettings cs in settings)
                {
                    if (cs.ProviderName == providerName)
                    {
                        returnValue = cs.ConnectionString;
                        break;
                    }
                }
            }
            return returnValue;
        }
        /// <summary>
        /// Obtiene los datos en función de la sentencia que se le pasa
        /// </summary>
        /// <param name="query"> Consulta necesaria para obtener los datos</param>
        /// <returns></returns>
        public DataTable getDatos(string query)
        {
            // Creamos un DataSet
            DataSet ds = new DataSet();
            // Obtenemos los datos en función de la conexión y de la sentencia SELECT
            MySqlDataAdapter adapt = new MySqlDataAdapter(query, con);
            // Guardamos los datos en el DataAdapter (Equivalente a ResulSet)
            adapt.Fill(ds);
            // Devolvemos el DataAdapter
            return ds.Tables[0];
        }
    }
}
