using ClinicaVeterinaria.API.Api.db;
using ClinicaVeterinaria.API.Api.dto;
using ClinicaVeterinaria.API.Api.model;
using Microsoft.EntityFrameworkCore;

namespace ClinicaVeterinaria.API.Api.repositories
{
    public class PetRepository
    {
        private readonly ClinicaDBContext Context;

        public PetRepository(ClinicaDBContext context)
        {
            Context = context;
            context.Database.Migrate();
        }

        public PetRepository() { }

        public virtual async Task<List<Pet>> FindAll()
        {
            var pets = await Context.Pets.ToListAsync();
            return pets.FindAll(p => p.Active == true) ?? new();
        }

        public virtual async Task<Pet?> FindById(Guid id)
        {
            return await Context.Pets.FirstOrDefaultAsync(u => u.Id == id);
        }

        public virtual async Task<Pet> Create(Pet pet)
        {
            Context.Pets.Add(pet);
            await Context.SaveChangesAsync();

            return pet;
        }

        public virtual async Task<Pet?> Update(PetDTOupdate pet)
        {
            var found = Context.Pets.FirstOrDefault(u => u.Id == pet.Id);
            if (found != null)
            {
                found.Name = pet.Name ?? found.Name;
                found.Weight = pet.Weight ?? found.Weight;
                found.Size = pet.Size ?? found.Size;
                Context.Pets.Update(found);
                await Context.SaveChangesAsync();

                return found;
            }
            return null;
        }

        public virtual async Task<Pet?> Delete(Guid id, bool active)
        {
            var found = Context.Pets.FirstOrDefault(u => u.Id == id);
            if (found != null)
            {
                found.Active = active;
                Context.Pets.Update(found);
                await Context.SaveChangesAsync();

                return found;
            }
            return null;
        }
    }
}
