using DicomEditor.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace DicomEditor.Views;

/// <summary>
/// Interaction logic for AllTagQuery.xaml
/// </summary>
public partial class AllTagsQueryWindow : Window
{
    public AllTagsQueryWindow()
    {
        InitializeComponent();
    }

    private async void tbTagValue_TextChanged(object sender, TextChangedEventArgs e)
    {
        var vm = this.DataContext as AllTagsQueryWindowViewModel;
        var tb = sender as TextBox;
        await vm.QueryTags(tb.Text).ConfigureAwait(false);
    }

    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
        var vm = this.DataContext as AllTagsQueryWindowViewModel;
        vm.DisposeTagsStore();
    }
}
