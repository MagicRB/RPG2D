using System;
using System.Collections.Generic;

namespace RPG2D.TOML
{
    public class ModList
    {
        public ModList()
        {
            Mods = new List<Mod>();
        }
        
        public List<Mod> Mods { get; set; }
    }
}