using DAL;
using DAL.Modelos;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class ServicePago
    {
        private readonly DBPago dBPago;
        private readonly ServiceEstudiante serviceEstudiante;
        public ServicePago(DBPago dBPago, ServiceEstudiante serviceEstudiante)
        {
            this.dBPago = dBPago;
            this.serviceEstudiante = serviceEstudiante;
        }
        public async Task ModificarEstadoEstudiante()
        {
            var estudiantes = await serviceEstudiante.Leer();
            var hoy = DateTime.Today;

            foreach (var item in estudiantes)
            {
                if (item.estado == "activo" && item.FechaRegistro.HasValue)
                {
                    var fechaRegistro = item.FechaRegistro.Value.ToDateTime(TimeOnly.MinValue);

                    // Verifica que el día del mes coincida
                    bool mismoDiaMes = fechaRegistro.Day == hoy.Day;

                    // Y que no sea hoy la fecha exacta
                    bool noEsHoy = fechaRegistro.Date != hoy;

                    if (mismoDiaMes && noEsHoy)
                    {
                        item.estado = "pendiente";
                        await serviceEstudiante.Actualizar(item);
                    }
                }
            }
        }
        public async Task ActualizarPago(Pago actualizado)
        {
            if (actualizado != null)
            {
                await dBPago.Actualizar(actualizado);
                if(actualizado.IdEstudianteNavigation.estado=="Inactivo")
                {
                    actualizado.IdEstudianteNavigation.estado = "Activo";
                    await serviceEstudiante.Actualizar(actualizado.IdEstudianteNavigation);
                }
            }
        }
        public async Task AgregarPago(Pago pago)
        {
            await dBPago.Agregar(pago);
        }
        public async Task GenerarPagos()
        {
            List<Estudiante> estudiantes = await serviceEstudiante.Leer();
            foreach (var item in estudiantes)
            {
                Pago ultimo = await Buscar(item.Id);
                if (ultimo == null)
                {
                    if (item.FechaRegistro.Value.AddMonths(1).ToDateTime(TimeOnly.MinValue) >= DateTime.Today)
                    {
                        //Crear un nuevo pago
                        Pago nuevoPago = new Pago();
                        nuevoPago.IdEstudiante = item.Id;
                        nuevoPago.FechaPago = item.FechaRegistro.Value.AddMonths(1);
                        nuevoPago.Estado = "Pendiente";
                        try
                        {
                            await NotificacionesCorreo.EnviarCorreoAsync(item.Correo, "Pago Pendiente", $"Estimado {item.Nombres + " " + item.Apellidos}, su pago está pendiente para el día {nuevoPago.FechaPago.ToShortDateString()}.");

                        }
                        catch (Exception ex)
                        {
                            throw;
                        }
                        await dBPago.Agregar(nuevoPago);
                        ultimo.IdEstudianteNavigation.estado = "Inactivo";
                        await serviceEstudiante.Actualizar(ultimo.IdEstudianteNavigation);
                    }
                    if (item.FechaRegistro.Value.AddDays(27).ToDateTime(TimeOnly.MinValue) < DateTime.Today)
                    {
                        try
                        {
                            await NotificacionesCorreo.EnviarCorreoAsync(item.Correo, "Fecha proximo pago", $"Estimado {item.Nombres + " " + item.Apellidos}, su pago está pendiente para el día {item.FechaRegistro.Value.AddMonths(1).ToShortDateString()}.");
                        }
                        catch (Exception ex)
                        {
                            throw;
                        }
                    }
                }
                else
                {
                    if (ultimo.FechaPago.AddMonths(1).ToDateTime(TimeOnly.MinValue) >= DateTime.Today)
                    {
                        //Crear un nuevo pago
                        if (ultimo.IdEstudianteNavigation.estado == "Activo")
                        {
                            Pago nuevoPago = new Pago();
                            nuevoPago.IdEstudiante = item.Id;
                            nuevoPago.FechaPago = ultimo.FechaPago.AddMonths(1);
                            nuevoPago.Estado = "Pendiente";
                            try
                            {
                                await NotificacionesCorreo.EnviarCorreoAsync(item.Correo, "Pago Pendiente", $"Estimado {ultimo.IdEstudianteNavigation.Nombres + " " + ultimo.IdEstudianteNavigation.Apellidos}, su pago está pendiente para el día {nuevoPago.FechaPago.ToShortDateString()}.");
                            }
                            catch (Exception ex)
                            {
                                throw;
                            }
                            await dBPago.Agregar(nuevoPago);
                            ultimo.IdEstudianteNavigation.estado = "Inactivo";
                            await serviceEstudiante.Actualizar(ultimo.IdEstudianteNavigation);
                        }
                    }
                    if (ultimo.FechaPago.AddDays(27).ToDateTime(TimeOnly.MinValue) < DateTime.Today)
                    {
                        try
                        {
                            await NotificacionesCorreo.EnviarCorreoAsync(ultimo.IdEstudianteNavigation.Correo, "Fecha proximo pago", $"Estimado {item.Nombres + " " + item.Apellidos}, su pago está pendiente para el día {item.FechaRegistro.Value.AddMonths(1).ToShortDateString()}.");
                        }
                        catch (Exception ex)
                        {
                            throw;
                        }
                    }
                }
            }
        }
        
        public async Task<Pago> Buscar(string idEstudiante)
        {
            List<Pago> pagos = await Leer();
            if(pagos != null)
            {
                return pagos.Where(p => p.IdEstudiante == idEstudiante).ToList().LastOrDefault();
            }
            return null;
        }
        public async Task<List<Pago>> Leer()
        {
            return await dBPago.Leer();
        }
    }
}
