using DicomEditor.Models;
using FellowOakDicom;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DicomEditor.Utils;

public static class DicomFileUtil
{

    public static int GetFilesCount(string dir)
    {
        var files = GetFilesInternal(dir);
        return files.Length;
    }
    private static SemaphoreSlim SemaphoreSlim = new SemaphoreSlim(50);
    public static async Task<DicomStudy> ReadDicomFile(string file)
    {
        try
        {
            await SemaphoreSlim.WaitAsync();
            var tempDfile = await DicomFile.OpenAsync(file, FileReadOption.SkipLargeTags);
            if (!tempDfile.Dataset.TryGetString(DicomTag.StudyInstanceUID, out var studyInstanceUID))
            {

            }
            //获取SeriesInstanceUID
            if (!tempDfile.Dataset.TryGetString(DicomTag.SeriesInstanceUID, out var seriesInstanceUID))
            {

            }
            //SeriesNumber:顺序编号
            if (!tempDfile.Dataset.TryGetString(DicomTag.SeriesNumber, out var seriesNumber))
            {

            }
            if (!tempDfile.Dataset.TryGetString(DicomTag.InstanceNumber, out var instanceNumber))
            {

            }
            if (!tempDfile.Dataset.TryGetString(DicomTag.StudyDescription, out var studyDescription))
            {

            }
            if (!tempDfile.Dataset.TryGetString(DicomTag.SeriesDescription, out var seriesDescription))
            {

            }

            var study = new DicomStudy();
            study.StudyInstanceUID = studyInstanceUID;
            study.StudyDescription = studyDescription;


            var dcmSeriesFile = new DicomSeriesFile
            {
                FilePath = file,
                SeriesNumber = seriesNumber,
                InstanceNumber = instanceNumber,
            };
            //try
            //{
            //    Application.Current.Dispatcher.Invoke(() =>
            //    {
            //        DicomImage dicomImage = new DicomImage(tempDfile.Dataset, 0);
            //        var imageSource = dicomImage?.RenderImage()?.AsWriteableBitmap();
            //        dcmSeriesFile.DicomImageSource = imageSource;
            //    });

            //}
            //catch (Exception ex)
            //{

            //}



            var tempDicomSeries = study.DicomSeriesList.FirstOrDefault(s => s.SeriesInstanceUID == seriesInstanceUID);
            if (tempDicomSeries == null)
            {
                tempDicomSeries = new DicomSeries
                {
                    SeriesDescription = seriesDescription,
                    SeriesInstanceUID = seriesInstanceUID
                };
                study.DicomSeriesList.Add(tempDicomSeries);
            }
            tempDicomSeries.DicomSeriesImgFiles.Add(dcmSeriesFile);

            return study;
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            SemaphoreSlim.Release();
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="dir">key: Study instance UID</param>
    /// <param name="progressCallback"></param>
    /// <param name="errorCallback"></param>
    /// <returns></returns>
    public static async Task<Dictionary<string, DicomStudy>> ReadFilesFromDictory(string dir, Action<(int index, string fileName)> progressCallback = null, Action<string> errorCallback = null)
    {
        var files = GetFilesInternal(dir);
        Dictionary<string, DicomStudy> dict1 = new();
        DicomStudy study = null;
        bool error = false;
        int errorFiles = 0;
        List<Task<DicomStudy>> taskList = new List<Task<DicomStudy>>();
        for (int i = 0; i < files.Length; i++)
        {
            var file = files[i];
            DicomFile tempDfile = null;
            try
            {
                var dicomStudyTask = ReadDicomFile(file);
                taskList.Add(dicomStudyTask);
            }
            catch (Exception)
            {

                continue;
            }
        }

        var dicomFiles = await Task.WhenAll(taskList);
        foreach (var studyItem in dicomFiles)
        {
            if (!dict1.ContainsKey(studyItem.StudyInstanceUID))
            {
                dict1.Add(studyItem.StudyInstanceUID, studyItem);
                continue;
            }
            var tempStudy = dict1[studyItem.StudyInstanceUID];
            var tempSereis = tempStudy.DicomSeriesList.FirstOrDefault(i => i.SeriesInstanceUID.Equals(studyItem.DicomSeriesList.FirstOrDefault().SeriesInstanceUID));
            if (tempSereis != null)
            {
                tempSereis.DicomSeriesImgFiles.AddRange(studyItem.DicomSeriesList.FirstOrDefault().DicomSeriesImgFiles);
                continue;
            }
            tempStudy.DicomSeriesList.AddRange(studyItem.DicomSeriesList);

        }
        return dict1;
    }

    private static string[] GetFilesInternal(string dir)
    {
        return Directory.GetFiles(dir, "*.dcm", SearchOption.AllDirectories);
    }
}
