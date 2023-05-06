namespace ClinicaVeterinaria.API.Api.dto
{
    public class HistoryDTO
    {
        public HistoryDTO
            (
            Guid petId,
            HashSet<VaccineDTO> vaccines,
            Dictionary<string, string> ailmentTreatment
            )
        {
            PetId = petId;
            Vaccines = vaccines;
            AilmentTreatment = ailmentTreatment;
        }

        public Guid PetId { get; set; }
        public HashSet<VaccineDTO> Vaccines { get; set; }
        public Dictionary<string, string> AilmentTreatment { get; set; }
    }

    public class HistoryDTOvaccines
    {
        public HistoryDTOvaccines(HashSet<VaccineDTO> vaccines)
        {
            Vaccines = vaccines;
        }

        public HashSet<VaccineDTO> Vaccines { get; set; }
    }

    public class HistoryDTOailmentTreatment
    {
        public HistoryDTOailmentTreatment(Dictionary<string, string> ailmentTreatment)
        {
            AilmentTreatment = ailmentTreatment;
        }

        public Dictionary<string, string> AilmentTreatment { get; set; }
    }
}