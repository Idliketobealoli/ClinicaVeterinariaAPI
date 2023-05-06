using ClinicaVeterinaria.API.Api.dto;
using ClinicaVeterinaria.API.Api.mappers;
using ClinicaVeterinaria.API.Api.repositories;

namespace ClinicaVeterinaria.API.Api.services
{
    public class HistoryService
    {
        private readonly HistoryRepository HisRepo;
        private readonly VaccineRepository VacRepo;

        public HistoryService(HistoryRepository hisRepo, VaccineRepository vacRepo)
        {
            HisRepo = hisRepo;
            VacRepo = vacRepo;
        }

        public HistoryService() { }

        public virtual async Task<List<HistoryDTO>> FindAll()
        {
            var entities = await HisRepo.FindAll();
            var entitiesDTOs = new List<HistoryDTO>();
            foreach (var entity in entities)
            {
                entitiesDTOs.Add(entity.ToDTO());
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
            else return new Either<HistoryDTO, string>(entity.ToDTO());
        }

        public virtual async Task<Either<HistoryDTOvaccines, string>> FindByPetIdVaccinesOnly(Guid id)
        {
            var entity = await HisRepo.FindByPetId(id);
            if (entity == null)
            {
                return new Either<HistoryDTOvaccines, string>
                    ($"History with PetId {id} not found.");
            }
            else return new Either<HistoryDTOvaccines, string>(entity.ToDTOvaccines());
        }

        public virtual async Task<Either<HistoryDTOailmentTreatment, string>> FindByPetIdAilmTreatOnly(Guid id)
        {
            var entity = await HisRepo.FindByPetId(id);
            if (entity == null)
            {
                return new Either<HistoryDTOailmentTreatment, string>
                    ($"History with PetId {id} not found.");
            }
            else return new Either<HistoryDTOailmentTreatment, string>(entity.ToDTOailmentTreatment());
        }

        public virtual async Task<Either<HistoryDTO, string>> AddVaccine(Guid id, VaccineDTO vaccine)
        {
            var history = await HisRepo.FindByPetId(id);
            if (history != null)
            {
                var newVaccine = vaccine.FromDTO(id);
                history.Vaccines.Add(newVaccine);
                await VacRepo.Create(newVaccine);
                await HisRepo.Update(history.Id, history);
                return new Either<HistoryDTO, string>(history.ToDTO());
            }
            else return new Either<HistoryDTO, string>
                    ($"History with PetId {id} not found.");
        }

        public virtual async Task<Either<HistoryDTO, string>> AddAilmentTreatment(Guid id, string ailment, string treatment)
        {
            var history = await HisRepo.FindByPetId(id);
            if (history != null)
            {
                history.AilmentTreatment.TryAdd(ailment, treatment);
                await HisRepo.Update(history.Id, history);
                return new Either<HistoryDTO, string>(history.ToDTO());
            }
            else return new Either<HistoryDTO, string>
                    ($"History with PetId {id} not found.");
        }
    }
}
