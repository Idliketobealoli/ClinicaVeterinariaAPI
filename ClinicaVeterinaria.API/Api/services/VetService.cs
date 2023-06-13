using ClinicaVeterinaria.API.Api.dto;
using ClinicaVeterinaria.API.Api.mappers;
using ClinicaVeterinaria.API.Api.repositories;
using ClinicaVeterinaria.API.Api.services.bcrypt;
using ClinicaVeterinaria.API.Api.services.tokens;

namespace ClinicaVeterinaria.API.Api.services
{
    public class VetService
    {
        private readonly VetRepository Repo;
        private readonly AppointmentRepository ARepo;

        public VetService(VetRepository repo, AppointmentRepository aRepo)
        {
            Repo = repo;
            ARepo = aRepo;
        }
        public VetService() { }

        // Finds all vets in the database and maps them to DTOs
        public virtual async Task<List<VetDTO>> FindAll()
        {
            var entities = await Repo.FindAll();
            var entitiesDTOs = new List<VetDTO>();
            foreach (var entity in entities)
            {
                entitiesDTOs.Add(entity.ToDTO());
            }
            return entitiesDTOs;
        }

        // Finds all vets in the database and maps them to DTOs for appointments
        public virtual async Task<List<VetDTOappointment>> FindAllAppointment()
        {
            var entities = await Repo.FindAll();
            var entitiesDTOs = new List<VetDTOappointment>();
            foreach (var entity in entities)
            {
                entitiesDTOs.Add(entity.ToDTOappointment());
            }
            return entitiesDTOs;
        }

        // Finds all vets in the database and maps them to DTOs ordered by their number of appointments received, with said number included
        public virtual async Task<List<VetDTOstats>> FindAllForStats()
        {
            var appointments = await ARepo.FindAll();
            var entities = await Repo.FindAll();
            var entitiesDTOs = new List<VetDTOstats>();
            foreach (var entity in entities)
            {
                var appointmentCount = appointments.Where(a => a.VetEmail == entity.Email).Count();
                entitiesDTOs.Add(entity.ToDTOstats(appointmentCount));
            }
            return entitiesDTOs.OrderByDescending(e => e.AppointmentAmount).ToList();
        }

        // Finds a vet in the database whose email matches the one given and maps it to DTO, or returns an error message
        public virtual async Task<Either<VetDTO, string>> FindByEmail(string email)
        {
            var user = await Repo.FindByEmail(email);
            if (user == null)
            {
                return new Either<VetDTO, string>
                    ($"Vet with email {email} not found.");
            }
            else return new Either<VetDTO, string>
                    (user.ToDTO());
        }

        // Finds a vet in the database whose email matches the one given and maps it to a shortened DTO, or returns an error message
        public virtual async Task<Either<VetDTOshort, string>> FindByEmailShort(string email)
        {
            var user = await Repo.FindByEmail(email);
            if (user == null)
            {
                return new Either<VetDTOshort, string>
                    ($"Vet with email {email} not found.");
            }
            else return new Either<VetDTOshort, string>
                    (user.ToDTOshort());
        }

        // Finds a vet in the database whose email matches the one given and maps it to an appointment DTO, or returns an error message
        public virtual async Task<Either<VetDTOappointment, string>> FindByEmailAppointment(string email)
        {
            var user = await Repo.FindByEmail(email);
            if (user == null) return new Either<VetDTOappointment, string>
                    ($"Vet with email {email} not found.");

            else return new Either<VetDTOappointment, string>
                    (user.ToDTOappointment());
        }

        // Registers a vet in the database if and only if its information is valid, and returns its data and a token
        public virtual async Task<Either<VetDTOandToken, string>> Register(VetDTOregister dto, IConfiguration? config)
        {
            if (dto.Role.ToUpper() != "VET" && dto.Role.ToUpper() != "ADMIN")
            {
                return new Either<VetDTOandToken, string>
                    ("Could not create vet. Role not defined correctly.");
            }
            var userByEmail = await Repo.FindByEmail(dto.Email);
            var userBySSNumber = await Repo.FindBySSNum(dto.SSNumber);
            if (userBySSNumber != null || userByEmail != null)
                return new Either<VetDTOandToken, string>
                    ("Cannot use either that email or that Social Security number.");
            else
            {
                var user = dto.FromDTOregister();
                var created = await Repo.Create(user);

                if (created != null)
                {
                    var token = TokenService.CreateToken(created, config);
                    return new Either<VetDTOandToken, string>
                        (created.ToDTOwithToken(token));
                }

                else return new Either<VetDTOandToken, string>
                        ("Could not register vet.");
            }
        }

        // Logs in a vet in the database if and only if its information is valid, and returns its data and a token
        public virtual async Task<Either<VetDTOandToken, string>> Login(VetDTOloginOrChangePassword dto, IConfiguration? config)
        {
            var userByEmail = await Repo.FindByEmail(dto.Email);

            if (userByEmail != null && CipherService.Decode(dto.Password, userByEmail.Password))
            {
                var token = TokenService.CreateToken(userByEmail, config);
                return new Either<VetDTOandToken, string>
                    (userByEmail.ToDTOwithToken(token));
            }
            else
            {
                return new Either<VetDTOandToken, string>
                    ("Incorrect email or password.");
            }
        }

        // Lets a vet change their password, if and only if its information is valid, and returns its data
        public virtual async Task<Either<VetDTO, string>> ChangePassword(VetDTOloginOrChangePassword dto)
        {
            var user = await Repo.UpdatePassword(dto.Email, CipherService.Encode(dto.Password));

            if (user != null) return new Either<VetDTO, string>
                    (user.ToDTO());

            else return new Either<VetDTO, string>
                    ($"Vet with email {dto.Email} not found.");
        }

        // Disables a vet in the database, making it invisible to non-targeted search operations.
        public virtual async Task<Either<VetDTO, string>> Delete(string email)
        {
            var user = await Repo.SwitchActivity(email, false);

            if (user != null) return new Either<VetDTO, string>
                    (user.ToDTO());

            else return new Either<VetDTO, string>
                    ($"Vet with email {email} not found.");
        }
    }
}
