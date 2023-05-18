using System;

namespace DicomEditor.ViewModels;

class InputValueWindowViewModel : BaseBinding.BindingBase
{
    private string _TagValue;
    public string TagValue
    {
        get => _TagValue;
        set => Set(ref _TagValue, value);
    }
}
