using System.Diagnostics;
using System.Windows;

namespace GeckoLoader
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			RunAndModGame();
		}

		private void RunAndModGame()
		{
			ProcessStartInfo startInfo = new ProcessStartInfo();
			startInfo.FileName = @"GEX.exe";
			startInfo.WorkingDirectory = @"C:\GOG Games\Gex\";
			startInfo.Arguments = "XAchWieGutDasKeinerWeis";
			var process = Process.Start(startInfo);

			GameProcess game = new GameProcess(process);
			
			if (!game.IsConnected)
			{
				MessageBox.Show("Could not acquire GEX.exe process. Try again.");
				return;
			}

			if ((bool)chkDebugOptions.IsChecked)
			{
				byte[] asm = new byte[] { 0x68, 0xFF, 0xFF, 0, 0 };
				//game.Write(0x406151, asm); //help>search for help (deprecated and not debug)
				game.Write(0x406170, asm); //show functions (no effect)
				game.Write(0x40617E, asm); //show debug info
				game.Write(0x40618C, asm); //show vram
				game.Write(0x40619a, asm); //show framerate
			}

			if ((bool)chkLevelSelect.IsChecked)
			{
				ulong start = (ulong)game.Allocate(0x1000);

				ulong nextstring = start;

				ulong datastart = start + 0x800;

				for (int i = 0; i < Levels.Count; i++)
				{
					var pair = Levels[i];

					game.Write(nextstring, pair.Name);
					game.Write(datastart + (ulong)(i * 8), (uint)nextstring);
					game.Write(datastart + (ulong)(i * 8 + 4), (uint)pair.Index);

					nextstring += (uint)pair.Name.Length + 1;
					uint rem = (uint)nextstring % 4;
					nextstring += 4 - rem;

				}

				//point game to the new list for both level selects
				game.Write(0x45A580, (uint)datastart);
				game.Write(0x45A584, (uint)datastart);

				//update these count values
				game.Write(0x45A578, (uint)Levels.Count + 1);
				game.Write(0x45A57C, (uint)Levels.Count + 1);
			}

			Close();

		}

		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			Close();
		}
	}
}
