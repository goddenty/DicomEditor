using DicomEditor.Common;
using System;
using System.Linq;
using System.Windows;

namespace DicomEditor;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        DispatcherUnhandledException += App_DispatcherUnhandledException;
        if (e.Args.Length > 0)
        {
            String targetFileName = e.Args.FirstOrDefault();
            StartUpFileInfo.StartupFilename = targetFileName;
        }
    }

    private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
    {
        MessageBox.Show(e.Exception.Message);
        e.Handled = true;
    }
}
