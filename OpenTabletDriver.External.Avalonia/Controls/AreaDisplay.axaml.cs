using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Media.Immutable;
using OpenTabletDriver.External.Avalonia.Extensions;
using OpenTabletDriver.External.Avalonia.ViewModels;

namespace OpenTabletDriver.External.Avalonia.Controls;

#nullable enable

/// <summary>
///   A control that displays the available area and the mapped area. <br/>
///   The available area is the maximum area size that can be mapped. <br/>
///   The mapped area is the area that can be moved, resized, and rotated.
/// </summary>
/// <remarks>
///   Most of it is shamelessly stolen from The OpenTabletDriver UI Revamp.
/// </remarks>
public partial class AreaDisplay : UserControl
{
    private readonly ImmutableSolidColorBrush _accentBrush = new(0x0AFFFFFF);
    private readonly ImmutableSolidColorBrush _mappedBrush = new(0x800078D7);

    private bool _isDragging;
    private Point _mappedClickOffset;

    private double _scale;
    private double _xOffset;
    private double _yOffset;

    // --------------------------------- Constructor --------------------------------- //

    public AreaDisplay()
    {
        InitializeComponent();
    }

    // --------------------------------- Properties --------------------------------- //

    public static readonly StyledProperty<bool> IsReadOnlyProperty = AvaloniaProperty.Register<AreaDisplay, bool>(nameof(IsReadOnly), false);

    public bool IsReadOnly
    {
        get => GetValue(IsReadOnlyProperty);
        set => SetValue(IsReadOnlyProperty, value);
    }

    // ---------------------------------   Methods   --------------------------------- //

    protected override void OnDataContextChanged(EventArgs e)
    {
        if (DataContext is AreaDisplayViewModel vm)
        {
            _xOffset = Math.Abs(Math.Min(vm.AvailableArea.X, 0));
            _yOffset = Math.Abs(Math.Min(vm.AvailableArea.Y, 0));

            VIEW_AreaCanvas.Children.Clear();

            this.TryFindResource("AreaDisplayAvailableAreaBrush", out IBrush? accentBrush);
            this.TryFindResource("AreaDisplayMappedBrush", out IBrush? mappedBrush);

            // Rebuild the Defined Area's Border
            var border = CreateAvailableArea(vm.AvailableArea, accentBrush ?? _accentBrush);
            VIEW_AreaCanvas.Children.Add(border);

            // Rebuild the Mapped Area's Border
            var mappedBorder = CreateMappedArea(vm.MappedArea, mappedBrush ?? _mappedBrush);
            VIEW_AreaCanvas.Children.Add(mappedBorder);
        }

        base.OnDataContextChanged(e);
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        if (DataContext is AreaDisplayViewModel vm)
        {
            var padding = VIEW_AreaBorder.Padding;
            var maxCanvasSize = finalSize.Deflate(padding);

            var scaledWidth = maxCanvasSize.Height / vm.AvailableArea.Height * vm.AvailableArea.Width;
            var scaledHeight = maxCanvasSize.Width / vm.AvailableArea.Width * vm.AvailableArea.Height;

            if (scaledWidth > maxCanvasSize.Width)
            {
                VIEW_AreaCanvas.Width = maxCanvasSize.Width;
                VIEW_AreaCanvas.Height = scaledHeight;
            }
            else
            {
                VIEW_AreaCanvas.Width = scaledWidth;
                VIEW_AreaCanvas.Height = maxCanvasSize.Height;
            }

            _scale = VIEW_AreaCanvas.Bounds.Width / vm.AvailableArea.Width;

            foreach (var border in VIEW_AreaCanvas.Children.OfType<Border>())
            {
                var mapping = (Area)border.Tag!;
                SetSize(border, mapping, _scale);
                SetPosition(border, mapping, _scale);
            }
        }

        return base.ArrangeOverride(finalSize);
    }

    protected override Size MeasureOverride(Size availableSize)
    {
        // layout-wise idk how this fixes bounds overflow but it does
        return base.MeasureOverride(availableSize.Deflate(VIEW_AreaBorder.Padding));
    }

    private static Border CreateAvailableArea(Area area, IBrush brush)
    {
        var border = new Border
        {
            Background = brush,
            CornerRadius = new CornerRadius(6),
            Tag = area
        };

        border.Classes.Add("AvailableArea");

        return border;
    }

    private Border CreateMappedArea(Area area, IBrush brush)
    {
        var visual = new Border
        {
            Background = brush,
            CornerRadius = new CornerRadius(6),
            BorderBrush = Brushes.White,
            BorderThickness = new Thickness(1),
            Tag = area
        };

        area.PropertyChanged += (_, _) => InvalidateArrange();
        
        visual.PointerPressed += (s, e) => OnPointerPressedMappedArea(s, e, visual);
        visual.PointerReleased += OnPointerReleasedMappedArea;
        visual.PointerMoved += (s, e) => OnPointerMovedMappedArea(s, e, visual);

        return visual;
    }

    private void OnPointerPressedMappedArea(object? sender, PointerPressedEventArgs e, Border visual)
    {
        if (IsReadOnly) return;

        var point = e.GetCurrentPoint(visual);

        if (point.Properties.IsLeftButtonPressed)
        {
            _isDragging = true;
            _mappedClickOffset = point.Position;
            e.Pointer.Capture(visual);
        }
    }

    private void OnPointerReleasedMappedArea(object? sender, PointerReleasedEventArgs e)
    {
        if (IsReadOnly) return;

        if (e.InitialPressMouseButton == MouseButton.Left)
        {
            _isDragging = false;
            e.Pointer.Capture(null);
        }
    }

    private void OnPointerMovedMappedArea(object? sender, PointerEventArgs e, Border visual)
    {
        if (_isDragging)
        {
            var point = e.GetPosition(VIEW_AreaCanvas) - _mappedClickOffset;

            if (visual.Tag is Area mapping)
            {
                mapping.UntranslatedX = (point.X / _scale) - _xOffset;
                mapping.UntranslatedY = (point.Y / _scale) - _yOffset;
            }
        }
    }

    private static void SetSize(Border border, Area mapping, double scale)
    {
        border.Width = (mapping.Width * scale) - 1;
        border.Height = (mapping.Height * scale) - 1;
    }

    private void SetPosition(Border border, Area mapping, double scale)
    {
        Canvas.SetLeft(border, (mapping.UntranslatedX + _xOffset) * scale);
        Canvas.SetTop(border, (mapping.UntranslatedY + _yOffset) * scale);
        border.RenderTransform = new RotateTransform(mapping.Rotation);
    }
}
