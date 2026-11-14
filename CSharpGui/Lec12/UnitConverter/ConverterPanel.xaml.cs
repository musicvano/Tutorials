using System.Windows;
using System.Windows.Controls;

namespace UnitConverter;

public partial class ConverterPanel : UserControl
{
    private bool updating;   // захист від взаємних викликів подій

    public ConverterPanel(Category category)
    {
        InitializeComponent();
        valueSlider.Minimum = category.Min;
        valueSlider.Maximum = category.Max;
        fromBox.ItemsSource = category.Units;
        toBox.ItemsSource = category.Units;
        fromBox.SelectedIndex = 0;
        toBox.SelectedIndex = 1;
        valueBox.Text = "1";
    }

    private void Slider_ValueChanged(object sender,
        RoutedPropertyChangedEventArgs<double> e)
    {
        if (updating)
        {
            return;
        }
        updating = true;
        valueBox.Text = $"{e.NewValue:0.#}";
        updating = false;
        Convert();
    }

    private void ValueBox_TextChanged(object sender,
        TextChangedEventArgs e)
    {
        if (!updating && double.TryParse(valueBox.Text, out double v))
        {
            updating = true;
            valueSlider.Value = v;   // Slider обмежує значення сам
            updating = false;
        }
        Convert();
    }

    private void Unit_SelectionChanged(object sender,
        SelectionChangedEventArgs e) => Convert();

    private void Swap_Click(object sender, RoutedEventArgs e) =>
        (fromBox.SelectedIndex, toBox.SelectedIndex) =
            (toBox.SelectedIndex, fromBox.SelectedIndex);

    private void Convert()
    {
        if (fromBox.SelectedItem is not Unit from
            || toBox.SelectedItem is not Unit to)
        {
            return;
        }
        if (!double.TryParse(valueBox.Text, out double value))
        {
            resultText.Text = "Enter a number";
            return;
        }
        double result = to.FromBase(from.ToBase(value));
        resultText.Text =
            $"{value:0.###} {from} = {result:0.###} {to}";
    }
}
