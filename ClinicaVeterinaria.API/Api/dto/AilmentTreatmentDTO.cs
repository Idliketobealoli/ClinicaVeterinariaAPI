namespace ClinicaVeterinaria.API.Api.dto
{
    public class AilmentTreatmentDTO
    {
        public string Ailment { get; set; }
        public string Treatment { get; set; }

        public AilmentTreatmentDTO(string ailment, string treatment)
        {
            Ailment = ailment;
            Treatment = treatment;
        }
    }
}
