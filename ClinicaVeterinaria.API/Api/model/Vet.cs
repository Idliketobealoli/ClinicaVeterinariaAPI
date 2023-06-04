using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicaVeterinaria.API.Api.model
{
    [Table("Vets")]
    public class Vet: IUser
    {
        public Vet
            (
            string name,
            string surname,
            string email,
            string sSNumber,
            string password,
            Role role,
            string specialty,
            bool active
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
            Active = active;
        }

        [Required, Key]
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
        [Required]
        public bool Active { get; set; }
    }
}
