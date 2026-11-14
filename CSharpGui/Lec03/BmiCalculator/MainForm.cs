namespace BmiCalculator;

public partial class MainForm : Form
{
    private const decimal DefaultHeight = 170m, DefaultWeight = 65m;

    public MainForm()
    {
        InitializeComponent();
        ShowBmi();
    }

    // Один обробник для подій ValueChanged обох полів.
    private void inputs_ValueChanged(object sender, EventArgs e) =>
        ShowBmi();

    private void resetButton_Click(object sender, EventArgs e)
    {
        heightNumeric.Value = DefaultHeight;
        weightNumeric.Value = DefaultWeight;
    }

    private void ShowBmi()
    {
        double height = (double)heightNumeric.Value / 100;   // м
        double weight = (double)weightNumeric.Value;
        double bmi = weight / (height * height);

        var (category, color) = bmi switch
        {
            < 18.5 => ("Underweight", Color.LightSkyBlue),
            < 25 => ("Normal weight", Color.LightGreen),
            < 30 => ("Overweight", Color.Khaki),
            _ => ("Obesity", Color.LightCoral)
        };
        bmiLabel.Text = $"BMI: {bmi:F1}";
        categoryLabel.Text = category;
        categoryLabel.BackColor = color;

        double min = 18.5 * height * height;
        double max = 24.9 * height * height;
        rangeLabel.Text = $"Normal weight: {min:F1}–{max:F1} kg";
    }
}
