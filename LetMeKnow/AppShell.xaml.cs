using LetMeKnow.Views;
using Xamarin.Forms;

namespace LetMeKnow
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(SettingsPage), typeof(SettingsPage));
            Routing.RegisterRoute(nameof(HomePage), typeof(HomePage));
        }

    }
}
