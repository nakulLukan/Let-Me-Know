using LetMeKnow.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LetMeKnow.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        private HomeViewModel viewModel;
        public HomePage()
        {
            InitializeComponent();
            viewModel = BindingContext as HomeViewModel;
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