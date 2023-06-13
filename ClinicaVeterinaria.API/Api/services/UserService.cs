using ClinicaVeterinaria.API.Api.dto;
using ClinicaVeterinaria.API.Api.mappers;
using ClinicaVeterinaria.API.Api.repositories;
using ClinicaVeterinaria.API.Api.services.bcrypt;
using ClinicaVeterinaria.API.Api.services.tokens;

namespace ClinicaVeterinaria.API.Api.services
{
    public class UserService
    {
        private readonly UserRepository Repo;
        private readonly PetRepository PRepo;

        public UserService(UserRepository repo, PetRepository pRepo)
        {
            Repo = repo;
            PRepo = pRepo;
        }

        public UserService() { }

        // Finds all users in the database and maps them to DTOs
        public virtual async Task<List<UserDTO>> FindAll()
        {
            var entities = await Repo.FindAll();
            var entitiesDTOs = new List<UserDTO>();
            foreach (var entity in entities)
            {
                entitiesDTOs.Add(entity.ToDTO());
            }
            return entitiesDTOs;
        }

        // Finds a user in the database whose email matches the one given and maps it to DTO, or returns an error message
        public virtual async Task<Either<UserDTO, string>> FindByEmail(string email)
        {
            var user = await Repo.FindByEmail(email);
            if (user == null)
            {
                return new Either<UserDTO, string>
                    ($"User with Email {email} not found.");
            }
            else return new Either<UserDTO, string>(user.ToDTO());
        }

        // Finds a user in the database whose email matches the one given and maps it to a shortened DTO, or returns an error message
        public virtual async Task<Either<UserDTOshort, string>> FindByEmailShort(string email)
        {
            var user = await Repo.FindByEmail(email);
            if (user == null)
            {
                return new Either<UserDTOshort, string>
                    ($"User with Email {email} not found.");
            }
            else return new Either<UserDTOshort, string>(user.ToDTOshort());
        }

        // Registers a user in the database if and only if its information is valid, and returns its data and a token
        public virtual async Task<Either<UserDTOandToken, string>> Register(UserDTOregister dto, IConfiguration? config)
        {
            var userByEmail = await Repo.FindByEmail(dto.Email);
            var userByPhone = await Repo.FindByPhone(dto.Phone);
            if (userByPhone == null && userByEmail == null)
            {
                var user = dto.FromDTOregister();
                var created = await Repo.Create(user);
                if (created != null)
                {
                    var token = TokenService.CreateToken(created, config);
                    return new Either<UserDTOandToken, string>(created.ToDTOwithToken(token));
                }
                else return new Either<UserDTOandToken, string>
                        ("Could not register user.");
            }
            else
            {
                return new Either<UserDTOandToken, string>
                    ("Cannot use either that email or that phone number.");
            }
        }

        // Logs in a user in the database if and only if its information is valid, and returns its data and a token
        public virtual async Task<Either<UserDTOandToken, string>> Login(UserDTOloginOrChangePassword dto, IConfiguration? config)
        {
            var userByEmail = await Repo.FindByEmail(dto.Email);
            if (userByEmail != null && CipherService.Decode(dto.Password, userByEmail.Password))
            {
                var token = TokenService.CreateToken(userByEmail, config);
                return new Either<UserDTOandToken, string>(userByEmail.ToDTOwithToken(token));
            }
            else
            {
                return new Either<UserDTOandToken, string>
                    ("Incorrect email or password.");
            }
        }

        // Lets a user change their password, if and only if its information is valid, and returns its data
        public virtual async Task<Either<UserDTO, string>> ChangePassword(UserDTOloginOrChangePassword dto)
        {
            var user = await Repo.UpdatePassword(dto.Email, CipherService.Encode(dto.Password));
            if (user != null)
            {
                return new Either<UserDTO, string>(user.ToDTO());
            }
            else return new Either<UserDTO, string>
                    ($"User with email {dto.Email} not found.");
        }

        // Disables a user in the database, making it invisible to non-targeted search operations,
        // and also disabling all its pets.
        public virtual async Task<Either<UserDTO, string>> Delete(string email)
        {
            var user = await Repo.SwitchActivity(email, false);
            if (user != null)
            {
                var allPets = await PRepo.FindAll();
                if (allPets != null)
                {
                    var pets = from pet in allPets
                        where (pet.OwnerEmail == email)
                        select pet;
                    foreach(var pet in pets)
                    {
                        await PRepo.Delete(pet.Id, false);
                    }
                }
                return new Either<UserDTO, string>(user.ToDTO());
            }
            else return new Either<UserDTO, string>
                    ($"User with email {email} not found.");
        }
    }
}
