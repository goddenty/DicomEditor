using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Media;

namespace DicomEditor.Models;

public class DicomStudyNode
{
    public string StudyInstanceUID { get; set; }
    public string SeriesInstanceUID { get; set; }
    public string SeriesNumber { get; set; }
    public string InstanceNumber { get; set; }
    public string FilePath { get; set; }
    public string StudyDescription { get; set; }
    public string SeriesDescription { get; set; }
    public bool IsSeries { get; set; }
    public string ImagesCount { get; set; }
    public string SeriesCount { get; set; }
    public ImageSource DicomImageSource { get; set; }
    public string Count
    {
        get
        {
            if (IsSeries)
            {
                return ImagesCount;
            }
            return SeriesCount;
        }
    }
    public string InstanceUID
    {
        get
        {
            if (IsSeries)
            {
                return SeriesInstanceUID;
            }
            return StudyInstanceUID;
        }
    }
    public string Description
    {
        get
        {
            if (IsSeries)
            {
                if (string.IsNullOrEmpty(FilePath))
                {
                    return SeriesDescription;
                }
                return Path.GetFileName(FilePath);
            }
            return StudyDescription;
        }
    }
    public ObservableCollection<DicomStudyNode> Children { get; set; }
    public DicomStudyNode()
    {
        Children = new ObservableCollection<DicomStudyNode>();
    }
}
