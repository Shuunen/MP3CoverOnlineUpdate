using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Mp3AlbumCoverUpdater
{
	static class Program
	{
		const string logFile = "debug.log";
		static StreamWriter logInstance;
		static bool hadError = false;
		static string thisExe, thisFolder, thisBaseFolder;
		
		/// <summary>
		/// 应用程序的主入口点。
		/// </summary>
		[STAThread]
		static void Main()
		{			
			thisExe = Application.ExecutablePath; // C:\Projects\mp3-cover-online-update\GetWebPicture\bin\Mp3AlbumCoverUpdater.exe
			thisFolder = Path.GetDirectoryName(Application.ExecutablePath) + "\\";  // C:\Projects\mp3-cover-online-update\GetWebPicture\bin\	
			thisBaseFolder = thisFolder.Replace("GetWebPicture\\bin\\", ""); // C:\Projects\mp3-cover-online-update\
			logInstance = new StreamWriter(thisBaseFolder + logFile);
			logInstance.AutoFlush = true;
			
			// splash
			Log("\n", "draw");
			Log("           ___                                     _     _        ", "draw");
			Log(" _____ ___|_  |   ___ ___ _ _ ___ ___    _ _ ___ _| |___| |_ ___  ", "draw");
			Log("|     | . |_  |  |  _| . | | | -_|  _|  | | | . | . | .'|  _| -_| ", "draw");
			Log("|_|_|_|  _|___|  |___|___|\\_/|___|_|    |___|  _|___|__,|_| |___| ", "draw");
			Log("      |_|                                   |_|                  			 ", "draw");
			Log("			 ", "draw");
			
			// log parameters
			Log("Variables", "title");
			Log("thisExe : " + thisExe); // 
			Log("thisFolder : " + thisFolder); // 
			Log("thisBaseFolder : " + thisBaseFolder); // 
			
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new frmMp3Album());
			
			// close log
			Log("mp3-cover-online-update end", "end");

			if (hadError) {
				MessageBox.Show("Some error(s) happended, look at " + thisBaseFolder + logFile);
			}
		}
		
		static string logPrefix, logSuffix, logSpaces;
		const int logMargin = 18;

		static void Log(string str, string type = "debug")
		{

			logPrefix = "";
			logSuffix = "";

			switch (type) {
				case "error":
					hadError = true;
					logPrefix = "\n\n[ ERROR ] ";
					logSuffix = "\n\n";
					break;
				case "debug":
					logPrefix = "[ debug ] ";
					break;
				case "title":
					logPrefix = "\n\n- ";
					logSuffix = "\n------------------------------";
					break;
				case "end":
					logPrefix = "\n\n=============================\n= ";
					logSuffix = "\n=============================";
					break;
				case "draw":
					logPrefix = " ";
					break;
			}
			
			int index = str.IndexOf(":", StringComparison.Ordinal);
			if (index > 0 && (logMargin - index) > 0) {
				try {
					logSpaces = new string(' ', logMargin - index);
					str = str.Substring(0, index) + logSpaces + " : " + str.Substring(index + 1);
				} catch (Exception ex) {
					str = str + "\n\n" + ex + "\n\n";
				}
			}
			
			str = logPrefix + str + logSuffix;
			
			logInstance.WriteLine(str);

			logInstance.Flush();

			if (type == "end") {
				logInstance.Close();
			}
		}
		
		static string RemoveSpecialCharacters(string str)
		{
			return Regex.Replace(str, "[^a-zA-Z0-9_.]+", "", RegexOptions.Compiled);
		}
	}
}