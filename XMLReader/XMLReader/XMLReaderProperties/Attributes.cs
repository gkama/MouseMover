using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace XMLReader.XMLReaderProperties
{
    class Attributes
    {
        //Path
        public string XMLString { get; set; }

        //Empty Contrustor
        public Attributes() { }
        //Constructor with XML Doc Path
        public Attributes(string xmlString)
        {
            XMLString = xmlString;
        }

        public List<string> GetAttributes()
        {
            List<string> _Attributes = new List<string>();
            string[] stringSeparators = new string[] { "\r\n" };
            foreach (string s in ReadAttributesOnly().Split(stringSeparators, StringSplitOptions.None))
                if (!string.IsNullOrEmpty(s))
                    _Attributes.Add(s);
            return _Attributes;
        }

        //Attribute Reader
        public string ReadAttributes()
        {
            try
            {
                StringBuilder attrOutput = new StringBuilder();
                XmlReader xmlReader = XmlReader.Create(new StringReader(XMLString));
                while (xmlReader.Read())
                {
                    switch (xmlReader.NodeType)
                    {
                        case XmlNodeType.Element: // The node is an element.
                            {
                                attrOutput.Append("<" + xmlReader.Name);

                                while (xmlReader.MoveToNextAttribute()) // Read the attributes.
                                    attrOutput.Append(" " + xmlReader.Name + "='" + xmlReader.Value + "'");
                                attrOutput.Append(">").Append("\r\n");
                                break;
                            }
                    }
                }
                return attrOutput.ToString();
            }
            catch (XmlException)
            {
                return "Could not parse file. Make sure it's in the correct format.";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
        public string ReadAttributesOnly()
        {
            try
            {
                StringBuilder attrOnlyOutput = new StringBuilder();
                XmlReader xmlReader = XmlReader.Create(new StringReader(XMLString));
                while (xmlReader.Read())
                {
                    switch (xmlReader.NodeType)
                    {
                        case XmlNodeType.Element: // The node is an element.
                            {
                                while (xmlReader.MoveToNextAttribute()) // Read the attributes.
                                    attrOnlyOutput.Append(xmlReader.Name + "='" + xmlReader.Value + "'").Append("\r\n");
                                break;
                            }
                    }
                }
                return attrOnlyOutput.ToString();
            }
            catch (XmlException)
            {
                return "Could not parse file. Make sure it's in the correct format.";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
    }
}
