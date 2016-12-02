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
    public class XMLFileReader
    {
        ///
        /// Wrapper Class
        ///

        //Path
        private string XMLDocPath { get; set; }

        //Wrapper Variables
        private XMLFileProperties.Elements _Elements = new XMLFileProperties.Elements();
        private XMLFileProperties.Text _Text = new XMLFileProperties.Text();
        private XMLFileProperties.Attributes _Attributes = new XMLFileProperties.Attributes(); 

        //Constructor with XML Doc Path
        public XMLFileReader(string xmlDocPath)
        {
            if (File.Exists(xmlDocPath))
            { //If the path exists
                XMLDocPath = xmlDocPath;

                _Elements.XMLDocPath = XMLDocPath;
                _Text.XMLDocPath = XMLDocPath;
                _Attributes.XMLDocPath = XMLDocPath;
            }
            else
            {
                throw new Exception("File Path DOES NOT EXIST: " + xmlDocPath);
            }
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
        public string ReadTagsText()
        { 
            return _Text.ReadTagsText(); 
        }
        public string ReadTextOnly()
        { 
            return _Text.ReadTextOnly(); 
        }
        public void ConvertFileToTxt(string DestinationPath)
        {
            _Text.ConvertFileToTxt(DestinationPath);
        }

        //Attributes
        public string ReadAttributes()
        {
            return _Attributes.ReadAttributes();
        }
        public string ReadAttributesOnly()
        {
            return _Attributes.ReadAttributesOnly();
        }
        #endregion



        //Read everything
        public string Read()
        {
            try
            {
                StringBuilder readOutput = new StringBuilder();
                XmlReader xmlReader = XmlReader.Create(XMLDocPath);
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
            catch (FileNotFoundException)
            {
                return "Could not find file: " + XMLDocPath;
            }
            catch (XmlException)
            {
                return "Could not parse file: " + XMLDocPath;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
    }
}
