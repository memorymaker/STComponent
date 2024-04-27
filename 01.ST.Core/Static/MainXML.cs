using System;
using System.Collections.Generic;
using System.Xml;

namespace ST.Core
{
    public static class MainXML
    {
        private const string FilePath = "ST.Main.xml";
        private const string MenuNodes = "/configuration/menu/add";
        private const string ValueNodes = "/configuration/value/add";
        private const string ValueNodesParent = "/configuration/value";

        private static XmlDocument LoadXmlDocument()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(FilePath);
            return doc;
        }

        public static MenuItem GetMenu(string id)
        {
            id = id.Trim();
            MenuItem rsItem = null;

            XmlDocument doc = LoadXmlDocument();
            XmlNodeList list = doc.SelectNodes(MenuNodes);
            foreach (XmlNode xn in list)
            {
                if (id == xn.Attributes["id"]?.Value || id == xn.Attributes["ID"]?.Value || id == xn.Attributes["Id"]?.Value || id == xn.Attributes["iD"]?.Value)
                {
                    rsItem = new MenuItem();
                    for (int i = 0; i < xn.Attributes.Count; i++)
                    {
                        switch (xn.Attributes[i].Name.ToLower())
                        {
                            case "id": rsItem.ID = xn.Attributes[i].Value; break;
                            case "name": rsItem.Name = xn.Attributes[i].Value; break;
                            case "default": rsItem.Default = xn.Attributes[i].Value; break;
                            case "hotkeyindex": rsItem.HotKeyIndex = xn.Attributes[i].Value; break;
                            case "dll": rsItem.DLL = xn.Attributes[i].Value; break;
                            case "class": rsItem.Class = xn.Attributes[i].Value; break;
                        }
                    }
                    break;
                }
            }
            return rsItem;
        }

        public static List<MenuItem> GetMenuList()
        {
            List<MenuItem> rs = new List<MenuItem>();

            XmlDocument doc = LoadXmlDocument();
            XmlNodeList list = doc.SelectNodes(MenuNodes);
            foreach (XmlNode xn in list)
            {
                MenuItem item = new MenuItem();
                for (int i = 0; i < xn.Attributes.Count; i++)
                {
                    switch (xn.Attributes[i].Name.ToLower())
                    {
                        case "id": item.ID = xn.Attributes[i].Value; break;
                        case "name": item.Name = xn.Attributes[i].Value; break;
                        case "default": item.Default = xn.Attributes[i].Value; break;
                        case "hotkeyindex": item.HotKeyIndex = xn.Attributes[i].Value; break;
                        case "dll": item.DLL = xn.Attributes[i].Value; break;
                        case "class": item.Class = xn.Attributes[i].Value; break;
                    }
                }
                rs.Add(item);

            }
            return rs;
        }

        public static ValueItem GetValue(string id)
        {
            ValueItem rsItem = null;

            XmlDocument doc = LoadXmlDocument();
            XmlNodeList list = doc.SelectNodes(ValueNodes);
            foreach (XmlNode xn in list)
            {
                if (id == xn.Attributes["id"]?.Value || id == xn.Attributes["ID"]?.Value || id == xn.Attributes["Id"]?.Value || id == xn.Attributes["iD"]?.Value)
                {
                    rsItem = new ValueItem();
                    for (int i = 0; i < xn.Attributes.Count; i++)
                    {
                        switch (xn.Attributes[i].Name.ToLower())
                        {
                            case "id": rsItem.ID = xn.Attributes[i].Value; break;
                            case "type": rsItem.Type = xn.Attributes[i].Value; break;
                        }
                    }

                    string value = xn.InnerText;
                    switch (rsItem.Type.ToLower())
                    {
                        case "int": rsItem.Value = int.Parse(value); break;
                        case "long": rsItem.Value = long.Parse(value); break;
                        case "float": rsItem.Value = float.Parse(value); break;
                        case "single": rsItem.Value = Single.Parse(value); break;
                        case "double": rsItem.Value = double.Parse(value); break;
                        case "decimal": rsItem.Value = decimal.Parse(value); break;
                        case "bool": rsItem.Value = bool.Parse(value); break;
                        default: rsItem.Value = value; break;
                    }
                    break;
                }
            }
            return rsItem;
        }

        public static List<ValueItem> GetValueList()
        {
            List<ValueItem> rs = new List<ValueItem>();

            XmlDocument doc = LoadXmlDocument();
            XmlNodeList list = doc.SelectNodes(ValueNodes);
            foreach (XmlNode xn in list)
            {
                ValueItem item = new ValueItem();
                for (int i = 0; i < xn.Attributes.Count; i++)
                {
                    switch (xn.Attributes[i].Name.ToLower())
                    {
                        case "id": item.ID = xn.Attributes[i].Value; break;
                        case "type": item.Type = xn.Attributes[i].Value; break;
                    }
                }
                
                string value = xn.InnerText;
                switch (item.Type.ToLower())
                {
                    case "int": item.Value = int.Parse(value); break;
                    case "long": item.Value = long.Parse(value); break;
                    case "float": item.Value = float.Parse(value); break;
                    case "single": item.Value = Single.Parse(value); break;
                    case "double": item.Value = double.Parse(value); break;
                    case "decimal": item.Value = decimal.Parse(value); break;
                    case "bool": item.Value = bool.Parse(value); break;
                    default: item.Value = value; break;
                }
                rs.Add(item);
            }
            return rs;
        }

        public static void SetValue(string id, object value)
        {
            XmlDocument doc = LoadXmlDocument();
            XmlNodeList list = doc.SelectNodes(ValueNodes);

            // ------------ Get targetNode
            // Find targetNode
            XmlNode targetNode = null;
            foreach (XmlNode xn in list)
            {
                if (id == xn.Attributes["id"]?.Value || id == xn.Attributes["ID"]?.Value || id == xn.Attributes["Id"]?.Value || id == xn.Attributes["iD"]?.Value)
                {
                    targetNode = xn;
                    break;
                }
            }
            // Set targetNode
            if (targetNode == null)
            {
                AddNode(doc, ValueNodesParent, "add", id);
                list = doc.SelectNodes(ValueNodes);
                targetNode = list[list.Count - 1];
            }

            // ------------ Set Type
            // Get typeAttrbuteIndex
            int typeAttrbuteIndex = -1;
            for (int i = 0; i < targetNode.Attributes.Count; i++)
            {
                if (targetNode.Attributes[i].Name.ToLower() == "type")
                {
                    typeAttrbuteIndex = i;
                    break;
                }
            }

            // Get typeText
            string typeText;
            switch (value.GetType().Name.ToLower())
            {
                case "int32": typeText = "int"; break;
                case "int64": typeText = "long"; break;
                case "float": typeText = "float"; break;
                case "single": typeText = "single"; break;
                case "double": typeText = "double"; break;
                case "decimal": typeText = "decimal"; break;
                case "bool": typeText = "bool"; break;
                default: typeText = "text"; break;
            }
            // Set Type
            if (typeAttrbuteIndex >= 0)
            {
                targetNode.Attributes[typeAttrbuteIndex].Value = typeText;
            }
            else
            {
                XmlAttribute typeAttribute = doc.CreateAttribute("type");
                typeAttribute.Value = typeText;
                targetNode.Attributes.Append(typeAttribute);
            }

            // ----------- Set Value & Save
            targetNode.InnerText = value.ToString();
            doc.Save(FilePath);
        }

        public static void AddNode(XmlDocument doc, string parentNodePath, string newNodeString, string id)
        {
            // Get newNode & Create ParentNode
            XmlNode parentNode = doc.SelectSingleNode(parentNodePath);
            if (parentNode == null)
            {
                string[] arrParentNode = parentNodePath.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries);
                string forParentPath = string.Empty;
                string forParentParentPath = string.Empty;
                for (int i = 0; i < arrParentNode.Length; i++)
                {
                    forParentPath += "/" + arrParentNode[i];
                    XmlNode node = doc.SelectSingleNode(forParentPath);
                    if (node == null)
                    {
                        XmlElement forNewElement = doc.CreateElement(arrParentNode[i]);
                        if (i == 0)
                        {
                            doc.AppendChild(forNewElement);
                        }
                        else
                        {
                            XmlNode forParentNode = doc.SelectSingleNode(forParentParentPath);
                            forParentNode.AppendChild(forNewElement);
                        }
                    }

                    if (i == arrParentNode.Length - 1)
                    {
                        parentNode = doc.SelectSingleNode(forParentPath);
                        break;
                    }

                    forParentParentPath += "/" + arrParentNode[i];
                }
            }
            XmlElement newNode = doc.CreateElement(newNodeString);

            // Set ID Attribute
            XmlAttribute newAttribute = doc.CreateAttribute("id");
            newAttribute.Value = id;
            newNode.SetAttributeNode(newAttribute);

            // Append
            parentNode.AppendChild(newNode);
        }

        public class MenuItem
        {
            public string ID;
            public string Name;
            public string Default;
            public string HotKeyIndex;
            public string DLL;
            public string Class;
        }

        public class ValueItem
        {
            public string ID;
            public string Type;
            public object Value;
        }
    }
}
