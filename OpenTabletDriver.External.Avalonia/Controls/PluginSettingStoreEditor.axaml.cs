using System;
using Avalonia.Controls;
using OpenTabletDriver.External.Avalonia.ViewModels;

namespace OpenTabletDriver.External.Avalonia.Controls;

#nullable enable

public partial class PluginSettingStoreEditor : UserControl
{
    // --------------------------------- Constructor --------------------------------- //

    public PluginSettingStoreEditor()
    {
        InitializeComponent();
    }

    protected override void OnDataContextChanged(EventArgs e)
    {
        base.OnDataContextChanged(e);

        if (DataContext is PluginSettingStoreEditorViewModel vm)
        {
            
        }
    }
}
