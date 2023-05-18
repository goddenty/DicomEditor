using DicomEditor.Common;
using DicomEditor.Models;
using DicomEditor.Utils;
using DicomEditor.ViewModels;
using DicomEditor.Views;
using FellowOakDicom;
using FellowOakDicom.Imaging;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DicomEditor;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private MainWindowViewModel vmodel;
    private DicomFile dfile;



    public MainWindow()
    {
        InitializeComponent();
        vmodel = new MainWindowViewModel();
        this.Loaded += MainWindow_Loaded;
        this.DataContext = vmodel;
        this.AllowDrop = true;
        this.Drop += MainWindow_Drop;
        new DicomSetupBuilder()
.RegisterServices(s => s.AddFellowOakDicom().AddImageManager<WPFImageManager>())
.Build();
    }

    private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
    {
        if (String.IsNullOrWhiteSpace(StartUpFileInfo.StartupFilename)) return;
        await HandleFileOpen(StartUpFileInfo.StartupFilename);
    }


    private async void MainWindow_Drop(object sender, DragEventArgs e)
    {
        if (e.Data.GetDataPresent(DataFormats.FileDrop))
        {
            // Note that you can have more than one file.
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

            // Assuming you have one file that you care about, pass it off to whatever
            // handling code you have defined.
            if (File.Exists(files[0]))
            {
                await HandleFileOpen(files[0]);
            }
            else if (Directory.Exists(files[0]))
            {
                await HandleDirectoryOpen(files[0]);
            }
        }
    }

    private async Task HandleDirectoryOpen(string selectDir)
    {
        bool error = false;
        int errorFiles = 0;
        vmodel.ProgressValue = 0;
        vmodel.ProgressMax = DicomFileUtil.GetFilesCount(selectDir);
        if (vmodel.ProgressMax == 0) return;
        var sw = Stopwatch.StartNew();
        var dcmDict = await DicomFileUtil.ReadFilesFromDictory(selectDir, (data) =>
        {
        });
        sw.Stop();
        vmodel.StatusMsg = $"Open folder consumes: {sw.Elapsed.TotalSeconds};";
        DicomStudyNode temp;
        foreach (var keyValuePair in dcmDict)
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
            vmodel.DicomStudyNodeItems.Add(temp);

            if (error)
            {
                MessageBox.Show($"Error files count: {errorFiles}");
            }
        }
    }

    private async Task HandleFileOpen(string selectFile)
    {
        try
        {
            vmodel.StatusMsg = "Loading...";
            dfile = await DicomFile.OpenAsync(selectFile);
            this.Title = $"Dicom Editor -- {System.IO.Path.GetFileName(selectFile)}";
            Init();

        }
        catch (Exception ex)
        {
            vmodel.StatusMsg = "Loading error";
            MessageBox.Show(ex.Message);
        }
    }

    private void BtnFind_Click(object sender, RoutedEventArgs e)
    {
        if (dfile == null) return;
        var tag = vmodel.InputTag;
        if (string.IsNullOrWhiteSpace(tag))
        {
            Init();
            return;
        }
        var tagname = DicomTag.Parse(tag);
        string tagValue = null;
        if (dfile.Dataset.TryGetString(tagname, out tagValue) || dfile.FileMetaInfo.TryGetString(tagname, out tagValue))
        {
            vmodel.DicomModelList.Clear();
            vmodel.DicomModelList.Add(new Models.DicomUIModel
            {
                TagName = tagname.ToString(),
                TagValue = tagValue,
                Description = tagname.DictionaryEntry.Name
            });
        }
    }

    private void BtnChange_Click(object sender, RoutedEventArgs e)
    {
        if (vmodel.SelectDicomItem == null) return;
        if (dfile == null) return;
        InputValueWindow win = new InputValueWindow(vmodel.SelectDicomItem.TagValue);
        if (win.ShowDialog() == true)
        {
            var tag = DicomTag.Parse(vmodel.SelectDicomItem.TagName);
            if (dfile.Dataset.TryGetString(tag, out string tvalue))
            {
                dfile.Dataset.AddOrUpdate(tag, win.TargetValue);
            }
            else if (dfile.FileMetaInfo.TryGetString(tag, out tvalue))
            {
                dfile.FileMetaInfo.AddOrUpdate(tag, win.TargetValue);
            }
            Init();
            vmodel.StatusMsg = "Modify ok";
        }
    }

    private async void BtnSave_Click(object sender, RoutedEventArgs e)
    {
        if (dfile == null) return;
        SaveFileDialog dia = new SaveFileDialog();
        dia.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        dia.DefaultExt = "dcm";
        dia.Filter = "DICOM|*.dcm";
        if (dia.ShowDialog() == true)
        {
            try
            {
                await dfile.SaveAsync(dia.FileName);
                vmodel.StatusMsg = "Save ok";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }

    private async void BtnLoad_Click(object sender, RoutedEventArgs e)
    {
        OpenFileDialog dia = new OpenFileDialog();
        dia.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
        dia.DefaultExt = "dcm";
        dia.Filter = "DICOM|*.dcm;*.IMA";

        if (dia.ShowDialog() == true)
        {
            var selectFile = dia.FileName;
            await HandleFileOpen(selectFile);
        }
    }

    private IList<DicomUIModel> GetDataSet(DicomDataset dDataSet)
    {
        var list = new List<DicomUIModel>();
        foreach (var pi in dDataSet)
        {
            var tempDSet = dDataSet;
            if (pi is DicomSequence ds)
            {
                foreach (var di in ds)
                {
                    tempDSet = di;
                    GetDataSet(di);
                }
            }
            var target = new Models.DicomUIModel()
            {
                TagName = pi.Tag.ToString(),
                Code = pi.Tag.DictionaryEntry.ValueRepresentations[0].Code,
                Description = pi.Tag.DictionaryEntry.Name
            };

            if (pi.Tag.IsPrivate && vmodel.FilterPrivateFlag)
            {
                continue;
            }

            if (pi.Tag.DictionaryEntry.ValueRepresentations[0].IsString)
            {
                if (tempDSet.TryGetSingleValue(pi.Tag, out String objValue))
                {
                    target.TagValue = objValue;
                }
            }
            else if (pi.Tag.DictionaryEntry.ValueRepresentations[0].Is16bitLength)
            {
                if (tempDSet.TryGetSingleValue(pi.Tag, out long value))
                {
                    target.TagValue = value.ToString();
                }
            }
            else if (pi.Tag.DictionaryEntry.ValueRepresentations[0].IsMultiValue)
            {
                if (tempDSet.TryGetValues<string>(pi.Tag, out var value))
                {
                    target.TagValue = String.Join(',', value);
                }
            }
            list.Add(target);
        }
        return list;
    }

    private void Init()
    {
        try
        {

            vmodel.DicomModelList.Clear();
            vmodel.MemoryList.Clear();
            vmodel.DicomMetaDataList.Clear();

            var dataSetList = GetDataSet(dfile.Dataset);
            foreach (var item in dataSetList)
            {
                vmodel.DicomModelList.Add(item);
                vmodel.MemoryList.Add(item);
            }
            vmodel.DataSetCount = dataSetList.Count;

            var metaDataSetList = GetDataSet(dfile.FileMetaInfo);
            foreach (var item in metaDataSetList)
            {
                vmodel.DicomMetaDataList.Add(item);
            }
            vmodel.MetaDataSetCount = metaDataSetList.Count;

            vmodel.StatusMsg = "Loading over";

            vmodel.DicomImageSource = CreateDicomImageSource(dfile.Dataset);
            vmodel.LeftTopItems.Clear();
            vmodel.LeftBottomItems.Clear();
            vmodel.LeftTopItems.Add($"PatientName {dfile.Dataset.GetString(DicomTag.PatientName)}");
            vmodel.LeftTopItems.Add($"PatientAge  {dfile.Dataset.GetString(DicomTag.PatientAge)}");
            vmodel.LeftTopItems.Add($"PatientSex  {dfile.Dataset.GetString(DicomTag.PatientSex)}");
            vmodel.LeftTopItems.Add($"StudyDescription  {dfile.Dataset.GetString(DicomTag.StudyDescription)}");
            vmodel.LeftBottomItems.Add($"StudyDate  {dfile.Dataset.GetString(DicomTag.StudyDate)}");
            vmodel.LeftBottomItems.Add($"Window Width  {dfile.Dataset.GetString(DicomTag.WindowWidth)}");
            vmodel.LeftBottomItems.Add($"WindowCenter  {dfile.Dataset.GetString(DicomTag.WindowCenter)}");
            vmodel.LeftBottomItems.Add($"Study UID   {dfile.Dataset.GetString(DicomTag.StudyInstanceUID)}");
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    private ImageSource CreateDicomImageSource(DicomDataset dataset)
    {

        var dcmImage = new DicomImage(dataset, 0);
        var bitmap = dcmImage.RenderImage().AsWriteableBitmap();
        return bitmap;
    }

    private void BtnClose_Click(object sender, RoutedEventArgs e)
    {
        dfile = null;
        vmodel.DicomModelList.Clear();
    }

    private void tbTagValue_TextChanged(object sender, TextChangedEventArgs e)
    {
        var tb = sender as TextBox;
        vmodel.DicomModelList.Clear();
        if (string.IsNullOrWhiteSpace(tb.Text))
        {
            vmodel.MemoryList.ForEach(item =>
            {
                vmodel.DicomModelList.Add(item);
            });
            return;
        }
        var result = vmodel.MemoryList.Where(item => item.TagName.Contains(tb.Text, StringComparison.OrdinalIgnoreCase) || item.Description.Contains(tb.Text, StringComparison.OrdinalIgnoreCase));
        if (result.Count() > 0)
        {

            foreach (var item in result)
            {
                vmodel.DicomModelList.Add(item);
            }
        }
    }

    private void Base64_Click(object sender, RoutedEventArgs e)
    {
        this.Hide();
        Base64Window window = new Base64Window();
        window.ShowDialog();
        this.Show();
    }

    private void Json_Click(object sender, RoutedEventArgs e)
    {
        this.Hide();
        JsonFormatWindow window = new JsonFormatWindow();
        window.ShowDialog();
        this.Show();
    }
    //Tree Item Selected Changed
    private async void DicomStudyOverView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
    {
        var newValue = e.NewValue as DicomStudyNode;
        if (string.IsNullOrEmpty(newValue.FilePath)) return;
        await HandleFileOpen(newValue.FilePath);
    }

    private void TagsQuery_Click(object sender, RoutedEventArgs e)
    {
        this.Hide();
        var window = new AllTagsQueryWindow()
        {
            DataContext = new AllTagsQueryWindowViewModel()
        };
        window.ShowDialog();
        this.Show();
    }

}
