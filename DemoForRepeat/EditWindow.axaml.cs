using System;
using System.IO;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using DemoForRepeat.Models;

namespace DemoForRepeat;

public partial class EditWindow : Window
{
    private readonly int _currentUserId;
    private readonly Product _product;
    private string imageName;
    
    
    public EditWindow()
    {
        InitializeComponent();
    }

    public EditWindow(Product product, int currentUserId)
    {
        InitializeComponent();
        _currentUserId = currentUserId;
        _product = product;
        
        imageName = _product.Image ?? Guid.NewGuid().ToString("N");
        
        LoadManufacrturerComboBox(); 
        GetInfo();
    }


    private void Back_Click(object? sender, RoutedEventArgs e)
    {
        var catalogWindow = new ProductWindow(_currentUserId);
        catalogWindow.Show();
        this.Close();
    }

    private void GetInfo()
    {
        var context = new DemoContext();
        
        NameBox.Text = _product.ProductName;
        DecriptionBox.Text = _product.Description;
        PriceBox.Text = _product.Price.ToString();
        NewImageBox.Source = new Bitmap(_product.Image);
        
        
    }
    
    

    private async void SaveProduct_Click(object? sender, RoutedEventArgs e)
    {
        int.TryParse(PriceBox.Text, out int price);
    
        using var context = new DemoContext();
    
        int? manufacturerId = null;

        if (ManufacturerBox.SelectedItem is string manufacturerName)
        {
            var result = context.Manufacturers.Where(x => x.ManufacturerName == manufacturerName).Select(x => x.ManufacturerId).FirstOrDefault();
            manufacturerId = result;
        }
    
        
        var productToUpdate = context.Products.FirstOrDefault(p => p.ProductId == _product.ProductId);
    
        if (productToUpdate != null)
        {
            productToUpdate.ProductName = NameBox.Text;
            productToUpdate.Description = DecriptionBox.Text;
            productToUpdate.Price = price;
            productToUpdate.ManufacturerId = manufacturerId;
            productToUpdate.Image = imageName;
        
            await context.SaveChangesAsync();
        }
    
        var catalogWindow = new ProductWindow(_currentUserId);
        catalogWindow.Show();
        this.Close();
    }

    

    private async void AddImage_Click(object? sender, RoutedEventArgs e)
    {
        var topLevel = TopLevel.GetTopLevel(this);

        var file = await topLevel.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
        {
            Title = "Выбор изображения на замену",
            FileTypeChoices = new []
            {
                FilePickerFileTypes.All
            }
        });

        if (file != null)
        {
            NewImageBox.Source = new Bitmap(file.Path.LocalPath);
            var targetPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, imageName + Path.GetExtension(file.Name));
            File.Copy(file.Path.LocalPath, targetPath);
            imageName = targetPath;
        }

        
    }
    
    private async void LoadManufacrturerComboBox()
    {
        var context = new DemoContext();
        var manufacturers = context.Manufacturers.Select(x => x.ManufacturerName).ToList();
        ManufacturerBox.ItemsSource = manufacturers;

 
        var manufacturerName = context.Manufacturers.Where(x => x.ManufacturerId == _product.ManufacturerId).Select(x => x.ManufacturerName).FirstOrDefault();

        ManufacturerBox.SelectedItem = manufacturerName;
    }

    
}
