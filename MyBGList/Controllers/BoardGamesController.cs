using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyBGList.DataContext;
using MyBGList.DTO;
using MyBGList.Models;
using System.Linq.Dynamic.Core;

namespace MyBGList.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BoardGamesController : ControllerBase
    {
        private readonly ILogger<BoardGamesController> _logger;
        private readonly ApplicationDbContext _context;

        public BoardGamesController(ILogger<BoardGamesController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet(Name = "GetBoardGames")]
        [ResponseCache(Location = ResponseCacheLocation.Any, Duration = 60)] // Caching this route for 1minute
        public async Task<RestDTO<BoardGame[]>> Get(int pageNumber = 0, int pageSize = 10, string? sortColumn = "Name", string? sortOrder = "ASC", string? filterQuery = null)
        {

            var query = _context.BoardGames.AsQueryable();
            if (!string.IsNullOrEmpty(filterQuery))
                query = query.Where(b => b.Name.Contains(filterQuery));
            var recordCount = await query.CountAsync();
            query = query
            .OrderBy($"{sortColumn} {sortOrder}")
            .Skip(pageNumber * pageSize)
            .Take(pageSize);

            return new RestDTO<BoardGame[]>()
            {
                Data = await query.ToArrayAsync(),
                PageNumber = pageNumber,
                PageSize = pageSize,
                RecordCount = recordCount,
                Links = new List<LinkDTO> {
                                new LinkDTO(Url.Action(null, "BoardGames", new { pageNumber, pageSize }, Request.Scheme)!,
                                            "self",
                                             "GET"
                                               ),
}
            };
        }

    }
}
