using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicaVeterinaria.API.Api.model
{
    [Table("Histories")]
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

        [Required, Key]
        public Guid Id { get; set; }
        public Guid PetId { get; set; }
        [Required]
        public HashSet<Vaccine> Vaccines { get; set; }
        [Required]
        public Dictionary<string, string> AilmentTreatment { get; set; }
    }
}