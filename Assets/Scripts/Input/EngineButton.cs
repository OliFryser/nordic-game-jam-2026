using System;

namespace Input
{
    public readonly struct EngineButton
    {
        public EngineButton(int buttonIndex)
        {
            DashboardLayout dashboardLayout = new DashboardLayout();
            ButtonIndex = buttonIndex;
            int sectionIndex = Math.Min(buttonIndex / dashboardLayout.SectionLength, dashboardLayout.SectionCount - 1);
            InSectionIndex = buttonIndex % dashboardLayout.SectionLength;
            Section = (DashboardSection)sectionIndex;

            // Make overflow keys belong to last section
            if (buttonIndex >= DashboardLayout.TotalKeys - 1)
            {
                InSectionIndex = new DashboardLayout().SectionLength + InSectionIndex;
            }
        }

        public DashboardSection Section { get; }
        public int InSectionIndex { get; }
        public int ButtonIndex { get; }
    }
}