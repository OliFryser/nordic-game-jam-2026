using System.Linq;

namespace Input
{
    public static class DashboardLayout
    {
        public static readonly string[] Sections =
        {
            "Engine",
            "Lights",
            "Storage",
        };

        public static int GetSectionStartFromName(string name)
        {
            int sectionIndex = 0;
            for (var i = 0; i < Sections.Length; i++)
            {
                if (name == Sections[i])
                {
                    sectionIndex = i;
                }
            }

            return GetSectionStart(sectionIndex);
        }

        public const int TotalKeys = 25;

        public static int SectionLength => TotalKeys / Sections.Length;

        public static int GetSectionStart(int section)
        {
            return section * SectionLength;
        }
    }
}