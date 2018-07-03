using System;
using System.Collections.Generic;
using System.Reflection;
using Nett;
using RPG2D.TOML;
using UnityEngine;

namespace RPG2D.Managers
{
    public static class ModManager
    {
        public static Dictionary<string, RPG2D.BaseClasses.Mod> ModInstances
        {
            get { return _modInstances;  }
        }
        
        private static Dictionary<string, RPG2D.BaseClasses.Mod> _modInstances = new Dictionary<string, RPG2D.BaseClasses.Mod>();

        public static List<TOML.Mod> ModTOMLs
        {
            get { return _modTOMLs; }
        }
        
        private static List<TOML.Mod> _modTOMLs = new List<TOML.Mod>();

        public static List<Assembly> ModAssemblies
        {
            get { return _modAssemblies; }
        }

        private static List<Assembly> _modAssemblies = new List<Assembly>();
        
        private static void _loadModAssembly(Assembly assembly)
        {
            var types = assembly.GetExportedTypes();
            foreach (var type in types)
            {
                if (type != null)
                {
                    if (type.FullName.Contains("___MOD___"))
                    {
                        var mod = Activator.CreateInstance(type) as RPG2D.BaseClasses.Mod;
                        _modInstances[assembly.FullName.Substring(0, assembly.FullName.IndexOf(' '))] = mod;
                    }
                }
            }
        }
        
        private static void _loadMod(Mod mod)
        {
            Debug.Log("Loading mod " + mod.InternalName);
            
            _loadModAssembly(Assembly.LoadFrom(Application.persistentDataPath + "/Mods/" + mod.InternalName + "/" + mod.InternalName + ".dll"));
        }
        
        public static void LoadMods()
        {
            TOML.ModList modList = new TOML.ModList();
            
            modList.Mods.Add(new TOML.Mod("TestMod", new Version(0,0,0)));
            
            Toml.WriteFile(modList, Application.persistentDataPath + "/ModList.tml");
            
            modList = Toml.ReadFile<TOML.ModList>(Application.persistentDataPath + "/ModList.tml");

            foreach (var mod in modList.Mods)
            {
                _modTOMLs.Add(mod);
                _loadMod(mod);
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
        }
    }
}