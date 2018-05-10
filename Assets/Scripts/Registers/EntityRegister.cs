using System;
using System.Collections.Generic;
using RPG2D.BaseClasses;

namespace RPG2D.Registers
{
    public class EntityRegister
    {
        private static Dictionary<string, System.Type> _entityDictionary = new Dictionary<string, Type>();

        public static bool IsRegitered(string name)
        {
            return _entityDictionary.ContainsKey(name);
        }
        
        public static Dictionary<string, Type> EntityDictionary
        {
            get { return _entityDictionary; }
        }

        public static void RegisterEntityType(System.Type type, string name)
        {
            _entityDictionary[name] = type;
        }

        public static System.Type GetRegisteredEntityType(string name)
        {
            return _entityDictionary[name];
        }

        public static Entity GetRegisteredEntityInstance(string name)
        {
            return (Entity)Activator.CreateInstance(GetRegisteredEntityType(name));
        }
    }
}