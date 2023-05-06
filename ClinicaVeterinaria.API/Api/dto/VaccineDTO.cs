namespace ClinicaVeterinaria.API.Api.dto
{
    public class VaccineDTO
    {
        public VaccineDTO(string name, DateOnly date)
        {
            Name = name;
            Date = date;
        }

        public string Name { get; set; }
        public DateOnly Date { get; set; }
    }
}
