namespace PokemonReview.Models
{
    public class Country
    {
        public int Id { get; set; }
        public string Name { get; set; }

        //a country can have many owwners...
        public ICollection<Owner> Owners { get; set; }
    }
}
