using ClinicaVeterinaria.API.Api.db;
using ClinicaVeterinaria.API.Api.model;
using Microsoft.EntityFrameworkCore;

namespace ClinicaVeterinaria.API.Api.repositories
{
    public class HistoryRepository
    {
        private readonly ClinicaDBContext Context;

        public HistoryRepository(ClinicaDBContext context)
        {
            Context = context;
            context.Database.Migrate();
        }

        public HistoryRepository() { }

        public virtual async Task<List<History>> FindAll()
        {
            var histories = await Context.Histories.ToListAsync();
            return histories ?? new();
        }

        public virtual async Task<History?> FindById(Guid id)
        {
            return await Context.Histories.FirstOrDefaultAsync(u => u.Id == id);
        }

        public virtual async Task<History?> FindByPetId(Guid id)
        {
            return await Context.Histories.FirstOrDefaultAsync(u => u.PetId == id);
        }

        public virtual async Task<History> Create(History history)
        {
            Context.Histories.Add(history);
            await Context.SaveChangesAsync();

            return history;
        }

        public virtual async Task<History?> Update(Guid id, History history)
        {
            var found = Context.Histories.FirstOrDefault(u => u.Id == id);
            if (found != null)
            {
                history.Id = found.Id;
                Context.Histories.Update(history);
                await Context.SaveChangesAsync();

                return history;
            }
            return null;
        }

        public virtual async Task<History?> Delete(Guid id)
        {
            var found = Context.Histories.FirstOrDefault(u => u.Id == id);
            if (found != null)
            {
                Context.Histories.Remove(found);
                await Context.SaveChangesAsync();

                return found;
            }
            return null;
        }
    }
}
