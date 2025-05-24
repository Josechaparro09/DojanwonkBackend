using DAL.Modelos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class DBPago
    {
        private readonly DbDojankwonContext dbDojankwonContext;

        public DBPago(DbDojankwonContext dbDojankwonContext)
        {
            this.dbDojankwonContext = dbDojankwonContext;
        }
        public async Task Agregar(Pago pago)
        {
            dbDojankwonContext.Pagos.Add(pago);
            await dbDojankwonContext.SaveChangesAsync();
        }
        public async Task<List<Pago>> Leer()
        {
            return await dbDojankwonContext.Pagos.ToListAsync();
        }
        public async Task<bool> Actualizar(Pago actualizado)
        {
            if (!dbDojankwonContext.Pagos.Any(p => p.Id == actualizado.Id)) return false;
            dbDojankwonContext.Pagos.Update(actualizado);
            await dbDojankwonContext.SaveChangesAsync();
            return true;
        }
        public async Task<Pago> Buscar(string cc)
        {
            return await dbDojankwonContext.Pagos.FindAsync(cc);
        }
        public async Task<bool> Eliminar(string cc)
        {
            Pago eliminar = await Buscar(cc);
            if (eliminar == null) return false;
            dbDojankwonContext.Pagos.Remove(eliminar);
            await dbDojankwonContext.SaveChangesAsync();
            return true;
        }
    }
}
