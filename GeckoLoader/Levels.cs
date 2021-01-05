using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeckoLoader
{
	public partial class MainWindow
	{
		public static List<LevelPair> Levels = new List<LevelPair>()
		{
			/*the level table which the index value refers to is located at file offset x557B0 in the exe.
			 * format is as follows from what I can tell:
			 * 
			 * short	some sort of flags and/or world identification
			 * byte		lev file index
			 * byte		unk - is xD for every entry besides secret ending arts
			 * byte		mus file index for the intro audio. not used in the PC version, with the files being removed entirely.
			 * byte		mus file index for the main loop audio.
			 * byte		unk
			 * byte		unk - could be related to gex's initial joke
			 */

			// * used as a prefix to denote levels not originally available in the psx/pc level selects (ie: 3DO only)

			new LevelPair("grave1", 0x01),					//008.lev
			new LevelPair("grave2", 0x02),					//009.lev
			new LevelPair("grave3", 0x03),					//010.lev
			new LevelPair("grave4", 0x04),					//016.lev
			new LevelPair("*grave5", 0x05),					//017.lev
			new LevelPair("*grave6", 0x06),					//018.lev
			new LevelPair("grave7 (boss)", 0x07),			//019.lev
			new LevelPair("cartoon1", 0x08),				//020.lev
			new LevelPair("cartoon2", 0x09),				//021.lev
			new LevelPair("cartoon3", 0x0A),				//022.lev
			new LevelPair("cartoon4", 0x0B),				//028.lev
			new LevelPair("cartoon5", 0x0C),				//029.lev
			new LevelPair("*cartoon6", 0x0D),				//030.lev
			new LevelPair("cartoon7 (boss)", 0x0E),			//031.lev
			new LevelPair("*jungle1", 0x0F),				//032.lev
			new LevelPair("jungle2", 0x10),					//033.lev
			new LevelPair("jungle3", 0x11),					//034.lev
			new LevelPair("*jungle4", 0x12),				//040.lev
			new LevelPair("*jungle5", 0x13),				//041.lev
			new LevelPair("*jungle6", 0x14),				//042.lev
			new LevelPair("jungle7 (boss)", 0x15),			//043.lev
			new LevelPair("rez1", 0x16),					//044.lev
			new LevelPair("rez2", 0x17),					//045.lev
			new LevelPair("*rez3", 0x18),					//046.lev
			new LevelPair("*rez4", 0x19),					//051.lev
			new LevelPair("*rez5", 0x1A),					//052.lev
			new LevelPair("*rez6", 0x1B),					//053.lev
			new LevelPair("rez7 (boss)", 0x1C),				//054.lev
			new LevelPair("scifi1", 0x1D),					//136.lev
			new LevelPair("scifi2", 0x1E),					//140.lev
			new LevelPair("scifi3", 0x1F),					//143.lev
			new LevelPair("scifi4", 0x20),					//146.lev
			//new LevelPair("*scifi5", 0x21),				//149.lev - just a source string
			//new LevelPair("*scifi6", 0x22),				//152.lev - just a source string
			//new LevelPair("*scifi7 (boss)", 0x23),		//155.lev - hang - missing data?
			new LevelPair("kungfu1", 0x24),					//055.lev
			new LevelPair("kungfu2", 0x25),					//056.lev
			new LevelPair("kungfu3", 0x26),					//057.lev
			//new LevelPair("*kungfu4", 0x27),				//062.lev - crash - object related?
			new LevelPair("*kungfu5", 0x28),				//063.lev
			//new LevelPair("*kungfu6", 0x29),				//064.lev - crash - object related?
			new LevelPair("kungfu7 (boss)", 0x2A),			//065.lev
			new LevelPair("*secret1", 0x2B),				//066.lev
			//new LevelPair("*secret2", 0x2C),				//067.lev - just a source string
			//new LevelPair("*secret3", 0x2D),				//068.lev - hang - missing data?
			new LevelPair("*secret4", 0x2E),				//070.lev
			new LevelPair("*secret5", 0x2F),				//071.lev
			new LevelPair("*secret6", 0x30),				//072.lev
			//new LevelPair("*secret7", 0x31),				//073.lev - just a source string
			new LevelPair("mainmap1 (grave)", 0x32),		//074.lev
			new LevelPair("mainmap2 (cartoon)", 0x33),		//080.lev
			new LevelPair("mainmap3 (jungle)", 0x34),		//081.lev
			new LevelPair("mainmap4 (rez)", 0x35),			//087.lev
			new LevelPair("mainmap5 (scifi)", 0x36),		//088.lev
			new LevelPair("mainmap6 (kungfu)", 0x37),		//094.lev
			new LevelPair("mainmap7 (main dome)", 0x38),	//096.lev
			new LevelPair("bonus1 (jungle)", 0x39),			//101.lev
			new LevelPair("bonus2 (grave)", 0x3A),			//102.lev
			new LevelPair("bonus3 (rez)", 0x3B),			//103.lev
			new LevelPair("bonus4 (kungfu)", 0x3C),			//104.lev
			new LevelPair("bonus5 (cartoon)", 0x3D),		//105.lev
			//new LevelPair("*bonus6", 0x3e),				//106.lev - just a source string
			new LevelPair("bonus7 (scifi)", 0x3f),			//107.lev
			new LevelPair("glue1 (Title)", 0x40),			//108.lev
			new LevelPair("*glue2 (Win)", 0x41),			//114.lev
			new LevelPair("*glue3 (Options)", 0x42),		//115.lev
			new LevelPair("glue4 (Game Over)", 0x43),		//116.lev
			new LevelPair("*glue5 (Start Game)", 0x44),		//124.lev
			new LevelPair("*glue6 (reward)", 0x45),			//125.lev
			new LevelPair("glue7 (credits)", 0x46),			//135.lev
		
			//level indices x47 -> x88 point to data used by the secret ending art and is not "level" data that I've seen. (lev 170 - 238)
		
			new LevelPair("sl67", 0x89),					//158.lev
			new LevelPair("sl68", 0x8A),					//161.lev
			new LevelPair("sl69", 0x8B),					//164.lev
			new LevelPair("sl70", 0x8C),					//165,lev - secret credits on pc/psx, was ??? on 3DO
			new LevelPair("*sl71", 0x8D),					//166.lev - replaced with credits on pc/psx, was a test boss fight on 3DO
			//new LevelPair("sl72", 0x8E),					//167.lev - just a source string
			//new LevelPair("sl73", 0x8F),					//168.lev - just a source string
			//new LevelPair("sl74", 0x90),					//169.lev - just a source string
		};
	}

	public class LevelPair
	{
		public string Name { get; set; }
		public int Index { get; set; }

		public LevelPair(string name, int index)
		{
			Name = name;
			Index = index;
		}
	}

}
