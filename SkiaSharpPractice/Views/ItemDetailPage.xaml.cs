using SkiaSharpPractice.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace SkiaSharpPractice.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}