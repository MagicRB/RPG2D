using System;
using System.Collections.Generic;

namespace RPG2D.TOML
{
    public class Mod
    {
        public Mod(String internalName, Version version)
        {
            InternalName = internalName;
            
            Dependencies = new List<Dependency>();
        }

        public Mod()
        {
            
        }
        
        public String InternalName { get; set; }
        
        public List<Dependency> Dependencies { get; set; }
    }
}