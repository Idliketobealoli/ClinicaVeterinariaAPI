namespace ClinicaVeterinaria.API.Api.errors
{
    public class UserError : DomainError
    {
        public UserError(string message)
            : base(message) { }
    }

    public class UserErrorNotFound : UserError
    {
        public UserErrorNotFound(string message)
            : base(message) { }
    }

    public class UserErrorUnauthorized : UserError
    {
        public UserErrorUnauthorized(string message)
            : base(message) { }
    }
}
