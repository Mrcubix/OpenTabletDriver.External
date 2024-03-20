using System;
using System.ComponentModel;
using Avalonia;
using CommunityToolkit.Mvvm.ComponentModel;

namespace OpenTabletDriver.External.Avalonia.ViewModels;

#nullable enable

public partial class AreaDisplayViewModel : ViewModelBase
{
    private Rect _availableArea;
    private bool _restricting;

    #region Constructors

    public AreaDisplayViewModel() : this(new Area(0, 0, 152.0, 95.0), new Area(36, 22, 68, 38.25, true))
    {
    }

    public AreaDisplayViewModel(Rect availableArea)
    {
        _availableArea = availableArea;
        AvailableArea = new Area(availableArea);

        MappedArea = new Area(availableArea.X + availableArea.Width / 2, availableArea.Y + availableArea.Height / 2, 
                              availableArea.Width, availableArea.Height, true);

        MappedArea.PropertyChanged += OnMappedAreaChanged;
    }

    public AreaDisplayViewModel(Rect availableArea, Area mappedArea) : this(availableArea)
    {
        MappedArea = mappedArea;
    }

    public AreaDisplayViewModel(Area availableArea, Area mappedArea)
    {
        AvailableArea = availableArea;
        _availableArea = new Rect(availableArea.X, availableArea.Y, availableArea.Width, availableArea.Height);

        MappedArea = mappedArea;
        
        MappedArea.PropertyChanged += OnMappedAreaChanged;
    }

    public AreaDisplayViewModel(double x, double y, double width, double height, double rotation) : this(new Area(x, y, width, height), new Area(x, y, width, height, rotation))
    {
    }

    #endregion

    #region Properties

    /// <summary>
    ///   The area available for mapping.
    /// </summary>
    /// <remarks>
    ///   This represent the maximum area size that can be mapped.
    /// </remarks>
    public Area AvailableArea { get; }

    /// <summary>
    ///   The area that is mapped.
    /// </summary>
    public Area MappedArea { get; }

    #endregion

    #region Methods

    /// <summary>
    ///   The Area might go out of bounds if it got bigger while close to the edge, if it's bigger than the available area, or if it's rotated.
    /// </summary>
    /// <remarks> 
    ///   Shamelessly stolen from The OpenTabletDriver UI Revamp.
    /// </remarks>
    private void RestrictToAvailableArea()
    {
        var mappingRect = ToRect(MappedArea);

        if (MappedArea.Rotation != 0)
        {
            var theta = MappedArea.Rotation * Math.PI / 180.0;
            var centerPoint = mappingRect.Center;

            var translationMatrix =
                Matrix.CreateTranslation(-centerPoint.X, -centerPoint.Y) *
                Matrix.CreateRotation(theta) *
                Matrix.CreateTranslation(centerPoint.X, centerPoint.Y);

            mappingRect = mappingRect.TransformToAABB(translationMatrix);
        }

        if (mappingRect.Width > AvailableArea.Width || mappingRect.Height > AvailableArea.Height)
        {
            var scaledWidth = AvailableArea.Height / mappingRect.Height * mappingRect.Width;
            var scaledHeight = AvailableArea.Width / mappingRect.Width * mappingRect.Height;

            if (scaledWidth >= mappingRect.Width)
            {
                var reductionScale = AvailableArea.Width / mappingRect.Width;
                var mapRatio = MappedArea.Width / MappedArea.Height;
                var xDiff = (mappingRect.Width - AvailableArea.Width) * reductionScale;
                var width = MappedArea.Width - xDiff;
                MappedArea.UntranslatedX = Math.Round(MappedArea.UntranslatedX + xDiff / 2.0, 2);
                MappedArea.Width = Math.Round(width, 2);
                MappedArea.Height = Math.Round(width / mapRatio, 2);
            }
            else
            {
                var reductionScale = AvailableArea.Height / mappingRect.Height;
                var mapRatio = MappedArea.Height / MappedArea.Width;
                var yDiff = (mappingRect.Height - AvailableArea.Height) * reductionScale;
                var height = MappedArea.Height - yDiff;
                MappedArea.UntranslatedY = Math.Round(MappedArea.UntranslatedY + yDiff / 2.0, 2);
                MappedArea.Height = Math.Round(height, 2);
                MappedArea.Width = Math.Round(height / mapRatio, 2);
            }
        }

        if (mappingRect.Left < _availableArea.Left)
        {
            MappedArea.UntranslatedX = Math.Round(MappedArea.UntranslatedX + (_availableArea.Left - mappingRect.Left), 2);
        }
        else if (mappingRect.Right > _availableArea.Right)
        {
            MappedArea.UntranslatedX = Math.Round(MappedArea.UntranslatedX + (_availableArea.Right - mappingRect.Right), 2);
        }

        if (mappingRect.Top < _availableArea.Top)
        {
            MappedArea.UntranslatedY = Math.Round(MappedArea.UntranslatedY + (_availableArea.Top - mappingRect.Top), 2);
        }
        else if (mappingRect.Bottom > _availableArea.Bottom)
        {
            MappedArea.UntranslatedY = Math.Round(MappedArea.UntranslatedY + (_availableArea.Bottom - mappingRect.Bottom), 2);
        }
    }

    private static Rect ToRect(Area mapping)
    {
        return new Rect(mapping.UntranslatedX, mapping.UntranslatedY, mapping.Width, mapping.Height);
    }

    #endregion

    #region Event Handlers

    private void OnMappedAreaChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (_restricting)
            return;

        _restricting = true;
        RestrictToAvailableArea();
        _restricting = false;
    }

    #endregion
}

/// <summary>
///   Represents a rectangular area.
/// </summary>
/// <remarks>
///   X and Y are the coordinates area's center.
/// </remarks>
public partial class Area : ObservableObject
{
    [ObservableProperty]
    private double _x;

    [ObservableProperty]
    private double _y;

    [ObservableProperty]
    private double _width;

    [ObservableProperty]
    private double _height;

    [ObservableProperty]
    private double _rotation;

    #region Constructors

    public Area(double x, double y, double width, double height, bool centerOrigin = false)
    {
        X = x;
        Y = y;
        Width = width;
        Height = height;
        Rotation = 0;
        CenterOrigin = centerOrigin;
    }

    public Area(double x, double y, double width, double height, double rotation, bool centerOrigin = false)
    {
        X = x;
        Y = y;
        Width = width;
        Height = height;
        Rotation = rotation;
        CenterOrigin = centerOrigin;
    }

    public Area(Point position, Size size, double rotation = 0, bool centerOrigin = false)
    {
        X = position.X;
        Y = position.Y;
        Width = size.Width;
        Height = size.Height;
        Rotation = rotation;
        CenterOrigin = centerOrigin;
    }

    public Area(Rect rect, double rotation = 0, bool centerOrigin = false)
    {
        X = rect.X;
        Y = rect.Y;
        Width = rect.Width;
        Height = rect.Height;
        Rotation = rotation;
        CenterOrigin = centerOrigin;
    }

    #endregion

    #region Properties

    public double UntranslatedX
    {
        get => CenterOrigin ? X - Width / 2.0 : X;
        set => X = Math.Round(CenterOrigin ? value + Width / 2.0 : value, 2);
    }
    public double UntranslatedY
    {
        get => CenterOrigin ? Y - Height / 2.0 : Y;
        set => Y = Math.Round(CenterOrigin ? value + Height / 2.0 : value, 2);
    }

    public bool CenterOrigin { get; set; }

    #endregion

    #region Operators

    public static Area operator +(Area a, Area b)
    {
        return new Area(a.X, a.Y, a.Width + b.Width, a.Height + b.Height);
    }

    public static Area operator -(Area a, Area b)
    {
        return new Area(a.X, a.Y, a.Width - b.Width, a.Height - b.Height);
    }

    public static Area operator *(Area a, double multiplier)
    {
        return new Area(a.X, a.Y, a.Width * multiplier, a.Height * multiplier);
    }

    public static Area operator /(Area a, double divisor)
    {
        return new Area(a.X, a.Y, a.Width / divisor, a.Height / divisor);
    }

    #endregion
}
