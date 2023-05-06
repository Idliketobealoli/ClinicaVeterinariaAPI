namespace ClinicaVeterinaria.API.Api.errors
{
    public abstract class DomainError
    {
        public string Message { get; set; }

        public DomainError(string message)
        {
            Message = message;
        }
    }
}
