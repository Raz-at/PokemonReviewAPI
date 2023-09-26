using AutoMapper;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Core.Infrastructure;
using PokemonReview.Dto;
using PokemonReview.Interfaces;
using PokemonReview.Models;
using PokemonReview.Repository;
using System.Collections.Generic;

namespace PokemonReview.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PokemonController : Controller
    {
        private readonly IPokemonRepository _pokemonRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IReviewRepository _reviewRepository;
        private readonly IOwnerRepository _ownerRepository;
        private readonly IMapper _mapper; 

        public PokemonController(IPokemonRepository pokemonRepository,IOwnerRepository ownerRepository , 
                                 ICategoryRepository categoryRepository, 
                                 IReviewRepository reviewRepository,IMapper mapper)
        {
            _pokemonRepository = pokemonRepository;
            _categoryRepository = categoryRepository;
            _reviewRepository = reviewRepository;
            _ownerRepository = ownerRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]
        public IActionResult GetPokemons()
        {
            var pokemon = _mapper.Map<List<PokemonDto>>(_pokemonRepository.GetPokemons());
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                return Ok(pokemon);
            }
        }

        [HttpGet("{pokeId}")]
        [ProducesResponseType(200, Type=typeof(Pokemon))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemon(int pokeId)
        {
            if (!_pokemonRepository.PokemonExists(pokeId))
                return NotFound();

            var pokemon = _mapper.Map<PokemonDto>(_pokemonRepository.GetPokemon(pokeId));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                return Ok(pokemon);
            }
        }

        [HttpGet("{pokeId}/rating")]
        [ProducesResponseType(200, Type = typeof(decimal))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemonRating(int pokeId)
        {
            if (!_pokemonRepository.PokemonExists(pokeId))
                return NotFound();

            var pokemon = _pokemonRepository.GetPokemonRating(pokeId);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                return Ok(pokemon);
            }
        }


        [HttpGet("pokemon/{name}")]
        [ProducesResponseType(200, Type = typeof(Pokemon))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemon(string name)
        {
            var pokemon =_mapper.Map<PokemonDto>(_pokemonRepository.GetPokemon(name));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                return Ok(pokemon);
            }
        }


        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreatePokemon([FromQuery] int ownerId , [FromQuery] int categoryId,[FromBody] PokemonDto pokemonCreate)
        {
            if (pokemonCreate == null)
                return BadRequest(ModelState);

            var pokemon = _pokemonRepository.GetPokemons().Where(x => x.Name.Trim().ToUpper()
            == pokemonCreate.Name.Trim().ToUpper()).FirstOrDefault();

            if (pokemon != null)
            {
                ModelState.AddModelError("", "Pokemon already exist");

            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var pokemonMap = _mapper.Map<Pokemon>(pokemonCreate);
            
            if (!_pokemonRepository.CreatePokemon(ownerId , categoryId ,pokemonMap))
            {
                ModelState.AddModelError("", "Error while adding new owner..");
                return StatusCode(400, ModelState);
            }
            return Ok("Succefully Created.");

        }

        [HttpPut("{pokemonId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult UpdatePokemon([FromQuery] int ownerId, [FromQuery] int categoryId, int pokemonId, [FromBody] PokemonDto pokemon)
        {
            if (pokemon == null)
                return BadRequest(ModelState);

            if (pokemonId != pokemon.Id)
                return BadRequest(ModelState);

            if (!_pokemonRepository.PokemonExists(pokemonId))
                return BadRequest(ModelState);

            var pokemonMap = _mapper.Map<Pokemon>(pokemon);

            if (!_pokemonRepository.UpdatePokemon(ownerId , categoryId , pokemonMap))
            {
                ModelState.AddModelError("", "Error updating the pokemon");
                return StatusCode(400, ModelState);
            }
            return NoContent();
        }

        [HttpDelete("{pokemonId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult DeleteCategory([FromQuery] int pokemonId)
        {
            if (!_pokemonRepository.PokemonExists(pokemonId))
                return BadRequest(ModelState);


            var reviewToDelete = _reviewRepository.GetReviewOfAPokemon(pokemonId);
            var pokemonToDelete = _pokemonRepository.GetPokemon(pokemonId);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_reviewRepository.DeleteReviews(reviewToDelete.ToList()))
            {
                ModelState.AddModelError("", "Error deleting the review");
                return StatusCode(400, ModelState);
            }

            if (!_pokemonRepository.DeletePokemon(pokemonToDelete))
            {
                ModelState.AddModelError("", "Error deleting the pokemon");
                return StatusCode(400, ModelState);
            }
            return NoContent();
        }




    }
}
