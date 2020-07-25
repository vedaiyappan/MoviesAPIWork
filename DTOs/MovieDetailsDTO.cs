using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesAPIWork.DTOs
{
    public class MovieDetailsDTO : MovieDTO
    {
        public List<GenreDTO> Genres { get; set; }
        public List<ActorDTO> Actors { get; set; }
    }
}
