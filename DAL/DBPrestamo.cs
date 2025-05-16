using DAL.Modelos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class DBPrestamo
    {
        private readonly DbDojankwonContext dbDojankwonContext;

        public DBPrestamo(DbDojankwonContext dbDojankwonContext)
        {
            this.dbDojankwonContext = dbDojankwonContext;
        }
        public async Task Agregar(Prestamo prestamo)
        {
            dbDojankwonContext.Prestamos.Add(prestamo);
            await dbDojankwonContext.SaveChangesAsync();
        }
        public async Task<List<Prestamo>> Leer()
        {
            return await dbDojankwonContext.Prestamos.ToListAsync();
        }
        public async Task<bool> Actualizar(Prestamo actualizado)
        {
            if (!dbDojankwonContext.Prestamos.Any(p => p.Id == actualizado.Id)) return false;
            dbDojankwonContext.Prestamos.Update(actualizado);
            await dbDojankwonContext.SaveChangesAsync();
            return true;
        }
        public async Task<Prestamo> Buscar(int id)
        {
            return await dbDojankwonContext.Prestamos.FindAsync(id);
        }
        public async Task<bool> Eliminar(int id)
        {
            Prestamo eliminar = await Buscar(id);
            if (eliminar == null) return false;
            dbDojankwonContext.Prestamos.Remove(eliminar);
            await dbDojankwonContext.SaveChangesAsync();
            return true;
        }
    }
}
