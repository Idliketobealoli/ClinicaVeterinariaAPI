namespace ClinicaVeterinaria.API.Api.model
{
    public enum State
    {
        PENDING,
        PROGRESS,
        FINISHED
    }

    public static class States
    {
        public static string ToString(State state)
        {
            return state switch
            {
                State.PENDING => "PENDING",
                State.PROGRESS => "PROGRESS",
                State.FINISHED => "FINISHED",
                _ => "PENDING"
            };
        }

        public static State FromString(string state)
        {
            return state.ToUpper() switch
            {
                "PENDING" => State.PENDING,
                "PROGRESS" => State.PROGRESS,
                "FINISHED" => State.FINISHED,
                _ => State.PENDING
            };
        }
    }
}
