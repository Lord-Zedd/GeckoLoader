using System.Diagnostics;
using System.Linq;
using System.Windows;

namespace GeckoLoader
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		private void Application_Startup(object sender, StartupEventArgs e)
		{
			var proc = Process.GetProcessesByName("gex").FirstOrDefault();
			if (proc != null)
			{
				MessageBox.Show("GEX.exe is already running. Please exit the game and try again.");
				Shutdown();
			}
		}
	}
}
