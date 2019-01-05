namespace WPFDemo
{
    using System.Collections.Generic;
    using System.Windows.Controls;
    using System.Windows.Media;

    public class DrawingCanvas : Panel
    {
        private readonly List<Visual> visuals = new List<Visual>();

        protected override Visual GetVisualChild(int index)
        {
            return this.visuals[index];
        }

        protected override int VisualChildrenCount => this.visuals.Count;

        public void AddVisual(Visual visual)
        {
            this.visuals.Add(visual);

            this.AddVisualChild(visual);
            this.AddLogicalChild(visual);
        }
    }
}