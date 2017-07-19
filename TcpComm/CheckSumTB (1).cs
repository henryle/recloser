using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TcpComm
{
    public static class CheckSumTB
    {
        /*****************************************************************************
         ** Function name:		crc16_update
         **
         ** Descriptions:		Calc crc16 with predefine CRC16_POLY 
         ** parameters:		crc : the Crc before
         **			a : current data input to calculate CRC
         ** Returned value:		current CRC
         **
         *****************************************************************************/
        //#define CRC16_POLY 		0xA001 //Polynomial: x^16 + x^15 + x^2 + 1 (0xa001)
        //uint16_t   crc16_update(uint16_t crc, uint8_t a)
        //{
        // int i;

        //        crc ^= a;
        //        for (i = 0; i < 8; ++i)
        //        {
        //            if (crc & 1)
        //                crc = (crc >> 1) ^ CRC16_POLY;
        //            else
        //                crc = (crc >> 1);
        //        }

        //        return crc;
        //}
        const ushort CRC16_POLY = 0xA001;
        //#define CRC16_POLY

        private static ushort crc16_update(ushort crc, byte a)
        {
            int i;

            crc ^= a;
            for (i = 0; i < 8; ++i)
            {
                if ((crc & 1) != 0)
                    //crc = (crc >> 1) ^ Defines.CRC16_POLY;
                    crc = (ushort)((crc >> 1) ^ CRC16_POLY);
                else
                    crc = (ushort)(crc >> 1);
            }

            return crc;
        }

        //internal static partial class Defines
        //{
        //    public const int CRC16_POLY = 0xA001;
        //}
        /*****************************************************************************
         ** Function name:		crc16_update
         **
         ** Descriptions:		Calc crc16 with predefine CRC16_POLY 
         ** parameters:		crc : the Crc before
         **			a : current data input to calculate CRC
         ** Returned value:		current CRC
         **
         *****************************************************************************/
        //uint16_t crc16_check( uint8_t *InputData,uint8_t bufersize)
        //{
        //        uint16_t crc = 0xffff, i;
        //                crc = crc16_update(crc,0x1B);
        //                crc = crc16_update(crc,0x32);
        //                crc = crc16_update(crc,0x01);
        //                crc = crc16_update(crc,0x2A);
        //                crc = crc16_update(crc,0x0A);
        //                crc = crc16_update(crc,bufersize);//khong tinh 2 byte crc va end footer

        //     for (i = 0; i < bufersize-4; i++)
        //    {
        //                crc = crc16_update(crc, InputData[i]);
        //    }

        //        return crc; 
        //}
        private static ushort crc16_check(byte[] InputData, byte bufersize)
        {
            ushort crc = 0xffff;
            ushort i;
            crc = crc16_update(crc, 0x1B);
            crc = crc16_update(crc, 0x32);
            crc = crc16_update(crc, 0x01);
            crc = crc16_update(crc, 0x2A);
            crc = crc16_update(crc, 0x0A);
            crc = crc16_update(crc, bufersize); //khong tinh 2 byte crc va end footer

            for (i = 0; i < bufersize - 4; i++)
            {
                crc = crc16_update(crc, InputData[i]);
            }

            return crc;
        }
        // Bao tessted
        private static UInt16 crc16_updateBAO(int crc, byte a)
        {
            int i;
            int t = 1;
            crc ^= a;
            for (i = 0; i < 8; ++i)
            {
                t = crc & 1;
                if (t != 0)
                    crc = (crc >> 1) ^ 40961;
                else
                    crc = (crc >> 1);
            }

            return (UInt16)crc;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="InputData"></param>
        /// <param name="bufersize">size of InputData + 4 (1 byte len + 2 byte crc + 1 byte end footer)</param>
        /// <returns></returns>
        public static UInt16 crc16_checkBAO(byte[] InputData, byte bufersize)
        {
            UInt16 crc = 0xffff, i;
            crc = crc16_updateBAO(crc, 0x1B);//00011011
            crc = crc16_updateBAO(crc, 0x32);//00110010
            crc = crc16_updateBAO(crc, 0x01);
            crc = crc16_updateBAO(crc, 0x2A);
            crc = crc16_updateBAO(crc, 0x0A);
            crc = crc16_updateBAO(crc, bufersize);//khongtinh 2 byte crcva end footer

            for (i = 0; i < bufersize - 4; i++)
            {
                crc = crc16_updateBAO(crc, InputData[i]);
            }

            return crc;
        }
        public static UInt16 crc16_checkReceiveBAO(byte[] InputData, byte bufersize)
        {
            UInt16 crc = 0xffff, i; //01 2A 0A 11
            //bufersize = sizeof<byte>(InputData) 
            // input data = from start to end data byte (before crc)
            //crc = crc16_updateBAO(crc, (byte)(bufersize + 3) /*2 crc + 1 end */);

            for (i = 0; i < bufersize ; i++)
            {
                crc = crc16_updateBAO(crc, InputData[i]);
            }

            return crc;
        }
    }
}
