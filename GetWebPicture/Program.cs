using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Mp3AlbumCoverUpdater
{
	static class Program
	{		
		static string thisExe, thisFolder, thisBaseFolder;
		
		static List<Provider> Providers = new List<Provider>();

		[STAThread]
		static void Main()
		{			
			thisExe = Application.ExecutablePath; // C:\Projects\mp3-cover-online-update\GetWebPicture\bin\Mp3AlbumCoverUpdater.exe
			thisFolder = Path.GetDirectoryName(Application.ExecutablePath) + "\\";  // C:\Projects\mp3-cover-online-update\GetWebPicture\bin\	
			thisBaseFolder = thisFolder.Replace("GetWebPicture\\bin\\", ""); // C:\Projects\mp3-cover-online-update\
						
			// splash			
			Logger.Draw("\n");
			Logger.Draw("           ___                                     _     _        ");
			Logger.Draw(" _____ ___|_  |   ___ ___ _ _ ___ ___    _ _ ___ _| |___| |_ ___  ");
			Logger.Draw("|     | . |_  |  |  _| . | | | -_|  _|  | | | . | . | .'|  _| -_| ");
			Logger.Draw("|_|_|_|  _|___|  |___|___|\\_/|___|_|    |___|  _|___|__,|_| |___| ");
			Logger.Draw("      |_|                                   |_|                  			 ");
			Logger.Draw("			 ");
			
			// log parameters
			Logger.Title("Variables");
			Logger.Log("thisExe : " + thisExe); // 
			Logger.Log("thisFolder : " + thisFolder); // 
			Logger.Log("thisBaseFolder : " + thisBaseFolder); // 
						
			Providers.Add(new Provider("Google", "googURL"));
			Providers.Add(new Provider("LastFm", "lastFmURL"));
			foreach (var provider in Providers) {
				Logger.Log("provider : " + provider.ID + " -> " + provider.Label + " (" + provider.Url + ")");
			}
						
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new frmMp3Album());
			
			// close log
			// Logger.Close("mp3-cover-online-update end");	
		}
	}
}