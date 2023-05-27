namespace ClinicaVeterinaria.API.Api.dto
{
    public class VaccineDTO
    {
        public VaccineDTO(string name, string date)
        {
            Name = name;
            Date = date;
        }

        public string Name { get; set; }
        public string Date { get; set; }
    }
}
