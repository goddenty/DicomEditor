using System;
using System.Text;
using System.Windows;

namespace DicomEditor.Views;

/// <summary>
/// Interaction logic for Base64Window.xaml
/// </summary>
public partial class Base64Window : Window
{
    public Base64Window()
    {
        InitializeComponent();
    }

    private void Encode_Click(object sender, RoutedEventArgs e)
    {
        string text = this.DecodeText.Text;
        this.EncodeText.Text = Convert.ToBase64String(Encoding.UTF8.GetBytes(text));
    }

    private void Decode_Click(object sender, RoutedEventArgs e)
    {
        string text = this.EncodeText.Text;
        this.DecodeText.Text = Encoding.UTF8.GetString(Convert.FromBase64String(text));
    }

    private void Copy_Click(object sender, RoutedEventArgs e)
    {
        Clipboard.SetText(this.EncodeText.Text);
    }

    private void Clear_Click(object sender, RoutedEventArgs e)
    {
        this.EncodeText.Text = String.Empty;
        this.DecodeText.Text = String.Empty;
    }

    private void Paste_Click(object sender, RoutedEventArgs e)
    {
        var pasteText = Clipboard.GetText();
        this.DecodeText.Text = pasteText;

    }
}
