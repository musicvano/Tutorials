using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;

namespace Catalog;

public record Product(string Name, string Category,
    decimal Price, int Stock);

public class CatalogViewModel : ObservableObject
{
    public CatalogViewModel()
    {
        Products =
        [
            new("Laptop 14\"", "Computers", 32999m, 4),
            new("Mouse", "Accessories", 649.5m, 25),
            new("Monitor 27\"", "Computers", 11499m, 0),
            new("Keyboard", "Accessories", 1299m, 12),
            new("USB-C Hub", "Accessories", 899m, 7)
        ];
        ProductsView = CollectionViewSource.GetDefaultView(Products);
        ProductsView.Filter = Matches;
        ProductsView.GroupDescriptions.Add(
            new PropertyGroupDescription(nameof(Product.Category)));
        ProductsView.SortDescriptions.Add(new SortDescription(
            nameof(Product.Name), ListSortDirection.Ascending));
    }

    public ObservableCollection<Product> Products { get; }

    public ICollectionView ProductsView { get; }

    public string SearchText
    {
        get;
        set
        {
            if (SetProperty(ref field, value))
            {
                ProductsView.Refresh();   // застосувати фільтр
            }
        }
    } = "";

    public Product? SelectedProduct
    {
        get;
        set => SetProperty(ref field, value);
    }

    private bool Matches(object item) =>
        item is Product p && p.Name.Contains(SearchText,
            StringComparison.CurrentCultureIgnoreCase);
}
