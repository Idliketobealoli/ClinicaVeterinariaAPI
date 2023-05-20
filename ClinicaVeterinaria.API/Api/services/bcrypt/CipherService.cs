namespace ClinicaVeterinaria.API.Api.services.bcrypt
{
    public static class CipherService
    {
        public static string Encode(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, 12);
        }

        public static bool Decode(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }
}
