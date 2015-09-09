using System.Collections.Generic;

using System.Windows.Controls;
using System.Windows.Media;

namespace WPFDemo
{
    public class DrawingCanvas : Panel
    {
        private List<Visual> visuals = new List<Visual>();

        protected override Visual GetVisualChild(int index)
        {
            return visuals[index];
        }

        protected override int VisualChildrenCount => visuals.Count;

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);
        }

        public void AddVisual(Visual visual)
        {
            visuals.Add(visual);

            base.AddVisualChild(visual);
            base.AddLogicalChild(visual);
        }
    }
}