using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Mp3AlbumCoverUpdater
{
	public static class Program
	{		
		static string thisExe, thisFolder, thisBaseFolder;
		
		public static List<Provider> Providers = new List<Provider>();

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
						
			// Providers.Add(new Provider("LastFm", "lastFmURL","lastFmReferer"));
			Providers.Add(new Provider("Google", "http://www.google.com/search?newwindow=1&safe=strict&biw=1366&bih=654&site=imghp&tbm=isch&sa=1&q=", "http://www.google.com"));		
			Providers.Add(new Provider("XiaMi", "http://www.xiami.com/search?spm=a1z1s.3521873.23310045.1.AKUtUf&key=", "http://www.xiami.com/"));
			Providers.Add(new Provider("BaiDu", "http://image.baidu.com/i?tn=baiduimage&ipn=r&ct=201326592&cl=2&lm=-1&st=-1&fm=result&fr=&sf=1&fmq=&pv=&ic=0&nc=1&z=&se=1&showtab=0&fb=0&width=&height=&face=0&istype=2&ie=utf-8&word=", "http://www.baidu.com"));
			Providers.Add(new Provider("SouGou", "http://pic.sogou.com/pics?ie=utf8&p=40230504&interV=kKIOkrELjboMmLkEk7oTkKIMkbELjbgQmLkElbcTkKILmrELjboLmLkEkr4TkKIRmLkEk78TkKILkbELjboN_1861238217&query=","http://www.sogou.com/"));
			Providers.Add(new Provider("360", "http://image.so.com/i?ie=utf-8&q=", "http://www.so.com/"));
			foreach (var provider in Providers) {
				Logger.Log("provider : ID : " + provider.ID + " | Label : " + provider.Label + " | Url : " + provider.Url);
			}
						
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new frmMp3Album());
			
			// close log
			// Logger.Close("mp3-cover-online-update end");	
		}
	}
}