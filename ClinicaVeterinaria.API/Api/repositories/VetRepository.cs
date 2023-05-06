using ClinicaVeterinaria.API.Api.db;
using ClinicaVeterinaria.API.Api.model;
using Microsoft.EntityFrameworkCore;

namespace ClinicaVeterinaria.API.Api.repositories
{
    public class VetRepository
    {
        private readonly IDbContextFactory<ClinicaDBContext> ContextFactory;

        public VetRepository() { }

        public VetRepository(IDbContextFactory<ClinicaDBContext> contextFactory)
        {
            ContextFactory = contextFactory;
        }

        public virtual async Task<List<Vet>> FindAll()
        {
            using ClinicaDBContext context = ContextFactory.CreateDbContext();
            var vets = await context.Vets.ToListAsync();
            return vets ?? new();
        }

        public virtual async Task<Vet?> FindById(Guid id)
        {
            using ClinicaDBContext context = ContextFactory.CreateDbContext();
            return await context.Vets.FirstOrDefaultAsync(u => u.Id == id);
        }

        public virtual async Task<Vet?> FindByEmail(string email)
        {
            using ClinicaDBContext context = ContextFactory.CreateDbContext();
            return await context.Vets.FirstOrDefaultAsync(u => u.Email == email);
        }

        public virtual async Task<Vet?> FindBySSNum(string ssnum)
        {
            using ClinicaDBContext context = ContextFactory.CreateDbContext();
            return await context.Vets.FirstOrDefaultAsync(u => u.SSNumber == ssnum);
        }

        public virtual async Task<Vet> Create(Vet vet)
        {
            using (ClinicaDBContext context = ContextFactory.CreateDbContext())
            {
                context.Vets.Add(vet);
                await context.SaveChangesAsync();
                return vet;
            }
        }

        public virtual async Task<Vet?> UpdatePassword(string email, string password)
        {
            using ClinicaDBContext context = ContextFactory.CreateDbContext();
            var foundVet = context.Vets.FirstOrDefault(u => u.Email == email);
            if (foundVet != null)
            {
                foundVet.Password = password;
                context.Vets.Update(foundVet);
                await context.SaveChangesAsync();
                return foundVet;
            }
            return null;
        }

        public virtual async Task<Vet?> Delete(string email)
        {
            using ClinicaDBContext context = ContextFactory.CreateDbContext();
            var foundVet = context.Vets.FirstOrDefault(u => u.Email == email);
            if (foundVet != null)
            {
                context.Vets.Remove(foundVet);
                await context.SaveChangesAsync();
                return foundVet;
            }
            return null;
        }
    }
}