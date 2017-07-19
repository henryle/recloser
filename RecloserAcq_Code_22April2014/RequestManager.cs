using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Windows.Forms;

namespace RecloserAcq
{
    public class RequestManager
    {
        string RequestFileName = Application.StartupPath + "\\DeviceFiles\\Request.xml";
        private static List<Request> _requestList;

        private RequestManager()
        {
            LoadRequest();
        }

        private static RequestManager _instance;
        public static RequestManager Instance
        {
            get
            { 
                if(_instance == null)
                {
                    _instance = new RequestManager();
                }

                return _instance;
            }
        }

        public void LoadRequest()
        {
            try
            {
                XmlSerializer ser = new XmlSerializer(typeof(List<Request>));
                using (var stream = File.OpenRead(RequestFileName))
                {
                    _requestList = ser.Deserialize(stream) as List<Request>;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                _requestList = new List<Request>();
            }
        }

        public void SaveRequest()
        {
            try
            {
                XmlSerializer ser = new XmlSerializer(typeof(List<Request>));
                using (var stream = File.CreateText(RequestFileName))
                {
                    ser.Serialize(stream, _requestList);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public List<Request> RequestList
        {
            get
            {
                //if (_requestList == null)
                //    LoadRequest();
                return _requestList;
            }
        }
    }
}
