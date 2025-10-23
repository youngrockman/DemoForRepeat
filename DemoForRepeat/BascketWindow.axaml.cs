using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using DemoForRepeat.Models;
using Microsoft.EntityFrameworkCore;

namespace DemoForRepeat;

public partial class BascketWindow : Window
{
    private readonly int _currentUserId;
    private readonly int _orderId;
    
    public BascketWindow()
    {
        InitializeComponent();
        
    }

    public BascketWindow(int currentUserId, int orderId)
    {
        InitializeComponent();
        _currentUserId = currentUserId;
        _orderId = orderId;
        LoadOrderItems();
        
    }

    private async void LoadOrderItems()
    {
        var context = new DemoContext();
        var orderItems =  context.OrdersProducts.Where(x=>x.OrderId == _orderId).Include(x=>x.Product).Select(x=> new
        {
            x.Product,
            x.Quantity,
            TotalPrice = x.Product.Price * x.Quantity
        }).ToList();
        
        
        OrderItemsListBox.ItemsSource = orderItems;
    }

    private async void RemoveItem_Click(object? sender, RoutedEventArgs e)
    {
        var productId = (int)(sender as Button)!.Tag!;
        
        var context = new DemoContext();
        
        var orderItem = context.OrdersProducts.FirstOrDefault(x => x.ProductId == productId);

        if (orderItem != null)
        {
            context.OrdersProducts.Remove(orderItem);
            await context.SaveChangesAsync();
        }
        LoadOrderItems();
    }
    
    private void BackToProducts_Click(object? sender, RoutedEventArgs e)
    {
        var productWindow = new ProductWindow(_currentUserId);
        productWindow.Show();
        this.Close();
    }

    
    
    
}
