using ClinicaVeterinaria.API.Api.db;
using ClinicaVeterinaria.API.Api.model;
using Microsoft.EntityFrameworkCore;

namespace ClinicaVeterinaria.API.Api.repositories
{
    public class AppointmentRepository
    {
        private readonly IDbContextFactory<ClinicaDBContext> ContextFactory;

        public AppointmentRepository(IDbContextFactory<ClinicaDBContext> contextFactory)
        {
            ContextFactory = contextFactory;
        }

        public AppointmentRepository() { }

        public virtual async Task<List<Appointment>> FindAll()
        {
            using ClinicaDBContext context = ContextFactory.CreateDbContext();
            var appointment = await context.Appointments.ToListAsync();
            return appointment ?? new();
        }

        public virtual async Task<Appointment?> FindById(Guid id)
        {
            using ClinicaDBContext context = ContextFactory.CreateDbContext();
            return await context.Appointments.FirstOrDefaultAsync(u => u.Id == id);
        }

        public virtual async Task<Appointment> Create(Appointment appointment)
        {
            using ClinicaDBContext context = ContextFactory.CreateDbContext();
            context.Appointments.Add(appointment);
            await context.SaveChangesAsync();

            return appointment;
        }

        public virtual async Task<Appointment?> Delete(Guid id)
        {
            using ClinicaDBContext context = ContextFactory.CreateDbContext();
            var found = context.Appointments.FirstOrDefault(u => u.Id == id);
            if (found != null)
            {
                context.Appointments.Remove(found);
                await context.SaveChangesAsync();

                return found;
            }
            return null;
        }
    }
}
