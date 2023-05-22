﻿using ClinicaVeterinaria.API.Api.dto;
using ClinicaVeterinaria.API.Api.mappers;
using ClinicaVeterinaria.API.Api.repositories;
using ClinicaVeterinaria.API.Api.services.bcrypt;
using ClinicaVeterinaria.API.Api.services.tokens;

namespace ClinicaVeterinaria.API.Api.services
{
    public class UserService
    {
        private readonly UserRepository Repo;

        public UserService(UserRepository repo)
        {
            Repo = repo;
        }

        public UserService() { }

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

        public virtual async Task<Either<UserDTO, string>> Delete(string email)
        {
            var user = await Repo.Delete(email);
            if (user != null)
            {
                return new Either<UserDTO, string>(user.ToDTO());
            }
            else return new Either<UserDTO, string>
                    ($"User with email {email} not found.");
        }
    }
}
