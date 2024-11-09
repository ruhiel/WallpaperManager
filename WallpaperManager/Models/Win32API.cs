using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WallpaperManager.Models
{
    public class Win32API
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct Rect
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        [DllImport("shell32.dll")]
        public static extern int SHGetDesktopFolder(
            ref IShellFolder ppshf);

        [DllImport("shell32.dll")]
        public static extern Int32 SHCreateShellItemArrayFromIDLists(
            uint cidl,
            IntPtr rgpidl,
            out IShellItemArray ppsiItemArray);

        [StructLayout(LayoutKind.Explicit, Size = 520)]
        public struct STRRETinternal
        {
            [FieldOffset(0)]
            public IntPtr pOleStr;

            [FieldOffset(0)]
            public IntPtr pStr;  // LPSTR pStr;   NOT USED

            [FieldOffset(0)]
            public uint uOffset;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct STRRET
        {
            public uint uType;
            public STRRETinternal data;
        }

        public enum ESFGAO : uint
        {
            SFGAO_CANCOPY = 0x00000001,
            SFGAO_CANMOVE = 0x00000002,
            SFGAO_CANLINK = 0x00000004,
            SFGAO_LINK = 0x00010000,
            SFGAO_SHARE = 0x00020000,
            SFGAO_READONLY = 0x00040000,
            SFGAO_HIDDEN = 0x00080000,
            SFGAO_FOLDER = 0x20000000,
            SFGAO_FILESYSTEM = 0x40000000,
            SFGAO_HASSUBFOLDER = 0x80000000,
        }
        public enum ESHCONTF
        {
            SHCONTF_FOLDERS = 0x0020,
            SHCONTF_NONFOLDERS = 0x0040,
            SHCONTF_INCLUDEHIDDEN = 0x0080,
            SHCONTF_INIT_ON_FIRST_NEXT = 0x0100,
            SHCONTF_NETPRINTERSRCH = 0x0200,
            SHCONTF_SHAREABLE = 0x0400,
            SHCONTF_STORAGE = 0x0800
        }
        public enum ESHGDN
        {
            SHGDN_NORMAL = 0x0000,
            SHGDN_INFOLDER = 0x0001,
            SHGDN_FOREDITING = 0x1000,
            SHGDN_FORADDRESSBAR = 0x4000,
            SHGDN_FORPARSING = 0x8000,
        }

        [ComImport]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [Guid("000214E6-0000-0000-C000-000000000046")]
        public interface IShellFolder
        {
            void ParseDisplayName(IntPtr hwnd, IntPtr pbc, String pszDisplayName, IntPtr pchEaten, out IntPtr ppidl, IntPtr pdwAttributes);
            void EnumObjects(IntPtr hwnd, ESHCONTF grfFlags, out IntPtr ppenumIDList);
            void BindToObject(IntPtr pidl, IntPtr pbc, [In] ref Guid riid, out IntPtr ppv);
            void BindToStorage(IntPtr pidl, IntPtr pbc, [In] ref Guid riid, out IntPtr ppv);
            [PreserveSig]
            Int32 CompareIDs(Int32 lParam, IntPtr pidl1, IntPtr pidl2);
            void CreateViewObject(IntPtr hwndOwner, [In] ref Guid riid, out IntPtr ppv);
            void GetAttributesOf(UInt32 cidl, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 0)] IntPtr[] apidl, ref ESFGAO rgfInOut);
            void GetUIObjectOf(IntPtr hwndOwner, UInt32 cidl, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] IntPtr[] apidl, [In] ref Guid riid, UInt32 rgfReserved, out IntPtr ppv);
            void GetDisplayNameOf(IntPtr pidl, ESHGDN uFlags, out STRRET pName);
            void SetNameOf(IntPtr hwnd, IntPtr pidl, String pszName, ESHCONTF uFlags, out IntPtr ppidlOut);
        }

        public enum SIGDN : uint
        {
            NORMALDISPLAY = 0,
            PARENTRELATIVEPARSING = 0x80018001,
            PARENTRELATIVEFORADDRESSBAR = 0x8001c001,
            DESKTOPABSOLUTEPARSING = 0x80028000,
            PARENTRELATIVEEDITING = 0x80031001,
            DESKTOPABSOLUTEEDITING = 0x8004c000,
            FILESYSPATH = 0x80058000,
            URL = 0x80068000,
            PARENTRELATIVE = 0x80080001,
            PARENTRELATIVEFORUI = 0x80094001
        }
        public enum SIATTRIBFLAGS
        {
            SIATTRIBFLAGS_AND = 1,
            SIATTRIBFLAGS_APPCOMPAT = 3,
            SIATTRIBFLAGS_OR = 2
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct PROPERTYKEY
        {
            public Guid fmtid;
            public uint pid;
        }

        [ComImport]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [Guid("43826d1e-e718-42ee-bc55-a1e261c37bfe")]
        public interface IShellItem
        {
            void BindToHandler(IntPtr pbc,
                [MarshalAs(UnmanagedType.LPStruct)] Guid bhid,
                [MarshalAs(UnmanagedType.LPStruct)] Guid riid,
                out IntPtr ppv);
            void GetParent(out IShellItem ppsi);
            void GetDisplayName(SIGDN sigdnName, out IntPtr ppszName);
            void GetAttributes(uint sfgaoMask, out uint psfgaoAttribs);
            void Compare(IShellItem psi, uint hint, out int piOrder);
        }

        [ComImport, Guid("B63EA76D-1F85-456F-A19C-48159EFA858B"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IShellItemArray
        {
            // Not supported: IBindCtx
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
            void BindToHandler([In, MarshalAs(UnmanagedType.Interface)] IntPtr pbc, [In] ref Guid rbhid,
                         [In] ref Guid riid, out IntPtr ppvOut);
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
            void GetPropertyStore([In] int Flags, [In] ref Guid riid, out IntPtr ppv);
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
            void GetPropertyDescriptionList([In] ref PROPERTYKEY keyType, [In] ref Guid riid, out IntPtr ppv);
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
            void GetAttributes([In] SIATTRIBFLAGS dwAttribFlags, [In] uint sfgaoMask, out uint psfgaoAttribs);
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
            void GetCount(out uint pdwNumItems);
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
            void GetItemAt([In] uint dwIndex, [MarshalAs(UnmanagedType.Interface)] out IShellItem ppsi);
            // Not supported: IEnumShellItems (will use GetCount and GetItemAt instead)
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
            void EnumItems([MarshalAs(UnmanagedType.Interface)] out IntPtr ppenumShellItems);
        }

        public const Int32 S_OK = 0;
        public const Int32 S_FALSE = 1;
        public static bool SUCCEEDED(Int32 hr)
        {
            return hr >= 0;
        }
        public static bool FAILED(Int32 hr)
        {
            return hr < 0;
        }
    }
}
