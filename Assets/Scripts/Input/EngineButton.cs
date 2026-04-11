using System;

namespace Input
{
    public readonly struct EngineButton
    {
        public EngineButton(int buttonIndex)
        {
            ButtonIndex = buttonIndex;
            SectionIndex = Math.Min(buttonIndex / DashboardLayout.SectionLength, DashboardLayout.Sections.Length - 1);
            InSectionIndex = buttonIndex % DashboardLayout.SectionLength;

            // Make overflow keys belong to last section
            if (buttonIndex >= DashboardLayout.TotalKeys - 1)
            {
                InSectionIndex = DashboardLayout.SectionLength + InSectionIndex;
            }
        }

        public int SectionIndex { get; }
        public int InSectionIndex { get; }
        public int ButtonIndex { get; }
    }
}