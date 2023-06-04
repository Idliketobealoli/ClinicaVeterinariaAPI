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
        }

        [Required, Key]
        public Guid Id { get; set; }
        public Guid PetId { get; set; }
    }
}