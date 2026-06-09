using Microsoft.AspNetCore.Http;
using Servicios_Medicos.Repository;
using Servicios_Medicos.Services.Abstract;

namespace Servicios_Medicos.Services
{
    public class Ubicacion : IUbicacion
    {
        private readonly UbicacionBD _ubicacionBD;

        public Ubicacion(UbicacionBD ubicacionBD)
        {
            _ubicacionBD = ubicacionBD;
        }

        public async Task CargarUbicaciones(
            IFormFile archivo)
        {
            if (archivo == null || archivo.Length == 0)
                throw new Exception("Debe seleccionar un archivo.");

            var extension =
                Path.GetExtension(archivo.FileName)
                    .ToLower();

            using var stream =
                archivo.OpenReadStream();

            if (extension == ".xlsx" ||
                extension == ".xls")
            {
                var datos =
                    await _ubicacionBD
                        .LeerExcel(stream);

                await _ubicacionBD
                    .GuardarUbicaciones(datos);
            }
            
            else
            {
                throw new Exception(
                    "Formato de archivo no permitido.");
            }
        }
    }
}