namespace ClinicaVeterinaria.API.Api.errors
{
    public class AppointmentError : DomainError
    {
        public AppointmentError(string message)
            : base(message) { }
    }

    public class AppointmentErrorNotFound : AppointmentError
    {
        public AppointmentErrorNotFound(string message)
            : base(message) { }
    }

    public class AppointmentErrorBadRequest : AppointmentError
    {
        public AppointmentErrorBadRequest(string message)
            : base(message) { }
    }
}
