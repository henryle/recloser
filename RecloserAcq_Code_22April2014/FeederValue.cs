using System;

namespace RecloserAcq.Device
{
    class FeederValue : ParsableObject
    {
        public double value { set; get; }

        public FeederValue()
        {

        }

        public FeederValue(String data)
            : base(data)
        {
            value = double.Parse(dataList[0]);
        }
    }
}
