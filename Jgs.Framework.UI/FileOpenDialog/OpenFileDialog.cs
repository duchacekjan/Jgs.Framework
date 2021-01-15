using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using resx = Jgs.Framework.UI.Resources.Resources;

namespace Jgs.Framework.UI.FileOpenDialog
{
    public class OpenFileDialog
    {
        private WindowsFormsAssemblyAccessor m_assemblyAccessor;

        public string InitialDirectory { get; set; }

        public string Title { get; set; }

        public OpenDialogOptions Options { get; set; }

        public string Filter { get; set; }

        public string FileName { get; private set; }

        public IList<string> FileNames { get; private set; }

        public bool RestoreCurrentDirectory { get; set; }

        private bool PickFolders => Options.HasFlag(OpenDialogOptions.PickFolders);

        public bool ShowDialog()
        {
            return ShowDialog(IntPtr.Zero);
        }

        public bool ShowDialog(IntPtr hWndOwner)
        {

            if (Environment.OSVersion.Version.Major < 6)
            {
                throw new NotSupportedException("Not supported on this OS Version");
            }

            var dialog = CreateDialog();
#if NET48
            var accessor = m_assemblyAccessor ?? (m_assemblyAccessor = new WindowsFormsAssemblyAccessor());
#else
            var accessor = m_assemblyAccessor ??= new WindowsFormsAssemblyAccessor();
#endif
            var result = accessor.ShowDialog(dialog, hWndOwner, PickFolders);
            if (result)
            {
                if (!RestoreCurrentDirectory)
                {
                    Environment.CurrentDirectory = PickFolders
                        ? dialog.FileName
                        : System.IO.Path.GetDirectoryName(dialog.FileName);
                }

                FileName = dialog.FileName;
                FileNames = dialog.FileNames?.ToList();
            }
            return result;
        }

        private System.Windows.Forms.OpenFileDialog CreateDialog()
        {
            return new System.Windows.Forms.OpenFileDialog
            {
                CheckFileExists = false,
                Title = GetTitle(),
                InitialDirectory = GetInitialDirectory(),
                Filter = GetFilter(),
                AddExtension = GetAddExtensions(),
                DereferenceLinks = GetDeferenceLinks(),
                Multiselect = Options.HasFlag(OpenDialogOptions.Multiselect)
            };
        }

        private string GetTitle()
        {
            return Title;
        }

        private string GetInitialDirectory()
        {
            return string.IsNullOrEmpty(InitialDirectory)
                ? Environment.CurrentDirectory
                : InitialDirectory;
        }

        private string GetFilter()
        {
            return PickFolders ? resx.FolderFilter : Filter;
        }

        private bool GetAddExtensions()
        {
            return !PickFolders && Options.HasFlag(OpenDialogOptions.AddExtension);
        }

        private bool GetDeferenceLinks()
        {
            return PickFolders || Options.HasFlag(OpenDialogOptions.DereferenceLinks);
        }
    }
}
