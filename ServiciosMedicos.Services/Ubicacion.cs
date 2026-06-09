using Microsoft.AspNetCore.Http;
using Servicios_Medicos.Repository;
using Servicios_Medicos.Services.Abstract;
using ServiciosMedicos.Entities;

namespace Servicios_Medicos.Services
{
    public class Ubicacion : IUbicacion
    {
        private readonly UbicacionBD _ubicacionBD;
        private readonly BitacoraRepository _bitacora;

        public Ubicacion(UbicacionBD ubicacionBD, BitacoraRepository bitacora)
        {
            _ubicacionBD = ubicacionBD;
            _bitacora = bitacora;
        }

        public async Task CargarUbicaciones(IFormFile archivo, int idUsuario)
        {
            if (archivo == null || archivo.Length == 0)
                throw new Exception("Debe seleccionar un archivo.");

            var extension = Path.GetExtension(archivo.FileName).ToLower();

            if (extension != ".xlsx" && extension != ".xls")
                throw new Exception("Formato de archivo no permitido.");

            using var stream = archivo.OpenReadStream();

            var datos = await _ubicacionBD.LeerExcel(stream);

            await _ubicacionBD.GuardarUbicaciones(datos);

            await _bitacora.Registrar(idUsuario, "se realizó la carga de información", new
            {
                tabla = "Provincia, Canton, Distrito"
            });
        }
    }
}