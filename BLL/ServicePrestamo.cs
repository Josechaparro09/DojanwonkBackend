using DAL.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class ServicePrestamo
    {
        private readonly DBPrestamo dBPrestamo;
        private readonly ServiceArticulo serviceArticulo;
        public ServicePrestamo(DBPrestamo dBPrestamo, ServiceArticulo serviceArticulo)
        {
            this.dBPrestamo = dBPrestamo;
            this.serviceArticulo = serviceArticulo;
        }
        public bool VerificarDisponibilidad(ICollection<DetallePrestamo> detallePrestamos)
        {
            foreach (var item in detallePrestamos)
            {
                if(item.Cantidad > item.IdArticuloNavigation.Disponibles)
                {
                    return false;
                }
            }
            return true;
        }
        public async Task<bool> Alquilar(Prestamo agregar)
        {
            if(!VerificarDisponibilidad(agregar.DetallePrestamos)) return false;
            foreach (var item in agregar.DetallePrestamos)
            {
                Articulo articulo = await serviceArticulo.Buscar(item.IdArticulo);
                articulo.Disponibles = articulo.Disponibles - item.Cantidad;
                await serviceArticulo.Actualizar(articulo);
            }
            await dBPrestamo.Agregar(agregar);
            return true;

        }
        public async Task<bool> Devolver(int id)
        {
            Prestamo prestamo = await Buscar(id);
            if (prestamo == null || prestamo.Estado=="Devuelto") return false;
    
            foreach (var item in prestamo.DetallePrestamos)
            {
                Articulo articulo = await serviceArticulo.Buscar(item.IdArticulo);
                articulo.Disponibles = articulo.Disponibles + item.Cantidad;
                await serviceArticulo.Actualizar(articulo);
            }
            prestamo.Estado = "Devuelto";
            await dBPrestamo.Actualizar(prestamo);
            return true;
        }
        public async Task<List<Prestamo>> Leer()
        {
            return await dBPrestamo.Leer();
        }
        public async Task<Prestamo> Buscar(int id)
        {
            return await dBPrestamo.Buscar(id);
        }
        public async Task<bool> Actualizar(Prestamo actualizado)
        {
            return await dBPrestamo.Actualizar(actualizado);
        }
        public async Task<bool> Eliminar(int id)
        {
            return await dBPrestamo.Eliminar(id);
        }
        public async Task<List<Prestamo>> Pendientes()
        {
            var prestamos = await Leer();
            return prestamos.Where(p => p.Estado == "Pendiente").ToList();
        }

    }
}
