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
                appointment.InitialDate,
                appointment.FinishDate,
                pet.ToDTOshort(),
                appointment.Issue,
                appointment.State,
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
                appointment.InitialDate,
                pet.ToDTOshort()
                );
        }
    }
}
