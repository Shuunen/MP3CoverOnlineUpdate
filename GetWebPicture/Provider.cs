/*
 * Created by SharpDevelop.
 * User: ROMAINRA
 * Date: 19/02/2016
 * Time: 17:08
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Text.RegularExpressions;

namespace Mp3AlbumCoverUpdater
{
	/// <summary>
	/// Description of Provider.
	/// </summary>
	public class Provider
	{
		public Provider(string label, string url, string referer)
		{
			this.ID = this.generateIdFromString(label);
			this.Label = label;
			this.Url = url;
			this.Referer = referer;
		}

		public string Label { get; set; }
		public string Url { get; set; }
		public string Referer { get; set; }
		public string ID { get; set; }
		
		string generateIdFromString(string str)
		{
			str = Regex.Replace(str, "[^a-zA-Z0-9]+", "", RegexOptions.Compiled);
			//Log("pass 1 : " + str);
			return str;
		}
	}
}
