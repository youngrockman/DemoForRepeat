using System.Linq;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using DemoForRepeat.Models;
using Microsoft.EntityFrameworkCore;
using MsBox.Avalonia;

namespace DemoForRepeat;

public partial class ProductWindow : Window
{
    private readonly int _currentUserId;
    
    public ProductWindow()
    {
        InitializeComponent();
    }
    
    public ProductWindow(int userUserId)
    {
        InitializeComponent();
        _currentUserId = userUserId;
         Get();
        LoadComboBox();
    }


    private void LoadComboBox()
    {
        var context = new DemoContext();
        var manufacturers = context.Manufacturers.Select(x => x.ManufacturerName).ToList();
        manufacturers.Add("Все производители");
        SortBox.ItemsSource = manufacturers.OrderByDescending(x=>x == "Все производители");
        
    }

    private async void Get()
    {
        var context = new DemoContext();

        var products = await context.Products.Include(x=>x.Manufacturer).ToListAsync();

        switch (OrderBox.SelectedIndex)
        {
            case 0:
                 products = context.Products.OrderBy(x=>x.Price).ToList();
                break;
            case 1:
                products = context.Products.OrderByDescending(x => x.Price).ToList();
                break;
        }

        
        
        if (!string.IsNullOrWhiteSpace(SearchBox.Text))
        {   
            var searchText = SearchBox.Text.ToLower();
            products = await context.Products.Where(x=>x.ProductName.ToLower().Contains(searchText)).ToListAsync();
        }

        if (SortBox.SelectedItem != null && SortBox.SelectedItem.ToString() != "Все производители")
        {
            var manufacturer = SortBox.SelectedItem.ToString();
            products = products.Where(x => x.Manufacturer?.ManufacturerName == manufacturer).ToList();
        }
        
        
        
        ProductBox.ItemsSource = products;
    }

    private void OrderBox_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        Get();
    }

    private void SearchBox_OnKeyUp(object? sender, KeyEventArgs e)
    {
        Get();
    }

    private void SortBox_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        Get();
    }

    private void Back_Button(object? sender, RoutedEventArgs e)
    {
        var mainWindow = new MainWindow();
        mainWindow.Show();
        this.Close();
    }

    private async void DeleteProduct_Click(object? sender, RoutedEventArgs e)
    {
        var context = new DemoContext();
        var productId = (int)(sender as Button)!.Tag!;

        var selectProduct = context.Products.FirstOrDefault(x => x.ProductId == productId);

        if (selectProduct != null)
        {
            context.Products.Remove(selectProduct);
            await context.SaveChangesAsync();
        }
        
        Get();

    }

    private void AddProdcutClick(object? sender, RoutedEventArgs e)
    {
        var addProductWindow = new AddProductWindow(_currentUserId);
        addProductWindow.Show();
        this.Close();
    }

    private void ProductListBox_Click(object? sender, TappedEventArgs tappedEventArgs)
    {
        if (ProductBox.SelectedItem is Product product)
        {
            var editWindow = new EditWindow(product, _currentUserId);
            editWindow.Show();
            this.Close();
        }
    }
}