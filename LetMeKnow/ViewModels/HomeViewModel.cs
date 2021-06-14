using LetMeKnow.Entities;
using LetMeKnow.Models;
using LetMeKnow.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace LetMeKnow.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        private readonly AppDbContext _dbContext;
        private readonly VaccineService _vaccineService;

        #region Bindings

        public ObservableCollection<VaccSessAvailDto> Appointments { get; set; }
        private bool hasNoAppointments;
        public bool HasNoAppointments { get => hasNoAppointments; set => SetProperty(ref hasNoAppointments, value); }

        private bool hasConfigured = false;
        public bool HasConfigured { get => hasConfigured; set => SetProperty(ref hasConfigured, value); }

        public ICommand RefreshCommand { get; set; }
        private bool hasRefreshed;
        public bool HasRefreshed { get => hasRefreshed; set => SetProperty(ref hasRefreshed, value); }

        #endregion
        public HomeViewModel()
        {
            _dbContext = Registry.Container.Resolve<AppDbContext>();
            _vaccineService = Registry.Container.Resolve<VaccineService>();
            Appointments = new ObservableCollection<VaccSessAvailDto>();
            RefreshCommand = new Command(async () => await RefreshSessions());
        }

        public async Task OnInit()
        {
            MCSubscribe();
            await RefreshSessions();
        }

        public void OnEnd()
        {
            MCUnsubscribe();
        }

        private async Task RefreshSessions()
        {
            var setting = _dbContext.Settings
                            .Include(x => x.District)
                            .Query().FirstOrDefault();
            if (setting == null || setting.District == null)
            {
                HasConfigured = false;
                return;
            }

            HasConfigured = true;
            var sessions = await _vaccineService.PopulateAppointmentsAsPerSettings(setting);
            HasRefreshed = false;
            RebindAppointmentList(sessions);
        }

        private void RebindAppointmentList(List<VaccSessAvailDto> sessions)
        {
            if (sessions.Any())
            {
                HasNoAppointments = false;
            }
            else
            {
                HasNoAppointments = true;
            }
            Appointments.Clear();
            foreach (var session in sessions)
            {
                Appointments.Add(session);
            }
        }

        private void MCSubscribe()
        {
            MessagingCenter.Subscribe<SurfAppointmentService, List<VaccSessAvailDto>>(this, MCKey.OnSessionAvailabiltyCheck, (sender, arg) =>
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    RebindAppointmentList(arg);
                });
            });
        }

        private void MCUnsubscribe()
        {
            MessagingCenter.Unsubscribe<SurfAppointmentService, List<VaccSessAvailDto>>(this, MCKey.OnSessionAvailabiltyCheck);
        }
    }
}
