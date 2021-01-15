using System;

namespace Jgs.UI.FileOpenDialog
{
    [Flags]
    public enum OpenDialogOptions
    {
        None = 0,
        AddExtension = 1,
        Multiselect = 2,
        PickFolders = 4,
        DereferenceLinks = 8
    }
}
