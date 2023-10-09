using MauiNotificationv1.Models;
using Newtonsoft.Json;
using System.Text;

namespace MauiNotificationv1;

public partial class MainPage : ContentPage
{
	int count = 0;
	string _deviceToken = null;

	public MainPage()
	{
		InitializeComponent();

        if (Preferences.ContainsKey("DeviceToken"))
        {
            _deviceToken = Preferences.Get("DeviceToken", null);
        }

        if (Preferences.ContainsKey("NavigationID"))
        {
			var id = Preferences.Get("NavigationID", null);
			if (id != null)
			{
				// do something
			}
        }
    }

	private async void OnCounterClicked(object sender, EventArgs e)
	{
		count++;

		if (count == 1)
			CounterBtn.Text = $"Clicked {count} time";
		else
			CounterBtn.Text = $"Clicked {count} times";

		SemanticScreenReader.Announce(CounterBtn.Text);

		var pushNotificationRequest = new PushNotificationRequest
		{
			notification = new NotificationMessageBody
			{
				title = "Notification Title",
				body = "Notification Body"
			},
			registration_ids = new List<string>() { _deviceToken },
			data = new Dictionary<string, string>() { { "NavigationID", "1"} }
		};

		var url = "https://fcm.googleapis.com/fcm/send";

		using ( var httpClient = new HttpClient() )
		{
			httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("key", "=" + "AAAABMUTEJw:APA91bEk1qj32HjwFkfGzgw9oR9iAfqGLQv6yOflKHEPIkTmJhlR8Wx8FvrnPb6_vHd6OY_aErfVEQFB-d3OxInrF4nRyAVe21VIVgxM13s9IYxusCvUlrYFIas2o5hiHzsf8dPOd4C7");
			
			string json = JsonConvert.SerializeObject(pushNotificationRequest);
			var res = await httpClient.PostAsync(url, new StringContent(json, Encoding.UTF8, "application/json"));

			if(res.StatusCode == System.Net.HttpStatusCode.OK)
			{
				await DisplayAlert("Information", "Sent notification", "OK");
			}
		}
	}
}

