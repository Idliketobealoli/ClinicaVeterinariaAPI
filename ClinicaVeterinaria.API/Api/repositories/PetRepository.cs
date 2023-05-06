using ClinicaVeterinaria.API.Api.db;
using ClinicaVeterinaria.API.Api.dto;
using ClinicaVeterinaria.API.Api.model;
using Microsoft.EntityFrameworkCore;

namespace ClinicaVeterinaria.API.Api.repositories
{
    public class PetRepository
    {
        private readonly IDbContextFactory<ClinicaDBContext> ContextFactory;

        public PetRepository(IDbContextFactory<ClinicaDBContext> contextFactory)
        {
            ContextFactory = contextFactory;
        }

        public PetRepository() { }

        public virtual async Task<List<Pet>> FindAll()
        {
            using ClinicaDBContext context = ContextFactory.CreateDbContext();
            var pets = await context.Pets.ToListAsync();
            return pets ?? new();
        }

        public virtual async Task<Pet?> FindById(Guid id)
        {
            using ClinicaDBContext context = ContextFactory.CreateDbContext();
            return await context.Pets.FirstOrDefaultAsync(u => u.Id == id);
        }

        public virtual async Task<Pet> Create(Pet pet)
        {
            using ClinicaDBContext context = ContextFactory.CreateDbContext();
            context.Pets.Add(pet);
            await context.SaveChangesAsync();

            return pet;
        }

        public virtual async Task<Pet?> Update(PetDTOupdate pet)
        {
            using ClinicaDBContext context = ContextFactory.CreateDbContext();
            var found = context.Pets.FirstOrDefault(u => u.Id == pet.Id);
            if (found != null)
            {
                found.Name = pet.Name ?? found.Name;
                found.Weight = pet.Weight ?? found.Weight;
                found.Size = pet.Size ?? found.Size;
                context.Pets.Update(found);
                await context.SaveChangesAsync();

                return found;
            }
            return null;
        }

        public virtual async Task<Pet?> Delete(Guid id)
        {
            using ClinicaDBContext context = ContextFactory.CreateDbContext();
            var found = context.Pets.FirstOrDefault(u => u.Id == id);
            if (found != null)
            {
                context.Pets.Remove(found);
                await context.SaveChangesAsync();

                return found;
            }
            return null;
        }
    }
}
