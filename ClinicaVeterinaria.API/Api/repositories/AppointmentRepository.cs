using ClinicaVeterinaria.API.Api.db;
using ClinicaVeterinaria.API.Api.model;
using Microsoft.EntityFrameworkCore;

namespace ClinicaVeterinaria.API.Api.repositories
{
    public class AppointmentRepository
    {
        private readonly ClinicaDBContext Context;

        public AppointmentRepository(ClinicaDBContext context)
        {
            Context = context;
            context.Database.Migrate();
        }

        public AppointmentRepository() { }

        public virtual async Task<List<Appointment>> FindAll()
        {
            var appointment = await Context.Appointments.ToListAsync();
            return appointment ?? new();
        }

        public virtual async Task<Appointment?> FindById(Guid id)
        {
            return await Context.Appointments.FirstOrDefaultAsync(u => u.Id == id);
        }

        public virtual async Task<Appointment> Create(Appointment appointment)
        {
            Context.Appointments.Add(appointment);
            await Context.SaveChangesAsync();

            return appointment;
        }

        public virtual async Task<Appointment?> UpdateState(Guid id, State? state)
        {
            if (state == null) { return null; }
            var appointment = await Context.Appointments.FirstOrDefaultAsync(u => u.Id == id);
            if (appointment != null)
            {
                appointment.State = state.Value;
                Context.Appointments.Update(appointment);
                await Context.SaveChangesAsync();
            }
            return appointment;
        }

        public virtual async Task<Appointment?> Delete(Guid id)
        {
            var found = Context.Appointments.FirstOrDefault(u => u.Id == id);
            if (found != null)
            {
                Context.Appointments.Remove(found);
                await Context.SaveChangesAsync();

                return found;
            }
            return null;
        }
    }
}
