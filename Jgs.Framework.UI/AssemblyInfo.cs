using System.Windows;
using System.Windows.Markup;

[assembly: ThemeInfo(
    ResourceDictionaryLocation.None, //where theme specific resource dictionaries are located
                                     //(used if a resource is not found in the page,
                                     // or application resource dictionaries)
    ResourceDictionaryLocation.SourceAssembly //where the generic resource dictionary is located
                                              //(used if a resource is not found in the page,
                                              // app, or any theme specific resource dictionaries)
)]

[assembly: XmlnsDefinition("jgs:wpf.controls", "Jgs.Framework.UI.Components")]
[assembly: XmlnsDefinition("jgs:wpf.converters", "Jgs.Framework.UI.Converters")]
[assembly: XmlnsDefinition("jgs:wpf.extensions", "Jgs.Framework.UI.Extensions")]
[assembly: XmlnsDefinition("jgs:wpf.markup", "Jgs.Framework.UI.Markup")]
[assembly: XmlnsDefinition("jgs:wpf.resx", "Jgs.Framework.UI.Resources")]