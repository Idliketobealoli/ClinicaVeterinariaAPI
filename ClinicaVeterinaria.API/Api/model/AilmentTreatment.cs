using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ClinicaVeterinaria.API.Api.model
{
    [Table("AilmentTreatments")]
    public class AilmentTreatment
    {
        public AilmentTreatment(Guid petId, string ailment, string treatment)
        {
            Id = Guid.NewGuid();
            PetId = petId;
            Ailment = ailment;
            Treatment = treatment;
        }

        [Required, Key]
        public Guid Id { get; set; }
        [Required]
        public Guid PetId { get; set; }
        [Required]
        public string Ailment { get; set; }
        [Required]
        public string Treatment { get; set; }
    }
}
