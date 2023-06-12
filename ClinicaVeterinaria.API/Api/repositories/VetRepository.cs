using ClinicaVeterinaria.API.Api.db;
using ClinicaVeterinaria.API.Api.model;
using Microsoft.EntityFrameworkCore;

namespace ClinicaVeterinaria.API.Api.repositories
{
    public class VetRepository
    {
        private readonly ClinicaDBContext Context;

        public VetRepository(ClinicaDBContext context)
        {
            Context = context;
            context.Database.Migrate();
        }

        public VetRepository() { }

        public virtual async Task<List<Vet>> FindAll()
        {
            var vets = await Context.Vets.ToListAsync();
            return vets.FindAll(v => v.Active == true) ?? new();
        }

        public virtual async Task<Vet?> FindById(Guid id)
        {
            return await Context.Vets.FirstOrDefaultAsync(u => u.Id == id);
        }

        public virtual async Task<Vet?> FindByEmail(string email)
        {
            return await Context.Vets.FirstOrDefaultAsync(u => u.Email == email);
        }

        public virtual async Task<Vet?> FindBySSNum(string ssnum)
        {
            return await Context.Vets.FirstOrDefaultAsync(u => u.SSNumber == ssnum);
        }

        public virtual async Task<Vet> Create(Vet vet)
        {
             Context.Vets.Add(vet);
             await Context.SaveChangesAsync();
             return vet;
        }

        public virtual async Task<Vet?> UpdatePassword(string email, string password)
        {
            var foundVet = Context.Vets.FirstOrDefault(u => u.Email == email);
            if (foundVet != null)
            {
                foundVet.Password = password;
                Context.Vets.Update(foundVet);
                await Context.SaveChangesAsync();
                return foundVet;
            }
            return null;
        }

        public virtual async Task<Vet?> SwitchActivity(string email, bool active)
        {
            var foundVet = Context.Vets.FirstOrDefault(u => u.Email == email);
            if (foundVet != null)
            {
                foundVet.Active = active;
                Context.Vets.Update(foundVet);
                await Context.SaveChangesAsync();
                return foundVet;
            }
            return null;
        }
    }
}
