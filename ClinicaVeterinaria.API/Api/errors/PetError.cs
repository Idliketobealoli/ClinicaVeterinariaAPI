namespace ClinicaVeterinaria.API.Api.errors
{
    public class PetError : DomainError
    {
        public PetError(string message)
            : base(message) { }
    }

    public class PetErrorNotFound : PetError
    {
        public PetErrorNotFound(string message)
            : base(message) { }
    }

    public class PetErrorBadRequest : PetError
    {
        public PetErrorBadRequest(string message)
            : base(message) { }
    }
}
