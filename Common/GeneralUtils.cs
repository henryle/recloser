using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Ionic.Zip;

namespace FA_Accounting.Common
{
    public static class GeneralUtils
    {
        [DllImport("user32.dll")]
        public static extern bool LockWindowUpdate(IntPtr hwndLock);

        [DllImport("user32.dll")]
        public static extern int FindWindow(
            string lpClassName, // class name 
            string lpWindowName // window name 
        );

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImportAttribute("user32.dll")] //SW_NORMAL = 1, SW_MAXIMIZE = 3, SW_SHOW = 5, SW_RESTORE = 9,...
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        public static string CleanupSpaces(string aValue)
        {
            while (aValue.Contains("  "))
                aValue = aValue.Replace("  ", " ");
            return aValue.Trim();
        }

        public static string GeneratePersonalFullName(string aTitle, string aSurname, string aFirstName)
        {
            return GeneralUtils.CleanupSpaces(aTitle + " " + aFirstName + " " + aSurname);
        }

         public static DialogResult ShowQuesionMessage(string aMessage, string aCaption)
        {
            //show message
            if (aCaption == "") aCaption = "Question";
            return MessageBox.Show(aMessage, aCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
        }
        
        public static void ShowInformationMessage(string aMessage, string aCaption)
        {
            //show message
            if (aCaption == "") aCaption = "Information";
            MessageBox.Show(aMessage, aCaption, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        }
        public static void ShowExclamationMessage(string aMessage, string aCaption)
        {
            //show message
            if (aCaption == "") aCaption = "Exclamation";
            MessageBox.Show(aMessage, aCaption, MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
        }

        public static DateTime NormalizeTime(DateTime aTime)
        {
            return ConversionUtils.DefaultSQLDate.AddHours(aTime.Hour).AddMinutes(aTime.Minute).AddSeconds(aTime.Second);
        }

        public static DateTime DeNormalizeTime(DateTime aTime, DateTime aDate)
        {
            return aDate.Date.AddHours(aTime.Hour).AddMinutes(aTime.Minute);
        }

        public static DateTime DayStart(DateTime aDate)
        {
            return new DateTime(aDate.Year, aDate.Month, aDate.Day, 0, 0, 0, 0);
        }

        public static DateTime DayEnd(DateTime aDate)
        {
            aDate = DayStart(aDate.AddDays(1)).AddMilliseconds(-1);
            return aDate;
        }

        public static string ConstructDoubleColor(Color aColor1, Color aColor2)
        {
            string Color1 = ConversionUtils.ConvertColorToString(aColor1);
            string Color2 = ConversionUtils.ConvertColorToString(aColor2);
            return Color1 + "|" + Color2;
        }

        public static Color GetBackgroundColor(string aColor)
        {
            if (aColor.Contains("|"))
            {
                string[] Colors = aColor.Split(new char[] { '|' });
                return ConversionUtils.ConvertStringToColor(Colors[0]);
            }
            else
                return Color.WhiteSmoke;
        }

        public static Color GetForegroundColor(string aColor)
        {
            if (aColor.Contains("|"))
            {
                string[] Colors = aColor.Split(new char[] { '|' });
                return ConversionUtils.ConvertStringToColor(Colors[1]);
            }
            else
                return Color.Black;
        }

        public static string PaddedString(char aChar, int aLength)
        {
            string retVal = "";
            for (int i = 0; i < aLength; i++)
                retVal += aChar.ToString();
            return retVal;
        }

        //public static string StringAdjustCase(string aString)
        //{
        //    return StringAdjustCase(aString, (eAutoCorrectCaseMode)Enum.Parse(typeof(eAutoCorrectCaseMode), ConfigurationControl.GlobalKeys.GetValue(ConfigurationControl.GroupNameKeyEditor, ConfigurationControl.GlobalKeys.KeyNameAutoCaseMode), true));
        //}

        public static string StringAdjustCase(string aString, eAutoCorrectCaseMode aAutoCorrectCaseMode)
        {
            if (aAutoCorrectCaseMode == eAutoCorrectCaseMode.Title)
                aString = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(aString);
            if (aAutoCorrectCaseMode == eAutoCorrectCaseMode.Upper)
                aString = CultureInfo.CurrentCulture.TextInfo.ToUpper(aString);
            if (aAutoCorrectCaseMode == eAutoCorrectCaseMode.Lower)
                aString = CultureInfo.CurrentCulture.TextInfo.ToLower(aString);
            return aString;
        }

        //public static string StringTrim(string aString)
        //{
        //    return StringTrim(aString, (eAutoTrimMode)Enum.Parse(typeof(eAutoTrimMode), ConfigurationControl.GlobalKeys.GetValue(ConfigurationControl.GroupNameKeyEditor, ConfigurationControl.GlobalKeys.KeyNameAutoTrimMode), true));
        //}

        public static string StringTrim(string aString, eAutoTrimMode aAutoTrimMode)
        {
            if (aAutoTrimMode == eAutoTrimMode.Both)
                aString = aString.Trim();
            if (aAutoTrimMode == eAutoTrimMode.Left)
                aString = aString.TrimStart();
            if (aAutoTrimMode == eAutoTrimMode.Right)
                aString = aString.TrimEnd();
            return aString;
        }

        public static string StringNumeric(string aString)
        {
            return Regex.Replace(aString, "[^0-9]", "");
        }

        public static string StringAlphaNumeric(string aString)
        {
            return Regex.Replace(aString, "[^0-9a-zA-Z]", "").ToUpper();
        }

        public static string StringAlpha(string aString)
        {
            return Regex.Replace(aString, "[^a-zA-Z]", "").ToUpper();
        }

        public static string getFileName(string aString)
        {
            return Regex.Match(aString, @"[^\\]*$").Value;
        }
        
        public static string StringDate(string aString, string aFormat)
        {
            try
            {
                DateTime dt = Convert.ToDateTime(aString);
                return ConversionUtils.ConvertDateTimeToString(dt, aFormat);
            }
            catch
            {
                return "";
            }
        }

        public static string StringTime(string aString, string aFormat)
        {
            try
            {
                DateTime dt = Convert.ToDateTime(aString);
                return ConversionUtils.ConvertTimeToString(dt, aFormat);
                
            }
            catch
            {
                return "";
            }
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
        
        public static string ChopString(IDeviceContext aDc, string aString, int aWidth, Font aFont)
        {
            if (aString.Contains("\n"))
            {
                string[] Strings = aString.Split(new char[] { '\n' });
                aString = "";
                foreach (string str in Strings)
                    aString += ChopSingleString(aDc, str, aWidth, aFont) + "\n";
            }
            else
                aString = ChopSingleString(aDc, aString, aWidth, aFont);

            return aString;
        }

        public static string ChopSingleString(IDeviceContext aDc, string aString, int aWidth, Font aFont)
        {
            Size newSize = TextRenderer.MeasureText(aDc, aString, aFont);
            if (newSize.Width > aWidth)
            {
                while (newSize.Width > aWidth)
                {
                    aString = aString.Remove(aString.Length - 1);
                    newSize = TextRenderer.MeasureText(aString + "...", aFont);
                }
                return aString + "...";
            }
            else
                return aString;
        }

        public static string String2SQL(string aString)
        {
            aString = aString.Replace("'", "''");
            aString = aString.Replace("[", "[[]");            
            return aString;
        }

        public static bool SaveBinaryToFile(string fileName, byte[] data)
        {
            try
            {
                Stream sr = File.Open(fileName, FileMode.Create);
                BinaryWriter br = new BinaryWriter(sr);
                br.Write(data, 0, data.Length);
                br.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static byte[] ReadFileData(string fileName)
        {
            Stream sr = File.Open(fileName, FileMode.Open);
            BinaryReader rd = new BinaryReader(sr);
            
            MemoryStream ms = new MemoryStream();
            int index = 0;
            byte[] data = new byte[1024];

            while (true)
            {
                long count = rd.Read(data, 0, data.Length);
                if (count == 0)
                {
                    break;
                }
                else
                {
                    index = index + (int)count;
                    ms.Write(data, 0, (int)count);
                }
            }

            return ms.ToArray();            
        }

        public static byte[] FontToBinary(Font font)
        {
            MemoryStream stream = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, font);
            return stream.ToArray();
        }

        public static Font BinaryToFont(byte[] data)
        {
            MemoryStream stream = new MemoryStream(data);
            BinaryFormatter formatter = new BinaryFormatter();
            var font = formatter.Deserialize(stream) as Font;
            return font;
        }

        static public string removeSpecialCharacters(string orig)
        {
            string rv;

            // replacing with space allows the camelcase to work a little better in most cases.
            rv = orig.Replace("\\", "");
            rv = rv.Replace("(", "");
            rv = rv.Replace(")", "");
            rv = rv.Replace("/", "");
            rv = rv.Replace("-", "");
            rv = rv.Replace(",", "");
            rv = rv.Replace(">", "");
            rv = rv.Replace("<", "");
            rv = rv.Replace("-", "");
            rv = rv.Replace("&", "");
            rv = rv.Replace("?", "");
            rv = rv.Replace(":", "");
            rv = rv.Replace(@"""", "");
            rv = rv.Replace("*", "");
            rv = rv.Replace("<", "");
            rv = rv.Replace(">", "");
            rv = rv.Replace("|", "");

            // single quotes shouldn't result in CamelCase variables like Patient's -> PatientS
            // "smart" forward quote
            rv = rv.Replace("'", "");

             rv = rv.Replace("\u2019", ""); // smart forward (possessive) quote.

            // make sure to get rid of double spaces.
            rv = rv.Replace("   ", " ");
            rv = rv.Replace("  ", " ");

            rv = rv.Trim(' '); // Remove leading and trailing spaces.

            return (rv);
        }

        public static Image LoadImageFromFile(string path)
        {
            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                return Image.FromStream(fs);
            }
        }

        public static int GetNumber(string str)
        {
            int num = 0;
            int i = 0;
            for (i = 0; i < str.Length; i++)
            {
                var a = str.Substring(i, 1);
                if (a.CompareTo("0") < 0 || a.CompareTo("9") > 0)
                {
                    break;
                }
            }
            if (i > 0)
                num = int.Parse(str.Substring(0, i));
            return num;
        }

        public static double GetDouble(string str)
        {
            double num = 0;
            int i = 0;
            var temp = str;
            for (i = 0; i < str.Length; i++)
            {
                var a = str.Substring(i, 1);
                if ((a.CompareTo("0") < 0 || a.CompareTo("9") > 0) && a != "*" && a != "." && a != ",")
                {
                    temp = temp.Replace(a, "");
                }
            }

            num = GetDoubleSub(temp);
            return num;
        }

        public static double GetDoubleSub(string str)
        {
            try
            {
                double num = 0;
                var i = str.IndexOf("*");
                if (i > 0)
                    num = Convert.ToDouble(str.Substring(0, i)) * GetDoubleSub(str.Substring(i + 1));
                else
                    num = Convert.ToDouble(str);
                return num;
            }
            catch 
            {
                return 0;
            }
        }

        public static Image ResizeImage(Image imgToResize, Size size, System.Drawing.Drawing2D.InterpolationMode mode = System.Drawing.Drawing2D.InterpolationMode.High)
        {
            int sourceWidth = imgToResize.Width;
            int sourceHeight = imgToResize.Height;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = ((float)size.Width / (float)sourceWidth);
            nPercentH = ((float)size.Height / (float)sourceHeight);

            if (nPercentH < nPercentW)
                nPercent = nPercentH;
            else
                nPercent = nPercentW;

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap b = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage((Image)b);
            g.InterpolationMode = mode;
            g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
            g.DrawImage(imgToResize, new Rectangle(0, 0, destWidth, destHeight), new Rectangle(0, 0, sourceWidth, sourceHeight), GraphicsUnit.Pixel);
            //g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            g.Dispose();

            return (Image)b;
        }

        public static string StringEnsureLength(string aString, int maxLength)
        {
            if (aString != null && aString.Length > maxLength)
                aString = aString.Substring(0, maxLength);
            return aString;
        }

        public static bool MoveFile(string srcFile, string destPath)
        {
            try
            {
                //move file to destPath
                if (!Directory.Exists(destPath)) Directory.CreateDirectory(destPath);
                string filename = srcFile.Substring(srcFile.LastIndexOf('\\') + 1);
                string destinationfile = Path.Combine(destPath, filename);

                if (File.Exists(destinationfile)) File.Delete(destinationfile);
                File.Move(srcFile, destinationfile);

                return true;
            }
            catch (Exception ex)
            {
                LogService.WriteError("MoveFile", srcFile, ex);
                return false;
            }
        }
        public static string GetZipFilePath(string srcFile)
        {
            //Path.GetFileNameWithoutExtension(srcFile);
            FileInfo fi = new FileInfo(srcFile);
            string destPath = Path.Combine(GlobalVariables.LocalUploadPath,fi.Directory.Name);
            if (!Directory.Exists(destPath)) Directory.CreateDirectory(destPath);
            //Path.GetDirectoryName
            string filefullname;
            string filename = Path.GetFileName(srcFile);
            
            filefullname = Path.Combine(destPath, filename + ".zip");

            return filefullname; //Path.Combine(destPath, Path.GetFileName(filefullname));// fileName.Substring(fileName.LastIndexOf('\\') + 1));
            
        }
        public static string ArchiveFile(string srcFile)
        {
            string fileReturn = string.Empty;
            string destFullName = GetZipFilePath(srcFile);
            ZipFile zip = null;
            try
            {
                string folderdestpath = destFullName.Substring(0, destFullName.LastIndexOf('\\'));
                if (!Directory.Exists(folderdestpath)) Directory.CreateDirectory(folderdestpath);
                if (!Directory.Exists(folderdestpath))
                    return string.Empty;
                if (!File.Exists(srcFile))
                {
                    return "Khongtimthayfile";
                }
                //string fileName = srcFile.Replace(GlobalVariables.UploadFileExt, ".zip");
            
                //string fileName = srcFile.Substring(0, srcFile.LastIndexOf('.')) + ".zip";
                //fileReturn = GetZipFilePath(srcFile, destPath);//Path.Combine(destPath, fileName.Substring(fileName.LastIndexOf('\\') + 1));

                zip = new ZipFile();
            
                
                zip.Password = GlobalVariables.ZIPPassword;
                zip.AddFile(srcFile, "");
                if (File.Exists(destFullName))
                    File.Delete(destFullName);
                zip.Save(destFullName);
                zip.Dispose();
            }
            catch (Exception ex)
            {
                LogService.WriteError("ArchiveFile", srcFile, ex);
                //return string.Empty;
                return "Khongzipduocfile";
            }
            finally
            {
                if(zip!=null)
                    zip.Dispose();
            }
            return destFullName;
        }

        public static string ExtractFile(string fullPathFile)
        {
            // extract at the same folder
            string fileReturn = string.Empty;
            string fileName = fullPathFile.Substring(fullPathFile.LastIndexOf("\\") + 1);
            string destFilePath = fullPathFile.Substring(0, fullPathFile.LastIndexOf("\\"));

            //string destFilePath = Path.Combine(filePath, fileName);
            if (File.Exists(fullPathFile))
            {
                if (fullPathFile.EndsWith(".zip", StringComparison.OrdinalIgnoreCase))
                {
                    ZipFile zip = ZipFile.Read(fullPathFile);
                    try
                    {
                        zip.Password = GlobalVariables.ZIPPassword;                       
                        zip[0].Extract(destFilePath, ExtractExistingFileAction.OverwriteSilently);
                        fileReturn = Path.Combine(destFilePath, zip[0].FileName);
                        File.Delete(fullPathFile);
                    }
                    catch
                    {
                        try
                        {
                            zip.Password = GlobalVariables.ZIPPassword; 
                            zip[0].Extract(destFilePath, ExtractExistingFileAction.OverwriteSilently);
                            fileReturn = Path.Combine(destFilePath, zip[0].FileName);
                            File.Delete(fullPathFile);
                        }
                        catch
                        {
                            fileReturn = Path.Combine(destFilePath, zip[0].FileName);
                        }
                    }
                    finally
                    {
                        zip.Dispose();
                    }
                }
                else 
                {
                    fileReturn = fullPathFile;
                }
            }
            return fileReturn;
        }
    }
}
