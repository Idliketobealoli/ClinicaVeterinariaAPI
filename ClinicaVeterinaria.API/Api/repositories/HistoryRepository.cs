using ClinicaVeterinaria.API.Api.db;
using ClinicaVeterinaria.API.Api.model;
using Microsoft.EntityFrameworkCore;

namespace ClinicaVeterinaria.API.Api.repositories
{
    public class HistoryRepository
    {
        private readonly IDbContextFactory<ClinicaDBContext> ContextFactory;

        public HistoryRepository(IDbContextFactory<ClinicaDBContext> contextFactory)
        {
            ContextFactory = contextFactory;
        }

        public HistoryRepository() { }

        public virtual async Task<List<History>> FindAll()
        {
            using ClinicaDBContext context = ContextFactory.CreateDbContext();
            var histories = await context.Histories.ToListAsync();
            return histories ?? new();
        }

        public virtual async Task<History?> FindById(Guid id)
        {
            using ClinicaDBContext context = ContextFactory.CreateDbContext();
            return await context.Histories.FirstOrDefaultAsync(u => u.Id == id);
        }

        public virtual async Task<History?> FindByPetId(Guid id)
        {
            using ClinicaDBContext context = ContextFactory.CreateDbContext();
            return await context.Histories.FirstOrDefaultAsync(u => u.PetId == id);
        }

        public virtual async Task<History> Create(History history)
        {
            using ClinicaDBContext context = ContextFactory.CreateDbContext();
            context.Histories.Add(history);
            await context.SaveChangesAsync();

            return history;
        }

        public virtual async Task<History?> Update(Guid id, History history)
        {
            using ClinicaDBContext context = ContextFactory.CreateDbContext();
            var found = context.Histories.FirstOrDefault(u => u.Id == id);
            if (found != null)
            {
                history.Id = found.Id;
                context.Histories.Update(history);
                await context.SaveChangesAsync();

                return history;
            }
            return null;
        }

        public virtual async Task<History?> Delete(Guid id)
        {
            using ClinicaDBContext context = ContextFactory.CreateDbContext();
            var found = context.Histories.FirstOrDefault(u => u.Id == id);
            if (found != null)
            {
                context.Histories.Remove(found);
                await context.SaveChangesAsync();

                return found;
            }
            return null;
        }
    }
}
