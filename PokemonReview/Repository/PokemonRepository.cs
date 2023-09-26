using Microsoft.AspNetCore.Mvc;
using PokemonReview.Data;
using PokemonReview.Interfaces;
using PokemonReview.Models;

namespace PokemonReview.Repository
{
    public class PokemonRepository : IPokemonRepository
    {
        private readonly DataContext _context;

        public PokemonRepository(DataContext context)
        {
            _context = context;
        }


        //this is to map many to many relationships....
        public bool CreatePokemon(int ownerId, int categoryId, Pokemon pokemon)
        {
            var pokemonOwner = _context.Owners.FirstOrDefault(x => x.Id == ownerId);
            var category = _context.Categories.FirstOrDefault(x => x.Id == categoryId);

            //to also fill the many to many relation table ie pokemonOwner
            var OwnerPokemon = new PokemonOwner()
            {
                Owner = pokemonOwner,
                Pokemon = pokemon,
            };
            _context.Add(OwnerPokemon);

            //to also fill the many to many relation table ie pokemonCategory
            var CategoryPokemon = new PokemonCategory()
            {
                Category = category,
                Pokemon = pokemon,
            };
            _context.Add(CategoryPokemon);
            _context.Add(pokemon);
            return Save();            
        }

        public bool DeletePokemon(Pokemon pokemon)
        {
            _context.Remove(pokemon);
            return Save();
        }

        public Pokemon GetPokemon(int id)
        {
            return _context.Pokemons.FirstOrDefault(p => p.Id == id);
        }

        public Pokemon GetPokemon(string name)
        {
            return _context.Pokemons.FirstOrDefault(p => p.Name == name);
        }

        public decimal GetPokemonRating(int pokeId)
        {
            var review = _context.Reviews.Where(p => p.Pokemon.Id == pokeId);

            if (review.Count() <= 0)
            {
                return 0;
            }
            return ((decimal)review.Sum(r => r.Rating) / review.Count());
        }

        public ICollection<Pokemon> GetPokemons()
        {
            return _context.Pokemons.OrderBy(p=> p.Id).ToList();
        }

        public bool PokemonExists(int pokeId)
        {
            return _context.Pokemons.Any(p => p.Id == pokeId);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdatePokemon(int ownerId, int categoryId, Pokemon pokemon)
        {
            _context.Update(pokemon);
            return Save();
        }
    }
}
