using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using System.IO;
using FA_Accounting.Common;
namespace RecloserAcq.Device
{
    public class RecloserADVCTCPIP : RecloserADVC
    {
        public RecloserADVCTCPIP(int port)
            : base(port)
        {
            _deviceTime = null;
        }

        public RecloserADVCTCPIP()
            : this(0)            
        { }

        public override eDeviceType DeviceType
        {
            get
            {
                return eDeviceType.RecloserADVCTCPIP;
            }
        }
        public override bool sendConnectCommand()
        {
            try
            {
                timerBetweenEachPoll.Stop();
                timerBetweenEachRequest.Stop();
                //1B 32 01 2A 0A 52 45 51 0A 4F 53 0A 7E 0A 34 42 46 37 03
                //hex_login_init1: 0x1B, 0x32 , 0x01 , 0x2A , 0x0A , 0x52 , 0x45 , 0x51 , 0x0A , 0x4F , 0x53 , 0x0A , 0x7E , 0x0A , 0x34 , 0x42 , 0x46 , 0x37 , 0x03 , 0x1B , 0x32 , 0x01 , 0x2A , 0x0A , 0x52 , 0x45 , 0x51 , 0x0A , 0x4F , 0x53 , 0x0A , 0x7E , 0x0A , 0x34 , 0x42 , 0x46 , 0x37 , 0x03
                Listener.Send(new byte[] { 0x00, 0x2c, 0x00, 0x01, 0x53, 0x45, 0x52, 0x56, 0x45, 0x52, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x41, 0x41, 0x41, 0x41, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 });
                sleep(2);
                // hex_login_init2: 0x1B, 0x32, 0x01 , 0x2A , 0x0A , 0x52 , 0x45 , 0x51 , 0x0A , 0x44 , 0x49 , 0x44 , 0x2D , 0x33 , 0x36 , 0x31  , 0x0A , 0x7E , 0x0A , 0x39 , 0x46 , 0x34 , 0x34 , 0x03
                
                return true;
            }
            catch (Exception ex)
            {
                LogService.WriteError("RecloserADVC45_sendConnectCommand", ex.ToString());
                return false;
            }

            finally
            {
                timerBetweenEachRequest.Enabled = true;
                timerBetweenEachRequest.Start();
                timerBetweenEachPoll.Enabled = false;
                timerBetweenEachPoll.Stop();
                _iRequest = 0;
            }



        }

        /***   copy from RecloserADVC45 */
    }
}
