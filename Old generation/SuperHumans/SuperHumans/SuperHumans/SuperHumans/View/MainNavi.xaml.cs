using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SuperHumans.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MainNavi : ContentPage
	{
		public MainNavi ()
		{
			InitializeComponent ();
            //MainNaviListView.ItemsSource = new List<ListViewTemplate>
            //{
            //    new ListViewTemplate{Name="QA",Description="Main Navi 1",OrderNum=1},
            //    new ListViewTemplate { Name = "SuperHuman", Description = "Main Navi 2", OrderNum = 2 },
            //    new ListViewTemplate { Name = "Logout", Description = "Main Navi 4", OrderNum = 4 },
            //    new ListViewTemplate { Name = "Register", Description = "Main Navi 3", OrderNum = 3 }
            //};
        }

        void QAButton(object sender, ItemTappedEventArgs e)
        {
            Navigation.PushAsync(new QA_Page());
        }

        void SuperhumanButton(object sender, ItemTappedEventArgs e)
        {
            Navigation.PushAsync(new Superhuman());
        }

        void PreviousPageButton(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        } //

        async void OnTapped(object sender, ItemTappedEventArgs e)
        {
            var Selected = e.Item as ListViewTemplate;
            //DisplayAlert("Item Tapped", Selected.Name, "Okay");
            switch (Selected.OrderNum)
            {
                case 1:
                    await Navigation.PushAsync(new QA_Page());
                    break;
                case 2:
                    await Navigation.PushAsync(new Superhuman());
                    break;
                case 3:
                    await Navigation.PushAsync(new NewUsersRegister());
                    break;
                case 4:
                    await Navigation.PushAsync(new Logout());
                    break;
            }
            //Remove selected colour when touched
            ((ListView)sender).SelectedItem = null;

        }
        
        void LogoutButton(object sender, ItemTappedEventArgs e)
        {
            Navigation.PushAsync(new Logout());
        }

        public class ListViewTemplate
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public int OrderNum { get; set; }
        }
    }
}