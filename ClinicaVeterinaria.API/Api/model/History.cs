using System.ComponentModel.DataAnnotations;

namespace ClinicaVeterinaria.API.Api.model
{
    public class History
    {
        public History
            (
            Guid petId
            )
        {
            Id = Guid.NewGuid();
            PetId = petId;
            Vaccines = new HashSet<Vaccine>();
            AilmentTreatment = new Dictionary<string,string>();
        }

        [Key]
        public Guid Id { get; set; }
        public Guid PetId { get; set; }
        [Required]
        public HashSet<Vaccine> Vaccines { get; set; }
        [Required]
        public Dictionary<string, string> AilmentTreatment { get; set; }
    }
}