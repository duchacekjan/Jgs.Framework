namespace Jgs.UI.Components.TagControlCore
{
    public interface ITag
    {
        object Id { get; set; }

        string Text { get; set; }

        string Description { get; set; }

        string Background { get; set; }

        string CloseButtonTooltip { get; set; }
    }
}
