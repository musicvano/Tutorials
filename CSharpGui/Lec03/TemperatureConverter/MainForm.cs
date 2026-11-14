using System.ComponentModel;

namespace TemperatureConverter;

public partial class MainForm : Form
{
    private const double AbsoluteZero = -273.15;

    public MainForm()
    {
        InitializeComponent();
    }

    private void celsiusTextBox_Validating(object sender,
        CancelEventArgs e)
    {
        if (double.TryParse(celsiusTextBox.Text, out double c)
            && c >= AbsoluteZero)
        {
            errorProvider.SetError(celsiusTextBox, "");
        }
        else
        {
            errorProvider.SetError(celsiusTextBox,
                $"Enter a number not less than {AbsoluteZero}");
            e.Cancel = true;        // фокус залишається в полі
        }
    }

    private void convertButton_Click(object sender, EventArgs e)
    {
        if (!ValidateChildren())    // перевірити всі поля форми
        {
            return;
        }
        double celsius = double.Parse(celsiusTextBox.Text);
        double fahrenheit = celsius * 9 / 5 + 32;
        double kelvin = celsius - AbsoluteZero;
        resultLabel.Text = $"{fahrenheit:F1} °F    {kelvin:F2} K";
    }
}
