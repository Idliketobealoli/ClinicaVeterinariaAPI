using ClinicaVeterinaria.API.Api.model;

namespace ClinicaVeterinaria.API.Api.dto
{
    public class AppointmentDTO
    {
        public UserDTOshort User { get; set; }
        public string InitialDate { get; set; }
        public string FinishDate { get; set; }
        public PetDTOshort Pet { get; set; }
        public string Issue { get; set; }
        public string State { get; set; }
        public VetDTOappointment Vet { get; set; }

        public AppointmentDTO
            (
            UserDTOshort user,
            string initialDate,
            string finishDate,
            PetDTOshort pet,
            string issue,
            string state,
            VetDTOappointment vet
            )
        {
            User = user;
            InitialDate = initialDate;
            FinishDate = finishDate;
            Pet = pet;
            Issue = issue;
            State = state;
            Vet = vet;
        }
    }

    public class AppointmentDTOshort
    {
        public Guid Id { get; set; }
        public string InitialDate { get; set; }
        public PetDTOshort Pet { get; set; }

        public AppointmentDTOshort
            (
            Guid id,
            string initialDate,
            PetDTOshort pet
            )
        {
            Id = id;
            InitialDate = initialDate;
            Pet = pet;
        }
    }

    public class AppointmentDTOcreate
    {
        public string UserEmail { get; set; }
        public string InitialDate { get; set; }
        public string FinishDate { get; set; }
        public string PetId { get; set; }
        public string Issue { get; set; }
        public string State { get; set; }
        public string VetEmail { get; set; }

        public AppointmentDTOcreate
            (
            string userEmail, string initialDate,
            string finishDate, string petId, string issue,
            string state, string vetEmail
            )
        {
            UserEmail = userEmail;
            InitialDate = initialDate;
            FinishDate = finishDate;
            PetId = petId;
            Issue = issue;
            State = state;
            VetEmail = vetEmail;
        }
    }
}