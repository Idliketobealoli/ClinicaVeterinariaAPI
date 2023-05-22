namespace ClinicaVeterinaria.API.Api.model
{
    public enum Role
    {
        USER,
        VET,
        ADMIN
    }

    public static class Roles
    {
        public static Role FromString(string role)
        {
            return role.ToUpper() switch
            {
                "ADMIN" => Role.ADMIN,
                "VET" => Role.VET,
                _ => Role.USER
            };
        }

        public static string ToString(Role role)
        {
            return role switch
            {
                Role.ADMIN => "ADMIN",
                Role.VET => "VET",
                _ => "USER"
            };
        }
    }
}
