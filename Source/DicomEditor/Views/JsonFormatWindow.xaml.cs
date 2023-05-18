using Newtonsoft.Json;
using System;
using System.Windows;

namespace DicomEditor.Views;

/// <summary>
/// Interaction logic for JsonFormatWindow.xaml
/// </summary>
public partial class JsonFormatWindow : Window
{
    public JsonFormatWindow()
    {
        InitializeComponent();
    }

    private void Format_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(this.src.Text)) return;
        try
        {
            var obj = JsonConvert.DeserializeObject(this.src.Text);
            this.target.Text = JsonConvert.SerializeObject(obj, Formatting.Indented);
        }
        catch (Exception ex)
        {
            this.target.Text = ex.Message;
        }
    }

    private void Paste_Click(object sender, RoutedEventArgs e)
    {
        this.src.Text = Clipboard.GetText();
    }

    private void Copy_Click(object sender, RoutedEventArgs e)
    {
        Clipboard.SetText(this.target.Text);
    }

    private void Clear_Click(object sender, RoutedEventArgs e)
    {
        this.src.Text = String.Empty;
        this.target.Text = String.Empty;
    }
}
