using System;

namespace Input
{
    public class DashboardLayout
    {
        public const int TotalKeys = 84;
        public const int StartingKey = 24;

        public int SectionLength => TotalKeys / SectionCount;
        public int SectionCount => Enum.GetValues(typeof(DashboardSection)).Length;
     
        public int GetSectionStartFromDashboardSection(DashboardSection section)
        {
            int sectionIndex = (int)section;
            return GetSectionStart(sectionIndex);
        }
        
        private int GetSectionStart(int section)
            => section * SectionCount + StartingKey;
    }
}