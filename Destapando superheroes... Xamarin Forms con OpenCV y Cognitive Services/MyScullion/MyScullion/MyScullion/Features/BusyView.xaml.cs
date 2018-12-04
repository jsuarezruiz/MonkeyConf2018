using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyScullion.Features
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class BusyView : ContentView
	{
		public BusyView ()
		{
			InitializeComponent ();
		}
	}
}