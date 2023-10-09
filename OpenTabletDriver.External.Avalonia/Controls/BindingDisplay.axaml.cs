using System;
using Avalonia.Controls;
using OpenTabletDriver.External.Avalonia.ViewModels;
using OpenTabletDriver.External.Avalonia.Views;

namespace OpenTabletDriver.External.Avalonia.Controls;

public partial class BindingDisplay : UserControl
{
    // --------------------------------- Constructor --------------------------------- //

    public BindingDisplay()
    {
        InitializeComponent();
    }

    protected override void OnDataContextChanged(EventArgs e)
    {
        base.OnDataContextChanged(e);

        if (DataContext is BindingDisplayViewModel vm)
        {
            vm.OnShowBindingEditorDialog += ShowBindingEditorDialog;
            vm.OnShowAdvancedBindingEditorDialog += ShowAdvancedBindingEditorDialog;
        }
    }

    private void ShowBindingEditorDialog(object? sender, BindingDisplayViewModel e)
    {
        if (DataContext is BindingDisplayViewModel vm)
        {
            if (TopLevel.GetTopLevel(this) is AppMainWindow window)
            {
                window.ShowBindingEditorDialog(sender, vm);
            }
        }
    }

    private void ShowAdvancedBindingEditorDialog(object? sender, BindingDisplayViewModel e)
    {
        if (DataContext is BindingDisplayViewModel vm)
        {
            if (TopLevel.GetTopLevel(this) is AppMainWindow window)
            {
                window.ShowAdvancedBindingEditorDialog(sender, vm);
            }
        }
    }
}
