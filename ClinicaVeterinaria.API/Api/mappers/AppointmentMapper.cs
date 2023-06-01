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
                appointment.InitialDate.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                appointment.FinishDate.ToString("yyyy-MM-ddTHH:mm:ssZ"),
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
                appointment.InitialDate.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                pet.ToDTOshort()
                );
        }

        public static Appointment FromDTO(this AppointmentDTOcreate dto)
        {
            var iDate = DateTime.Parse(dto.InitialDate);
            var fDate = DateTime.Parse(dto.FinishDate);
            return new
                (
                dto.UserEmail,
                new DateTime(iDate.Year, iDate.Month, iDate.Day, iDate.Hour, iDate.Minute, 0, DateTimeKind.Utc),
                new DateTime(fDate.Year, fDate.Month, fDate.Day, fDate.Hour, fDate.Minute, 0, DateTimeKind.Utc),
                Guid.Parse(dto.PetId),
                dto.Issue,
                dto.VetEmail
                );
        }
    }
}
