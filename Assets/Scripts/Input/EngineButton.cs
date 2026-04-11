using System;

namespace Input
{
    public readonly struct EngineButton
    {
        public EngineButton(int buttonIndex)
        {
            ButtonIndex = buttonIndex;
            int sectionIndex = Math.Min(buttonIndex / DashboardLayout.SectionLength, DashboardLayout.SectionCount - 1);
            InSectionIndex = buttonIndex % DashboardLayout.SectionLength;
            Section = (DashboardSection)sectionIndex;

            // Make overflow keys belong to last section
            if (buttonIndex >= DashboardLayout.TotalKeys - 1)
            {
                InSectionIndex = DashboardLayout.SectionLength + InSectionIndex;
            }
        }

        public DashboardSection Section { get; }
        public int InSectionIndex { get; }
        public int ButtonIndex { get; }
    }
}