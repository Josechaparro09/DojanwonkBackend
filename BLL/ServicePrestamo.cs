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
            if (!VerificarDisponibilidad(agregar.DetallePrestamos)) 
                throw new ArgumentException("No existen disponibles");
            if(await PrestamoEstudiante(agregar.Estudiante.Id) != null)
                throw new ArgumentException("El estudiante ya tiene un prestamo activo");
            foreach (var item in agregar.DetallePrestamos)
            {
                Articulo articulo = await serviceArticulo.Buscar(item.IdArticulo);
                articulo.Disponibles = articulo.Disponibles - item.Cantidad;
                await serviceArticulo.Actualizar(articulo);
            }
            agregar.Estado = "En prestamo";
            await dBPrestamo.Agregar(agregar);
            return true;

        }
        public async Task<Prestamo> PrestamoEstudiante(string IdEstudiante)
        {
            List<Prestamo> prestamos = await dBPrestamo.Leer();
            var prestamosEstudiante=prestamos.Where(p => p.Estudiante.Id == IdEstudiante).ToList();
            return prestamosEstudiante.FirstOrDefault(p => p.Estado == "En prestamo");
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
            return await Devolver(actualizado.Id);
        }
        public async Task<bool> Eliminar(int id)
        {
            return await dBPrestamo.Eliminar(id);
        }
        public async Task<List<Prestamo>> EnPrestamo()
        {
            var prestamos = await Leer();
            return prestamos.Where(p => p.Estado == "En Prestamo").ToList();
        }

    }
}
