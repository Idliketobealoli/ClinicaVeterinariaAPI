using ClinicaVeterinaria.API.Api.model;

namespace ClinicaVeterinaria.API.Api.dto
{
    public class AppointmentDTO
    {
        public UserDTOshort User { get; set; }
        public DateTime InitialDate { get; set; }
        public DateTime FinishDate { get; set; }
        public PetDTOshort Pet { get; set; }
        public string Issue { get; set; }
        public State State { get; set; }
        public VetDTOappointment Vet { get; set; }

        public AppointmentDTO
            (
            UserDTOshort user,
            DateTime initialDate,
            DateTime finishDate,
            PetDTOshort pet,
            string issue,
            State state,
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
        public DateTime InitialDate { get; set; }
        public PetDTOshort Pet { get; set; }

        public AppointmentDTOshort
            (
            Guid id,
            DateTime initialDate,
            PetDTOshort pet
            )
        {
            Id = id;
            InitialDate = initialDate;
            Pet = pet;
        }
    }
}