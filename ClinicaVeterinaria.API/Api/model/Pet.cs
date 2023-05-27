using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicaVeterinaria.API.Api.model
{
    [Table("Pets")]
    public class Pet
    {
        public Pet
            (
            Guid id, string name, string species, string race,
            double weight, double size, Sex sex, DateOnly birthDate,
            string ownerEmail
            )
        {
            Id = id;
            Name = name;
            Species = species;
            Race = race;
            Weight = weight;
            Size = size;
            Sex = sex;
            BirthDate = birthDate;
            OwnerEmail = ownerEmail;
            History = new History(id);
        }

        [Required, Key]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Species { get; set; }
        [Required]
        public string Race { get; set; }
        [Required]
        public double Weight { get; set; }
        [Required]
        public double Size { get; set; }
        [Required]
        public Sex Sex { get; set; }
        [Required]
        public DateOnly BirthDate { get; set; }
        [Required]
        public string OwnerEmail { get; set; }
        [Required]
        public History History { get; set; }
    }
}