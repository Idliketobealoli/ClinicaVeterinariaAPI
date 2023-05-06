namespace ClinicaVeterinaria.API.Api.errors
{
    public class VetError : DomainError
    {
        public VetError(string message)
            : base(message) { }
    }

    public class VetErrorNotFound : VetError
    {
        public VetErrorNotFound(string message)
            : base(message) { }
    }

    public class VetErrorBadRequest : VetError
    {
        public VetErrorBadRequest(string message)
            : base(message) { }
    }

    public class VetErrorUnauthorized : VetError
    {
        public VetErrorUnauthorized(string message)
            : base(message) { }
    }
}
