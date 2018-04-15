using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;
using Microsoft.CSharp;
using UnityEngine;

namespace RPG2D.Managers
{
    public static class ModManager
    {
        private static Dictionary<string, RPG2D.BaseClasses.Mod> _modInstances = new Dictionary<string, RPG2D.BaseClasses.Mod>();
        private static Dictionary<string, Assembly> _modAssemblies = new Dictionary<string, Assembly>();

        public static Dictionary<string, RPG2D.BaseClasses.Mod> ModInstances
        {
            get { return _modInstances; }
        }
        
        private static Assembly CompileCSharpFile(string path, string outputFile, List<string> dependencies)
        {
            var provider = new CSharpCodeProvider();
            var param = new CompilerParameters();

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                param.ReferencedAssemblies.Add(assembly.Location);
            }

            foreach (var dependency in dependencies)
            {
                if (!dependency.EndsWith(".dll"))
                {
                    string[] compiledFiles = Directory.GetFiles(Application.persistentDataPath + "/Mods/" + dependency + "/Compiled");
                    foreach (var file in compiledFiles)
                    {
                        param.ReferencedAssemblies.Add(file);
                    }

                    string[] preCompiledFiles = Directory.GetFiles(Application.persistentDataPath + "/Mods/" + dependency + "/PreCompiled");
                    foreach (var file in preCompiledFiles)
                    {
                        param.ReferencedAssemblies.Add(file);
                    }
                }
                else
                {
                    param.ReferencedAssemblies.Add(dependency);
                }
            }

            param.GenerateExecutable = false;
            param.GenerateInMemory = true;
            param.OutputAssembly = outputFile;

            var result = provider.CompileAssemblyFromSource(param, File.ReadAllText(path));

            if (result.Errors.Count > 0)
            {
                var msg = new StringBuilder();
                foreach (CompilerError error in result.Errors)
                {
                    msg.AppendFormat("Error {0} ({1}) {2}: {3}\n", error.FileName, error.ErrorNumber, error.Line, error.ErrorText);
                }

                throw new Exception(msg.ToString());
            }

            return result.CompiledAssembly;
        }

        private static bool LoadMod(string internalModName, List<string> dependencies)
        {
            string modXmlPath = Application.persistentDataPath + "/Mods/" + internalModName + "/Mod.xml";

            XmlDocument modXml = new XmlDocument();

            modXml.LoadXml(File.ReadAllText(modXmlPath));

            string[] compiledFiles = Directory.GetFiles(Application.persistentDataPath + "/Mods/" + internalModName + "/Compiled");
            foreach (var compiledFile in compiledFiles)
            {
                File.Delete(compiledFile);
            }

            XmlNodeList assemblies = modXml.GetElementsByTagName("Assemblies");

            string path = "";
            string outputFilePath = "";
            bool isPrecompiled = false;

            foreach (XmlNode assembly in assemblies)
            {
                XmlNodeList assemblyChildNodes = assembly.ChildNodes;
                foreach (XmlNode assemblyChildNode in assemblyChildNodes)
                {
                    XmlNodeList assemblyChildDatas = assemblyChildNode.ChildNodes;
                    foreach (XmlNode assemblyChildData in assemblyChildDatas)
                    {
                        switch (assemblyChildData.Name)
                        {
                            case "Path":
                                path = assemblyChildData.InnerText;
                                break;
                            case "OutputFilePath":
                                outputFilePath = assemblyChildData.InnerText;
                                break;
                            case "IsPrecompiled":
                                if (assemblyChildData.InnerText == "False" || assemblyChildData.InnerText == "false")
                                {
                                    isPrecompiled = false;
                                }
                                else if (assemblyChildData.InnerText == "True" || assemblyChildData.InnerText == "true")
                                {
                                    isPrecompiled = true;
                                }

                                break;
                        }
                    }

                    _modAssemblies[internalModName] = CompileCSharpFile(Application.persistentDataPath + "/Mods/" + internalModName + "/" + path, Application.persistentDataPath + "/Mods/" + internalModName + "/" + outputFilePath, dependencies);
                }
            }


            foreach (var modAssembly in _modAssemblies)
            {
                var types = modAssembly.Value.GetExportedTypes();
                foreach (var type in types)
                {
                    if (type != null)
                    {
                        if (type.FullName.Contains("___MOD___"))
                        {
                            var mod = Activator.CreateInstance(type) as RPG2D.BaseClasses.Mod;
                            _modInstances[internalModName] = mod;
                        }
                    }
                }
            }

            return true;
        }

        public static bool LoadMods()
        {
            string modListXmlPath = Application.persistentDataPath + "/Mods/ModList.xml";

            XmlDocument modListXml = new XmlDocument();

            modListXml.LoadXml(File.ReadAllText(modListXmlPath));

            string internalName = "";
            List<string> dependencies = new List<string>();

            Debug.Log("Found these mods:");

            XmlNodeList modList = modListXml.GetElementsByTagName("Mods");
            foreach (XmlNode mod in modList)
            {
                XmlNodeList modNodes = mod.ChildNodes;
                foreach (XmlNode modNode in modNodes)
                {
                    foreach (XmlNode modNodeData in modNode)
                    {
                        switch (modNodeData.Name)
                        {
                            case "InternalName":
                                internalName = modNodeData.InnerText;
                                break;
                            case "Dependencies":
                                XmlNodeList dependencyNodes = modNodeData.ChildNodes;
                                foreach (XmlNode dependencyNode in dependencyNodes)
                                {
                                    dependencies.Add(dependencyNode.InnerText);
                                }

                                break;
                        }
                    }
                }

                if (internalName != "")
                {
                    Debug.Log("    -" + internalName);
                    LoadMod(internalName, dependencies);
                }
            }

            foreach (var modInstance in _modInstances)
            {
                modInstance.Value.PreInit();
            }

            foreach (var modInstance in _modInstances)
            {
                modInstance.Value.Init();
            }

            foreach (var modInstance in _modInstances)
            {
                modInstance.Value.PostInit();
            }

            foreach (var modInstance in _modInstances)
            {
                modInstance.Value.LoadContent();
            }

            return true;
        }
    }
}