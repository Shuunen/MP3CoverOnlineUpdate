using System;
using System.Configuration;
using System.Data;
using System.Text;
using System.Net;
using System.IO;
using System.Collections;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using Mp3Lib;
using Id3Lib;
using System.IO.Compression;
 
namespace Mp3AlbumCoverUpdater
{
	public partial class frmMp3Album : Form
	{
		public frmMp3Album()
		{
			InitializeComponent();
		}

		WebClient myWebClient = new WebClient();
		ArrayList imgUrls;
		Mp3File selectedMp3File;
		Mp3FileInfo selectedMp3FileInfo;
		string selectedPath = ReadSetting("selectedPath");
		DataTable tableResults = null;
		List<string> listError = new List<string>();
		Provider selectedProvider;
		
		const string regexMultipleSpaces = @"\s+";
		const string regexPotentialSeparators = @"[_-]+";
		const string regexUnwantedChars = @"[^a-zA-Z0-9\s]+";
		const string regexLineReturns = @"(?:\r\n|\n|\r)";
		const string regexImageUrl = @"http:\/\/.[^""]*?\.jpg";
		
		private delegate void TempDelegate(Image image);
		private delegate void ChangeControlEnable(Button bt);

		public class ThreadInfo
		{
			public int iStart { get; set; }
			public int iEnd { get; set; }
		}

		void btnSearch_Click(object sender, EventArgs e)
		{
			SetLoadingStatus(true);
									
			foreach (var provider in Program.Providers) {
				if (provider.ID == cobEngine.Text) {
					selectedProvider = provider;
				}
			}
			
			string searchUrl = selectedProvider.Url + searchInput.Text;
			Logger.Log("searchUrl : " + searchUrl);
			imgUrls = GetHyperLinks(GetHtml(searchUrl));
			
			flpPicture.Controls.Clear();
						
			var maxThreads = int.Parse(ReadSetting("maxThreads", "3"));
			var maxImages = int.Parse(ReadSetting("maxImages", "15"));
			
			int urlsCount = imgUrls.Count > maxImages ? maxImages : imgUrls.Count;
			int urlsPerBatch = urlsCount / maxThreads;
			int urlsRemaining = urlsCount % maxThreads;			
			Logger.Log("urlsCount : " + urlsCount);
			Logger.Log("urlsPerBatch : " + urlsPerBatch);
			Logger.Log("urlsRemaining : " + urlsRemaining);
			
			// if no url to parse
			if (urlsCount == 0) {
				Logger.Log("no covers found");
				SetLoadingStatus(false);
				btnSearch.Text = "no covers found";
				return;
			}
			
			try {
									
				Cursor = Cursors.WaitCursor;
				
				// start threads
				for (int i = 0; i < maxThreads; i++) {
					var ti = new ThreadInfo();
					if (i == 0) {
						ti.iStart = 0;
						ti.iEnd = urlsPerBatch - 1;
					} else {
						ti.iStart = i * urlsPerBatch;
						ti.iEnd = (i * urlsPerBatch) + urlsPerBatch - 1;
					}
					Logger.Log("starting ThreadPool, i  : " + i + " | " + "ti.iStart : " + ti.iStart + " | " + "ti.iEnd : " + ti.iEnd);
					ThreadPool.QueueUserWorkItem(new WaitCallback(Mp3AlbumCoverUpdaterToForm), ti);
				}
				
				// check if urls remaining
				if (urlsRemaining != 0) {
					var ti = new ThreadInfo();
					ti.iStart = urlsPerBatch * maxThreads;
					ti.iEnd = urlsCount - 1;
					Logger.Log("starting ThreadPool" + " | " + "ti.iStart : " + ti.iStart + " | " + "ti.iEnd : " + ti.iEnd);
					ThreadPool.QueueUserWorkItem(new WaitCallback(Mp3AlbumCoverUpdaterToForm), ti);
				}
				
				RegisteredWaitHandle registeredWaitHandle = null;
				registeredWaitHandle = ThreadPool.RegisterWaitForSingleObject(new AutoResetEvent(false), new WaitOrTimerCallback(delegate(object obj, bool timeout) {
					int workerThreads = 0;
					int maxWordThreads = 0;
					int compleThreads = 0;
					ThreadPool.GetAvailableThreads(out workerThreads, out compleThreads);
					ThreadPool.GetMaxThreads(out maxWordThreads, out compleThreads);                   
					if (workerThreads == maxWordThreads) {
						registeredWaitHandle.Unregister(null);
						Logger.Log("thread workers ended !");
						Cursor = Cursors.Default;
						SetLoadingStatus(false);
					}
				}), null, 1000, false);			
				
			} catch (Exception ex) {
				Logger.Error("error while searching covers : " + ex);
			} 
		}

		void SetLoadingStatus(Boolean isLoading)
		{
			if (isLoading) {
				btnSearch.Text = "Searching...";            
				btnSearch.Enabled = false;       
			} else {
				btnSearch.Text = "Search";
				btnSearch.Enabled = true;
			}
		}

		void Mp3AlbumCoverUpdaterToForm(Object threadinfo)
		{
			Logger.Log("Mp3AlbumCoverUpdaterToForm");
			var ti = (ThreadInfo)threadinfo;
			string url;
			for (int i = 0; i < ti.iEnd - ti.iStart + 1; i++) {
				Image image = null;
				try {
					url = imgUrls[ti.iStart + i].ToString();
					Logger.Log("getting url : " + url);
					image = GetUrlImage(url);
					flpPicture.Invoke(new TempDelegate(AddPictureBox), new object[] { image });
				} catch (Exception ex) {
					Logger.Error("error while cover display : " + ex);
				}	                    
			}
        
		}

		void AddPictureBox(Image image)
		{
			// Logger.Title("AddPictureBox");
			if (image != null) {
				var picbox = new PictureBox();
				picbox.Size = new Size(100, 100);
				picbox.Click += new EventHandler(picbox_Click);
				picbox.SizeMode = PictureBoxSizeMode.StretchImage;
				picbox.Image = image;
				flpPicture.Controls.Add(picbox);
			} else {
				Logger.Error("cannot display null image");
			}
		}

		void picbox_Click(object sender, EventArgs e)
		{
			var picbox = sender as PictureBox;
			selectedCover.Image = picbox.Image;
			if (picbox.Image == null) {
				return;
			}
			label1.Text = picbox.Image.Size.ToString();
		}

		
		string GetHtml(string url)
		{
			Logger.Title("GetHtml");
			Logger.Log("url : " + url);
			
			string html = "";
			try {				
				Stream myStream = myWebClient.OpenRead(url);
				var sr = new StreamReader(myStream);
				html = sr.ReadToEnd();
				html = html.Replace("\\", "");
				html = string.Join(" ", Regex.Split(html, regexLineReturns));
				html = Regex.Replace(html, regexMultipleSpaces, " ");
				Logger.Log("html : " + html);
				myStream.Close();
				
			} catch (Exception ex) {
				Logger.Error("error while getting html from url : " + ex);
			}
			return html;
		}

		ArrayList GetHyperLinks(string htmlCode)
		{
			Logger.Title("GetHyperLinks");
			
			var links = new ArrayList();  
			var r = new Regex(regexImageUrl, RegexOptions.IgnoreCase);
			var m = r.Matches(htmlCode);
			
			string link;
			for (int i = 0; i < m.Count; i++) {
				link = m[i].ToString();
				Logger.Log("link found : " + link);
				links.Add(link);
			}           
			return links;
		}

		Image GetUrlImage(string url)
		{
			//Logger.Title("GetUrlImage");
			//Logger.Log("url : " + url);
			//Logger.Log("referer : " + selectedProvider.Referer);
			
			var request = (HttpWebRequest)WebRequest.Create(url);
			request.Method = "GET";				
			request.Referer = selectedProvider.Referer;
			request.ContentType = "application/x-www-form-urlencoded";			
			Image image;
			try {
				var response = (HttpWebResponse)request.GetResponse();
				var myStream = response.GetResponseStream();
				image = Image.FromStream(myStream); 
				myStream.Close();
			} catch (Exception ex) {
				Logger.Error("error while getting image from url : " + ex);				
				listError.Add(url);
				image = null;
			}          
			return image;
		}

		void Form1_Load(object sender, EventArgs e)
		{           
			Logger.Title("GUI Load");
			string defaultProvider = Program.Providers[0].ID;
			Logger.Log("set default provider to : " + defaultProvider);
			cobEngine.Text = defaultProvider;
			MouseWheel += Form1_MouseWheel;
			
			UpdateBtnOnlyMissing();
						
			if (selectedPath.Length > 0) {
				InitFilesLoading();
			}
		}
		
		void Form1_MouseWheel(object sender, MouseEventArgs e)
		{
			var aPoint = new Point(e.X, e.Y);
			aPoint.Offset(Location.X, Location.Y);
			var aRec1 = new Rectangle(flpPicture.Location.X, flpPicture.Location.Y, flpPicture.Width, flpPicture.Height);
			if (RectangleToScreen(aRec1).Contains(aPoint)) {
				flpPicture.AutoScrollPosition = new Point(0, flpPicture.VerticalScroll.Value - e.Delta / 20);
			}
		}

		void btnUpdate_Click(object sender, EventArgs e)
		{
			selectedMp3File.TagHandler.Picture = selectedCover.Image;
			selectedMp3File.Update();
			fileList.SelectedRows[0].Cells[2].Value = selectedCover.Image;
		}

		void SetSelectedPath(string path)
		{
			selectedPath = path;
			Logger.Log("selectedPath : " + path);		
			AddUpdateAppSettings("selectedPath", path);			 
		}
		
		void OpenFile_Click(object sender, EventArgs e)
		{
			Logger.Title("OpenFile_Click");
			var fbd = new FolderBrowserDialog();
			if (fbd.ShowDialog() == DialogResult.OK) {                            
				try {					
					SetSelectedPath(fbd.SelectedPath);
					InitFilesLoading();
				} catch (Exception ex) {
					Logger.Error("error while frmWaitingBox : " + ex);
				}                           
			} else {
				Logger.Log("no selected path");
			}
           
		}

		void GetFiles(object sender, EventArgs e)
		{
			var table = new DataTable();
			var col1 = new DataColumn("Filename", typeof(string));
			var col2 = new DataColumn("Path", typeof(string));
			var col3 = new DataColumn("Cover", typeof(Image));           
			table.Columns.Add(col1);
			table.Columns.Add(col2);
			table.Columns.Add(col3);
			
			var dir = new DirectoryInfo(selectedPath);
			var files = dir.GetFiles("*.mp3");
			var currentFile = "";
			try {
				foreach (FileInfo file in files) {					
					var mp3File = new Mp3File(file.FullName);
					if(ReadSetting("onlyMissing", "yes") == "yes" && mp3File.TagHandler.Picture != null){
						continue;
					}					
									
					currentFile = file.Name;
					
					var row = table.NewRow();	
					row["Filename"] = file.Name;
					row["Path"] = file.FullName;					
					row["Cover"] = mp3File.TagHandler.Picture;					
					table.Rows.Add(row.ItemArray);
				}
			} catch (Exception ex) {
				Logger.Error("error while processing file : " + currentFile + "\n" + ex);
			} finally {
				tableResults = table;
			}           
            
		}

		void InitFilesLoading()
		{
			Logger.Title("InitFilesLoading");
			Logger.Log("selectedPath : " + selectedPath);
			
			var frm = new frmWaitingBox(new EventHandler<EventArgs>(GetFiles), 30 * 60 * 1000, "", false, true);
			if (frm.ShowDialog() == DialogResult.OK) {
				fileList.DataSource = tableResults;
			}
		}
		void fileList_SelectionChanged(object sender, EventArgs e)
		{
			if (fileList.SelectedRows.Count <= 0) {
				return;
			}

			try {
				string path = fileList.SelectedRows[0].Cells["Path"].Value.ToString();
				selectedMp3File = new Mp3File(path);
				selectedMp3FileInfo = new Mp3FileInfo(path);				
				string searchText = selectedMp3FileInfo.Title.Trim() + " " + selectedMp3FileInfo.Artist.Trim();
				searchText = Regex.Replace(searchText, regexPotentialSeparators, " "); // replace potential separators by spaces
				searchText = Regex.Replace(searchText, regexUnwantedChars, ""); // remove unwanted chars
				searchText = Regex.Replace(searchText, regexMultipleSpaces, " ");
				searchInput.Text = searchText;
				if (btnSearch.Enabled) {
					SetLoadingStatus(false);
				} else {
					Logger.Log("TODO : you changed song selection while searching, this should stop search");
				}
				currentCover.Image = selectedMp3FileInfo.AlbumCover;

			} catch (Exception ex) {
				Logger.Error("error while setting searchText & current cover : " + ex);
				currentCover.Image = null;
			}
		}

		void UpdateBtnOnlyMissing()
		{
			btnOnlyMissing.Text = (ReadSetting("onlyMissing", "yes") == "yes") ? "Show all track" : "Show only missing";
		}

		void btnOnlyMissing_Click(object sender, EventArgs e)
		{
			string opposite = (ReadSetting("onlyMissing", "yes") == "yes") ? "no" : "yes";
			AddUpdateAppSettings("onlyMissing", opposite);
			UpdateBtnOnlyMissing();
			InitFilesLoading();
		}

		void selectedCover_DoubleClick(object sender, EventArgs e)
		{
			var ofd = new OpenFileDialog();
			ofd.Filter = "Image|*.jpg;*.jpeg;*.png;*.bmp;*.gif";
			if (ofd.ShowDialog() == DialogResult.OK) {
				selectedCover.Image = Image.FromFile(ofd.FileName);
			}
		}

		void ShowErrorList()
		{
			foreach (var error in listError) {
				Logger.Error(error);
			}
			listError.Clear();
		}
		
		static string ReadSetting(string key, string fallback = "")
		{
			string result = "";
			try {
				var appSettings = ConfigurationManager.AppSettings;
				result = appSettings[key] ?? fallback;
				Logger.Log("setting " + key + " : " + result);
			} catch (Exception ex) {
				Logger.Error("error reading setting : " + ex);
			} 
			return result;
		}
		
		static void AddUpdateAppSettings(string key, string value)
		{
			try {
				var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
				var settings = configFile.AppSettings.Settings;
				if (settings[key] == null) {
					settings.Add(key, value);
				} else {
					settings[key].Value = value;
				}
				Logger.Log("now setting " + key + " is set to : " + value);
				configFile.Save(ConfigurationSaveMode.Modified);
				ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
			} catch (Exception ex) {
				Logger.Error("error writing setting : " + ex);
			} 
		}

		string PostData(string targetUrl, string postDataStr, string refererUrl)
		{
			var request = (HttpWebRequest)WebRequest.Create(targetUrl);
			request.CookieContainer = new CookieContainer();
			var cookie = request.CookieContainer;
			request.Referer = refererUrl;
			request.Accept = "*/*";
			//request.Headers["Accept-Language"] = "zh-cn";
			request.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.1; WOW64; Trident/6.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; .NET4.0C; .NET4.0E)";
			request.KeepAlive = true;
			request.ContentType = "application/x-www-form-urlencoded";
			request.Method = "POST";

			var encoding = Encoding.UTF8;
			var postData = encoding.GetBytes(postDataStr);
			request.ContentLength = postData.Length;
			var requestStream = request.GetRequestStream();
			requestStream.Write(postData, 0, postData.Length);

			var response = (HttpWebResponse)request.GetResponse();
			var responseStream = response.GetResponseStream();
			if (response.Headers["Content-Encoding"] != null && response.Headers["Content-Encoding"].ToLower().Contains("gzip")) {
				responseStream = new GZipStream(responseStream, CompressionMode.Decompress);
			}

			var streamReader = new StreamReader(responseStream, encoding);
			var retString = streamReader.ReadToEnd();

			streamReader.Close();
			responseStream.Close();
			return retString;
		}
	}
}