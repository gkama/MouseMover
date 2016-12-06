using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace XMLReader
{
    class XMLReader
    {
        //String
        private string XMLString { get; set; }

        //Wrapper Variables
        private XMLReaderProperties.Elements _Elements = new XMLReaderProperties.Elements();
        private XMLReaderProperties.Text _Text = new XMLReaderProperties.Text();
        private XMLReaderProperties.Attributes _Attributes = new XMLReaderProperties.Attributes();

        //Constructor with XML Doc Path
        public XMLReader(string xmlString)
        {
            XMLString = xmlString;

            _Elements.XMLString = xmlString;
            _Text.XMLString = xmlString;
            _Attributes.XMLString = xmlString;
        }

        #region Wrapper Functions - Elements/Text/Attributes
        //Wrapper Functions
        //Elements
        public List<string> Elements()
        {
            return _Elements.GetElements();
        }
        public string ReadElements()
        {
            return _Elements.ReadElements();
        }
        public string GetElement(string TagName)
        {
            return _Elements.GetElement(TagName);
        }
        public string GetElementValue(string TagName)
        {
            return _Elements.GetElementValue(TagName);
        }
        public Dictionary<string, string> ReadElementsText()
        {
            return _Elements.ReadElementsText();
        }
        public Dictionary<string, string> ReadElementsAttributes()
        {
            return _Elements.ReadElementsAttributes();
        }

        //Text
        public List<string> Text()
        {
            return _Text.GetText();
        }
        public string ReadTagsText()
        {
            return _Text.ReadTagsText();
        }
        public string ReadTextOnly()
        {
            return _Text.ReadTextOnly();
        }

        //Attributes
        public List<string> Attributes()
        {
            return _Attributes.GetAttributes();
        }
        public string ReadAttributes()
        {
            return _Attributes.ReadAttributes();
        }
        public string ReadAttributesOnly()
        {
            return _Attributes.ReadAttributesOnly();
        }
        #endregion


        //MAIN FOR TESTING
        /*
        static void Main()
        {
            XMLReader r = new XMLReader("<?xml version=\"1.0\" encoding=\"utf-8\"?><output><outputOrigin href=\"asd\">T</outputOrigin><outputID>GBCTST90     </outputID><formatOrigin>P</formatOrigin><formatID>GBCTST93    </formatID><jobTime /><alwaysCreate>N</alwaysCreate><deliveries><delivery><transType>TST</transType><transMethod>        </transMethod><emailAddr>georgi.kamacharov@convergys.com</emailAddr><fileName /></delivery></deliveries><scn>M9F526YNJ</scn><scn>M9F526YNJ2</scn><scn>M9F526YNJ4</scn><logic /></output>");
            XMLFileReader rr = new XMLFileReader(@"C:\Work\Testing\Archive\test.xml");
            bool b = r.ReadElementsText().Equals(rr.ReadElementsText());

            Console.WriteLine(rr.ReadElementsText());
        }*/

        //Read everything
        public string Read()
        {
            try
            {
                StringBuilder readOutput = new StringBuilder();
                XmlReader xmlReader = XmlReader.Create(new StringReader(XMLString));
                while (xmlReader.Read())
                {
                    switch (xmlReader.NodeType)
                    {
                        case XmlNodeType.Element: // The node is an element.
                            {
                                readOutput.Append("<" + xmlReader.Name);

                                while (xmlReader.MoveToNextAttribute()) // Read the attributes.
                                    readOutput.Append(" " + xmlReader.Name + "='" + xmlReader.Value + "'");
                                readOutput.Append(">").Append("\r\n");
                                break;
                            }
                        case XmlNodeType.Text: //Display the text in each element.
                            readOutput.Append(xmlReader.Value).Append("\r\n");
                            break;
                        case XmlNodeType.EndElement: //Display the end of the element.
                            readOutput.Append("</" + xmlReader.Name).Append(">").Append("\r\n");
                            break;
                    }
                }
                return readOutput.ToString();
            }
            catch (XmlException)
            {
                return "Could not parse XML string. Make sure it's in the correct format.";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
    }
}
