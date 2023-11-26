using NLog;
using PoliGest.BackEnd.Modelo;
using PoliGest.BackEnd.Servicios;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace PoliGest.MVVM
{
    class MVUsuario : MVBaseCRUD<usuario>
    {
        /* Declaramos los objectos privados a usar */
        private GestionPolideportivaEntities gestionEnt;
        private UsuarioServicio usuServ;
        private SociosServicio socioServ;
        private RolServicio rolServ;
        private ReservaServicio reservaServ;

        private usuario user;
        private socios socio;
        private rol rolSel;
        private string nombre;
        private string dni;
        private string randomContraseña = string.Empty;

        private ListCollectionView listaAux;

        /* Creamos el log para guardar los posibles errores */
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /* Declaramos los objectos públics a usar */
        public List<usuario> listaUsuario { get { return usuServ.getAll().ToList(); } }

        public List<rol> listaRol { get { return rolServ.getAll().ToList(); } }

        public List<reserva> listaReserva { get { return reservaServ.getAll().ToList(); } }

        public List<Predicate<usuario>> criterios { get; set; }

        public ListCollectionView listaTablaUsuario { get { return listaAux; } }       

        // Creamos el constructor de la clase
        public MVUsuario(GestionPolideportivaEntities gestion)
        {
            gestionEnt = gestion;
            inicializa();
        }

        // Método que inicializa los objetos a usar
        private void inicializa()
        {
            usuServ = new UsuarioServicio(gestionEnt);
            servicio = new UsuarioServicio(gestionEnt);
            socioServ = new SociosServicio(gestionEnt);
            rolServ = new RolServicio(gestionEnt);
            reservaServ = new ReservaServicio(gestionEnt);
            user = new usuario();
            socio = new socios();
            rolSel = new rol();

            listaAux = new ListCollectionView(usuServ.getAll().ToList());
            criterios = new List<Predicate<usuario>>();
        }

        /* Creamos objetos usados en los binding de los diálogos */
        public usuario usuarioDatos
        {
            get { return user; }
            set { user = value; NotifyPropertyChanged(nameof(usuarioDatos)); }
        }

        public socios socioNuevo
        {
            get { return socio; }
            set { socio = value; NotifyPropertyChanged(nameof(socioNuevo)); }
        }

        public rol rolSeleccionado
        {
            get { return rolSel; }
            set { rolSel = value; NotifyPropertyChanged(nameof(rolSeleccionado)); }
        }

        public string textoNombre
        {
            get { return nombre; }
            set { nombre = value; NotifyPropertyChanged(nameof(textoNombre)); }
        }

        public string textoDni
        {
            get { return dni; }
            set { dni = value; NotifyPropertyChanged(nameof(textoDni)); }
        }

        /* Cogemos objetos de la base de datos */

        private socios buscarSocio
        {
            get { return gestionEnt.Set<socios>().Where(s => s.usuario_idusuario == user.idusuario).FirstOrDefault(); }
        }

        public usuario existeUsuario
        {
            get { return gestionEnt.Set<usuario>().Where(u => u.dni == user.dni).FirstOrDefault(); }
        }

        public bool loginDatosUsu
        {
            get { return usuServ.login(user.dni, user.contraseña); }
        }

        public rol cogerRolSocio
        {
            get { return gestionEnt.Set<rol>().Where(r => r.nombre.Equals("Socio")).FirstOrDefault(); }
        }

        public string cambioContraseña()
        {
            randomContraseña = string.Empty;

            Random rdn = new Random();
            string caracteres = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890%$#@";
            int longitud = caracteres.Length;
            char letra;
            int longitudContrasenia = 10;
            for (int i = 0; i < longitudContrasenia; i++)
            {
                letra = caracteres[rdn.Next(longitud)];
                randomContraseña += letra.ToString();
            }

            return randomContraseña;
        }

       /* Tanto 'filtroCombinadoLista' como 'addCriterios' són métodos usados para los filtros */
        
        public bool filtroCombinadoLista(Object item)
        {
            usuario usu = item as usuario;
            bool esta = true;
            if(criterios.Count()!= 0)
            {
                esta = criterios.TrueForAll(x => x(usu));
            }
            return esta;
        }

        public void addCriterios()
        {
            if(rolSeleccionado != null)
            {
                criterios.Add(new Predicate<usuario>(u => u.rol != null && u.rol.Equals(rolSeleccionado)));
            }
            
            if(!string.IsNullOrEmpty(textoDni) && !textoDni.Equals(""))
            {
                criterios.Add(new Predicate<usuario>(u => u.dni.ToUpper().Contains(textoDni.ToUpper())));
            }
            if (!string.IsNullOrEmpty(textoNombre) && !textoNombre.Equals(""))
            {
                criterios.Add(new Predicate<usuario>
                    (u => u.nombre != null && u.nombre.ToUpper().Contains(textoNombre.ToUpper()) ||
                    u.apellido1 != null && u.apellido1.ToUpper().Contains(textoNombre.ToUpper())
                    || (u.nombre + ", " + u.apellido1).ToUpper().Contains(textoNombre.ToUpper())));
            }
        }

        /* Método usado para editar el usuario */
        public bool editarUsuario()
        {
            
            bool correcto = true;
            try
            {
                socios socioUser = buscarSocio;
                if(socioUser != null && !user.rol.nombre.Equals("Socio"))
                {
                    socioServ.delete(socioUser);
                    socioServ.save();
                }

                if(socioUser == null && user.rol.nombre.Equals("Socio"))
                {
                    this.socio.usuario = user;
                    socioServ.add(this.socio);
                    socioServ.save();
                }

                if(socioUser != null && user.rol.nombre.Equals("Socio"))
                {
                    socioUser.observaciones = socio.observaciones;
                    socioServ.edit(socioUser);
                    socioServ.save();
                }

                update(user);
            }
            catch (DbUpdateException dbex)
            {
                correcto = false;
                logger.Error("\n" + dbex.InnerException + "\n" + dbex.StackTrace);
            }
            return correcto;
        }

        /* Método para guardar un nuevo usuario, en caso de que el nuevo usuario sea un socio guardamos primero el objeto socio */

        public bool guardarUsuario()
        {
            bool correcto = true;
            try
            {
                
                user.fecha_alta = DateTime.Now;

                add(this.user);

                this.user = existeUsuario;
                if (user.rol.nombre.Equals("Socio"))
                {
                    this.socio.usuario = user;
                    socioServ.add(this.socio);
                    socioServ.save();
                }
            }
            catch (DbUpdateException dbex)
            {
                correcto = false;
                logger.Error("\n" + dbex.InnerException + "\n" + dbex.StackTrace);
            }
            return correcto;

        }

        /* Metodo usado para borrar un usuario, en este caso no borramos el usuario como tal, solo le damos una fecha de baja, esto se hace para
         * no tener que eliminar las reservas de este socio poniendo las reservas como anuladas, no las queremos eliminar para futuros informes de la empresa
         Si el usuario a eliminar es un socio, eliminamos primero el objeto socio relacionado con este usuario y luego el usuario como tal*/
        public bool borrarUsuario()
        {
            bool correcto = true;
            socios socioBorrar = buscarSocio;
            try
            {
                foreach(reserva res in listaReserva){
                    if (user.idusuario == res.usuario_idusuario)
                    {
                        res.anulado = 1;
                        reservaServ.edit(res);
                    }
                    reservaServ.save();
                }
                

                if (user.rol.nombre.Equals("Socio")){
                    socioServ.delete(socioBorrar);
                    socioServ.save();
                }
                this.user.fecha_baja = DateTime.Now;
                update(this.user);
                usuServ.save();
            }
            catch (DbUpdateException dbex)
            {
                correcto = false;
                logger.Error("\n" + dbex.InnerException + "\n" + dbex.StackTrace);
            }
            return correcto;
        }

        /* Con este método comprobamos si el usuario que intenta entrar a la aplicación esta sancionado o no */

        public bool estaSancionado(socios socioUser, usuario userEx)
        {

            
            if (userEx.rol.nombre.Equals("Socio"))
            {
                if (socioUser.fecha_fin_penalizacion >= DateTime.Now.Date)
                    return true;
                else
                {
                    if (socioUser.penalizado == 1)
                    {
                        socioUser.penalizado = 0;
                        socioServ.edit(socioUser);
                        socioServ.save();
                    } 
                    
                }
               
            }
            return false;
        }
        public void crearCopia(usuario original, usuario copia)
        {
            copia.dni = original.dni;
            copia.nombre = original.nombre;
            copia.apellido1 = original.apellido1;
            copia.apellido2 = original.apellido2;
            copia.telefono = original.telefono;
            copia.cuenta_bancaria = original.cuenta_bancaria;
            copia.fecha_alta = original.fecha_alta;
            copia.fecha_baja = original.fecha_baja;
            copia.contraseña = original.contraseña;
            copia.idusuario = original.idusuario;
            copia.reserva = original.reserva;
            copia.rol = original.rol;
            copia.socios = original.socios;
        }
    }
}
