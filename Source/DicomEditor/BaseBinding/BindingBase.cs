using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DicomEditor.BaseBinding;

public class BindingBase : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    protected void Set<T>(ref T t, T value, [CallerMemberName] String memberName = null)
    {
        t = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
    }

}
