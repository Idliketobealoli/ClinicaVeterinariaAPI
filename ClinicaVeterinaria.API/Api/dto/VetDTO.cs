using ClinicaVeterinaria.API.Api.model;

namespace ClinicaVeterinaria.API.Api.dto
{
    public class VetDTO
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string SSNumber { get; set; }
        public string Role { get; set; }
        public string Specialty { get; set; }
        public bool IsActive { get; set; }

        public VetDTO
            (
            string name,
            string surname,
            string email,
            string sSNumber,
            Role role,
            string specialty,
            bool isActive
            )
        {
            Name = name;
            Surname = surname;
            Email = email;
            SSNumber = sSNumber;
            Role = Roles.ToString(role);
            Specialty = specialty;
            IsActive = isActive;
        }
    }

    public class VetDTOappointment
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set ; }

        public VetDTOappointment
            (
            string name,
            string surname,
            string email,
            bool isActive
            )
        {
            Name = name;
            Surname = surname;
            Email = email;
            IsActive = isActive;
        }
    }

    public class VetDTOloginOrChangePassword
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public VetDTOloginOrChangePassword
            (
            string email,
            string password
            )
        {
            Email = email;
            Password = password;
        }
    }

    public class VetDTOregister
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string SSNumber { get; set; }
        public string Password { get; set; }
        public string RepeatPassword { get; set; }
        public string Role { get; set; }
        public string Specialty { get; set; }

        public VetDTOregister
            (
            string name,
            string surname,
            string email,
            string sSNumber,
            string password,
            string repeatPassword,
            string role,
            string specialty
            )
        {
            Name = name;
            Surname = surname;
            Email = email;
            SSNumber = sSNumber;
            Password = password;
            RepeatPassword = repeatPassword;
            Role = role;
            Specialty = specialty;
        }
    }

    public class VetDTOshort
    {
        public string Name { get; set; }
        public string Surname { get; set; }

        public VetDTOshort
            (
            string name,
            string surname
            )
        {
            Name = name;
            Surname = surname;
        }
    }

    public class VetDTOstats
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public int AppointmentAmount { get; set; }

        public VetDTOstats
            (
            string name,
            string surname,
            string email,
            int appointmentAmount
            )
        {
            Name = name;
            Surname = surname;
            Email = email;
            AppointmentAmount = appointmentAmount;
        }
    }

    public class VetDTOandToken
    {
        public VetDTO DTO { get; set; }
        public string Token { get; set; }

        public VetDTOandToken(VetDTO dTO, string token)
        {
            DTO = dTO;
            Token = token;
        }
    }
}
