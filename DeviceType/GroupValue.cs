using System;

namespace RecloserAcq.Device
{
    class GroupValue : ParsableObject
    {
        public double _I1 { set; get; }
        public double _3I2 { set; get; }
        public double _3I0 { set; get; }
        public double _V1 { set; get; }
        public double _V2 { set; get; }
        public double _3V0 { set; get; }

        public GroupValue()
        {

        }

        public GroupValue(String data)
            : base(data)
        {
            _I1 = double.Parse(dataList[0]);
            _3I2 = double.Parse(dataList[1]);
            _3I0 = double.Parse(dataList[2]);
            _V1 = double.Parse(dataList[3]);
            _V2 = double.Parse(dataList[4]);
            _3V0 = double.Parse(dataList[5]);
        }
    }
}
