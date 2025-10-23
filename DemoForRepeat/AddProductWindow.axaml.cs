using System;
using System.IO;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using DemoForRepeat.Models;

namespace DemoForRepeat
{

    public partial class AddProductWindow : Window
    {
        
        private readonly int userId;
        string imageName = Guid.NewGuid().ToString("N");
        
        
        public AddProductWindow()
        {
            InitializeComponent();
        }
        
        
        public AddProductWindow(int _currentUserId)
        {
            InitializeComponent();
            userId = _currentUserId;
            LoadManufacrturerComboBox();
        }
    
     

        private void Back_Click(object? sender, RoutedEventArgs e)
        {
            var productWindow = new ProductWindow(userId);
            productWindow.Show();
            this.Close();
        }

        private async void LoadManufacrturerComboBox()
        {
            var context = new DemoContext();
            var manufacturers =  context.Manufacturers.Select(x=>x.ManufacturerName).ToList();
            
            ManufacturerBox.ItemsSource = manufacturers;
        }

        private async void AddProduct_Click(object? sender, RoutedEventArgs e)
        {
            var context = new DemoContext();
            
            int.TryParse(PriceBox.Text, out int price);

            int? manufacturerId = null;

            if (ManufacturerBox.SelectedItem is string manufacturerName)
            {
                manufacturerId = context.Manufacturers.Where(x => x.ManufacturerName == manufacturerName).Select(x => x.ManufacturerId).FirstOrDefault();
            }
            
            var newProduct = new Product
            {   
                ProductName = NameBox.Text,
                Description = DecriptionBox.Text,
                Price = price,
                Image = imageName,
                ManufacturerId = manufacturerId
            };
            
            context.Products.Add(newProduct);
            await context.SaveChangesAsync();
            var productWindow = new ProductWindow(userId);
            productWindow.Show();
            this.Close();
         
        }

        private async void AddImage_Click(object? sender, RoutedEventArgs e)
        {
            
            
            var topLevel = TopLevel.GetTopLevel(this);

            var file = await topLevel.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
                {
                    Title = "Save Image",
                    FileTypeChoices = new[]
                    {
                        new FilePickerFileType("Изображения")
                        {
                            Patterns = new[] { "*.jpg", "*.jpeg", "*.png" },
                        }
                    }
                }
            );

            if (file != null)
            {
                NewImageBox.Source = new Bitmap(file.Path.LocalPath);
                string targetPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,imageName + Path.GetExtension(file.Name));
                File.Copy(file.Path.LocalPath, targetPath);
                imageName = targetPath;
            }
            
        }

       
        
        
    }
}