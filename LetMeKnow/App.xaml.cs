using LetMeKnow.Entities;
using LetMeKnow.Services;
using Matcha.BackgroundService;
using Xamarin.Forms;

namespace LetMeKnow
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();
            DependencyService.Register<MockDataStore>();
            MainPage = new AppShell();
            Registry.Register();
        }

        protected override void OnStart()
        {
            int schedulerFrequency = 20;
            var dbContext = Registry.Container.Resolve<AppDbContext>();
            var setting = dbContext.Settings.Query().FirstOrDefault();
            if (setting != null)
            {
                schedulerFrequency = setting.Frequency;
            }
            BackgroundAggregatorService.Add(() => new SurfAppointmentService(60 / schedulerFrequency));

            //Start the background service
            BackgroundAggregatorService.StartBackgroundService();
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
