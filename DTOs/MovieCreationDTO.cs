using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoviesAPIWork.Helpers;
using MoviesAPIWork.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesAPIWork.DTOs
{
    public class MovieCreationDTO : MoviePatchDTO
    {
        //[Required]
        //[StringLength(300)]
        //public string Title { get; set; }
        //public string Summary { get; set; }
        //public bool InTheaters { get; set; }
        //public DateTime ReleaseDate { get; set; }
        [FileSizeValidator(MaxFileSizeInMbs: 4)]
        [ContentTypeValidator(ContentTypeGroup.Image)]
        public IFormFile Poster { get; set; }

        [ModelBinder(BinderType = typeof(TypeBinder<List<int>>))]
        public List<int> GenresIds { get; set; }

        [ModelBinder(BinderType = typeof(TypeBinder<List<ActorCreationDTO>>))]
        public List<ActorCreationDTO> Actors { get; set; }
    }
}
