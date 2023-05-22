using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicaVeterinaria.API.Api.model
{
    [Table("Vaccines")]
    public class Vaccine
    {
        public Vaccine(Guid petId, string name, DateOnly date)
        {
            Id = Guid.NewGuid();
            PetId = petId;
            Name = name;
            Date = date;
        }

        [Required, Key]
        public Guid Id { get; set; }
        [Required]
        public Guid PetId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public DateOnly Date { get; set; }
    }
}