using System;

namespace RecloserAcq.Device
{
    public class AmpleValue : ValueObject
    {
        public double N { set; get; }
        public double G { set; get; }

        public AmpleValue()
        {

        }

        public AmpleValue(String data)
            : base(data)
        {
            N = double.Parse(dataList[3]);
            G = double.Parse(dataList[4]);
        }
    }
}
