using ClinicaVeterinaria.API.Api.db;
using ClinicaVeterinaria.API.Api.model;
using Microsoft.EntityFrameworkCore;

namespace ClinicaVeterinaria.API.Api.repositories
{
    public class AilmentTreatmentRepository
    {
        private readonly ClinicaDBContext Context;

        public AilmentTreatmentRepository(ClinicaDBContext context)
        {
            Context = context;
            context.Database.Migrate();
        }

        public AilmentTreatmentRepository() { }

        public virtual async Task<List<AilmentTreatment>> FindAll()
        {
            var ats = await Context.ATs.ToListAsync();
            return ats ?? new();
        }

        public virtual async Task<List<AilmentTreatment>> FindByPetId(Guid petId)
        {
            var ats = await Context.ATs.ToListAsync();
            return ats.FindAll(v => v.PetId == petId);
        }

        public virtual async Task<AilmentTreatment?> FindById(Guid id)
        {
            return await Context.ATs.FirstOrDefaultAsync(u => u.Id == id);
        }

        public virtual async Task<AilmentTreatment> Create(AilmentTreatment at)
        {
            Context.ATs.Add(at);
            await Context.SaveChangesAsync();

            return at;
        }

        public virtual async Task<AilmentTreatment?> Update(Guid id, AilmentTreatment at)
        {
            var found = Context.ATs.FirstOrDefault(u => u.Id == id);
            if (found != null)
            {
                at.Id = found.Id;
                Context.ATs.Update(at);
                await Context.SaveChangesAsync();

                return at;
            }
            return null;
        }

        public virtual async Task<AilmentTreatment?> Delete(Guid id)
        {
            var found = Context.ATs.FirstOrDefault(u => u.Id == id);
            if (found != null)
            {
                Context.ATs.Remove(found);
                await Context.SaveChangesAsync();

                return found;
            }
            return null;
        }
    }
}
