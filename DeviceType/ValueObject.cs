using System;

namespace RecloserAcq.Device
{
    public class ValueObject : ParsableObject
    {
        public double A { set; get; }
        public double B { set; get; }
        public double C { set; get; }

        public ValueObject()
        {

        }

        public ValueObject(String data)
            : base(data)
        {
            A = double.Parse(dataList[0]);
            B = double.Parse(dataList[1]);
            C = double.Parse(dataList[2]);
        }
    }
}
