using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace FA_Accounting.Common
{
    public static class ConversionUtils
    {
        private static DateTime _DefaultSQLDate = new DateTime(1900, 1, 1);
        private static DateTime _MinSQLDate = new DateTime(1900, 1, 1);
        private static DateTime _MaxSQLDate = new DateTime(2999, 12, 31);

        public static DateTime DefaultSQLDate
        {
            get
            {
                return _DefaultSQLDate;
            }
        }
        public static DateTime MinSQLDate
        {
            get
            {
                return _MinSQLDate;
            }
        }
        public static DateTime MaxSQLDate
        {
            get
            {
                return _MaxSQLDate;
            }
        }

        public static object ConvertDBNulls(object value)
        {
            if (value == DBNull.Value)
                return null;
            else
                return value;
        }
        public static object ConvertTextToDateTime(string value)
        {
            return ConvertTextToDateTime(value, false);
        }
        public static object ConvertTextToDateTime(string value, bool DBnullType)
        {
            try
            {
                return Convert.ToDateTime(value);
            }
            catch
            {
                if (DBnullType)
                    return DBNull.Value;
                else
                    return null;
            }
        }
        public static object ConvertTextToTime(string value)
        {
            return ConvertTextToTime(value, false);
        }
        public static object ConvertTextToTime(string value, bool DBnullType)
        {
            try
            {
                return GeneralUtils.NormalizeTime(Convert.ToDateTime(value));
            }
            catch
            {
                if (DBnullType)
                    return DBNull.Value;
                else
                    return null;
            }
        }
        public static string ConvertObjToCRStringDateTime(object aObj)
        {
            if ((aObj == null) || (aObj == DBNull.Value))
            {
                return "";
            }
            else
            {
                return "#" + Convert.ToDateTime(aObj).ToString("MM/dd/yyyy") + "#";
            }
        }
        public static string ConvertObjToSQLStringDateTime(object aObj)
        {
            return ConvertObjToSQLStringDateTime(aObj, false);
        }
        public static string ConvertObjToSQLStringDateTime(object aObj, bool aIncludeTime)
        {
            try
            {
                if ((aObj == null) || (aObj == DBNull.Value))
                    return "NULL";
                else
                {
                    string FormatString = "MM/dd/yyyy";
                    if (aIncludeTime)
                        FormatString += " HH:mm";
                    return "'" + Convert.ToDateTime(aObj).ToString(FormatString) + "'";
                }
            }
            catch
            {
                return "NULL";
            }
        }
        public static string ConvertObjToRFStringDateTime(object aObj)
        {
            return ConvertObjToRFStringDateTime(aObj, false);
        }
        public static string ConvertObjToRFStringDateTime(object aObj, bool aIncludeTime)
        {
            try
            {
                if (aObj == null)
                    return "NULL";
                else
                {
                    string FormatString = "MM/dd/yyyy";
                    if (aIncludeTime)
                        FormatString += " HH:mm";
                    return "#" + Convert.ToDateTime(aObj).ToString(FormatString) + "#";
                }
            }
            catch
            {
                return "NULL";
            }
        }
        public static string ConvertDateTimeToExptendedString(object aDate, bool aShort)
        {
            try
            {
                string FormatString;
                if (aShort)
                    FormatString = "dddd, dd MMM";
                else
                    FormatString = "dddd, dd MMMM yyyy";

                return Convert.ToDateTime(aDate).ToString(FormatString);
            }
            catch
            {
                return "";
            }
        }
        
        public static string ConvertDateTimeToString(object aDate, string aFormat)
        {
            try
            {
                if (aDate == null)
                    return "";
                else
                {
                    return Convert.ToDateTime(aDate).ToString(aFormat).ToString();
                }
            }
            catch
            {
                return "";
            }
        }
        public static string ConvertTimeToString(object Date, string aFormat)
        {
            try
            {
                if (Date == null)
                    return "";
                else
                    return Convert.ToDateTime(Date).ToString(aFormat);
            }
            catch
            {
                return "";
            }
        }

        public static string ConvertColorToString(Color aColor)
        {
            if (aColor.IsNamedColor)
                return aColor.Name;
            else
                return aColor.R.ToString() + "," + aColor.G.ToString() + "," + aColor.B.ToString();
        }
        public static Color ConvertStringToColor(string aColor)
        {
            if (aColor.Contains(","))
            {
                string[] sRGB = aColor.Split(new Char[] { ',' });
                return Color.FromArgb(Convert.ToInt16(sRGB[0]), Convert.ToInt16(sRGB[1]), Convert.ToInt16(sRGB[2]));
            }
            else
            {
                return Color.FromName(aColor);
            }
        }
        public static string ConvertFontToString(Font aFont)
        {
            return aFont.Name + "|" + ((int)aFont.SizeInPoints).ToString() + "|" + aFont.Style.ToString();
        }
        public static Font ConvertStringToFont(string aFont)
        {
            string[] Props = aFont.Split(new char[] { '|' });

            FontStyle newStyle = FontStyle.Regular;

            if (Props[2].Contains("Bold"))
                newStyle = newStyle | FontStyle.Bold;
            if (Props[2].Contains("Italic"))
                newStyle = newStyle | FontStyle.Italic;
            if (Props[2].Contains("Strikeout"))
                newStyle = newStyle | FontStyle.Strikeout;
            if (Props[2].Contains("Underline"))
                newStyle = newStyle | FontStyle.Underline;

            return new Font(Props[0], Convert.ToInt32(Props[1]), newStyle);
        }
        public static string ConvertStringToSQLString(string aString)
        {
            return "'" + aString.Replace("'", "''") + "'";
        }
        public static string ConvertSQLStringToString(string aString)
        {
            return aString.Replace("''", "'");
        }
        public static int ConvertDBobjectToInt32(object aobjInp)
        {
            if ((aobjInp == null) || (aobjInp == DBNull.Value))
                return -1;
            else
                return Convert.ToInt32(aobjInp);
        }
        public static int ConvertBoolToBit(bool aValue)
        {
            if (aValue)
                return 1;
            else
                return 0;
        }
        public static short ConvertToKy(DateTime dt)
        {
            return (short)((dt.Year - 2004) * 12 + dt.Month);
        }
        /// <summary>
        /// Ra đầu tháng
        /// </summary>
        /// <param name="ky"></param>
        /// <returns></returns>
        public static DateTime ConvertToDateFromKy(short ky)
        {
            int year;
            year = ky / 12 + 2004;
            year = ky % 12 == 0 ? year - 1 : year;
            int month = ky % 12 == 0 ? 12 : ky % 12; 
            
            return new DateTime(year, month, 1);
        }
        public static DateTime ConvertToDateFromKy(short ky,bool cuoithang)
        {
            int year;
            year = ky / 12 + 2004;
            year = ky % 12 == 0 ? year - 1 : year;
            int month = ky % 12 == 0 ? 12 : ky % 12;
            
            return new DateTime(year, month, DateTime.DaysInMonth(year, month));
        }
        /// <summary>
        /// Không chuyển phần thập phân, chữ sỗ không bao có "," "." cho đơn vị nghìn
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string ChuyenSo(string number)
        {
            //string[] strTachPhanSauDauPhay;
            //if (number.Contains('.') || number.Contains(','))
            //{
            //    strTachPhanSauDauPhay = number.Split(',', '.');
            //    return (ChuyenSo(strTachPhanSauDauPhay[0]) + " " + ChuyenSo(strTachPhanSauDauPhay[1]));
            //}

            string[] dv = { "", "mươi", "trăm", "nghìn", "triệu", "tỉ" };
            string[] cs = { "không", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };
            string doc = string.Empty;
            int i, j, k, n, len, found, ddv, rd;

            len = number.Length;
            number += "ss";
            doc = "";
            found = 0;
            ddv = 0;
            rd = 0;

            i = 0;
            while (i < len)
            {
                //So chu so o hang dang duyet
                n = (len - i + 2) % 3 + 1;

                //Kiem tra so 0
                found = 0;
                for (j = 0; j < n; j++)
                {
                    if (number[i + j] != '0')
                    {
                        found = 1;
                        break;
                    }
                }

                //Duyet n chu so
                if (found == 1)
                {
                    rd = 1;
                    for (j = 0; j < n; j++)
                    {
                        ddv = 1;
                        switch (number[i + j])
                        {
                            case '0':
                                if (n - j == 3) doc += cs[0] + " ";
                                if (n - j == 2)
                                {
                                    if (number[i + j + 1] != '0') doc += "linh ";
                                    ddv = 0;
                                }
                                break;
                            case '1':
                                if (n - j == 3) doc += cs[1] + " ";
                                if (n - j == 2)
                                {
                                    doc += "mười ";
                                    ddv = 0;
                                }
                                if (n - j == 1)
                                {
                                    if (i + j == 0) k = 0;
                                    else k = i + j - 1;

                                    if (number[k] != '1' && number[k] != '0')
                                        doc += "mốt ";
                                    else
                                        doc += cs[1] + " ";
                                }
                                break;
                            case '5':
                                if ((i + j == len - 1) || (i + j + 3 == len - 1))
                                    doc += "lăm ";
                                else
                                    doc += cs[5] + " ";
                                break;
                            default:
                                doc += cs[(int)number[i + j] - 48] + " ";
                                break;
                        }

                        //Doc don vi nho
                        if (ddv == 1)
                        {
                            doc += ((n - j) != 1) ? dv[n - j - 1] + " " : dv[n - j - 1];
                        }
                    }
                }


                //Doc don vi lon
                if (len - i - n > 0)
                {
                    if ((len - i - n) % 9 == 0)
                    {
                        if (rd == 1)
                            for (k = 0; k < (len - i - n) / 9; k++)
                                doc += "tỉ ";
                        rd = 0;
                    }
                    else
                        if (found != 0) doc += dv[((len - i - n + 1) % 9) / 3 + 2] + " ";
                }

                i += n;
            }

            if (len == 1)
                if (number[0] == '0' || number[0] == '5') return cs[(int)number[0] - 48];
            string firstchar = doc.Substring(0, 1);
            doc = firstchar.ToUpper() + doc.Substring(1) + "đồng chẵn.";
            return doc ;
        }
        public static DateTime ConvertToDateTime(double excelDate)
        {
            if (excelDate < 1)
            {
                throw new ArgumentException("Excel dates cannot be smaller than 0.");
            }
            DateTime dateOfReference = new DateTime(1900, 1, 1);
            if (excelDate > 60d)
            {
                excelDate = excelDate - 2;
            }
            else
            {
                excelDate = excelDate - 1;
            }
            return dateOfReference.AddDays(excelDate);
        }
    }
    
}
