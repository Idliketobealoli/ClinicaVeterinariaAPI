using ClinicaVeterinaria.API.Api.dto;
using ClinicaVeterinaria.API.Api.mappers;
using ClinicaVeterinaria.API.Api.model;
using ClinicaVeterinaria.API.Api.repositories;

namespace ClinicaVeterinaria.API.Api.services
{
    public class HistoryService
    {
        private readonly HistoryRepository HisRepo;
        private readonly VaccineRepository VacRepo;
        private readonly AilmentTreatmentRepository AilRepo;

        public HistoryService(HistoryRepository hisRepo, VaccineRepository vacRepo, AilmentTreatmentRepository ailRepo)
        {
            HisRepo = hisRepo;
            VacRepo = vacRepo;
            AilRepo = ailRepo;
        }

        public HistoryService() { }

        public virtual async Task<List<HistoryDTO>> FindAll()
        {
            var entities = await HisRepo.FindAll();
            var entitiesDTOs = new List<HistoryDTO>();
            foreach (var entity in entities)
            {
                entitiesDTOs.Add(entity.ToDTO(VacRepo, AilRepo));
            }
            return entitiesDTOs;
        }

        public virtual async Task<Either<HistoryDTO, string>> FindByPetId(Guid id)
        {
            var entity = await HisRepo.FindByPetId(id);
            if (entity == null)
            {
                return new Either<HistoryDTO, string>
                    ($"History with PetId {id} not found.");
            }
            else return new Either<HistoryDTO, string>(entity.ToDTO(VacRepo, AilRepo));
        }

        public virtual async Task<Either<HistoryDTOvaccines, string>> FindByPetIdVaccinesOnly(Guid id)
        {
            var entity = await HisRepo.FindByPetId(id);
            if (entity == null)
            {
                return new Either<HistoryDTOvaccines, string>
                    ($"History with PetId {id} not found.");
            }
            else return new Either<HistoryDTOvaccines, string>(entity.ToDTOvaccines(VacRepo));
        }

        public virtual async Task<Either<HistoryDTOailmentTreatment, string>> FindByPetIdAilmTreatOnly(Guid id)
        {
            var entity = await HisRepo.FindByPetId(id);
            if (entity == null)
            {
                return new Either<HistoryDTOailmentTreatment, string>
                    ($"History with PetId {id} not found.");
            }
            else return new Either<HistoryDTOailmentTreatment, string>(entity.ToDTOailmentTreatment(AilRepo));
        }

        public virtual async Task<Either<HistoryDTO, string>> AddVaccine(Guid id, VaccineDTO vaccine)
        {
            var history = await HisRepo.FindByPetId(id);
            if (history != null)
            {
                var newVaccine = vaccine.FromDTO(id);
                await VacRepo.Create(newVaccine);
                //await HisRepo.Update(history.Id, history);
                return new Either<HistoryDTO, string>(history.ToDTO(VacRepo, AilRepo));
            }
            else return new Either<HistoryDTO, string>
                    ($"History with PetId {id} not found.");
        }

        public virtual async Task<Either<HistoryDTO, string>> AddAilmentTreatment(Guid id, AilmentTreatmentDTO ailmentTreatment)
        {
            var history = await HisRepo.FindByPetId(id);
            if (history != null)
            {
                var newAT = ailmentTreatment.FromDTO(id);
                await AilRepo.Create(newAT);
                return new Either<HistoryDTO, string>(history.ToDTO(VacRepo, AilRepo));
            }
            else return new Either<HistoryDTO, string>
                    ($"History with PetId {id} not found.");
        }
    }
}
