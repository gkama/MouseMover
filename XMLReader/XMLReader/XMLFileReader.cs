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
    public class XMLFileReader : XmlDocument
    {
        //Path
        public string XMLDocPath { get; set; }

        //Empty Contrustor
        public XMLFileReader() { }
        //Constructor with XML Doc Path
        public XMLFileReader(string xmlDocPath)
        {
            XMLDocPath = xmlDocPath;
        }
        
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
