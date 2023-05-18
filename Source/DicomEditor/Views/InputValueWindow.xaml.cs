using System;
using System.Windows;

namespace DicomEditor.Views;

/// <summary>
/// Interaction logic for InputValueWindow.xaml
/// </summary>
public partial class InputValueWindow : Window
{
    public String TargetValue { get; set; }
    public InputValueWindow(String tagValue)
    {
        InitializeComponent();
        this.DataContext = new ViewModels.InputValueWindowViewModel();
        (this.DataContext as ViewModels.InputValueWindowViewModel).TagValue = tagValue;
    }

    private void Cancel_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
    }

    private void OK_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = true;
        TargetValue = (this.DataContext as ViewModels.InputValueWindowViewModel).TagValue;
    }
}
