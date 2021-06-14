using LetMeKnow.Models.Metadata;
using LetMeKnow.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LetMeKnow.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        SettingsViewModel viewModel;
        public SettingsPage()
        {
            InitializeComponent();
            viewModel = BindingContext as SettingsViewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await viewModel.OnInit();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            viewModel.OnEnd();
        }
    }
}