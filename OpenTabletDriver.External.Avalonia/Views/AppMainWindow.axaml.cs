using Avalonia.Controls;
using OpenTabletDriver.External.Avalonia.ViewModels;

namespace OpenTabletDriver.External.Avalonia.Views;

#nullable enable

public abstract partial class AppMainWindow : Window
{
    public abstract void ShowBindingEditorDialog(object? sender, BindingDisplayViewModel e);

    public abstract void ShowAdvancedBindingEditorDialog(object? sender, BindingDisplayViewModel e);
}