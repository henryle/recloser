using System;

namespace RecloserAcq.Device
{
    public class VoltsValue : ValueObject
    {
        public double S { set; get; }

        public VoltsValue()
        {

        }

        public VoltsValue(String data)
            : base(data)
        {
            S = double.Parse(dataList[3]);
        }
    }
}
