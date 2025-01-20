using CursorAutoFree.UI.ViewModels;

namespace CursorAutoFree.UI.Views;

public partial class MainPage : ContentPage
{
    public MainPage(MainViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
} 