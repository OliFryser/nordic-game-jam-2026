using System;
using System.Linq;

namespace Input
{
    public static class DashboardLayout
    {
        public static int GetSectionStartFromName(DashboardSection section)
        {
            int sectionIndex = (int)section;
            return GetSectionStart(sectionIndex);
        }

        public const int TotalKeys = 25;

        public static int SectionLength => TotalKeys / SectionCount;
        public static int SectionCount => Enum.GetValues(typeof(DashboardSection)).Length;
        
        public static int GetSectionStart(int section)
        {
            return section * SectionLength;
        }
    }
}