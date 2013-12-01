using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace Mp3AlbumCoverUpdater
{
    /// <summary>
    /// ��ȡjsִ��֮�����ҳhtml��ǩbody���ֵĴ���
    /// </summary>
    public class FinalHtml
    {
        private String htmlString;
        private String url;
        private String htmlTitle;
        // ���html title��ǩ������
        public String HtmlTitle
        {
            get
            {
                if (success == false) return null;
                return htmlTitle;
            }
        }
        private List<String> linkList;
        private List<String> imageList;
        private bool success; // �Ƿ�ɹ�����
        /// <summary>
        /// �����ҳ�������ӵ����� һ��Ҫ��Run֮�����
        /// </summary>
        public List<String> LinkList
        {
            get
            {
                if (success == false) return null;
                return linkList;
            }
        }
        /// <summary>
        /// �������ͼ��ı�ǩ�� һ��Ҫ��Run֮�����
        /// </summary>
        public List<String> ImageList
        {
            get
            {
                if (success == false) return null;
                return imageList;
            }
        }
        /// <summary>
        /// ���ִ����js֮�����ҳbody ���ֵ�html����
        /// </summary>
        public String HtmlBody
        {
            get
            {
                if (success == false) return null;
                return htmlString;
            }
        }
        public FinalHtml()
        {
            linkList = new List<String>();
            imageList = new List<String>();
            htmlString = "";
            success = false;
        }
        /// <summary>
        /// ��鲢��������url
        /// </summary>
        /// <param name="url"></param>
        private void CheckURL(String url)
        {
            if (!url.StartsWith("http://") && !url.StartsWith("https://") && !url.StartsWith("file:///"))
                url = "http://" + url;
            this.url = url;
        }
        /// <summary>
        /// ����ָ���ļ�
        /// </summary>
        /// <param name="url">�ļ�URL</param>
        /// <param name="timeOut">��ʱʱ��</param>
        /// <returns>�Ƿ�ɹ����У�û�г�ʱ</returns>
        public bool Run(String url, int timeOut)
        {
            timeOut = 10000;
            CheckURL(url);
            Thread newThread = new Thread(NewThread);
            newThread.SetApartmentState(ApartmentState.STA);/// Ϊ�˴���WebBrowser���ʵ�� ���뽫��Ӧ�߳���Ϊ���̵߳�Ԫ
            newThread.Start();
            //�ල���߳�����ʱ��
            while (newThread.IsAlive && timeOut > 0)
            {
                Thread.Sleep(100);
                timeOut -= 100;
            }
            // ��ʱ����
            if (newThread.IsAlive)
            {
                if (success) return true;
                newThread.Abort();
                return false;
            }
            return true;
        }

        private void NewThread()
        {
            new FinalHtmlPerThread(this);
            Application.Run();// ѭ���ȴ�webBrowser ������� ���� DocumentCompleted �¼�
        }
        /// <summary>
        ///  ���ڴ���һ��url�ĺ�����
        /// </summary>
        class FinalHtmlPerThread : IDisposable
        {
            FinalHtml master;
            WebBrowser web;

            public FinalHtmlPerThread(FinalHtml master)
            {
                this.master = master;
                DealWithUrl();
            }
            private void DealWithUrl()
            {
                String url = master.url;
                web = new WebBrowser();
                bool success = false;
                try
                {
                    web.Url = new Uri(url);
                    web.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(web_DocumentCompleted); // ���¼���ί��
                    success = true;
                }
                finally
                {
                    if (!success)
                        Dispose();
                }

            }
            public void Dispose()
            {
                if (!web.IsDisposed)
                    web.Dispose();
            }
            private void ToList(HtmlElementCollection collection, List<String> list)
            {
                System.Collections.IEnumerator it = collection.GetEnumerator();
                while (it.MoveNext())
                {
                    HtmlElement htmlElement = (HtmlElement)it.Current;
                    list.Add(htmlElement.OuterHtml);
                }
            }
            private void web_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
            {
                //΢��ٷ��ش� һ����ҳ�ж��IframԪ�ؾ��п��ܴ�����δ��¼��� �����ᵽ��
                // vb �� C++ �Ľ�������� C# û���ἰ�� �����˳��ԣ�����������������жϳɹ�
                // ���δ��ȫ���� web.ReadyState = WebBrowserReadyState.Interactive
                if (web.ReadyState != WebBrowserReadyState.Complete) return;
                master.htmlTitle = web.Document.Title;
                ToList(web.Document.Links, master.linkList);
                ToList(web.Document.Images, master.imageList);
                master.htmlString = web.DocumentText;
                master.success = true;
                Thread.CurrentThread.Abort();
            }
        }
    }
}