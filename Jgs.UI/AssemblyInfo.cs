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

[assembly: XmlnsDefinition("jgs:wpf.controls", "Jgs.UI.Components")]
[assembly: XmlnsDefinition("jgs:wpf.converters", "Jgs.UI.Converters")]
[assembly: XmlnsDefinition("jgs:wpf.extensions", "Jgs.UI.Extensions")]
[assembly: XmlnsDefinition("jgs:wpf.markup", "Jgs.UI.Markup")]
[assembly: XmlnsDefinition("jgs:wpf.resx", "Jgs.UI.Resources")]