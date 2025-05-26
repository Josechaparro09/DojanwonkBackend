using DAL.Modelos;
using Microsoft.EntityFrameworkCore;
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
            if( await dBUsuario.Buscar(agregar.Cc) == null)
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
        public async Task<Usuario> Actualizar(Usuario usuario)
        {
            await dBUsuario.Actualizar(usuario);

            // Opcional: recargar desde la BD si quieres el estado final con relaciones, valores calculados, etc.
            List<Usuario> lista = await Leer();
            var actualizado = lista.FirstOrDefault(u => u.Cc == usuario.Cc);
            return actualizado;
        }
        public async Task<bool> Eliminar(string cc)
        {
            return await dBUsuario.Eliminar(cc);
        }
    }
}
