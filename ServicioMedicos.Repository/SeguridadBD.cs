using Dapper;
using DocumentFormat.OpenXml.InkML;
using Servicios_Medicos.Entities;
using System.Data;

namespace Servicios_Medicos.Repository
{
    public class SeguridadBD
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;


        public SeguridadBD(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }


        public async Task<SeguridadLog?> ObtenerUsuario(string username)
        {
            using (var connection = _dbConnectionFactory.CreateConnection())
            {
                return await connection.QuerySingleOrDefaultAsync<SeguridadLog>("ValidarUsuario", new { pUsuario = username }, commandType: CommandType.StoredProcedure
                );
            }
        }

        public async Task<bool> RegistrarUsuario(SeguridadLog usuario)
        {
            using (var connection = _dbConnectionFactory.CreateConnection())
            {
                var filas = await connection.ExecuteAsync("RegistrarUsuario",new
                    {
                        pUsuario = usuario.Usuario,
                        pContrasena = usuario.PasswordCifrada,
                        pIdRol = usuario.IdRol,
                        pEstado = usuario.Estado
                    },
                    commandType: CommandType.StoredProcedure
                );

                return filas > 0;
            }
        }

        public async Task RegistrarIntentoFallido(
            int usuario,
            int intentos)
        {
            using var connection = _dbConnectionFactory.CreateConnection();

            await connection.ExecuteAsync(
                "RegistrarIntentoFallido",
                new
                {
                    pIdUsuario = usuario,
                    pIntentos = intentos
                },
                commandType: CommandType.StoredProcedure);
        }
    }
}