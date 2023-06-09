﻿namespace ClinicaVeterinaria.API.Api.dto
{
    public class AppointmentDTO
    {
        public Guid Id { get; set; }
        public UserDTOshort User { get; set; }
        public string InitialDate { get; set; }
        public string FinishDate { get; set; }
        public PetDTOshort Pet { get; set; }
        public string Issue { get; set; }
        public string State { get; set; }
        public VetDTOappointment Vet { get; set; }

        public AppointmentDTO
            (
            Guid id,
            UserDTOshort user,
            string initialDate,
            string finishDate,
            PetDTOshort pet,
            string issue,
            string state,
            VetDTOappointment vet
            )
        {
            Id = id;
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
        public string Issue { get; set; }
        public PetDTOshort Pet { get; set; }

        public AppointmentDTOshort
            (
            Guid id,
            string initialDate,
            string issue,
            PetDTOshort pet
            )
        {
            Id = id;
            InitialDate = initialDate;
            Issue = issue;
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
        public string? VetEmail { get; set; }

        public AppointmentDTOcreate
            (
            string userEmail, string initialDate,
            string finishDate, string petId, string issue,
            string state, string? vetEmail
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