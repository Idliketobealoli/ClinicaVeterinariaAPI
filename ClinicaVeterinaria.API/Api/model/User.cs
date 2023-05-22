using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicaVeterinaria.API.Api.model
{
    [Table("Users")]
    public class User: IUser
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
            Role = Role.USER;
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

        [Required]
        public Role Role { get; set; }
    }
}
