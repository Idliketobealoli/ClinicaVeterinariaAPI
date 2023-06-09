using ClinicaVeterinaria.API.Api.model;
using ClinicaVeterinaria.API.Api.repositories;
using ClinicaVeterinaria.API.Api.services.bcrypt;

namespace ClinicaVeterinaria.API.Api.db
{
    public static class InitialData
    {
        public static void SetData(VetRepository vets)
        {
            var created = vets.Create(new("admin", "admin", "admin1707@gmail.com", "620797974", CipherService.Encode("admin1234"), Role.ADMIN, "Ser administrador", true));
            created.Wait();
        }
    }
}
