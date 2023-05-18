using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DicomEditor.Components;

//[TemplatePart(Name = "Main", Type = typeof(Border))]
public class DicomImageViewer : Control
{
    static DicomImageViewer()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(DicomImageViewer), new System.Windows.FrameworkPropertyMetadata(typeof(DicomImageViewer)));
    }


    public ObservableCollection<string> LeftTopCornerItems
    {
        get { return (ObservableCollection<string>)GetValue(LeftTopCornerItemsProperty); }
        set { SetValue(LeftTopCornerItemsProperty, value); }
    }

    // Using a DependencyProperty as the backing store for LeftTopCornerItems.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty LeftTopCornerItemsProperty =
        DependencyProperty.Register("LeftTopCornerItems", typeof(ObservableCollection<string>), typeof(DicomImageViewer), new PropertyMetadata(null));



    public ImageSource DicomImageSource
    {
        get { return (ImageSource)GetValue(DicomImageSourceProperty); }
        set { SetValue(DicomImageSourceProperty, value); }
    }

    // Using a DependencyProperty as the backing store for DicomImageSource.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty DicomImageSourceProperty =
        DependencyProperty.Register("DicomImageSource", typeof(ImageSource), typeof(DicomImageViewer), new PropertyMetadata(null));

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();
        //Border mainBorder = this.Template.FindName("Main", this) as Border;
    }



    public ObservableCollection<string> LeftBottomCornerItems
    {
        get { return (ObservableCollection<string>)GetValue(LeftBottomCornerItemsProperty); }
        set { SetValue(LeftBottomCornerItemsProperty, value); }
    }

    // Using a DependencyProperty as the backing store for LeftBottomCornerItems.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty LeftBottomCornerItemsProperty =
        DependencyProperty.Register("LeftBottomCornerItems", typeof(ObservableCollection<string>), typeof(DicomImageViewer), new PropertyMetadata(null));



}
