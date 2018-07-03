using System;

namespace RPG2D.TOML
{
    public class Dependency
    {
        public Dependency(String internalName, Version minVersion, Version targetVersion, Version maxVersion)
        {
            InternalName = internalName;
            MinVersion = minVersion;
            TargetVersion = targetVersion;
            MaxVersion = maxVersion;
        }
        
        public String InternalName { get; set; }
        public Version MinVersion { get; set; }
        public Version TargetVersion { get; set; }
        public Version MaxVersion { get; set; }
    }
}