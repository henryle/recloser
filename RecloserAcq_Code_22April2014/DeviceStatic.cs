using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using System.Data;
using System.Windows.Forms;

namespace RecloserAcq.Device
{
    public static class DeviceStatic
    {
        public static void SaveDevices(List<RecloserBase> list, string DeviceFile)
        {
            XmlSerializer ser = new XmlSerializer(typeof(List<RecloserBase>));
            using (var stream = File.CreateText(DeviceFile))
            {
                ser.Serialize(stream, list);
                stream.Close();
            }
        }
        public static  List<RecloserBase> GetDevices(string DeviceFile)
        {
            List<RecloserBase> list = null;
            if (File.Exists(DeviceFile))
            {
                XmlSerializer ser = new XmlSerializer(typeof(List<RecloserBase>));
                using (var stream = File.OpenRead(DeviceFile))
                {
                    var devices = ser.Deserialize(stream) as List<RecloserBase>;
                    list = devices;
                }
            }
            else
            {
                list = new List<RecloserBase>();
                list.Add(new CooperFXB(7000) { Name = "Cooper 1" });
                list.Add(new Nulec(7001) { Name = "Nulec 1" });
            }

            return list;
        }

        public static DataTable GetDeviceBtn(string DeviceFile)
        {
            List<DevicesBtn> list = null;
            if (File.Exists(DeviceFile))
            {
                XmlReader xmlFile = XmlReader.Create(DeviceFile, new XmlReaderSettings());
                DataSet dataSet = new DataSet();
                //Read xml to dataset
                dataSet.ReadXml(xmlFile);
                //Pass empdetails table to datagridview datasource
                
                //Close xml reader
                xmlFile.Close();
                return dataSet.Tables["DevicesBtn"];
            }
            else
            {
                list = new List<DevicesBtn>();
                list.Add(new DevicesBtn() { Text = "KCN 1" });
                list.Add(new DevicesBtn() { Text = "KCN 2" });
                list.Add(new DevicesBtn() { Text = "KCN 3" });

                return null;    
            }

            
        }
        public static bool IsPasswordValidated(string devicetype)
        {
            ValidatePassword vp = new ValidatePassword(true, devicetype);
            if (vp.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
            {
                //this.Close();
                return false;
            }
            return true;
        }
        public static bool Validate_SendCommand(string devicename, string command)
        {
            //require password
            if (System.Windows.Forms.MessageBox.Show("Bạn có chắc bạn muốn " + command + " " + devicename + "?", "Xac nhan", System.Windows.Forms.MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                return true;
            }
            return false;
        }
    }
}
