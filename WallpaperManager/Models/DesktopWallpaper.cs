using System.Runtime.InteropServices;

namespace WallpaperManager.Models
{
    public enum DesktopSlideshowOptions
    {
        ShuffleImages = 0x01,
    }
    public enum DesktopSlideshowState
    {
        Enabled = 0x01,
        Slideshow = 0x02,
        DisabledByRemoteSession = 0x04,
    }
    public enum DesktopSlideshowDirection
    {
        Forward = 0,
        Backward = 1,
    }
    public enum DesktopWallpaperPosition
    {
        Center = 0,
        Tile = 1,
        Stretch = 2,
        Fit = 3,
        Fill = 4,
        Span = 5,
    }

    [ComImport, Guid("B92B56A9-8B55-4E14-9A89-0199BBB6F93B"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [CoClass(typeof(DesktopWallpaperClass))]
    public interface IDesktopWallpaper
    {
        void SetWallpaper([MarshalAs(UnmanagedType.LPWStr)] string monitorID, [MarshalAs(UnmanagedType.LPWStr)] string wallpaper);
        [return: MarshalAs(UnmanagedType.LPWStr)]
        string GetWallpaper([MarshalAs(UnmanagedType.LPWStr)] string monitorID);

        [return: MarshalAs(UnmanagedType.LPWStr)]
        string GetMonitorDevicePathAt(uint monitorIndex);
        [return: MarshalAs(UnmanagedType.U4)]
        uint GetMonitorDevicePathCount();

        [return: MarshalAs(UnmanagedType.Struct)]
        Win32API.Rect GetMonitorRECT([MarshalAs(UnmanagedType.LPWStr)] string monitorID);

        void SetBackgroundColor([MarshalAs(UnmanagedType.U4)] uint color);
        [return: MarshalAs(UnmanagedType.U4)]
        uint GetBackgroundColor();

        void SetPosition([MarshalAs(UnmanagedType.I4)] DesktopWallpaperPosition position);
        [return: MarshalAs(UnmanagedType.I4)]
        DesktopWallpaperPosition GetPosition();

        void SetSlideshow([MarshalAs(UnmanagedType.Interface)] Win32API.IShellItemArray ppsiItemArray);
        [return: MarshalAs(UnmanagedType.Interface)]
        Win32API.IShellItemArray GetSlideshow();

        void SetSlideshowOptions(DesktopSlideshowOptions options, uint slideshowTick);
        void GetSlideshowOptions(out DesktopSlideshowOptions options, out uint slideshowTick);

        void AdvanceSlideshow([MarshalAs(UnmanagedType.LPWStr)] string monitorID, [MarshalAs(UnmanagedType.I4)] DesktopSlideshowDirection direction);

        [return: MarshalAs(UnmanagedType.U4)]
        DesktopSlideshowState GetStatus();

        void Enable([MarshalAs(UnmanagedType.Bool)] bool enable);
    }

    [ComImport, Guid("C2CF3110-460E-4fc1-B9D0-8A1C0C9CC4BD")]
    public class DesktopWallpaperClass
    {
    }
}