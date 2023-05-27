using ClinicaVeterinaria.API.Api.dto;
using ClinicaVeterinaria.API.Api.model;

namespace ClinicaVeterinaria.API.Api.mappers
{
    internal static class AppointmentMapper
    {
        public static AppointmentDTO ToDTO
            (
            this Appointment appointment, User user,
            Pet pet, Vet vet
            )
        {
            return new
                (
                user.ToDTOshort(),
                appointment.InitialDate.ToString(),
                appointment.FinishDate.ToString(),
                pet.ToDTOshort(),
                appointment.Issue,
                States.ToString(appointment.State),
                vet.ToDTOappointment()
                );
        }

        public static AppointmentDTOshort ToDTOshort
            (
            this Appointment appointment, Pet pet
            )
        {
            return new
                (
                appointment.Id,
                appointment.InitialDate.ToString(),
                pet.ToDTOshort()
                );
        }

        public static Appointment FromDTO(this AppointmentDTOcreate dto)
        {
            return new
                (
                dto.UserEmail,
                DateTime.Parse(dto.InitialDate),
                DateTime.Parse(dto.FinishDate),
                Guid.Parse(dto.PetId),
                dto.Issue,
                dto.VetEmail
                );
        }
    }
}
