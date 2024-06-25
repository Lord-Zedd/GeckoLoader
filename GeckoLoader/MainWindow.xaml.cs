using System.Diagnostics;
using System.Windows;
using System.IO;

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
			switch (cmbGame.SelectedIndex)
			{
				case 0:
					RunAndModGame();
					break;
				case 1:
					RunAndModGameBeta();
					break;
			}
		}

		private bool IsGexInTheRoomWithYouRightNow()
		{
			return Directory.GetFiles(Directory.GetCurrentDirectory(), "gex.exe").Length > 0;
		}

		private void RunAndModGame()
		{
			ProcessStartInfo startInfo = new ProcessStartInfo();
			startInfo.FileName = @"GEX.exe";
			if (!IsGexInTheRoomWithYouRightNow())
				startInfo.WorkingDirectory = @"C:\GOG Games\Gex\";
			startInfo.Arguments = "XAchWieGutDasKeinerWeis";
			var process = Process.Start(startInfo);

			GameProcess game = new GameProcess(process);
			
			if (!game.IsConnected)
			{
				MessageBox.Show("Could not acquire GEX.exe process. Try again.");
				return;
			}

			if ((bool)chkPause.IsChecked)
			{
				game.Write(0x4039D6, new byte[] { 0x90, 0x90, 0x90, 0x90, 0x90, 0x90 }); //lost focus
				game.Write(0x403FF7, new byte[] { 0x90, 0x90, 0x90, 0x90, 0x90 }); //menu bar gained focus
			}

			if ((bool)chkDebugOptions.IsChecked)
			{
				byte[] asm = new byte[] { 0x68, 0xFF, 0xFF, 0, 0 };
				//game.Write(0x406151, asm); //help>search for help (deprecated and not debug)
				game.Write(0x406170, asm); //show functions (no effect)
				game.Write(0x40617E, asm); //show debug info
				game.Write(0x40618C, asm); //show vram
				game.Write(0x40619a, asm); //show framerate

				if ((bool)chkDebugOut.IsChecked)
				{
					if ((bool)chkDebugOutSpam.IsChecked)
					{
						byte[] asmnop = new byte[] { 0x90, 0x90, 0x90, 0x90, 0x90 };

						//block the spam
						game.Write(0x40A0C8, asmnop);
						game.Write(0x40A129, asmnop);
						game.Write(0x40A144, asmnop);
						game.Write(0x40A15F, asmnop);
						game.Write(0x40A238, asmnop);
						game.Write(0x40A24A, asmnop);
						game.Write(0x40A3AF, asmnop);
						game.Write(0x40A3F8, asmnop);
						game.Write(0x40A488, asmnop);
					}

					//route debug print to console out
					game.Write(0x405390, new byte[] { 0xEB, 0xBE });
				}
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

		private void RunAndModGameBeta()
		{
			if (!IsGexInTheRoomWithYouRightNow())
			{
				MessageBox.Show("GeckoLoader should be placed in the same directory as the beta!");
				return;
			}

			ProcessStartInfo startInfo = new ProcessStartInfo();
			startInfo.FileName = @"GEX.exe";
			var process = Process.Start(startInfo);

			GameProcess game = new GameProcess(process);

			if (!game.IsConnected)
			{
				MessageBox.Show("Could not acquire GEX.exe process. Try again.");
				return;
			}

			//software breakpoint preventing modern PCs from playing
			game.Write(0x40A0CE, new byte[] { 0xEB });

			//skip missing file dialogs due to build lacking video files
			game.Write(0x403E30, new byte[] { 0xC3, 0x90, 0x90, 0x90 });

			if ((bool)chkPause.IsChecked)
			{
				game.Write(0x403356, new byte[] { 0x90, 0x90, 0x90, 0x90, 0x90, 0x90 }); //lost focus
			}

			if ((bool)chkDebugOptions.IsChecked)
			{
				byte[] asm = new byte[] { 0x68, 0xFF, 0xFF, 0, 0 };
				game.Write(0x40528B, asm); //show functions (no effect)
				game.Write(0x4052B0, asm); //show debug info
				game.Write(0x4052BE, asm); //show vram

				if ((bool)chkDebugOut.IsChecked)
				{
					if ((bool)chkDebugOutSpam.IsChecked)
					{
						byte[] asmnop = new byte[] { 0x90, 0x90, 0x90, 0x90, 0x90 };

						//block the spam
						game.Write(0x408DE8, asmnop);
						game.Write(0x408E49, asmnop);
						game.Write(0x408E64, asmnop);
						game.Write(0x408E7F, asmnop);
						game.Write(0x408F58, asmnop);
						game.Write(0x408F6A, asmnop);
						game.Write(0x4090CF, asmnop);
						game.Write(0x409118, asmnop);
						game.Write(0x4091A8, asmnop);
					}

					//route debug print to console out
					game.Write(0x4049D0, new byte[] { 0xEB, 0xBE });
				}
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
				game.Write(0x456A10, (uint)datastart);
				game.Write(0x456A14, (uint)datastart);

				//update these count values
				game.Write(0x456A08, (uint)Levels.Count + 1);
				game.Write(0x456A0C, (uint)Levels.Count + 1);
			}

			Close();
		}

		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			Close();
		}
	}
}
