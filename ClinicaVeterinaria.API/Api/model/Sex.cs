namespace ClinicaVeterinaria.API.Api.model
{
    public enum Sex
    {
        MALE,
        FEMALE
    }

    public static class Sexes
    {
        public static string ToString(Sex sex)
        {
            return sex switch
            {
                Sex.MALE => "MALE",
                Sex.FEMALE => "FEMALE",
                _ => "MALE"
            };
        }

        public static Sex FromString(string sex)
        {
            return sex.ToUpper() switch
            {
                "MALE" => Sex.MALE,
                "FEMALE" => Sex.FEMALE,
                _ => Sex.MALE
            };
        }
    }
}
