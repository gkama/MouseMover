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
        private XMLFileProperties.Tags _Tags = new XMLFileProperties.Tags();
        private XMLFileProperties.Text _Text = new XMLFileProperties.Text();
        private XMLFileProperties.Attributes _Attributes = new XMLFileProperties.Attributes(); 

        //Constructor with XML Doc Path
        public XMLFileReader(string xmlDocPath)
        {
            XMLDocPath = xmlDocPath;

            _Tags.XMLDocPath = XMLDocPath;
            _Text.XMLDocPath = XMLDocPath;
            _Attributes.XMLDocPath = XMLDocPath;
        }
             
        
        //For Testing
        //Entry point
        static int Main(string[] args)
        {
            string testPath = @"C:\Work\Testing\Archive\test.xml";
            XMLFileProperties.Tags XMLFileProp = new XMLFileProperties.Tags();
            XMLFileProp.XMLDocPath = testPath;
            
            //Console.Write(XMLFileProp.Read());
            Dictionary<string, string> t = XMLFileProp.ReadTagsTextValues();

            return 0;
        }
        
        #region Wrapper Functions - Tags/Text/Attributes
        //Wrapper Functions
        //Tags
        public List<string> Tags()
        {
            return _Tags.GetTags();
        }
        public string ReadTags()
        {
            return _Tags.ReadTags();
        }
        public Dictionary<string, string> ReadTagsTextValues()
        {
            return _Tags.ReadTagsTextValues();
        }
        public Dictionary<string, string> ReadTagsAttributesValues()
        {
            return _Tags.ReadTagsAttributesValues();
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
