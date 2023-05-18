using DicomEditor.ViewModels;
using System.Windows;

namespace DicomEditor.Views;

/// <summary>
/// Interaction logic for DicomStudyTree.xaml
/// </summary>
public partial class DicomStudyTree : Window
{
    public DicomStudyTree()
    {
        InitializeComponent();
    }
    public DicomStudyTree(DicomStudyTreeViewModel viewModel) : this()
    {
        this.DataContext = viewModel;
    }


}
