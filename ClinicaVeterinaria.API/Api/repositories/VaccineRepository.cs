using ClinicaVeterinaria.API.Api.db;
using ClinicaVeterinaria.API.Api.model;
using Microsoft.EntityFrameworkCore;

namespace ClinicaVeterinaria.API.Api.repositories
{
    public class VaccineRepository
    {
        private readonly IDbContextFactory<ClinicaDBContext> ContextFactory;

        public VaccineRepository(IDbContextFactory<ClinicaDBContext> contextFactory)
        {
            ContextFactory = contextFactory;
        }

        public VaccineRepository() { }

        public virtual async Task<List<Vaccine>> FindAll()
        {
            using ClinicaDBContext context = ContextFactory.CreateDbContext();
            var vaccines = await context.Vaccines.ToListAsync();
            return vaccines ?? new();
        }

        public virtual async Task<Vaccine?> FindById(Guid id)
        {
            using ClinicaDBContext context = ContextFactory.CreateDbContext();
            return await context.Vaccines.FirstOrDefaultAsync(u => u.Id == id);
        }

        public virtual async Task<Vaccine> Create(Vaccine vaccines)
        {
            using ClinicaDBContext context = ContextFactory.CreateDbContext();
            context.Vaccines.Add(vaccines);
            await context.SaveChangesAsync();

            return vaccines;
        }

        public virtual async Task<Vaccine?> Update(Guid id, Vaccine vaccine)
        {
            using ClinicaDBContext context = ContextFactory.CreateDbContext();
            var found = context.Vaccines.FirstOrDefault(u => u.Id == id);
            if (found != null)
            {
                vaccine.Id = found.Id;
                context.Vaccines.Update(vaccine);
                await context.SaveChangesAsync();

                return vaccine;
            }
            return null;
        }

        public virtual async Task<Vaccine?> Delete(Guid id)
        {
            using ClinicaDBContext context = ContextFactory.CreateDbContext();
            var found = context.Vaccines.FirstOrDefault(u => u.Id == id);
            if (found != null)
            {
                context.Vaccines.Remove(found);
                await context.SaveChangesAsync();

                return found;
            }
            return null;
        }
    }
}
