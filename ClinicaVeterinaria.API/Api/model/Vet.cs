using System.ComponentModel.DataAnnotations;

namespace ClinicaVeterinaria.API.Api.model
{
    public class Vet
    {
        public Vet
            (
            string name,
            string surname,
            string email,
            string sSNumber,
            string password,
            Role role,
            string specialty
            )
        {
            Id = Guid.NewGuid();
            Name = name;
            Surname = surname;
            Email = email;
            SSNumber = sSNumber;
            Password = password;
            Role = role;
            Specialty = specialty;
        }

        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string SSNumber { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public Role Role { get; set; }
        [Required]
        public string Specialty { get; set; }
    }

    public enum Role
    {
        VET,
        ADMIN
    }
}
