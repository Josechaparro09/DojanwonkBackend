using DAL;
using DAL.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class ServiceEstudiante
    {
        private readonly DBEstudiante dBEstudiante;
        private readonly DBGrupo dBGrupo;
        public ServiceEstudiante(DBEstudiante dBEstudiante, DBGrupo dBGrupo)
        {
            this.dBEstudiante = dBEstudiante;
            this.dBGrupo = dBGrupo;
        }
        public async Task<bool> Agregar(Estudiante agregar)
        {
            if (await dBEstudiante.Buscar(agregar.Id) == null)
            {
                agregar.estado = "Activo";
                agregar.CalcularEdad();
                if (agregar.edad >= 6 && agregar.edad <= 10)
                {
                    agregar.IdGrupoNavigation = await dBGrupo.Buscar(1);
                }
                if (agregar.edad>10 && agregar.edad <= 15)
                {
                    agregar.IdGrupoNavigation = await dBGrupo.Buscar(2);

                }
                if (agregar.edad >= 16)
                {
                    agregar.IdGrupoNavigation = await dBGrupo.Buscar(3);

                }
                await dBEstudiante.Agregar(agregar);
                return true;
            }
            throw new ArgumentException("Estudiante ya ha sido registrado");
        }
        public async Task<List<Estudiante>> Leer()
        {
            return await dBEstudiante.Leer();
        }
        public async Task<Estudiante> Buscar(string id)
        {
            return await dBEstudiante.Buscar(id);
        }
        public async Task<bool> Actualizar(Estudiante actualizado)
        {
            return await dBEstudiante.Actualizar(actualizado);
        }
        public async Task<bool> Eliminar(string id)
        {
            return await dBEstudiante.Eliminar(id);
        }
    }
}
