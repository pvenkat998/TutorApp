using System;
using System.Windows.Input;
using MvvmHelpers;
using TutorApp2.Models;
using TutorApp2.Views;
using Xamarin.Forms;

namespace TutorApp2
{
    public class RoomsViewModel : BaseViewModel
    {
        ITwilioMessenger twilioMessenger;

        public Command ConnectCommand { get; }

        Page page;

        public RoomsViewModel(Page page)
        {
            this.page = page;
            twilioMessenger = DependencyService.Get<ITwilioMessenger>();

            ConnectCommand = new Command(async () =>
            {
                var success = false;
                string message = string.Empty;
                try
                {
                    IsBusy = true;
                    success = await twilioMessenger.InitializeAsync();
                }
                catch (Exception ex)
                {
                    message = ex.Message;
                }
                finally
                {
                    IsBusy = false;
                }

                if (success)
                {
                    await page.DisplayAlert("Success", "Now joining #general.", "OK");
                    await page.Navigation.PushAsync(new MessagePage());
                }
                else
                {
                    await page.DisplayAlert("Sad Monkeys", $"Unable to join #general: {message}", "OK");
                }
            });
        }



    }
}

