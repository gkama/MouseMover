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
    class Elements
    {
        //Path
        public string XMLString { get; set; }

        //Empty Contrustor
        public Elements() { }
        //Constructor with XML Doc Path
        public Elements(string xmlString)
        {
            XMLString = xmlString;
        }

        public List<string> GetElements()
        {
            List<string> _Tags = new List<string>();
            string[] stringSeparators = new string[] { "\r\n" };
            foreach (string s in ReadElements().Split(stringSeparators, StringSplitOptions.None))
                if (!string.IsNullOrEmpty(s))
                    _Tags.Add(s.TrimStart('<').TrimEnd('>'));
            return _Tags;
        }

        //Get the list of all tags in the XML Document
        public string ReadElements()
        {
            try
            {
                StringBuilder AllTagsSB = new StringBuilder();
                XmlReader xmlReader = XmlReader.Create(new StringReader(XMLString));
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
            catch (XmlException)
            {
                return "Could not parse XML string. Make sure it's in the correct format.";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        //Return everything that is inside that Tag
        public string GetElement(string TagName)
        {
            try
            {
                StringBuilder elements = new StringBuilder();
                XmlDocument xDoc = new XmlDocument();
                xDoc.LoadXml(XMLString);

                XmlNodeList elList = xDoc.GetElementsByTagName(TagName);
                if (elList.Count == 0)
                    return "";
                else
                {
                    for (int i = 0; i < elList.Count; i++)
                        elements.Append(elList[i].InnerXml).Append("\r\n");
                }
                return elements.ToString();
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
        //Return specific tag's value, as long as it's not a parent
        public string GetElementValue(string TagName)
        {
            try
            {
                StringBuilder values = new StringBuilder();
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(XMLString);

                XmlNodeList elList = xmlDoc.GetElementsByTagName(TagName);
                for (int i = 0; i < elList.Count; i++)
                {
                    XmlNodeList elListNodes = elList[i].ChildNodes;
                    foreach (XmlNode xNode in elListNodes)
                    {
                        if (xNode.ChildNodes.Count == 0) //When there are no child nodes
                            values.Append(xNode.Value).Append(',');
                    }
                }
                //Return
                return values.ToString().TrimEnd(',');
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

        //Prase the XML file and store each tag with it's corresponding text value
        public Dictionary<string, string> ReadElementsText()
        {
            Dictionary<string, string> toReturn = new Dictionary<string, string>();
            try
            {
                StringBuilder readOutput = new StringBuilder();
                string currValue = string.Empty;
                XmlReader xmlReader = XmlReader.Create(new StringReader(XMLString));
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
            catch (XmlException)
            {
                throw new XmlException("Could not parse XML string. Make sure it's in the correct format.");
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        //Attributes
        public Dictionary<string, string> ReadElementsAttributes()
        {
            Dictionary<string, string> toReturn = new Dictionary<string, string>();
            try
            {
                StringBuilder readOutput = new StringBuilder();
                StringBuilder currValue = new StringBuilder();
                string currTag = string.Empty;
                XmlReader xmlReader = XmlReader.Create(new StringReader(XMLString));
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
            catch (XmlException)
            {
                throw new XmlException("Could not parse XML string. Make sure it's in the correct format.");
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
