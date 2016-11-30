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
    class Tags
    {
        //Path
        public string XMLDocPath { get; set; }

        //Empty Contrustor
        public Tags() { }
        //Constructor with XML Doc Path
        public Tags(string xmlDocPath)
        {
            XMLDocPath = xmlDocPath;
        }

        public List<string> GetTags()
        {
            List<string> _Tags = new List<string>();
            string[] stringSeparators = new string[] { "\r\n" };
            foreach (string s in ReadTags().Split(stringSeparators, StringSplitOptions.None))
                if (!string.IsNullOrEmpty(s))
                    _Tags.Add(s.TrimStart('<').TrimEnd('>'));
            return _Tags;
        }

        //Get the list of all tags in the XML Document
        public string ReadTags()
        {
            try
            {
                StringBuilder AllTagsSB = new StringBuilder();
                XmlReader xmlReader = XmlReader.Create(XMLDocPath);
                while (xmlReader.Read())
                {
                    switch (xmlReader.NodeType)
                    {
                        case XmlNodeType.Element:
                            {
                                if (xmlReader.IsStartElement())
                                    AllTagsSB.Append("<" + xmlReader.Name.Trim() + ">").Append("\r\n");
                                break;
                            }
                    }
                }

                if (string.IsNullOrEmpty(AllTagsSB.ToString()))
                    return "";
                else
                    return AllTagsSB.ToString();
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

        //Prase the XML file and store each tag with it's corresponding text value
        public Dictionary<string, string> ReadTagsTextValues()
        {
            Dictionary<string, string> toReturn = new Dictionary<string, string>();
            try
            {
                StringBuilder readOutput = new StringBuilder();
                string currValue = string.Empty;
                XmlReader xmlReader = XmlReader.Create(XMLDocPath);
                while (xmlReader.Read())
                {
                    switch (xmlReader.NodeType)
                    {
                        case XmlNodeType.Text: //Display the text in each element.
                            currValue = xmlReader.Value.Trim();
                            break;
                        case XmlNodeType.EndElement:
                            {
                                //IF Tag does not exist, add it
                                if (!toReturn.ContainsKey(xmlReader.Name.Trim()))
                                    toReturn.Add(xmlReader.Name.Trim(), currValue.Trim());
                                else
                                { //If Tag exists, add onto its value
                                    toReturn[xmlReader.Name.Trim()] = toReturn[xmlReader.Name.Trim()] + "," + currValue.Trim();
                                }
                                currValue = string.Empty;
                                break;
                            }
                    }
                }
                return toReturn;
            }
            catch (FileNotFoundException)
            {
                throw new FileNotFoundException("Could not find file: " + XMLDocPath);
            }
            catch (XmlException)
            {
                throw new XmlException("Could not parse file: " + XMLDocPath);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        //Attributes
        public Dictionary<string, string> ReadTagsAttributesValues()
        {
            Dictionary<string, string> toReturn = new Dictionary<string, string>();
            try
            {
                StringBuilder readOutput = new StringBuilder();
                StringBuilder currValue = new StringBuilder();
                string currTag = string.Empty;
                XmlReader xmlReader = XmlReader.Create(XMLDocPath);
                while (xmlReader.Read())
                {
                    switch (xmlReader.NodeType)
                    {
                        case XmlNodeType.Element:
                            {
                                currTag = xmlReader.Name;
                                while (xmlReader.MoveToNextAttribute()) // Read the attributes.
                                    currValue.Append(xmlReader.Name + "='" + xmlReader.Value + "'").Append(",");

                                //IF Tag does not exist, add it
                                if (!toReturn.ContainsKey(xmlReader.Name.Trim()))
                                    toReturn.Add(currTag, currValue.ToString().Trim().TrimEnd(','));
                                else
                                { //If Tag exists, add onto its value
                                    toReturn[currTag] = toReturn[currTag] + "," + currValue.ToString().Trim().TrimEnd(',');
                                }
                                currTag = string.Empty;
                                currValue = new StringBuilder();
                                break;
                            }
                    }
                }
                return toReturn;
            }
            catch (FileNotFoundException)
            {
                throw new FileNotFoundException("Could not find file: " + XMLDocPath);
            }
            catch (XmlException)
            {
                throw new XmlException("Could not parse file: " + XMLDocPath);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
