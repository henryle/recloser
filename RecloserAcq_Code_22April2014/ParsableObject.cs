using System;

namespace RecloserAcq.Device
{
    public class ParsableObject
    {
        public String[] dataList { set; get; }

        private String[] headeres =
            {
                "FEEDER",
                "I MAG (A)",
                "I ANG (DEG)",
                "V MAG (KV)",
                "V ANG (DEG)",
                "MW",
                "MVAR",
                "PF",
                //"MAG",
                //"ANG   (DEG)",
                //"FREQ (Hz)"
            };

        protected String toStand(String data)
        {
            while (data.IndexOf("  ") >= 0)
            {
                data = data.Replace("  ", " ");
            }

            if (data.IndexOf(" ") == 0)
            {
                data = data.Substring(1);
            }

            return data;
        }

        protected virtual String[] parse(String data)
        {
            foreach (var header in headeres)
            {
                data = data.Replace(header, "");
            }

            data = toStand(data);

            return data.Split(' ');
        }

        public ParsableObject()
        {

        }

        public ParsableObject(String data)
        {
            dataList = parse(data);
        }
    }
}
