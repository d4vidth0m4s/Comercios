using Comercios.Domain.Entities;
using Comercios.Domain.IRepository;
using Comercios.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Comercios.Infrastructure.Repositories
{
    public class ComerciosRepository : ICrearComerciosRepository
    {
        private readonly ComerciosDbContext _context;

        public ComerciosRepository(ComerciosDbContext context)
        {
            _context = context;
        }

        public async Task<string> CreateAsync(Comercio entity)
        {
            var comercioname = entity.Nombre;
            var res = await _context.Comercios.FirstOrDefaultAsync(u => u.Nombre.ToLower() == comercioname.ToLower());
            if (res != null) throw new UnauthorizedAccessException("res != null");

            _context.Comercios.Add(entity);
            await _context.SaveChangesAsync();
            return entity.Id;

        }

        public async Task<bool> UpdateAsync(Comercio entity, string userId)
        {

            var existing = await _context.Comercios
        .FirstOrDefaultAsync(c => c.Usuario_Id == userId);


            if (existing == null || existing.Usuario_Id != userId)
            {
                return false;
            }

            
            existing.Nombre = entity.Nombre;
            existing.Categorias = entity.Categorias;
            
            existing.FechaModificacion = DateTime.UtcNow;
            existing.ImgBannerUrl = entity.ImgBannerUrl;
            // Agrega aquí el resto de campos editables...

            if (entity.InfoComercio != null)
            {
                existing.InfoComercio ??= new DatosComercio();

                existing.InfoComercio.Direccion = entity.InfoComercio.Direccion;
                existing.InfoComercio.Ciudad = entity.InfoComercio.Ciudad;
                existing.InfoComercio.Telefono = entity.InfoComercio.Telefono;
            }

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Comercio?> ObtenerPorUsuarioIdAsync(string userId)
        {
            var normalized = userId.Trim().ToLower();

            return await _context.Comercios
                .Include(c => c.InfoComercio)
                .FirstOrDefaultAsync(c => c.Usuario_Id.ToLower() == normalized);
        }

        public async Task<(IReadOnlyList<Comercio> Comercios, int Total)> ObtenerComerciosAsync(
            int pagina,
            int limite,
            decimal? calificacionMinima)
        {
            var query = _context.Comercios
                .AsNoTracking()
                .Include(c => c.InfoComercio)
                .AsQueryable();

            if (calificacionMinima.HasValue)
                query = query.Where(c => c.Calificacion >= calificacionMinima.Value);

            var total = await query.CountAsync();

            var comercios = await query
                .OrderByDescending(c => c.Calificacion)
                .ThenByDescending(c => c.FechaCreacion)
                .Skip((pagina - 1) * limite)
                .Take(limite)
                .ToListAsync();

            return (comercios, total);
        }


    }
}
