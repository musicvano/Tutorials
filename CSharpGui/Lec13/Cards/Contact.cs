using System.Windows;
using System.Windows.Controls;

namespace Cards;

public record Contact(string Name, string Phone, string? Company);

// Обирає шаблон картки за даними, а не за типом.
public class ContactTemplateSelector : DataTemplateSelector
{
    public DataTemplate? PersonTemplate { get; set; }

    public DataTemplate? CompanyTemplate { get; set; }

    public override DataTemplate? SelectTemplate(object item,
        DependencyObject container) =>
        item is Contact { Company: not null }
            ? CompanyTemplate : PersonTemplate;
}
