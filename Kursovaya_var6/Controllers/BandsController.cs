using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Kursovaya_var6.Data;
using Kursovaya_var6.Models;

namespace Kursovaya_var6.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BandsController : ControllerBase
    {
        private readonly MBContext _context;

        public BandsController(MBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(string? search, string? sort)
        {
            var query = _context.Bands
                .Include(b => b.GroupName)
                .Include(b => b.Leader)
                .Include(b => b.Genre)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(b =>
                    b.GroupName.Name.Contains(search) ||
                    b.Leader.Name.Contains(search) ||
                    b.Genre.Name.Contains(search));
            }

            query = sort switch
            {
                "group_asc" => query.OrderBy(b => b.GroupName.Name),
                "group_desc" => query.OrderByDescending(b => b.GroupName.Name),

                "leader_asc" => query.OrderBy(b => b.Leader.Name),
                "leader_desc" => query.OrderByDescending(b => b.Leader.Name),

                "albums_asc" => query.OrderBy(b => b.AlbumsCount),
                "albums_desc" => query.OrderByDescending(b => b.AlbumsCount),

                "genre_asc" => query.OrderBy(b => b.Genre.Name),
                "genre_desc" => query.OrderByDescending(b => b.Genre.Name),

                _ => query
            };

            return Ok(await query.ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BandCreateDto dto)
        {
            var group = await _context.GroupNames.FirstOrDefaultAsync(x => x.Name == dto.GroupName);

            if (group == null)
            {
                group = new GroupName { Name = dto.GroupName };
                _context.GroupNames.Add(group);
                await _context.SaveChangesAsync();
            }

            var leader = await _context.Leaders.FirstOrDefaultAsync(x => x.Name == dto.Leader);

            if (leader == null)
            {
                leader = new Leader { Name = dto.Leader };
                _context.Leaders.Add(leader);
                await _context.SaveChangesAsync();
            }

            var genre = await _context.Genres.FirstOrDefaultAsync(x => x.Name == dto.Genre);

            if (genre == null)
            {
                genre = new Genre { Name = dto.Genre };
                _context.Genres.Add(genre);
                await _context.SaveChangesAsync();
            }

            var band = new Band
            {
                GroupNameId = group.Id,
                LeaderId = leader.Id,
                GenreId = genre.Id,
                AlbumsCount = dto.AlbumsCount
            };

            _context.Bands.Add(band);
            await _context.SaveChangesAsync();

            return Ok(band);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var band = await _context.Bands.FindAsync(id);

            if (band == null)
                return NotFound();

            _context.Bands.Remove(band);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("count")]
        public async Task<IActionResult> Count(int min, int max)
        {
            var count = await _context.Bands
                .CountAsync(b => b.AlbumsCount >= min && b.AlbumsCount <= max);

            return Ok(count);
        }
    }
}