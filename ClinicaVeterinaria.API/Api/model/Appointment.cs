using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicaVeterinaria.API.Api.model
{
    [Table("Appointments")]
    public class Appointment
    {
        public Appointment
            (
            string userEmail,
            DateTime initialDate,
            DateTime finishDate,
            Guid petId,
            string issue,
            string vetEmail
            )
        {
            Id = Guid.NewGuid();
            UserEmail = userEmail;
            InitialDate = initialDate;
            FinishDate = finishDate;
            PetId = petId;
            Issue = issue;
            State = State.PENDING;
            VetEmail = vetEmail;
        }

        [Required, Key]
        public Guid Id { get; set; }
        [Required]
        public string UserEmail { get; set; }
        [Required]
        public DateTime InitialDate { get; set; }
        [Required]
        public DateTime FinishDate { get; set; }
        [Required]
        public Guid PetId { get; set; }
        [Required]
        public string Issue { get; set; }
        [Required]
        public State State { get; set; }
        [Required]
        public string VetEmail { get; set; }
    }

    public enum State
    {
        PENDING,
        PROGRESS,
        FINISHED
    }
}
