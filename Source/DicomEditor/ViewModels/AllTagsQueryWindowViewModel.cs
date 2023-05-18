using DicomEditor.BaseBinding;
using DicomEditor.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace DicomEditor.ViewModels;

public class AllTagsQueryWindowViewModel : BindingBase
{
    public string InputTag { get; set; }
    public ObservableCollection<DicomTagModel> AllTagList { get; set; } = new ObservableCollection<DicomTagModel>();
    private readonly Dictionary<string, DicomTagModel> tagStore = new Dictionary<string, DicomTagModel>();

    public ICommand CopyRecordCommand { get; set; } 

    public DicomTagModel SelectedTag { get; set; }

    private string _Count;
    public string Count
    {
        get => _Count;
        set=>Set(ref  _Count, value);
    }

    public AllTagsQueryWindowViewModel()
    {
        CopyRecordCommand = new DelegateCommand(CopyRecordCommandHandler);
        LoadTagStore().GetAwaiter().GetResult();
        InitTagsList();
    }

    private void CopyRecordCommandHandler()
    {
        if (SelectedTag == null) return;
        Clipboard.SetText($"Tag|{SelectedTag.Tag}|VR|{SelectedTag.VR}|Name|{SelectedTag.Name}|");
    }

    public async Task QueryTags(string searchText)
    {
        if (string.IsNullOrWhiteSpace(searchText))
        {
            InitTagsList();
        }
        else
        {
            var actualSearchText = searchText.Trim('(', ')');
            if (actualSearchText.Contains(','))
            {
                var resultArray = actualSearchText.Split(',');
                actualSearchText = resultArray[0]+resultArray[1];
                
            }
            if (string.IsNullOrWhiteSpace(actualSearchText)) return;
            AllTagList.Clear();
            var queryResult= tagStore.Where(kv => kv.Key.Contains(actualSearchText));
            foreach (var kv in queryResult)
            {
                AllTagList.Add(kv.Value);
            }
            UpdateCount();
        }
    }

    private void InitTagsList()
    {
        AllTagList.Clear();
        foreach (var tag in tagStore.Values)
        {
            AllTagList.Add(tag);
        }
        UpdateCount();
    }

    private void UpdateCount()
    {
        Count = AllTagList.Count.ToString();
    }

    private async Task LoadTagStore()
    {
        if (tagStore.Count > 0) return;
        var filePath = "./Resources/AllDicomTags.txt";
        var fileContent = await File.ReadAllTextAsync(filePath).ConfigureAwait(false);
        var contentLins = fileContent.Split("\r\n");

        DicomTagModel tagInfo = null;

        foreach (var line in contentLins)
        {
            var threeParts = line.Split('\t');
            var tag1 = threeParts[0].Substring(1, 4);
            var tag2 = threeParts[0].Substring(6, 4);
            var tag = tag1 + tag2;

            tagInfo = new DicomTagModel(tag, threeParts[1], threeParts[2]);
            tagStore.Add(tag, tagInfo);
        }
    }

    public void DisposeTagsStore()
    {
        tagStore.Clear();
        AllTagList.Clear();
    }

}

public class DelegateCommand : ICommand
{
    private readonly Action handler;

    public event EventHandler CanExecuteChanged;

    public DelegateCommand(Action handler)
    {
        this.handler = handler;
    }

    public bool CanExecute(object parameter)
    {
        // Add your logic here to determine if the command can be executed
        return true;
    }

    public void Execute(object parameter)
    {
        // Add your logic here to be executed when the command is invoked
        handler();
    }
}
