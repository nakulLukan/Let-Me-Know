using LetMeKnow.Entities;
using LetMeKnow.Models.Metadata;
using LetMeKnow.Services;
using LetMeKnow.WebApi;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace LetMeKnow.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        private readonly AppDbContext _dbContext;

        #region Bindings

        public ObservableCollection<MetadataDto> States { get; }
        public ObservableCollection<MetadataDto> Districts { get; }
        public ObservableCollection<VaccineDto> Vaccines { get; set; }

        bool is18PlusChecked = false;
        public bool Is18PlusChecked { get => is18PlusChecked; set => SetProperty(ref is18PlusChecked, value); }
        public ICommand Is18PlusCommand { get; set; }

        bool is40PlusChecked = false;
        public bool Is40PlusChecked { get => is40PlusChecked; set => SetProperty(ref is40PlusChecked, value); }
        public ICommand Is40PlusCommand { get; set; }

        bool is45PlusChecked = false;
        public bool Is45PlusChecked { get => is45PlusChecked; set => SetProperty(ref is45PlusChecked, value); }
        public ICommand Is45PlusCommand { get; set; }

        bool dose1Enabled = false;
        public bool Dose1Enabled { get => dose1Enabled; set => SetProperty(ref dose1Enabled, value); }
        public ICommand Dose1Command { get; set; }

        bool dose2Enabled = false;
        public bool Dose2Enabled { get => dose2Enabled; set => SetProperty(ref dose2Enabled, value); }
        public ICommand Dose2Command { get; set; }

        private int frequency=1;
        public int Frequency { get => frequency; set=> SetProperty(ref frequency, value); }

        MetadataDto selectedDistrict;
        public MetadataDto SelectedDistrict
        {
            get => selectedDistrict;
            set
            {
                SetProperty(ref selectedDistrict, value);
            }
        }

        MetadataDto selectedState;
        public MetadataDto SelectedState
        {
            get => selectedState;
            set
            {
                if (SetProperty(ref selectedState, value))
                {
                    StateSelectedCommand.Execute(value);
                }
            }
        }

        VaccineDto selectedVaccine;
        public VaccineDto SelectedVaccine { get => selectedVaccine; set => SetProperty(ref selectedVaccine, value); }

        bool onlyFree;
        public bool OnlyFree { get => onlyFree; set => SetProperty(ref onlyFree, value); }

        bool surfAppointments = true;
        public bool SurfAppointments { get => surfAppointments; set => SetProperty(ref surfAppointments, value); }

        public ICommand StateSelectedCommand { get; }

        #endregion

        public SettingsViewModel()
        {
            States = new ObservableCollection<MetadataDto>();
            Districts = new ObservableCollection<MetadataDto>();
            StateSelectedCommand = new Command<MetadataDto>(async (x) => await OnStateSelected(x));
            _dbContext = Registry.Container.Resolve<AppDbContext>();

            Vaccines = new ObservableCollection<VaccineDto>(VaccineService.Vaccines);
            Dose1Command = new Command(() => Dose1Enabled = !Dose1Enabled);
            Dose2Command = new Command(() => Dose2Enabled = !Dose2Enabled);
            Is18PlusCommand = new Command(() => Is18PlusChecked = !Is18PlusChecked);
            Is40PlusCommand = new Command(() => Is40PlusChecked = !Is40PlusChecked);
            Is45PlusCommand = new Command(() => Is45PlusChecked = !Is45PlusChecked);
        }

        private async Task OnStateSelected(MetadataDto selectedState)
        {
            try
            {
                if (selectedState == null)
                {
                    return;
                }
                await RefreshDistricts(selectedState);
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Refresh districts failed:", e.Message);
            }
        }

        private async Task LoadStates()
        {
            var storedStates = _dbContext.States.Query().ToList();
            if (!storedStates.Any())
            {
                try
                {
                    storedStates = (await Domain.MetaDataBase.GetStates()).States.Select(x => new Entities.State
                    {
                        Id = x.StateId,
                        Name = x.StateName
                    }).ToList();

                    _dbContext.States.InsertBulk(storedStates);
                }
                catch
                {
                    Debug.WriteLine("Error");
                }
            }

            States.Clear();
            foreach (var state in storedStates)
            {
                States.Add(new MetadataDto
                {
                    Id = state.Id,
                    Name = state.Name
                });
            }
        }

        private async Task RefreshDistricts(MetadataDto selectedState)
        {
            var storedDistricts = _dbContext.Districts.Find(x => x.State.Id == selectedState.Id).ToList();
            if (!storedDistricts.Any())
            {
                storedDistricts = (await Domain.MetaDataBase.GetDistricts(selectedState.Id)).Districts.Select(x => new Entities.District
                {
                    Id = x.DistrictId,
                    Name = x.DistrictName,
                    State = new Entities.State
                    {
                        Id = selectedState.Id
                    }
                }).ToList();
                _dbContext.Districts.Insert(storedDistricts);
            }
            Districts.Clear();
            foreach (var district in storedDistricts)
            {
                Districts.Add(new MetadataDto
                {
                    Id = district.Id,
                    Name = district.Name
                });
            }
        }

        public async Task OnInit()
        {
            await LoadStates();
            await LoadSettings();
        }

        public void OnEnd()
        {
            var setting = _dbContext.Settings.Query().FirstOrDefault();
            if (setting == null)
            {
                setting = new Setting();
            }

            if (selectedState.Id != 0)
            {
                setting.State = new Entities.State
                {
                    Id = selectedState.Id,
                    Name = selectedState.Name
                };
            }
            else
            {
                setting.District = null;
            }

            if (selectedDistrict.Id != 0)
            {
                setting.District = new Entities.District
                {
                    Id = selectedDistrict.Id,
                    Name = selectedDistrict.Name,
                    State = setting.State
                };
            }
            else
            {
                setting.State = null;
            }

            setting.Is18Plus = Is18PlusChecked;
            setting.Is40Plus = Is40PlusChecked;
            setting.Is45Plus = Is45PlusChecked;

            setting.Dose1Enabled = dose1Enabled;
            setting.Dose2Enabled = dose2Enabled;

            if (SelectedVaccine !=null)
            {
                setting.Vaccine = new Entities.Vaccine
                {
                    Id = selectedVaccine.Id,
                    Name = selectedVaccine.Name
                };
            }
            else
            {
                setting.Vaccine = null;
            }

            setting.OnlyFree = onlyFree;
            setting.SurfAppointments = surfAppointments;
            setting.Frequency = frequency;

            if (setting.Id <= 0)
            {
                _dbContext.Settings.Insert(setting);
                return;
            }
            _dbContext.Settings.Update(setting);
        }

        private async Task LoadSettings()
        {
            var setting = _dbContext.Settings
                            .Include(x => x.District)
                            .Include(x => x.State)
                            .Query().SingleOrDefault();
            if (setting == null)
            {
                return;
            }

            Is18PlusChecked = setting.Is18Plus;
            Is40PlusChecked = setting.Is40Plus;
            Is45PlusChecked = setting.Is45Plus;

            Dose1Enabled = setting.Dose1Enabled;
            Dose2Enabled = setting.Dose2Enabled;

            if (setting.State != null)
            {
                var temp = States.FirstOrDefault(x => x.Id == setting.State.Id);
                SelectedState = temp;
                await RefreshDistricts(selectedState);
            }

            if (setting.District != null)
            {
                var temp = Districts.FirstOrDefault(x => x.Id == setting.District.Id);
                SelectedDistrict = temp;
            }

            if (setting.Vaccine != null)
            {
                SelectedVaccine = Vaccines.FirstOrDefault(x => x.Id == setting.Vaccine.Id);
            }

            OnlyFree = setting.OnlyFree;
            SurfAppointments = setting.SurfAppointments;
            Frequency = Math.Max(1, setting.Frequency);
        }
    }
}
