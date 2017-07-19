using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TcpComm
{
    //public class DataParser
    //{
    //    private static object _syncRoot = new object();
    //    private RingBuffer _ringBuffer;
    //    public Func<RingBuffer, byte[]> ParseCooperFxb { get; set; }
    //    public Func<RingBuffer, byte[]> ParseNulec { get; set; }

    //    private DataParser()
    //    {
    //        _ringBuffer = new RingBuffer(1024 * 4);
    //    }

    //    //#region Singleton
    //    //private DataParser()
    //    //{
    //    //    //_buffer = new Queue<byte>();
    //    //    _ringBuffer = new RingBuffer(1024 * 4);
    //    //}

    //    //private static object _syncRoot = new object();
    //    //private static DataParser _instance;
    //    //public static DataParser Instance
    //    //{
    //    //    get
    //    //    {
    //    //        lock (_syncRoot)
    //    //        {
    //    //            if (_instance == null)
    //    //            {
    //    //                _instance = new DataParser();
    //    //            }
    //    //        }

    //    //        return _instance;
    //    //    }
    //    //}
    //    //#endregion

    //    public void Add(byte[] data)
    //    {
    //        _ringBuffer.Write(data, 0, data.Length);

    //        lock (_syncRoot)
    //        {
    //            byte[] buff;

    //            if (ParseCooperFxb != null)
    //            {
    //                // Parse Cooper FXB
    //                buff = ParseCooperFxb(_ringBuffer);
    //                while (buff != null)
    //                {
    //                    // Raise received CooperFxb data

    //                    // continue
    //                    buff = ParseCooperFxb(_ringBuffer);
    //                }
    //            }

    //            if (ParseNulec != null)
    //            {
    //                // Parse Nulec
    //                buff = ParseNulec(_ringBuffer);
    //                while (buff != null)
    //                {
    //                    // Raise received Nulec data

    //                    // continue
    //                    buff = ParseNulec(_ringBuffer);
    //                }
    //            }
    //        }
    //    }
    //    // end
    //}
}
