/*
 * Created by SharpDevelop.
 * User: ROMAINRA
 * Date: 19/02/2016
 * Time: 17:34
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.IO;
using System.Windows.Forms;

namespace Mp3AlbumCoverUpdater
{
	/// <summary>
	/// Description of Log.
	/// </summary>
	public sealed class Logger
	{
		private static Logger instance = new Logger();
		
		const string logFile = "debug.log";
		static StreamWriter logInstance;
		static bool hadError = false;
		static string logPrefix, logSuffix, logSpaces;
		const int logMargin = 24;
		const string logDateFormat = "hh:mm:ss tt";
		
		public static Logger Instance {
			get {
				return instance;
			}
		}
		
		private Logger()
		{
			logInstance = new StreamWriter(logFile);
			logInstance.AutoFlush = true;
		}
		
		public static void Log(string str)
		{
			logPrefix = "[ " + DateTime.Now.ToString(logDateFormat) + "- log ] ";
			logSuffix = "";
			WriteLine(str);
		}
		
		public static void Draw(string str)
		{
			logPrefix = "";
			logSuffix = "";
			WriteLine(str);
		}
		
		public static void Title(string str)
		{
			logPrefix = "\n\n- ";
			logSuffix = "\n------------------------------";
			WriteLine(str);
		}
		
		public static void Error(string str)
		{
			hadError = true;
			logPrefix = "\n\n" + "[ " + DateTime.Now.ToString(logDateFormat) + "- ERROR ] ";
			logSuffix = "\n\n";
			WriteLine(str);
		}
		
		public static void Close(string str)
		{
			logPrefix = "\n\n=============================\n= ";
			logSuffix = "\n=============================";
			WriteLine(str);
			logInstance.Close();
			if (hadError) {
				MessageBox.Show("Some error(s) happened, look at " + logFile);
			}
		}
		
		static void WriteLine(string str)
		{
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
		}
	}
}
