using ClinicaVeterinaria.API.Api.dto;
using ClinicaVeterinaria.API.Api.model;
using ClinicaVeterinaria.API.Api.repositories;

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
                appointment.Id,
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
                appointment.Issue,
                pet.ToDTOshort()
                );
        }

        public static Appointment FromDTO(this AppointmentDTOcreate dto, VetRepository vRepo)
        {
            var iDate = DateTime.Parse(dto.InitialDate);
            var fDate = DateTime.Parse(dto.FinishDate);
            if (dto.VetEmail == null)
            {
                var emails = vRepo.FindAll();
                emails.Wait();

                return new
                (
                dto.UserEmail,
                new DateTime(iDate.Year, iDate.Month, iDate.Day, iDate.Hour, iDate.Minute, 0, DateTimeKind.Utc),
                new DateTime(fDate.Year, fDate.Month, fDate.Day, fDate.Hour, fDate.Minute, 0, DateTimeKind.Utc),
                Guid.Parse(dto.PetId),
                dto.Issue,
                emails.Result[new Random().Next(emails.Result.Count - 1)].Email
                );
            }
            else return new
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
