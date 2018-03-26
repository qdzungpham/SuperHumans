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
            MainNaviListView.ItemsSource = new List<ListViewTemplate>
            {
                new ListViewTemplate{Name="QA",Description="Main Navi 1",OrderNum=1},
                new ListViewTemplate { Name = "SuperHuman", Description = "Main Navi 2", OrderNum = 2 },
            };
        }

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
            }
            //Remove selected colour when touched
            ((ListView)sender).SelectedItem = null;

        }

        public class ListViewTemplate
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public int OrderNum { get; set; }
        }
    }
}