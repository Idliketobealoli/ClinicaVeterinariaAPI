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
                var user = UserRepo.FindByEmail(task.UserEmail);
                var pet = PetRepo.FindById(task.PetId);
                var vet = VetRepo.FindByEmail(task.VetEmail);
                Task.WaitAll(user, pet, vet);
                if (user.Result == null)
                    return new Either<AppointmentDTO, string>
                        ($"User with email {task.UserEmail} not found.");

                if (pet.Result == null)
                    return new Either<AppointmentDTO, string>
                        ($"Pet with id {task.PetId} not found.");

                if (vet.Result == null)
                    return new Either<AppointmentDTO, string>
                        ($"Vet with email {task.VetEmail} not found.");

                return new Either<AppointmentDTO, string>(task.ToDTO(user.Result, pet.Result, vet.Result));
            }
        }

        public virtual async Task<Either<AppointmentDTO, DomainError>> Create(Appointment appointment)
        {
            var userByEmail = UserRepo.FindByEmail(appointment.UserEmail);
            var vetByEmail = VetRepo.FindByEmail(appointment.VetEmail);
            var allAppointments = Repo.FindAll();
            var pet = PetRepo.FindById(appointment.PetId);

            Task.WaitAll(userByEmail, vetByEmail, allAppointments, pet);

            IEnumerable<Appointment>? newList = new List<Appointment>();
            if (allAppointments.Result != null)
            {
                newList =
                    from ap in allAppointments.Result
                    where (ap.InitialDate == appointment.InitialDate)
                    select ap;
            }

            if (
                userByEmail.Result != null && // Si el usuario existe en la DB.
                vetByEmail.Result != null &&  // Si el veterinario existe en la DB.
                !newList.Any() &&             // Si no hay otras citas en esa hora.
                appointment.InitialDate       // Si la fecha de inicio es
                < appointment.FinishDate &&   // anterior a la de fin.
                pet.Result != null            // Si la mascota existe en la DB.
            )
            {
                await Repo.Create(appointment);
                return new Either<AppointmentDTO, DomainError>
                    (appointment.ToDTO(userByEmail.Result, pet.Result, vetByEmail.Result));
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

            var usr = UserRepo.FindByEmail(appointment.UserEmail);
            var pt = PetRepo.FindById(appointment.PetId);
            var vt = VetRepo.FindByEmail(appointment.VetEmail);
            Task.WaitAll(usr, pt, vt);
            if (usr.Result == null)
                return new Either<AppointmentDTO, DomainError>
                    (new UserErrorNotFound($"User with email {appointment.UserEmail} not found."));

            if (pt.Result == null)
                return new Either<AppointmentDTO, DomainError>
                    (new PetErrorNotFound($"Pet with id {appointment.PetId} not found."));

            if (vt.Result == null)
                return new Either<AppointmentDTO, DomainError>
                    (new VetErrorNotFound($"Vet with email {appointment.VetEmail} not found."));

            var successfulResult = appointment.ToDTO(usr.Result, pt.Result, vt.Result);

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