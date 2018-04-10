namespace RPG2D.BaseClasses
{
    public abstract class Mod
    {
        public string InternalName;
        public string VisibleName;
        public string Version;

        public abstract bool PreInit();
        public abstract bool Init();
        public abstract bool PostInit();

        public abstract bool LoadContent();
    }
}
