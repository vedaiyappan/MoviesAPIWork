using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MoviesAPIWork.DTOs;
using MoviesAPIWork.Entities;
using MoviesAPIWork.Filters;
using MoviesAPIWork.Services;

namespace MoviesAPIWork.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors(PolicyName = "AllowAPIRequestIO")]

    // public class GenresController : ControllerBase
    public class GenresController : ControllerBase

    {
        //private readonly IRepository repository;
        private readonly ILogger<GenresController> logger;
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;


        public GenresController(ILogger<GenresController> logger ,
            ApplicationDbContext context, IMapper mapper)
        {
            //this.repository = repository;
            this.logger = logger;
            this.context = context;
            this.mapper = mapper;

        }
        [HttpGet] // api/genres
        //[HttpGet("list")] // api/genres/list
        //[HttpGet("/allgenres")] // allgenres
        //[ResponseCache(Duration = 60)]
        //[ServiceFilter(typeof(MyActionFilter))]
        public async Task<ActionResult<List<GenreDTO>>> Get()
        {
            //logger.LogInformation("Getting all the genres");

            //return await context.Genres.AsNoTracking().ToListAsync(); // repository.GetAllGenres();
            //var genresDTOs = mapper.Map<List<GenreDTO>>(genres);
            //return genresDTOs;

            var genres = await context.Genres.AsNoTracking().ToListAsync();
            var genresDTOs = mapper.Map<List<GenreDTO>>(genres);
            return genresDTOs;


        }
        [HttpGet("{Id:int}", Name = "getGenre")] // api/genres/example
        public async Task<ActionResult<GenreDTO>> Get(int Id)
        
        {

            var genre = await context.Genres.FirstOrDefaultAsync(x => x.Id == Id);

            if (genre == null)
            {
                return NotFound();
            }
            var genreDTO = mapper.Map<GenreDTO>(genre);

            return genreDTO;
        }

        [HttpPost]
        public async Task <ActionResult> Post([FromBody] GenreCreationDTO genreCreation)
        {
            var genre = mapper.Map<Genre>(genreCreation);
            context.Add(genre);
            await context.SaveChangesAsync();

            var genreDTO = mapper.Map<GenreDTO>(genre);

            return new CreatedAtRouteResult("getGenre", new { genreDTO.Id }, genreDTO);

            //context.Add(genre);
            //await context.SaveChangesAsync();

            //return new CreatedAtRouteResult("getGenre", new { Id = genre.Id }, genre);

        }


        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] GenreCreationDTO genreCreation)
        {
            var genre = mapper.Map<Genre>(genreCreation);
            genre.Id = id;
            context.Entry(genre).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]

        [DisableCors]
        public async Task<ActionResult> Delete(int id)
        {
            var exists = await context.Genres.AnyAsync(x => x.Id == id);
            if (!exists)
            {
                return NotFound();
            }

            context.Remove(new Genre() { Id = id });
            await context.SaveChangesAsync();

            return NoContent();
        }
    }

}
