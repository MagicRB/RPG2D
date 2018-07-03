namespace RPG2D
{
    public class Version
    {
        public int Major { get; set; }
        public int Minor { get; set; }
        public int Patch { get; set; }

        public Version(int major, int minor, int patch)
        {
            Major = major;
            Minor = minor;
            Patch = patch;
        }
        
        public override string ToString()
        {
            return Major + "." + Minor + "." + Patch;
        }
    }
}