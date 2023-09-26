using Microsoft.AspNetCore.Mvc;
using PokemonReview.Models;

namespace PokemonReview.Interfaces
{
    public interface IOwnerRepository 
    {
        ICollection<Owner> GetOwners();
        Owner GetOwner(int ownerId);
        bool OwnerExist(int ownerId);
        ICollection<Pokemon> GetPokemonByOwner(int ownerId);
        ICollection<Owner> GetOwnerOfAPokemon(int pokeId);

        bool CreateOwner(Owner owner);
        bool DeleteOwner(Owner owner);
        bool UpdateOwner(Owner owner);
        bool Save();

    }
}
