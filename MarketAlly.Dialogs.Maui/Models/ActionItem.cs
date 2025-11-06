namespace MarketAlly.Dialogs.Maui.Models
{
    /// <summary>
    /// Represents an action item in an action list dialog
    /// </summary>
    public class ActionItem
    {
        /// <summary>
        /// Gets or sets the dark theme image source
        /// </summary>
        public string? ImageDark { get; set; }

        /// <summary>
        /// Gets or sets the light theme image source
        /// </summary>
        public string? ImageLight { get; set; }

        /// <summary>
        /// Gets or sets the display name of the action
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the detail/description text
        /// </summary>
        public string? Detail { get; set; }

        /// <summary>
        /// Gets or sets the value associated with this action
        /// </summary>
        public int Value { get; set; }

        /// <summary>
        /// Gets or sets whether to show the image
        /// </summary>
        public bool ShowImage => !string.IsNullOrEmpty(ImageDark) || !string.IsNullOrEmpty(ImageLight);

        /// <summary>
        /// Gets whether this item has detail text
        /// </summary>
        public bool HasDetail => !string.IsNullOrEmpty(Detail);

        /// <summary>
        /// Gets or sets the unique identifier for this item
        /// </summary>
        public Guid ItemId { get; set; }

        /// <summary>
        /// Gets or sets the sub-items for hierarchical menus
        /// </summary>
        public List<ActionItem>? SubItems { get; set; }

        /// <summary>
        /// Gets whether this item has sub-items
        /// </summary>
        public bool HasSubItems => SubItems != null && SubItems.Count > 0;

        /// <summary>
        /// Creates a new action item with name, detail, and value
        /// </summary>
        public ActionItem(string name, string? detail, int value, Guid? itemId = null)
        {
            Name = name;
            Detail = detail;
            Value = value;
            ItemId = itemId ?? Guid.NewGuid();
        }

        /// <summary>
        /// Creates a new action item with name and value
        /// </summary>
        public ActionItem(string name, int value, Guid? itemId = null)
            : this(name, null, value, itemId)
        {
        }

        /// <summary>
        /// Creates a new action item with images
        /// </summary>
        public ActionItem(string name, string? detail, int value, string? imageDark, string? imageLight, Guid? itemId = null)
            : this(name, detail, value, itemId)
        {
            ImageDark = imageDark;
            ImageLight = imageLight;
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public ActionItem()
        {
            ItemId = Guid.NewGuid();
        }

        public override string ToString() => Name;
    }
}