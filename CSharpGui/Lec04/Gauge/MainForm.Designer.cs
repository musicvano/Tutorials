namespace Gauge;

partial class MainForm
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        gaugeControl1 = new GaugeControl();
        SuspendLayout();
        //
        // gaugeControl1
        //
        gaugeControl1.Location = new Point(40, 40);
        gaugeControl1.Name = "gaugeControl1";
        gaugeControl1.Size = new Size(240, 130);
        gaugeControl1.TabIndex = 0;
        gaugeControl1.Text = "gaugeControl1";
        gaugeControl1.Value = 65;
        //
        // MainForm
        //
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(320, 210);
        Controls.Add(gaugeControl1);
        Name = "MainForm";
        Text = "Gauge";
        ResumeLayout(false);
    }

    #endregion

    private GaugeControl gaugeControl1;
}
