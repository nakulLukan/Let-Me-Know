using LetMeKnow.Entities;
using Matcha.BackgroundService;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LetMeKnow.Services
{
    public class SurfAppointmentService : IPeriodicTask
    {
        private readonly AppDbContext _dbContext;
        private readonly VaccineService _vaccineService;
        public TimeSpan Interval { get; set; }

        public SurfAppointmentService(int seconds)
        {
            Interval = TimeSpan.FromSeconds(seconds);
            _dbContext = Registry.Container.Resolve<AppDbContext>();
            _vaccineService = Registry.Container.Resolve<VaccineService>();
        }

        public async Task<bool> StartJob()
        {
            try
            {
                var setting = _dbContext.Settings.Query().SingleOrDefault();
                if (!setting.SurfAppointments)
                {
                    return true;
                }
                var alertSerive = new AlertService();
                var sessions = await _vaccineService.PopulateAppointmentsAsPerSettings(setting);
                if (sessions.Any(x => x.AvailableCapacityDose1 > 0 || x.AvailableCapacityDose2 > 0))
                {
                    alertSerive.Alert();
                }

                MessagingCenter.Send(this, MCKey.OnSessionAvailabiltyCheck, sessions);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            return true;
        }
    }
}
