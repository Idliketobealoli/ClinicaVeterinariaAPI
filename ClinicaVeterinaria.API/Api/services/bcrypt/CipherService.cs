namespace ClinicaVeterinaria.API.Api.services.bcrypt
{
    public static class CipherService
    {
        // This static function will encode a password with a 12-Round BCrypt
        public static string Encode(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, 12);
        }

        // This static function will check if the given string matches the given ciphered string.
        // If it does, it will return true; else, it will return false.
        public static bool Decode(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }
}
