using DAL.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class ServiceUsuario
    {
        private readonly DBUsuario dBUsuario;
        public ServiceUsuario(DBUsuario dBUsuario)
        {
            this.dBUsuario = dBUsuario;
        }
        public async Task<bool> Agregar(Usuario agregar)
        {
            if( await dBUsuario.Buscar(agregar.Cc) != null)
            {
                await dBUsuario.Agregar(agregar);
                return true;
            }
            throw new ArgumentException("Error usuario existente");
        }
        public async Task<List<Usuario>> Leer()
        {
            return await dBUsuario.Leer();
        }
        public async Task<Usuario> Buscar(string cc)
        {
            return await dBUsuario.Buscar(cc);
        }
        public async Task<bool> Actualizar(Usuario actualizado)
        {
            return await dBUsuario.Actualizar(actualizado);
        }
        public async Task<bool> Eliminar(string cc)
        {
            return await dBUsuario.Eliminar(cc);
        }
    }
}
