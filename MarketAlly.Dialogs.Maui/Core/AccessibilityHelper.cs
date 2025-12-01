using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Controls.PlatformConfiguration.macOSSpecific;

namespace MarketAlly.Dialogs.Maui.Core
{
    /// <summary>
    /// Helper class for accessibility features
    /// </summary>
    public static class AccessibilityHelper
    {
        /// <summary>
        /// Sets the AutomationId for an element using the dialog naming convention
        /// Pattern: MADialog_{DialogType}_{ControlType}_{Name}
        /// </summary>
        /// <param name="element">The visual element</param>
        /// <param name="dialogType">The dialog type (e.g., "Alert", "Confirm")</param>
        /// <param name="controlType">The control type (e.g., "Button", "Entry")</param>
        /// <param name="name">The control name (e.g., "Ok", "Cancel", "Input")</param>
        public static void SetAutomationId(VisualElement element, string dialogType, string controlType, string name)
        {
            element.AutomationId = $"MADialog_{dialogType}_{controlType}_{name}";
        }

        /// <summary>
        /// Sets the semantic description for an element
        /// </summary>
        /// <param name="element">The visual element</param>
        /// <param name="description">The description for screen readers</param>
        public static void SetDescription(VisualElement element, string description)
        {
            SemanticProperties.SetDescription(element, description);
        }

        /// <summary>
        /// Sets the semantic hint for an element
        /// </summary>
        /// <param name="element">The visual element</param>
        /// <param name="hint">The hint for screen readers</param>
        public static void SetHint(VisualElement element, string hint)
        {
            SemanticProperties.SetHint(element, hint);
        }

        /// <summary>
        /// Sets the heading level for an element
        /// </summary>
        /// <param name="element">The visual element</param>
        /// <param name="level">The heading level</param>
        public static void SetHeadingLevel(VisualElement element, SemanticHeadingLevel level)
        {
            SemanticProperties.SetHeadingLevel(element, level);
        }

        /// <summary>
        /// Sets the tab order for a sequence of elements on macOS.
        /// On other platforms, tab order follows the visual tree order.
        /// </summary>
        /// <param name="page">The page containing the elements</param>
        /// <param name="elements">Elements in tab order</param>
        public static void SetTabOrder(Microsoft.Maui.Controls.Page page, params VisualElement[] elements)
        {
            // Use macOS-specific API for tab order
            page.On<macOS>().SetTabOrder(elements);
        }

        /// <summary>
        /// Focuses an element with an optional delay
        /// </summary>
        /// <param name="element">The element to focus</param>
        /// <param name="delayMs">Delay before focusing in milliseconds</param>
        public static async Task FocusAsync(VisualElement element, int delayMs = 100)
        {
            if (delayMs > 0)
            {
                await Task.Delay(delayMs);
            }
            element.Focus();
        }

        /// <summary>
        /// Announces a message to screen readers
        /// </summary>
        /// <param name="message">The message to announce</param>
        public static void AnnounceForAccessibility(string message)
        {
            SemanticScreenReader.Announce(message);
        }

        /// <summary>
        /// Announces that a dialog has opened
        /// </summary>
        /// <param name="dialogType">The dialog type name</param>
        /// <param name="title">Optional dialog title</param>
        public static void AnnounceDialogOpened(string dialogType, string? title = null)
        {
            var announcement = string.IsNullOrEmpty(title)
                ? $"{dialogType} dialog opened"
                : $"{dialogType} dialog: {title}";
            AnnounceForAccessibility(announcement);
        }

        /// <summary>
        /// Announces that a dialog has closed
        /// </summary>
        /// <param name="dialogType">The dialog type name</param>
        public static void AnnounceDialogClosed(string dialogType)
        {
            AnnounceForAccessibility($"{dialogType} dialog closed");
        }

        /// <summary>
        /// Checks if colors meet WCAG AA contrast requirements (4.5:1 for normal text)
        /// </summary>
        /// <param name="foreground">Foreground color</param>
        /// <param name="background">Background color</param>
        /// <returns>True if contrast ratio is at least 4.5:1</returns>
        public static bool MeetsWcagAA(Color foreground, Color background)
        {
            var ratio = GetContrastRatio(foreground, background);
            return ratio >= 4.5;
        }

        /// <summary>
        /// Checks if colors meet WCAG AAA contrast requirements (7:1 for normal text)
        /// </summary>
        /// <param name="foreground">Foreground color</param>
        /// <param name="background">Background color</param>
        /// <returns>True if contrast ratio is at least 7:1</returns>
        public static bool MeetsWcagAAA(Color foreground, Color background)
        {
            var ratio = GetContrastRatio(foreground, background);
            return ratio >= 7.0;
        }

        /// <summary>
        /// Calculates the contrast ratio between two colors
        /// </summary>
        /// <param name="color1">First color</param>
        /// <param name="color2">Second color</param>
        /// <returns>Contrast ratio (1 to 21)</returns>
        public static double GetContrastRatio(Color color1, Color color2)
        {
            var l1 = GetRelativeLuminance(color1);
            var l2 = GetRelativeLuminance(color2);

            var lighter = Math.Max(l1, l2);
            var darker = Math.Min(l1, l2);

            return (lighter + 0.05) / (darker + 0.05);
        }

        /// <summary>
        /// Calculates the relative luminance of a color
        /// </summary>
        private static double GetRelativeLuminance(Color color)
        {
            double r = AdjustGamma(color.Red);
            double g = AdjustGamma(color.Green);
            double b = AdjustGamma(color.Blue);

            return 0.2126 * r + 0.7152 * g + 0.0722 * b;
        }

        /// <summary>
        /// Applies gamma adjustment for luminance calculation
        /// </summary>
        private static double AdjustGamma(double value)
        {
            return value <= 0.03928
                ? value / 12.92
                : Math.Pow((value + 0.055) / 1.055, 2.4);
        }

        /// <summary>
        /// Applies standard accessibility properties to a dialog title
        /// </summary>
        /// <param name="titleLabel">The title label</param>
        /// <param name="dialogType">The dialog type</param>
        public static void ConfigureTitleAccessibility(Label titleLabel, string dialogType)
        {
            SetAutomationId(titleLabel, dialogType, "Label", "Title");
            SetHeadingLevel(titleLabel, SemanticHeadingLevel.Level1);
        }

        /// <summary>
        /// Applies standard accessibility properties to a dialog button
        /// </summary>
        /// <param name="button">The button</param>
        /// <param name="dialogType">The dialog type</param>
        /// <param name="buttonName">The button name (e.g., "Ok", "Cancel")</param>
        /// <param name="hint">Optional hint for screen readers</param>
        public static void ConfigureButtonAccessibility(Button button, string dialogType, string buttonName, string? hint = null)
        {
            SetAutomationId(button, dialogType, "Button", buttonName);
            if (!string.IsNullOrEmpty(hint))
            {
                SetHint(button, hint);
            }
        }

        /// <summary>
        /// Applies standard accessibility properties to an input field
        /// </summary>
        /// <param name="entry">The entry field</param>
        /// <param name="dialogType">The dialog type</param>
        /// <param name="description">Description of what the input is for</param>
        public static void ConfigureInputAccessibility(Entry entry, string dialogType, string description)
        {
            SetAutomationId(entry, dialogType, "Entry", "Input");
            SetDescription(entry, description);
        }
    }
}
