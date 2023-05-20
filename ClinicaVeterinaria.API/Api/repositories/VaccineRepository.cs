using ClinicaVeterinaria.API.Api.db;
using ClinicaVeterinaria.API.Api.model;
using Microsoft.EntityFrameworkCore;

namespace ClinicaVeterinaria.API.Api.repositories
{
    public class VaccineRepository
    {
        private readonly ClinicaDBContext Context;

        public VaccineRepository(ClinicaDBContext context)
        {
            Context = context;
            context.Database.Migrate();
        }

        public VaccineRepository() { }

        public virtual async Task<List<Vaccine>> FindAll()
        {
            var vaccines = await Context.Vaccines.ToListAsync();
            return vaccines ?? new();
        }

        public virtual async Task<Vaccine?> FindById(Guid id)
        {
            return await Context.Vaccines.FirstOrDefaultAsync(u => u.Id == id);
        }

        public virtual async Task<Vaccine> Create(Vaccine vaccines)
        {
            Context.Vaccines.Add(vaccines);
            await Context.SaveChangesAsync();

            return vaccines;
        }

        public virtual async Task<Vaccine?> Update(Guid id, Vaccine vaccine)
        {
            var found = Context.Vaccines.FirstOrDefault(u => u.Id == id);
            if (found != null)
            {
                vaccine.Id = found.Id;
                Context.Vaccines.Update(vaccine);
                await Context.SaveChangesAsync();

                return vaccine;
            }
            return null;
        }

        public virtual async Task<Vaccine?> Delete(Guid id)
        {
            var found = Context.Vaccines.FirstOrDefault(u => u.Id == id);
            if (found != null)
            {
                Context.Vaccines.Remove(found);
                await Context.SaveChangesAsync();

                return found;
            }
            return null;
        }
    }
}
