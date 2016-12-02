using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace XMLReader.XMLFileProperties
{
    class Text
    {
        //Path
        public string XMLDocPath { get; set; }

        //Empty Contrustor
        public Text() { }
        //Constructor with XML Doc Path
        public Text(string xmlDocPath)
        {
            XMLDocPath = xmlDocPath;
        }

        //Gets all the Text stored in the XML File
        public List<string> GetText()
        {
            List<string> _Text = new List<string>();
            string[] stringSeparators = new string[] { "\r\n" };
            foreach (string s in ReadTextOnly().Split(stringSeparators, StringSplitOptions.None))
                if (!string.IsNullOrEmpty(s))
                    _Text.Add(s);
            return _Text;
        }

        //TextReader only displays the tags (opened and closed) with any text in between them
        //if any text exists
        //It skips over tags' attributes
        public string ReadTagsText()
        {
            try
            {
                StringBuilder txtOutput = new StringBuilder();
                XmlTextReader xmlReader = new XmlTextReader(XMLDocPath);
                while (xmlReader.Read())
                {
                    switch (xmlReader.NodeType)
                    {
                        case XmlNodeType.Element: // The node is an element.
                            txtOutput.Append("<" + xmlReader.Name).Append(">").Append("\r\n");
                            break;
                        case XmlNodeType.Text: //Display the text in each element.
                            txtOutput.Append(xmlReader.Value).Append("\r\n");
                            break;
                        case XmlNodeType.EndElement: //Display the end of the element.
                            txtOutput.Append("</" + xmlReader.Name).Append(">").Append("\r\n");
                            break;
                    }
                }
                return txtOutput.ToString();
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
        public string ReadTextOnly()
        {
            try
            {
                StringBuilder txtOnlyOutput = new StringBuilder();
                XmlTextReader xmlReader = new XmlTextReader(XMLDocPath);
                while (xmlReader.Read())
                {
                    switch (xmlReader.NodeType)
                    {
                        case XmlNodeType.Text:
                            txtOnlyOutput.Append(xmlReader.Value).Append("\r\n");
                            break;
                    }
                }
                return txtOnlyOutput.ToString();
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

        //Copies the file over converted to .txt
        public void ConvertFileToTxt(string DestinationPath)
        {
            string fileName = Path.GetFileNameWithoutExtension(XMLDocPath) + ".txt";
            try
            {
                //If there is no file name specified in the Destination Path
                if (string.IsNullOrEmpty(Path.GetFileNameWithoutExtension(DestinationPath)))
                    File.Copy(XMLDocPath, Path.Combine(DestinationPath, fileName), true);
                else
                    File.Copy(XMLDocPath, DestinationPath, true);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
