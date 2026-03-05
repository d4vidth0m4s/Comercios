using Comercios.Application.Interfaz;
using Comercios.Grpc;
using Grpc.Core;

namespace Comercios.Services
{
    public class ComerciosGrpcService : ComerciosService.ComerciosServiceBase
    {
        private readonly IComercioServices _comercioServices;

        public ComerciosGrpcService(IComercioServices comercioServices)
        {
            _comercioServices = comercioServices;
        }

        public override async Task<ObtenerComercioPorUsuarioResponse> ObtenerComercioPorUsuario(
            ObtenerComercioPorUsuarioRequest request,
            ServerCallContext context)
        {
            if (string.IsNullOrWhiteSpace(request.UsuarioId))
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "usuario_id es requerido"));
            }

            var comercio = await _comercioServices.ObtenerPorUsuarioIdAsync(request.UsuarioId.Trim());
            if (comercio == null)
            {
                return new ObtenerComercioPorUsuarioResponse { Encontrado = false };
            }

            var response = new ObtenerComercioPorUsuarioResponse
            {
                Encontrado = true,
                ComercioId = comercio.Id,
                Nombre = comercio.Nombre,
                Descripcion = comercio.Description,
                Abierto = comercio.Abierto,
                Calificacion = Convert.ToDouble(comercio.Calificacion),
                ImgBannerUrl = comercio.ImgBannerUrl
            };

            if (comercio.Categorias != null)
            {
                response.Categorias.AddRange(comercio.Categorias);
            }

            if (comercio.InfoComercio != null)
            {
                response.Direccion = comercio.InfoComercio.Direccion;
                response.Ciudad = comercio.InfoComercio.Ciudad;
                response.Telefono = comercio.InfoComercio.Telefono;
            }

            return response;
        }
    }
}
