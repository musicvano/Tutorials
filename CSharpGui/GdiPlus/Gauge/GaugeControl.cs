using System.ComponentModel;

namespace Gauge;

// Півкругла шкала: дуга, довжина якої пропорційна Value.
public class GaugeControl : Control
{
    private int value;

    public GaugeControl()
    {
        SetStyle(ControlStyles.UserPaint
            | ControlStyles.AllPaintingInWmPaint
            | ControlStyles.OptimizedDoubleBuffer
            | ControlStyles.ResizeRedraw, true);
    }

    [Category("Gauge")]
    [Description("Current value from 0 to 100.")]
    [DefaultValue(0)]
    public int Value
    {
        get => value;
        set { this.value = Math.Clamp(value, 0, 100); Invalidate(); }
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        if (Width < 30 || Height < 30) return;
        var box = new Rectangle(10, 10, Width - 20, 2 * Height - 40);
        using var track = new Pen(Color.Gainsboro, 12);
        using var bar = new Pen(ForeColor, 12);
        e.Graphics.DrawArc(track, box, 180, 180);
        e.Graphics.DrawArc(bar, box, 180, 180f * value / 100);
    }
}
