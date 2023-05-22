namespace ClinicaVeterinaria.API.Api.model
{
    public interface IUser
    {
        Guid Id { get; set; }
        string Name { get; set; }
        string Surname { get; set; }
        string Email { get; set; }
        Role Role { get; set; }
    }
}
