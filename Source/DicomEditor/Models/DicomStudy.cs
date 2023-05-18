using System.Collections.Generic;
using System.Windows.Media;

namespace DicomEditor.Models;

public class DicomStudy
{
    public string StudyDescription { get; set; }
    public string StudyInstanceUID { get; set; }
    public string AccessNumber { get; set; }
    public List<DicomSeries> DicomSeriesList { get; set; }
    public DicomStudy()
    {
        DicomSeriesList = new List<DicomSeries>();
    }
}

public class DicomSeries
{
    public string SeriesInstanceUID { get; set; }
    public string SeriesDescription { get; set; }
    public List<DicomSeriesFile> DicomSeriesImgFiles { get; set; }
    public DicomSeries()
    {
        DicomSeriesImgFiles = new List<DicomSeriesFile>();
    }

}
public class DicomSeriesFile
{

    public string SeriesNumber { get; set; }
    public string InstanceNumber { get; set; }
    public string FilePath { get; set; }
    public ImageSource DicomImageSource { get; set; }
}
