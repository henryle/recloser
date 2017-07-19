using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;

namespace RecloserAcq
{
    [System.Xml.Serialization.XmlInclude(typeof(PropertyFormClass))]
    public class PropertyFormClass
    {
        
            
            
        private int _id;
        public int ID {
            get{return _id;}
            set{_id = value;}
        }
        private int _distancesplit1 = 200;
        public int DistanceSplit1{
            get{return _distancesplit1;}
            set{_distancesplit1 = value;}
        }
        private int _distancesplit2 = 200;
        public int DistanceSplit2{
            get{return _distancesplit2;}
            set{_distancesplit2 = value;}
        }
        private int _distancesplit3 = 200;
        public int DistanceSplit3{
            get{return _distancesplit3;}
            set{_distancesplit3 = value;}
        }
        private int _distancesplit6 = 200;
        public int DistanceSplit6{
            get{return _distancesplit6;}
            set{_distancesplit6 = value;}
        }
        private int _distanceAdvc = 200;
        public int DistanceAdvc{
            get{return _distanceAdvc;}
            set{_distanceAdvc = value;}
        }
        private int _distanceElster = 200;
        public int DistanceElster{
            get{return _distanceElster;}
            set{_distanceElster = value;}
        }
        private int _distancetubu = 200;
        public int DistanceTubu{
            get{return _distancetubu;}
            set{_distancetubu = value;}
        }
        
        private int _distancecooper = 200;
        public int DistanceCooper{
            get{return _distancecooper;}
            set{_distancecooper = value;}
        }
        public PropertyFormClass(){
            
        }
        
    }
    public static class PropertyFormFunc{
        public static PropertyFormClass getObj(string file){
            if (File.Exists(file))
            {
                PropertyFormClass propobj  = new PropertyFormClass();
                System.Xml.Serialization.XmlSerializer reader = new System.Xml.Serialization.XmlSerializer(propobj.GetType());

                // Read the XML file.
                System.IO.StreamReader tmpfile= new System.IO.StreamReader(file);

                // Deserialize the content of the file into a Book object.
                propobj = (PropertyFormClass) reader.Deserialize(tmpfile);
                return propobj;
            }
            return null;
        }
        public static void saveObj(string file,PropertyFormClass propobj)
        {
            XmlSerializer ser = new XmlSerializer(propobj.GetType());
            using (var stream = File.CreateText(file))
            {
                ser.Serialize(stream,propobj);
                stream.Close();
            }
           
        }
    }
    /*XmlSerializer serializer = new XmlSerializer(typeof(Book));
    using (StringReader reader = new StringReader(xmlDocumentText))
    {
        Book book = (Book)(serializer.Deserialize(reader));
    }
    public class Book
    {
        public string Title {get; set;}
        public string Subject {get; set;}
        public string Author {get; set;}
    }
    Suppose that I have XML that looks like this:

    <Book>
        <Title>The Lorax</Title>
        <Subject>Children's Literature</Subject>
        <Author>Theodor Seuss Geisel</Author>
    <Book>*/
}
