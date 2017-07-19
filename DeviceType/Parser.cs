using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TcpComm;

namespace RecloserAcq.Device
{
    public static class Parser
    {
        public static byte[] ParseCooperFxb(RingBuffer ringBuffer)
        {
            int start = ringBuffer.IndexOf(0);
            if (start != -1 && ringBuffer.Count > start + 1)
            {
                var len = ringBuffer[start + 1];
                var end = start + len;

                if (end < ringBuffer.Count)
                {
                    int checksum = 0;
                    for (int i = start; i <= end; i++)
                    {
                        checksum += ringBuffer[i];
                    }

                    if (checksum == 0 || (checksum % 0x100) == 0)
                    {
                        // Found
                        var buff = new byte[len + 1];
                        
                        // Move the tail index to start read byte
                        var unusedBytes = new byte[start];
                        ringBuffer.Read(unusedBytes, 0, start);

                        ringBuffer.Read(buff, 0, len + 1);
                        return buff;
                    }
                }
            }

            return null;
        }

        public static readonly byte Nulec_Answer_Start = 0x01;
        public static readonly byte Nulec_End = 0x03;
        public static readonly byte[] Nulec_StartBlock = new byte[] { 0x2A, 0x0A };
        public static readonly byte[] Nulec_EndBlock = new byte[] { 0x0A, 0x7E, 0x0A };
        //Answer  từ ĐK: 01 2A 0A        
        public static readonly byte TuBu_Answer_Start = 0x01;
        
        public static readonly byte[] TuBu_StartBlock = new byte[] { 0x2A, 0x0A };
        public static readonly byte[] LBS_Answer_Header = new byte[] { 0x01, 0x2A, 0x0A };
        public static readonly byte[] Recloser351_EndBlock1 = new byte[] { 0x3D, 0x3E, 0x3E }; // for value return not for authentication
        public static readonly byte[] Recloser351_EndBlock2 = new byte[] { 0x0D, 0x0A, 0x3D };
        public static readonly byte[] Recloser351R_Answer_Start1 = new byte[] {0x44, 0x41, 0x54 }; //DAT
        public static readonly byte[] Recloser351R_Answer_Start2 = new byte[] { 0x54, 0x49, 0x4d };//TIM
        public static readonly byte[] Recloser351R_Answer_Start3 = new byte[] { 0x53, 0x45, 0x52 };//SER
        public static readonly byte[] Recloser351R_Answer_Start4 = new byte[] {0x4D ,0x45 ,0x54 };//MET
        public static readonly byte[] Recloser351R_Answer_Start5 = new byte[] {0x2a,0x2,0x2a };//***
        public static readonly byte[] Recloser351R_Answer_EndBlock = new byte[] { 0x3D,0x3E,0x3E };
        //Elster1700
        //Frame start character (STX, start of text code 02H) indicating where the calculation of BCC
        //shall start from. This character is not required if there is no data set to follow.
        //6)  End character in the block (ETX, end of text, code 03H).
        //7)  End character in a partial block (EOT, end of text block, code 04H).
        //8)  Block check character (BCC), if required, in accordance with the characters 5) and 6).
        //Items 5) and 6) do not apply when the data block is transmitted without check characters.
        public static readonly byte Elster1700_Answer_Start = 0x01; //SOH
        public static readonly byte Elster1700_Answer_Start1 = 0x02; // STX
        public static readonly byte Elster1700_Answer_NAK = 0x15; // =(int)21 NAK return 1 byte eg. when access denied
        public static readonly byte Elster1700_Answer_ACK = 0x06; //ACK
        //public static readonly byte Elster1700_Answer_Endx03 = 0x03;
        //public static readonly byte Elster1700_Answer_End_close_quotation = 0x29; //)
        public static readonly byte[] Elster1700_Answer_End_closeX03 = new byte[] {0x29,0x03}; //)
        public static readonly byte[] Elster1700_Answer_End_0X03 = new byte[] { 0x30, 0x03 }; //)
        //Completion character (CR, carriage return, code 0DH; LF, line feed, code 0AH).
        public static readonly byte[] Elster1700_Answer_EndLineEnd = new byte[] {0x0D,0x0A}; 
        //("\x15", "<NAK>")("\x06", "<ACK>")("\x01", "<SOH>")("\x02", "<STX>")("\x03", "<ETX>")("\x0D", "<CR>")("\x0A", "<LF>");

        public static readonly byte TuBu_End = 0x03;
        public static readonly byte LBS_End = 0x03;
        
        public static byte[] ParseNulec(RingBuffer ringBuffer)
        {
            int start = ringBuffer.IndexOf(Nulec_Answer_Start);
            if (start != -1 && ringBuffer.Count > start + 1)
            {
                int end = ringBuffer.IndexOf(Nulec_End, start + 1);
                if (end != -1 && end - start > 5)
                {
                    if (ringBuffer[start + 1] == 0x2A && ringBuffer[start + 2] == 0x0A)
                    {
                        // Found
                        var len = end - start + 1;

                        // Move the tail index to start read byte
                        var unusedBytes = new byte[start];
                        ringBuffer.Read(unusedBytes, 0, start);

                        var buff = new byte[len];
                        //ringBuffer.Read(buff, start, len);
                        ringBuffer.Read(buff, 0, len);
                        return buff;
                    }
                }
            }

            return null;
        }
        public static byte[] ParseRecloserADVC(RingBuffer ringBuffer)
        {
            int start = ringBuffer.IndexOf(Nulec_Answer_Start);
            if (start != -1 && ringBuffer.Count > start + 1)
            {
                
                int end = ringBuffer.IndexOf(Nulec_End, start + 1);
                if (end != -1 && end - start > 5)
                {
                    if (ringBuffer[start + 1] == 0x2A && ringBuffer[start + 2] == 0x0A)
                    {
                        // Found
                        var len = end - start + 1;

                        // Move the tail index to start read byte
                        var unusedBytes = new byte[start];
                        ringBuffer.Read(unusedBytes, 0, start);

                        var buff = new byte[len];
                        //ringBuffer.Read(buff, start, len);
                        ringBuffer.Read(buff, 0, len);
                        return buff;
                    }
                }
            }

            return null;
        }
        public static byte[] ParseRecloserVP(RingBuffer ringBuffer)
        {
            int start = ringBuffer.IndexOf(Nulec_Answer_Start);
            if (start != -1 && ringBuffer.Count > start + 1)
            {
                int end = ringBuffer.IndexOf(Nulec_End, start + 1);
                if (end != -1 && end - start > 5)
                {
                    if (ringBuffer[start + 1] == 0x2A && ringBuffer[start + 2] == 0x0A)
                    {
                        // Found
                        var len = end - start + 1;

                        // Move the tail index to start read byte
                        var unusedBytes = new byte[start];
                        ringBuffer.Read(unusedBytes, 0, start);

                        var buff = new byte[len];
                        //ringBuffer.Read(buff, start, len);
                        ringBuffer.Read(buff, 0, len);
                        return buff;
                    }
                }
            }

            return null;
        }

        public static byte[] ParseTuBu(RingBuffer ringBuffer)
        {
            int start = ringBuffer.IndexOf(TuBu_Answer_Start);
            if (start != -1 && ringBuffer.Count > start + 1)
            {
                if (ringBuffer[start + 1] == 0x2A && ringBuffer.Count > start + 3 && ringBuffer[start + 2] == 0x0A)
                {
                    
                    int end = ringBuffer.IndexOf(TuBu_End);
                    int len = ringBuffer[start + 3] -1;
                        //&& end - start - 2 == ringBuffer[start+3] 
                    if (end != -1 && ringBuffer.Count >= len)
                    {
                        start += 4;
                        // Found
                        //len = end - start + 1; //len = nByteSend

                        // Move the tail index to start read byte
                        var unusedBytes = new byte[start];
                        ringBuffer.Read(unusedBytes, 0, start);

                        var buff = new byte[len];
                        //ringBuffer.Read(buff, start, len);
                        ringBuffer.Read(buff, 0, len);
                        return buff;
                    }
                }
            }

            return null;
        }
        public static byte[] ParseLBS(RingBuffer ringBuffer)
        {
            int start = ringBuffer.IndexOf(LBS_Answer_Header,3);
            if (start != -1)
            {
                

                    int end = ringBuffer.IndexOf(LBS_End);
                    int len = ringBuffer[start + 3] - 1;
                    //&& end - start - 2 == ringBuffer[start+3] 
                    if (end != -1 && ringBuffer.Count >= len)
                    {
                        start += 4;
                        // Found
                        //len = end - start + 1; //len = nByteSend

                        // Move the tail index to start read byte
                        var unusedBytes = new byte[start];
                        ringBuffer.Read(unusedBytes, 0, start);

                        var buff = new byte[len];
                        //ringBuffer.Read(buff, start, len);
                        ringBuffer.Read(buff, 0, len);
                        return buff;
                    }
                
            }

            return null;
        }
        public static byte[] ParseRecloser351R(RingBuffer ringBuffer)
        {

            int start ;
            start = GetStartRecloser351R(ringBuffer);

            if (start != -1 && ringBuffer.Count > start + 1)
            {
                int end = ringBuffer.IndexOf(Recloser351_EndBlock1, 3, start + 3);
                if (end != -1 && end - start > 5)
                {

                    // Found
                    var len = end - start + 1 + 2;

                    // Move the tail index to start read byte
                    var unusedBytes = new byte[start];
                    ringBuffer.Read(unusedBytes, 0, start);

                    var buff = new byte[len];
                    //ringBuffer.Read(buff, start, len);
                    ringBuffer.Read(buff, 0, len);
                    return buff;

                }
            }
            else
            {
                start = 0;
                int end = ringBuffer.IndexOf(Recloser351_EndBlock2, 3);
                if (end != -1 )
                {

                    // Found
                    var len = end - start + 1 + 2;

                    // Move the tail index to start read byte
                    var unusedBytes = new byte[start];
                    ringBuffer.Read(unusedBytes, 0, start);

                    var buff = new byte[len];
                    //ringBuffer.Read(buff, start, len);
                    ringBuffer.Read(buff, 0, len);
                    return buff;

                }
            }

            return null;
        }

        private static int GetStartRecloser351R(RingBuffer ringBuffer)
        {
            int start;
            start = ringBuffer.IndexOf(Recloser351R_Answer_Start1, 3);
            if (start == -1)
            {
                start = ringBuffer.IndexOf(Recloser351R_Answer_Start2, 3);
            }
            if (start == -1)
            {
                start = ringBuffer.IndexOf(Recloser351R_Answer_Start3, 3);
            }
            if (start == -1)
            {
                start = ringBuffer.IndexOf(Recloser351R_Answer_Start4, 3);
            }
            if (start == -1)
            {
                start = ringBuffer.IndexOf(Recloser351R_Answer_Start5, 3);
            }
            return start;
        }
        public static byte[] ParseElster1700(RingBuffer ringBuffer)
        {
            // if deny : return one byte only 21 == 0x15
           // int start;
            //start = GetStartElster(ringBuffer); no need start
            
            //if ( start>= 0 && (start == ringBuffer.IndexOf(Elster1700_Answer_NAK) || start == ringBuffer.IndexOf(Elster1700_Answer_ACK)))
            //{
            //    var len = 1;
            //        // Move the tail index to start read byte
            //        //var unusedBytes = new byte[start];
            //        //ringBuffer.Read(unusedBytes, 0, start);
            //        var buff = new byte[len];
            //        //ringBuffer.Read(buff, start, len);
            //        ringBuffer.Read(buff, 0, len);
            //    return buff;
            //}
            int end = -1;
            int len =1;
            if (end == -1)
            {
                end = ringBuffer.IndexOf(Elster1700_Answer_EndLineEnd, 2);
                len = end + 2;
            }
            if (end == -1)
            {
                end = ringBuffer.IndexOf(Elster1700_Answer_NAK);
                len = end + 1;
            }
            if (end == -1)
            {
                end = ringBuffer.IndexOf(Elster1700_Answer_ACK);
                len = end + 1;
            }
            if (end == -1)
            {
                end = ringBuffer.IndexOf(Elster1700_Answer_End_closeX03,2);
                len = end + 2;
                
            }
            if (end == -1)
            {
                end = ringBuffer.IndexOf(Elster1700_Answer_End_0X03, 2);
                len = end + 2;
            }
            
            if (/*start>=0 && */ end >=0 && ringBuffer.Count >= 0)
            {
                 // Move the tail index to start read byte
                    //var unusedBytes = new byte[start];
                    //ringBuffer.Read(unusedBytes, 0, start);
                
                var buff = new byte[len];
                //ringBuffer.Read(buff, start, len);
                ringBuffer.Read(buff, 0, len);
                return buff;


            }

            return null;
        }
        public static byte[] ParseWebcommand(RingBuffer ringBuffer)
        {
            byte[] endbytes  =  System.Text.Encoding.UTF8.GetBytes("_END"); 
            byte[] startbytes = System.Text.Encoding.UTF8.GetBytes("START_");
            //int start = ringBuffer.IndexOf(startbytes, 6);
            int end = ringBuffer.IndexOf(endbytes,4);
            if (end > 0)
            {
                var buff = new byte[end+4];
                //ringBuffer.Read(buff, start, len);
                ringBuffer.Read(buff, 0, end+4);
                return buff;
            }
            else
            {
                return null;
            }
        }
        private static int GetStartElster(RingBuffer ringBuffer)
        {
            int start;
            start = ringBuffer.IndexOf(Elster1700_Answer_Start);
            if (start == -1)
            {
                start = ringBuffer.IndexOf(Elster1700_Answer_Start1);
            }
            if (start == -1)
            {
                start = ringBuffer.IndexOf(Elster1700_Answer_NAK);
            }
            if (start == -1)
            {
                start = ringBuffer.IndexOf(Elster1700_Answer_ACK);
            }
            
            return start;
        }
        public static byte[] DoParse(Type t, RingBuffer buffer)
        {
            if (t == typeof(RecloserADVC)|| t==typeof(RecloserADVC45) || t==typeof(RecloserUSeries)|| t==typeof(RecloserADVCTCPIP))
            {
                return Parser.ParseRecloserADVC(buffer);
            }
            else if (t == typeof(RecloserVP))
            {
                return Parser.ParseRecloserVP(buffer);
            }
            else if (t == typeof(CooperFXB))
                return Parser.ParseCooperFxb(buffer);
            else if (t == typeof(Nulec) || t == typeof(Nulec_U))
                return Parser.ParseNulec(buffer);
            else if (t == typeof(TuBu))
                return Parser.ParseTuBu(buffer);
            else if (t == typeof(LBS))
                return Parser.ParseLBS(buffer);
            else if (t == typeof(Recloser351R))
                return Parser.ParseRecloser351R(buffer);
            else if (t == typeof(Elster1700))
                return Parser.ParseElster1700(buffer);
            else if (t == typeof(webcommand))
            {
                return Parser.ParseWebcommand(buffer);
            }
            else
                return null;
        }    
    }
}
