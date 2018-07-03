using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using RPG2D.API;
using RPG2D.Registers;
using UnityEngine;

namespace RPG2D.Managers
{
    public static class SaveManager
    {
        public static string ObjectToString(object obj, SurrogateSelector ss)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter bf = new BinaryFormatter();
                if (ss != null)
                    bf.SurrogateSelector = ss;
                bf.Serialize(ms, obj);         
                return Convert.ToBase64String(ms.ToArray());
            }
        }
        
        public static object StringToObject(string base64String, SurrogateSelector ss)
        {    
            byte[] bytes = Convert.FromBase64String(base64String);
            using (MemoryStream ms = new MemoryStream(bytes, 0, bytes.Length))
            {
                ms.Write(bytes, 0, bytes.Length);
                ms.Position = 0;
                
                BinaryFormatter bf = new BinaryFormatter();
                if (ss != null)
                    bf.SurrogateSelector = ss;
                
                return bf.Deserialize(ms);
            }
        }

        public static void Load()
        {
            SurrogateSelector surrogateSelector = new SurrogateSelector();
            SerializationSurrogates.Vector2IntSerializationSurrogate vector2I = new SerializationSurrogates.Vector2IntSerializationSurrogate();
            SerializationSurrogates.Vector2SerializationSurrogate vector2 = new SerializationSurrogates.Vector2SerializationSurrogate();
            SerializationSurrogates.Vector3SerializationSurrogate vector3 = new SerializationSurrogates.Vector3SerializationSurrogate();
            SerializationSurrogates.Vector3IntSerializationSurrogate vector3I = new SerializationSurrogates.Vector3IntSerializationSurrogate();
 
            surrogateSelector.AddSurrogate(typeof(Vector2Int),new StreamingContext(StreamingContextStates.All),vector2I);
            surrogateSelector.AddSurrogate(typeof(Vector2),new StreamingContext(StreamingContextStates.All),vector2);
            surrogateSelector.AddSurrogate(typeof(Vector3Int),new StreamingContext(StreamingContextStates.All),vector3I);
            surrogateSelector.AddSurrogate(typeof(Vector3),new StreamingContext(StreamingContextStates.All),vector3);
            
            XmlDocument xml = new XmlDocument();
            
            xml.LoadXml(File.ReadAllText(Application.persistentDataPath + "/world.xml"));
            
            XmlNode world = xml.GetElementsByTagName("World")[0];
            foreach (XmlNode worldEntry in world.ChildNodes)
            {
                if (worldEntry.Name == "Mods")
                {
                    XmlNodeList mods = worldEntry.ChildNodes;

                    foreach (XmlNode mod in mods)
                    {
                        bool found = false;
                        foreach (var modInstance in ModManager.ModInstances)
                        {
                            if (modInstance.Value.InternalName == mod.Attributes.GetNamedItem("InternalName").InnerText && modInstance.Value.Version == mod.Attributes.GetNamedItem("Version").InnerText)
                            {
                                found = true;
                            }

                            if (!found)
                            {
                                Debug.LogError("A Mod by the name of " + mod.Attributes.GetNamedItem("InternalName").InnerText + " version " + mod.Attributes.GetNamedItem("Version").InnerText + "is not loaded");
                            }
                        }
                    }
                } else if (worldEntry.Name == "Blocks")
                {
                    XmlNodeList blocks = worldEntry.ChildNodes;

                    foreach (XmlNode block in blocks)
                    {
                        if (BlockRegister.IsRegitered(block.Attributes.GetNamedItem("InternalName").InnerText))
                        {
                            BaseClasses.Block blockInstance = (BaseClasses.Block)StringToObject(block.InnerText, surrogateSelector);
                            World.SetBlock(blockInstance.Position, blockInstance);
                        }
                    }
                } else if (worldEntry.Name == "Player")
                {
                    BaseClasses.Player player = (BaseClasses.Player)StringToObject(worldEntry.InnerText, surrogateSelector);
                    PlayerRegister.Player = player;
                    player.Init();
                }
            }
        }
        
        public static void Save()
        {
            SurrogateSelector surrogateSelector = new SurrogateSelector();
            SerializationSurrogates.Vector2IntSerializationSurrogate vector2I = new SerializationSurrogates.Vector2IntSerializationSurrogate();
            SerializationSurrogates.Vector2SerializationSurrogate vector2 = new SerializationSurrogates.Vector2SerializationSurrogate();
            SerializationSurrogates.Vector3SerializationSurrogate vector3 = new SerializationSurrogates.Vector3SerializationSurrogate();
            SerializationSurrogates.Vector3IntSerializationSurrogate vector3I = new SerializationSurrogates.Vector3IntSerializationSurrogate();
 
            surrogateSelector.AddSurrogate(typeof(Vector2Int),new StreamingContext(StreamingContextStates.All),vector2I);
            surrogateSelector.AddSurrogate(typeof(Vector2),new StreamingContext(StreamingContextStates.All),vector2);
            surrogateSelector.AddSurrogate(typeof(Vector3Int),new StreamingContext(StreamingContextStates.All),vector3I);
            surrogateSelector.AddSurrogate(typeof(Vector3),new StreamingContext(StreamingContextStates.All),vector3);
            
            XmlDocument xml = new XmlDocument();
            XmlElement world = xml.CreateElement("World");
            world.SetAttribute("name", "Test");

            xml.AppendChild(world);

            XmlElement mods = xml.CreateElement("Mods");
            world.AppendChild(mods);

            foreach (var modInstance in ModManager.ModInstances)
            {
                XmlElement mod = xml.CreateElement("Mod");
                mod.SetAttribute("InternalName", modInstance.Value.InternalName);
                mod.SetAttribute("Version", modInstance.Value.Version);
                mods.AppendChild(mod);
            }

            XmlElement blocksXml = xml.CreateElement("Blocks");
            world.AppendChild(blocksXml);
            
            List<BaseClasses.Block> blocks = World.ReturnAllBlocks();
            foreach (var block in blocks)
            {
                XmlElement blockXml = xml.CreateElement("Block");
                blockXml.SetAttribute("InternalName", block.InternalName);
                blockXml.InnerText = ObjectToString(block, surrogateSelector);
                blocksXml.AppendChild(blockXml);
            }

            XmlElement player = xml.CreateElement("Player");
            player.SetAttribute("InternalName", PlayerRegister.Player.InternalName);
            player.InnerText = ObjectToString(PlayerRegister.Player, surrogateSelector);

            world.AppendChild(player);
            
            File.WriteAllText(Application.persistentDataPath + "/world.xml", xml.OuterXml);
        }
    }
}