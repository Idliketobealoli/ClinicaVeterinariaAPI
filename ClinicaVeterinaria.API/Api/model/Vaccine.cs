using System.ComponentModel.DataAnnotations;

namespace ClinicaVeterinaria.API.Api.model
{
    public class Vaccine
    {
        public Vaccine() { }
        public Vaccine(Guid petId, string name, DateOnly date)
        {
            Id = Guid.NewGuid();
            PetId = petId;
            Name = name;
            Date = date;
        }

        [Key]
        public Guid Id { get; set; }
        [Required]
        public Guid PetId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public DateOnly Date { get; set; }
    }
}