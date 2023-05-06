namespace ClinicaVeterinaria.API.Api.errors
{
    public class HistoryError : DomainError
    {

        public HistoryError(string message)
            : base(message) { }
    }

    public class HistoryErrorNotFound : HistoryError
    {
        public HistoryErrorNotFound(string message)
            : base(message) { }
    }
}
