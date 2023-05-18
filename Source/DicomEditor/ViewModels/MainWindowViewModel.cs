using DicomEditor.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Media;

namespace DicomEditor.ViewModels;

class MainWindowViewModel : BaseBinding.BindingBase
{
    private ObservableCollection<DicomUIModel> _DicomModelList = new ObservableCollection<DicomUIModel>();
    private ObservableCollection<DicomUIModel> _DicomMetaDataList = new ObservableCollection<DicomUIModel>();
    public ObservableCollection<DicomUIModel> DicomModelList => _DicomModelList;
    public ObservableCollection<DicomUIModel> DicomMetaDataList => _DicomMetaDataList;


    private DicomUIModel _SelectDicomItem;
    public DicomUIModel SelectDicomItem
    {
        get => _SelectDicomItem;
        set => Set(ref _SelectDicomItem, value);
    }

    private DicomUIModel _SelectedDicomMetaItem;
    public DicomUIModel SelectedDicomMetaItem
    {
        get => _SelectedDicomMetaItem;
        set => Set(ref _SelectedDicomMetaItem, value);
    }
    private int _DataSetCount;
    public int DataSetCount
    {
        get => _DataSetCount;
        set => Set(ref _DataSetCount, value);
    }
    private int _MetaDataSetCount;
    public int MetaDataSetCount
    {
        get => _MetaDataSetCount;
        set => Set(ref _MetaDataSetCount, value);
    }

    private string _InputTag;
    public string InputTag
    {
        get => _InputTag;
        set => Set(ref _InputTag, value);
    }

    private string _StatusMsg;
    public string StatusMsg
    {
        get => _StatusMsg;
        set => Set(ref _StatusMsg, value);
    }

    private bool _PopOpen;
    public bool PopOpen
    {
        get => _PopOpen;
        set => Set(ref _PopOpen, value);
    }

    private bool _FilterPrivateFlag;
    public bool FilterPrivateFlag
    {
        get => _FilterPrivateFlag;
        set => Set(ref _FilterPrivateFlag, value);
    }

    public List<DicomUIModel> MemoryList = new List<DicomUIModel>();

    public MainWindowViewModel()
    {
    }

    public ObservableCollection<string> LeftTopItems { get; set; } = new ObservableCollection<string>();
    public ObservableCollection<string> LeftBottomItems { get; set; } = new ObservableCollection<string>();

    private ImageSource _DicomImageSource;
    public ImageSource DicomImageSource
    {
        get => _DicomImageSource;
        set => Set(ref _DicomImageSource, value);
    }
    public ObservableCollection<DicomStudyNode> DicomStudyNodeItems { get; set; } = new ObservableCollection<DicomStudyNode>();

    private int _ProgressValue;
    public int ProgressValue
    {
        get => _ProgressValue;
        set => Set(ref _ProgressValue, value);
    }

    private int _ProgressMax;
    public int ProgressMax
    {
        get => _ProgressMax;
        set => Set(ref _ProgressMax, value);
    }
}
