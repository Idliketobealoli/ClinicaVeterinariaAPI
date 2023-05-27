using ClinicaVeterinaria.API.Api.dto;
using ClinicaVeterinaria.API.Api.errors;
using ClinicaVeterinaria.API.Api.mappers;
using ClinicaVeterinaria.API.Api.model;
using ClinicaVeterinaria.API.Api.repositories;

namespace ClinicaVeterinaria.API.Api.services
{
    public class AppointmentService
    {
        private readonly AppointmentRepository Repo;
        private readonly PetRepository PetRepo;
        private readonly UserRepository UserRepo;
        private readonly VetRepository VetRepo;

        public AppointmentService
        (
            AppointmentRepository repo, PetRepository petRepo,
            UserRepository userRepo, VetRepository vetRepo
        )
        {
            Repo = repo;
            PetRepo = petRepo;
            UserRepo = userRepo;
            VetRepo = vetRepo;
        }

        public AppointmentService() { }

        public virtual async Task<List<AppointmentDTOshort>> FindAll()
        {
            var entities = await Repo.FindAll();
            var entitiesDTOs = new List<AppointmentDTOshort>();
            foreach (var entity in entities)
            {
                if (entity != null)
                {
                    var pet = await PetRepo.FindById(entity.PetId);

                    if (pet != null)
                    {
                        var appointment = entity.ToDTOshort(pet);
                        entitiesDTOs.Add(appointment);
                    }
                }
            }

            return entitiesDTOs;
        }

        public virtual async Task<Either<AppointmentDTO, string>> FindById(Guid id)
        {
            var task = await Repo.FindById(id);
            if (task == null)
            {
                return new Either<AppointmentDTO, string>
                    ($"Appointment with id {id} not found.");
            }
            else
            {
                var user = await UserRepo.FindByEmail(task.UserEmail);
                var pet = await PetRepo.FindById(task.PetId);
                var vet = await VetRepo.FindByEmail(task.VetEmail);
                if (user == null)
                    return new Either<AppointmentDTO, string>
                        ($"User with email {task.UserEmail} not found.");

                if (pet == null)
                    return new Either<AppointmentDTO, string>
                        ($"Pet with id {task.PetId} not found.");

                if (vet == null)
                    return new Either<AppointmentDTO, string>
                        ($"Vet with email {task.VetEmail} not found.");

                return new Either<AppointmentDTO, string>(task.ToDTO(user, pet, vet));
            }
        }

        public virtual async Task<Either<AppointmentDTO, DomainError>> Create(AppointmentDTOcreate dto)
        {
            if (dto.State.ToUpper() != "PENDING" &&
                dto.State.ToUpper() != "PROGRESS" &&
                dto.State.ToUpper() != "FINISHED")
            {
                return new Either<AppointmentDTO, DomainError>
                    (new AppointmentErrorBadRequest("Incorrect state for the new appointment."));
            }
            var appointment = dto.FromDTO();
            var userByEmail = await UserRepo.FindByEmail(appointment.UserEmail);
            var vetByEmail = await VetRepo.FindByEmail(appointment.VetEmail);
            var allAppointments = await Repo.FindAll();
            var pet = await PetRepo.FindById(appointment.PetId);

            IEnumerable<Appointment>? newList = new List<Appointment>();
            if (allAppointments != null)
            {
                newList =
                    from ap in allAppointments
                    where (ap.InitialDate == appointment.InitialDate)
                    select ap;
            }

            if (
                userByEmail != null &&        // Si el usuario existe en la DB.
                vetByEmail != null &&         // Si el veterinario existe en la DB.
                !newList.Any() &&             // Si no hay otras citas en esa hora.
                appointment.InitialDate       // Si la fecha de inicio es
                < appointment.FinishDate &&   // anterior a la de fin.
                pet != null                   // Si la mascota existe en la DB.
            )
            {
                await Repo.Create(appointment);
                return new Either<AppointmentDTO, DomainError>
                    (appointment.ToDTO(userByEmail, pet, vetByEmail));
            }
            else
                return new Either<AppointmentDTO, DomainError>
                    (new AppointmentErrorBadRequest("Incorrect data for the new appointment."));
        }

        public virtual async Task<Either<AppointmentDTO, DomainError>> Delete(Guid id)
        {
            var appointment = await Repo.FindById(id);
            if (appointment == null)
            {
                return new Either<AppointmentDTO, DomainError>
                    (new AppointmentErrorNotFound($"Appointment with id {id} not found."));
            }

            var usr = await UserRepo.FindByEmail(appointment.UserEmail);
            var pt = await PetRepo.FindById(appointment.PetId);
            var vt = await VetRepo.FindByEmail(appointment.VetEmail);
            if (usr == null)
                return new Either<AppointmentDTO, DomainError>
                    (new UserErrorNotFound($"User with email {appointment.UserEmail} not found."));

            if (pt == null)
                return new Either<AppointmentDTO, DomainError>
                    (new PetErrorNotFound($"Pet with id {appointment.PetId} not found."));

            if (vt == null)
                return new Either<AppointmentDTO, DomainError>
                    (new VetErrorNotFound($"Vet with email {appointment.VetEmail} not found."));

            var successfulResult = appointment.ToDTO(usr, pt, vt);

            var deleted = await Repo.Delete(id);
            if (deleted != null)
            {
                return new Either<AppointmentDTO, DomainError>(successfulResult);
            }
            else
                return new Either<AppointmentDTO, DomainError>
                    (new AppointmentErrorBadRequest($"Could not delete Appointment with id {id}."));
        }
    }
}