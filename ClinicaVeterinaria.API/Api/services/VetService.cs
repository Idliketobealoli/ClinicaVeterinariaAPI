using ClinicaVeterinaria.API.Api.dto;
using ClinicaVeterinaria.API.Api.errors;
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

        public virtual async Task<Either<VetDTOappointment, string>> FindByEmailAppointment(string email)
        {
            var user = await Repo.FindByEmail(email);
            if (user == null) return new Either<VetDTOappointment, string>
                    ($"Vet with email {email} not found.");

            else return new Either<VetDTOappointment, string>
                    (user.ToDTOappointment());
        }

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

        public virtual async Task<Either<VetDTO, string>> ChangePassword(VetDTOloginOrChangePassword dto)
        {
            var user = await Repo.UpdatePassword(dto.Email, CipherService.Encode(dto.Password));

            if (user != null) return new Either<VetDTO, string>
                    (user.ToDTO());

            else return new Either<VetDTO, string>
                    ($"Vet with email {dto.Email} not found.");
        }

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
