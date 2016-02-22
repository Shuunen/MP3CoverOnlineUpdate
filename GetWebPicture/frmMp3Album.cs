using System;
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
		ArrayList httpList;
		int iThread;
		Mp3File Mp3File = null;
		private string strSelectPaht = "";
		private DataTable dtResult = null;
		private List<string> listError = new List<string>();
		private string strEngine = "";
		
		const string strurlBaidu = "http://image.baidu.com/i?tn=baiduimage&ipn=r&ct=201326592&cl=2&lm=-1&st=-1&fm=result&fr=&sf=1&fmq=&pv=&ic=0&nc=1&z=&se=1&showtab=0&fb=0&width=&height=&face=0&istype=2&ie=utf-8&word=";
		const string strurlGoogle = "http://www.google.com/search?newwindow=1&safe=strict&biw=1366&bih=654&site=imghp&tbm=isch&sa=1&q=";
		const string strurlXiaMi = "http://www.xiami.com/search?spm=a1z1s.3521873.23310045.1.AKUtUf&key=";
		const string strurlSouGou = "http://pic.sogou.com/pics?ie=utf8&p=40230504&interV=kKIOkrELjboMmLkEk7oTkKIMkbELjbgQmLkElbcTkKILmrELjboLmLkEkr4TkKIRmLkEk78TkKILkbELjboN_1861238217&query=";
		const string strurl360 = "http://image.so.com/i?ie=utf-8&q=";
		const string strurlDDG = "http://duckduckgo.com/?iax=1&ia=images&q=";
		       
		private delegate void TempDelegate(Image image);
		private delegate void ChangeControlEnable(Button bt);

		public class ThreadInfo
		{
			public int iStart { get; set; }
			public int iEnd { get; set; }
		}

		private void btnStart_Click(object sender, EventArgs e)
		{
			btnStart.Text = "Searching...";            
			btnStart.Enabled = false;       
			
			string strurl = "";         
			
			strurl = strurlDDG + txtKeyWord.Text;
			Logger.Log("strurl : " + strurl);
			strEngine = cobEngine.Text;
			Logger.Log("strEngine : " + strEngine);
			string html = GetHtml(strurl);            
            
			flpPicture.Controls.Clear();
			httpList = GetHyperLinks(html);
			iThread = 10;
			int iSum = httpList.Count;
			int iAve = iSum / iThread;
			int iMod = iSum % iThread;
			
			Logger.Log("httpList.Count : " + httpList.Count);

			try {
                
				for (int i = 0; i < iThread; i++) {
					var ti = new ThreadInfo();
					if (i == 0) {
						ti.iStart = 0;
						ti.iEnd = iAve - 1;
					} else {
						ti.iStart = i * iAve;
						ti.iEnd = (i * iAve) + iAve - 1;
					}
					ThreadPool.QueueUserWorkItem(new WaitCallback(Mp3AlbumCoverUpdaterToForm), ti);
				}
				if (iMod != 0) {
					var ti = new ThreadInfo();
					ti.iStart = iAve * iThread;
					ti.iEnd = iSum - 1;
					ThreadPool.QueueUserWorkItem(new WaitCallback(Mp3AlbumCoverUpdaterToForm), ti);

				}
				//AutoResetEvent mainAutoResetEvent = new AutoResetEvent(false);
				RegisteredWaitHandle registeredWaitHandle = null;
				registeredWaitHandle = ThreadPool.RegisterWaitForSingleObject(new AutoResetEvent(false), new WaitOrTimerCallback(delegate(object obj, bool timeout) {
					int workerThreads = 0;
					int maxWordThreads = 0;
					int compleThreads = 0;
					ThreadPool.GetAvailableThreads(out workerThreads, out compleThreads);
					ThreadPool.GetMaxThreads(out maxWordThreads, out compleThreads);                   
					if (workerThreads == maxWordThreads) {
                      
						//mainAutoResetEvent.Set();
						registeredWaitHandle.Unregister(null);
						btnStart.Invoke(new ChangeControlEnable(ChangeButtonEnable), new object[] { btnStart });

					}
				}), null, 1000, false);
				//mainAutoResetEvent.WaitOne();
               
				this.Cursor = Cursors.WaitCursor;
			} catch (Exception ex) {
				Logger.Error("error while toto : " + ex);
			} finally {
				this.Cursor = Cursors.Default;
			}
		}

		private void ChangeButtonEnable(Button bt)
		{
			bt.Text = "Search";
			bt.Enabled = true;
		}

		private void Mp3AlbumCoverUpdaterToForm(Object threadinfo)
		{
			var ti = (ThreadInfo)threadinfo;
            
			for (int i = 0; i < ti.iEnd - ti.iStart + 1; i++) {
				Image image = null;
				try {
					image = GetUrlImage(httpList[ti.iStart + i].ToString());
					flpPicture.Invoke(new TempDelegate(AddPictureBox), new object[] { image });
				} catch (Exception ex) {
					Logger.Error("error while toto : " + ex);
				}	                    
			}
        
		}

		private void AddPictureBox(Image image)
		{
			Logger.Title("AddPictureBox");
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
			ptbNew.Image = picbox.Image;
			if (picbox.Image == null) {
				return;
			}
			label1.Text = picbox.Image.Size.ToString();
		}

		void Form1_MouseWheel(object sender, MouseEventArgs e)
		{
			var aPoint = new Point(e.X, e.Y);
			aPoint.Offset(this.Location.X, this.Location.Y);
			var aRec1 = new Rectangle(flpPicture.Location.X, flpPicture.Location.Y, flpPicture.Width, flpPicture.Height);
          

			if (RectangleToScreen(aRec1).Contains(aPoint))
				flpPicture.AutoScrollPosition = new Point(0, flpPicture.VerticalScroll.Value - e.Delta / 20);
        
		}
		private string GetHtml(string url)
		{
			Logger.Title("GetHyperLinks");
			Logger.Log("url : " + url);
			
			string strHTML = "";
			try {				
				Stream myStream = myWebClient.OpenRead(url);
				var sr = new StreamReader(myStream);
				strHTML = sr.ReadToEnd();
				strHTML = strHTML.Replace("\\", "");                
				Logger.Log("strHTML : " + strHTML);
				myStream.Close();
				
			} catch (Exception ex) {
				Logger.Error("error while getting html from url : " + ex);
			}
			return strHTML;
		}

		private ArrayList GetHyperLinks(string htmlCode)
		{
			Logger.Title("GetHyperLinks");
			
			var links = new ArrayList();
			const string strRegex = @"http:\/\/.[^""]*?\.jpg";           
			var r = new Regex(strRegex, RegexOptions.IgnoreCase);
			var m = r.Matches(htmlCode);
			
			string link;
			for (int i = 0; i < m.Count; i++) {
				link = m[i].ToString();
				Logger.Log("link found : " + link);
				links.Add(link);
			}           
			return links;
		}

		private Image GetUrlImage(string strurl)
		{
			Logger.Title("GetUrlImage");
			Logger.Log("strurl : " + strurl);
			
			var request = (HttpWebRequest)WebRequest.Create(strurl);
			request.Method = "GET";
			string strReferer = "";
			switch (strEngine) {
				case "BaiDu":
					strReferer = "http://www.baidu.com";
					break;
				case "Google":
					strReferer = "http://www.google.com";
					break;
				case "XiaMi":
					strReferer = "http://www.xiami.com/";
					break;
				case "SouGou":
					strReferer = "http://www.sogou.com/";
					break;
				case "360":
					strReferer = "http://www.so.com/";
					break;
				default:
					Logger.Error("error setting referer for engine : " + strEngine);				
					break;
			}
			
			Logger.Log("strReferer : " + strReferer);
			request.Referer = strReferer;
			request.ContentType = "application/x-www-form-urlencoded";
			Image image;
			Stream myStream;
			try {
				var response = (HttpWebResponse)request.GetResponse();
				myStream = response.GetResponseStream();
				image = Image.FromStream(myStream); 
				myStream.Close();
			} catch (Exception ex) {
				Logger.Error("error while getting image from url : " + ex);				
				listError.Add(strurl);
				image = null;
			}          
			return image;
		}

		private void Form1_Load(object sender, EventArgs e)
		{           
			cobEngine.Text = "Google";
			this.MouseWheel += new MouseEventHandler(Form1_MouseWheel);

		}

		private void btnUpdate_Click(object sender, EventArgs e)
		{
			Mp3File.TagHandler.Picture = ptbNew.Image;
			Mp3File.Update();
			dgvList.SelectedRows[0].Cells[2].Value = ptbNew.Image;
		}

		private void OpenFile_Click(object sender, EventArgs e)
		{
			var fbd = new FolderBrowserDialog();
			if (fbd.ShowDialog() == DialogResult.OK) {
                
               
				try {
					strSelectPaht = fbd.SelectedPath;
					var frm = new frmWaitingBox(new EventHandler<EventArgs>(GetFiles), 30 * 60 * 1000, "", false, true);
					if (frm.ShowDialog() == DialogResult.OK) {
						dgvList.DataSource = dtResult;
					}
				} catch (Exception ex) {
					Logger.Error("error while toto : " + ex);
				}                           
			}
           
		}

		private void GetFiles(object sender, EventArgs e)
		{
			var dt = new DataTable();
			var dc1 = new DataColumn("����", typeof(string));
			var dc2 = new DataColumn("·��", typeof(string));
			var dc3 = new DataColumn("ר������", typeof(Image));
           
			dt.Columns.Add(dc1);
			dt.Columns.Add(dc2);
			dt.Columns.Add(dc3);
			var di = new DirectoryInfo(strSelectPaht);
			FileInfo[] files1 = di.GetFiles("*.mp3");
			string strTitel = "";
			try {
				foreach (FileInfo fi in files1) {
					DataRow dr = dt.NewRow();
					dr["����"] = fi.Name;
					dr["·��"] = fi.FullName;
					strTitel = fi.Name;
					Mp3File = new Mp3File(fi.FullName);
					dr["ר������"] = Mp3File.TagHandler.Picture;
					dt.Rows.Add(dr.ItemArray);
				}
			} catch (Exception ex) {
				Logger.Error("error while processing  " + strTitel + " : " + ex);
			} finally {
				dtResult = dt;
			}           
            
		}

		private void dgvList_SelectionChanged(object sender, EventArgs e)
		{
			if (dgvList.SelectedRows.Count <= 0)
				return;

			try {
				Mp3File = new Mp3File(dgvList.SelectedRows[0].Cells["·��"].Value.ToString());
               
				var mp3fileinfo = new Mp3FileInfo(dgvList.SelectedRows[0].Cells["·��"].Value.ToString());
				txtKeyWord.Text = mp3fileinfo.Title.Trim() + " " + mp3fileinfo.Artist.Trim();
				ptpOld.Image = mp3fileinfo.AlbumCover;

			} catch (Exception ex) {
				Logger.Error("error while toto : " + ex);
				ptpOld.Image = null;
			}
		}
       

		private void btnAoutUpdate_Click(object sender, EventArgs e)
		{
			MessageBox.Show("... todo :)");
		}

		private void ptbNew_DoubleClick(object sender, EventArgs e)
		{
			var ofd = new OpenFileDialog();
			ofd.Filter = "Image|*.jpg;*.jpeg;*.png;*.bmp;*.gif";
			if (ofd.ShowDialog() == DialogResult.OK) {
				ptbNew.Image = Image.FromFile(ofd.FileName);
			}
		}

		private void ShowErrorList()
		{
			File.WriteAllLines("debug.log", listError.ToArray(), Encoding.Default);
			listError.Clear();
		}

		private string PostData(string targetUrl, string postDataStr, string refererUrl)
		{
			var request = (HttpWebRequest)WebRequest.Create(targetUrl);
			request.CookieContainer = new CookieContainer();
			var cookie = request.CookieContainer;
			request.Referer = refererUrl;
			request.Accept = "*/*";
			request.Headers["Accept-Language"] = "zh-cn";
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