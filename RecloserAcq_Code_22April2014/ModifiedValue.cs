using System;

namespace RecloserAcq.Device
{
    class ModifiedValue : ValueObject
    {
        public double _3P { set; get; }

        public ModifiedValue()
        {

        }

        public ModifiedValue(String data)
            : base(data)
        {
            _3P = double.Parse(dataList[3]);
        }
    }
}
