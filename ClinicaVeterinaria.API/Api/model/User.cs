using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicaVeterinaria.API.Api.model
{
    [Table("Users")]
    public class User
    {
        public User
            (
            string name,
            string surname,
            string email,
            string phone,
            string password
            )
        {
            Id = Guid.NewGuid();
            Name = name;
            Surname = surname;
            Email = email;
            Phone = phone;
            Password = password;
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
        public string Phone { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
