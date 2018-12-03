namespace BRPLUSA.Revit.Installers._2018.Entities
{
    public class VersionData
    {
        private readonly int _major;
        private readonly int _minor;
        private readonly int _build;
        private readonly int _revision;

        public VersionData(): this(0, 0, 0, 0) { }

        public VersionData(int major, int minor, int build, int revision)
        {
            _major = major;
            _minor = minor;
            _build = build;
            _revision = revision;
        }

        public override string ToString()
        {
            return $"{_major}.{_minor}.{_build}.{_revision}";
        }

        public static bool operator >(VersionData v1, VersionData v2)
        {
            if (v1._major > v2._major)
                return true;
            if (v1._minor > v2._minor)
                return true;
            if (v1._build > v2._build)
                return true;

            return v1._revision > v2._revision;
        }

        public static bool operator <(VersionData v1, VersionData v2)
        {
            return (v1 > v2) && (v1 != v2);
        }
    }
}
