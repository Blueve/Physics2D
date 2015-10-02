using System.Collections.Generic;

using System.Windows.Controls;
using System.Windows.Media;

namespace WPFDemo
{
    public class DrawingCanvas : Panel
    {
        private readonly List<Visual> _visuals = new List<Visual>();

        protected override Visual GetVisualChild(int index)
        {
            return _visuals[index];
        }

        protected override int VisualChildrenCount => _visuals.Count;

        public void AddVisual(Visual visual)
        {
            _visuals.Add(visual);

            AddVisualChild(visual);
            AddLogicalChild(visual);
        }
    }
}