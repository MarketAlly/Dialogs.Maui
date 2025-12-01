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
        /// Gets or sets the action to execute when this item is selected.
        /// When set, this action will be automatically invoked after the dialog is dismissed.
        /// </summary>
        public Action? Action { get; set; }

        /// <summary>
        /// Gets or sets the async action to execute when this item is selected.
        /// When set, this action will be automatically invoked after the dialog is dismissed.
        /// Takes precedence over the synchronous Action property.
        /// </summary>
        public Func<Task>? AsyncAction { get; set; }

        /// <summary>
        /// Gets whether this item has an action defined
        /// </summary>
        public bool HasAction => Action != null || AsyncAction != null;

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
        /// Creates a new action item with a synchronous action callback
        /// </summary>
        /// <param name="name">Display name</param>
        /// <param name="action">Action to execute when selected</param>
        /// <param name="detail">Optional description text</param>
        public ActionItem(string name, Action action, string? detail = null)
            : this(name, detail, 0)
        {
            Action = action;
        }

        /// <summary>
        /// Creates a new action item with an async action callback
        /// </summary>
        /// <param name="name">Display name</param>
        /// <param name="asyncAction">Async action to execute when selected</param>
        /// <param name="detail">Optional description text</param>
        public ActionItem(string name, Func<Task> asyncAction, string? detail = null)
            : this(name, detail, 0)
        {
            AsyncAction = asyncAction;
        }

        /// <summary>
        /// Creates a new action item with a synchronous action callback and icons
        /// </summary>
        public ActionItem(string name, Action action, string? detail, string? imageDark, string? imageLight)
            : this(name, detail, 0)
        {
            Action = action;
            ImageDark = imageDark;
            ImageLight = imageLight;
        }

        /// <summary>
        /// Creates a new action item with an async action callback and icons
        /// </summary>
        public ActionItem(string name, Func<Task> asyncAction, string? detail, string? imageDark, string? imageLight)
            : this(name, detail, 0)
        {
            AsyncAction = asyncAction;
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

        /// <summary>
        /// Invokes the action associated with this item.
        /// AsyncAction takes precedence if both are set.
        /// </summary>
        /// <returns>A task that completes when the action finishes</returns>
        public async Task InvokeActionAsync()
        {
            if (AsyncAction != null)
            {
                await AsyncAction();
            }
            else if (Action != null)
            {
                Action();
            }
        }

        public override string ToString() => Name;
    }
}