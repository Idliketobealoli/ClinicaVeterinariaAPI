namespace ClinicaVeterinaria.API.Api.dto
{
    public class UserDTO
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public UserDTO
            (
            string name,
            string surname,
            string email,
            string phone
            )
        {
            Name = name;
            Surname = surname;
            Email = email;
            Phone = phone;
        }
    }

    public class UserDTOloginOrChangePassword
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public UserDTOloginOrChangePassword
            (
            string email,
            string password
            )
        {
            Email = email;
            Password = password;
        }
    }

    public class UserDTOregister
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public string RepeatPassword { get; set; }

        public UserDTOregister
            (
            string name,
            string surname,
            string email,
            string phone,
            string password,
            string repeatPassword
            )
        {
            Name = name;
            Surname = surname;
            Email = email;
            Phone = phone;
            Password = password;
            RepeatPassword = repeatPassword;
        }
    }

    public class UserDTOshort
    {
        public string Name { get; set; }
        public string Surname { get; set; }

        public UserDTOshort
            (
            string name,
            string surname
            )
        {
            Name = name;
            Surname = surname;
        }
    }

    public class UserDTOandToken
    {
        public UserDTO DTO { get; set; }
        public string Token { get; set; }

        public UserDTOandToken(UserDTO dTO, string token)
        {
            DTO = dTO;
            Token = token;
        }
    }
 }