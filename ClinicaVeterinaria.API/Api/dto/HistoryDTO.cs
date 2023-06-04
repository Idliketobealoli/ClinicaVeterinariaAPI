namespace ClinicaVeterinaria.API.Api.dto
{
    public class HistoryDTO
    {
        public HistoryDTO
            (
            Guid petId,
            HashSet<VaccineDTO> vaccines,
            HashSet<AilmentTreatmentDTO> ailmentTreatment
            )
        {
            PetId = petId;
            Vaccines = vaccines;
            AilmentTreatment = ailmentTreatment;
        }

        public Guid PetId { get; set; }
        public HashSet<VaccineDTO> Vaccines { get; set; }
        public HashSet<AilmentTreatmentDTO> AilmentTreatment { get; set; }
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
        public HistoryDTOailmentTreatment(HashSet<AilmentTreatmentDTO> ailmentTreatment)
        {
            AilmentTreatment = ailmentTreatment;
        }

        public HashSet<AilmentTreatmentDTO> AilmentTreatment { get; set; }
    }
}