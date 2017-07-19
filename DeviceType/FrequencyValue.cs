using System;

namespace RecloserAcq.Device
{
    class FrequencyValue : ParsableObject
    {
        public double value { set; get; }

        public FrequencyValue()
        {

        }

        public FrequencyValue(String data)
            : base(data)
        {
            value = double.Parse(dataList[0]);
        }
    }
}
