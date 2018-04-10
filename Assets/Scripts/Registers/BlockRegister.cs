using System;
using System.Collections.Generic;
using RPG2D.BaseClasses;

namespace RPG2D.Registers
{
    public static class BlockRegister
    {
        private static Dictionary<string, System.Type> _blockDictionary = new Dictionary<string, Type>();

        public static Dictionary<string, Type> BlockDictionary
        {
            get { return _blockDictionary; }
        }

        public static void RegisterBlockType(System.Type type, string name)
        {
            _blockDictionary[name] = type;;
        }

        public static System.Type GetRegisteredBlockType(string name)
        {
            return _blockDictionary[name];
        }

        public static Block GetRegisteredBlockInstance(string name)
        {
            return (Block)Activator.CreateInstance(GetRegisteredBlockType(name));
        }
    }
}