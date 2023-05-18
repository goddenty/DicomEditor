using DicomEditor.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DicomEditor.ViewModels;

public class DicomStudyTreeViewModel : BaseBinding.BindingBase
{
    public ObservableCollection<DicomStudyNode> DicomStudyNodeItems { get; set; }
    public DicomStudyTreeViewModel(Dictionary<string, DicomStudy> dict)
    {
        DicomStudyNodeItems = new ObservableCollection<DicomStudyNode>();
        DicomStudyNode temp;
        foreach (var keyValuePair in dict)
        {
            temp = new DicomStudyNode
            {
                IsSeries = false,
                StudyDescription = keyValuePair.Value.StudyDescription,
                StudyInstanceUID = keyValuePair.Value.StudyInstanceUID,
                SeriesCount = keyValuePair.Value.DicomSeriesList.Count.ToString(),
            };
            foreach (var item in keyValuePair.Value.DicomSeriesList)
            {
                DicomStudyNode tempSeries = new DicomStudyNode
                {
                    SeriesDescription = item.SeriesDescription,
                    SeriesInstanceUID = item.SeriesInstanceUID,
                    IsSeries = true,
                    ImagesCount = item.DicomSeriesImgFiles.Count.ToString(),
                };
                foreach (var dcmImgFile in item.DicomSeriesImgFiles)
                {
                    tempSeries.Children.Add(new DicomStudyNode
                    {
                        FilePath = dcmImgFile.FilePath,
                        SeriesNumber = dcmImgFile.SeriesNumber,
                        InstanceNumber = dcmImgFile.InstanceNumber,
                        SeriesDescription = item.SeriesDescription,
                        SeriesInstanceUID = item.SeriesInstanceUID,
                        IsSeries = true,
                    });
                }
                temp.Children.Add(tempSeries);
            }
            DicomStudyNodeItems.Add(temp);
        }
    }
}
