using System;
using System.Reflection;

namespace Jgs.Framework.UI.FileOpenDialog
{
    internal class WindowsFormsAssemblyAccessor
    {
#if NET5_0
        private const string PickFolders = "PICKFOLDERS";
#else
        private const string PickFolders = "FOS_PICKFOLDERS";
        private const string IFileDialog = "FileDialogNative+IFileDialog";
        private const string FileDialogNativeFOS = "FileDialogNative+FOS";
#endif
        private const string CreateVistaDialog = "CreateVistaDialog";
        private const string GetOptions = "GetOptions";
        private const string SetOptions = "SetOptions";
        private const string FileDialogVistaDialogEvents = "FileDialog+VistaDialogEvents";
        private const string OnBeforeVistaDialog = "OnBeforeVistaDialog";
        private const string Advise = "Advise";
        private const string Unadvise = "Unadvise";
        private const string Show = "Show";

        private readonly Assembly m_winFormsAssembly;
        private readonly string m_namespace;
        private readonly Type m_type;

        public WindowsFormsAssemblyAccessor()
        {
            m_type = typeof(System.Windows.Forms.OpenFileDialog);
            m_winFormsAssembly = m_type.Assembly;
            m_namespace = m_type.Namespace;
        }

        public bool ShowDialog(System.Windows.Forms.OpenFileDialog openFileDialog, IntPtr hwndOwner, bool pickFolders)
        {
            if (openFileDialog == null)
            {
                throw new ArgumentNullException(nameof(openFileDialog));
            }

            var flag = false;
            uint num = 0;
            var fileDialogType = typeof(System.Windows.Forms.FileDialog);
#if NET5_0
            var typeIFileDialog = GetIFileDialogType(fileDialogType);
#else
            var typeIFileDialog = GetType(IFileDialog);
#endif
            var dialog = Invoke<object>(openFileDialog, CreateVistaDialog);
            Invoke(fileDialogType, openFileDialog, OnBeforeVistaDialog, dialog);
            if (pickFolders)
            {
                var options = Invoke<uint>(fileDialogType, openFileDialog, GetOptions);
                options |= GetFOSValue(fileDialogType, PickFolders);
                Invoke(typeIFileDialog, dialog, SetOptions, options);
            }
            var pfde = Create(FileDialogVistaDialogEvents, openFileDialog);
            var parameters = new object[] { pfde, num };
            Invoke(typeIFileDialog, dialog, Advise, parameters);
            num = (uint)parameters[1];
            try
            {
                var num2 = Invoke<int>(typeIFileDialog, dialog, Show, hwndOwner);
                flag = 0 == num2;
            }
            finally
            {
                Invoke(typeIFileDialog, dialog, Unadvise, num);
                GC.KeepAlive(pfde);
            }

            return flag;
        }

        private static T Invoke<T>(Type type, object obj, string func, params object[] parameters)
        {
            var methInfo = type.GetMethod(func, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            return (T)methInfo.Invoke(obj, parameters);
        }

        private static void Invoke(Type type, object obj, string func, params object[] parameters)
        {
            var methInfo = type.GetMethod(func, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            methInfo.Invoke(obj, parameters);
        }

        private static uint GetFOSValue(Type fileDialogType, string name)
        {
#if NET5_0
            var type = fileDialogType.GetMethod(GetOptions, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .ReturnType;
#else
            var type = GetType(FileDialogNativeFOS);
#endif
            var fieldInfo = type.GetField(name);
            return (uint)fieldInfo.GetValue(null);
        }

        private static Type GetIFileDialogType(Type fileDialogType)
        {
            return fileDialogType.GetMethod(CreateVistaDialog, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .ReturnType;
        }

        private Type GetType(string typeName)
        {
            return m_winFormsAssembly.GetType($"{m_namespace}.{typeName}");
        }

        private T Invoke<T>(object obj, string func, params object[] parameters)
        {
            return Invoke<T>(m_type, obj, func, parameters);
        }

        private object Create(string name, params object[] parameters)
        {
            var type = GetType(name);
            var ctorInfos = type.GetConstructors();
            foreach (var ci in ctorInfos)
            {
                try
                {
                    return ci.Invoke(parameters);
                }
                catch { }
            }

            return null;
        }
    }
}
