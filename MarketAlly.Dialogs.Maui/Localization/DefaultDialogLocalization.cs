using MarketAlly.Dialogs.Maui.Interfaces;
using System.Globalization;

namespace MarketAlly.Dialogs.Maui.Localization
{
    /// <summary>
    /// Default implementation of dialog localization with English strings
    /// </summary>
    public class DefaultDialogLocalization : IDialogLocalization
    {
        private readonly Dictionary<string, string> _localizedStrings;
        private readonly CultureInfo _culture;

        public DefaultDialogLocalization() : this(CultureInfo.CurrentCulture)
        {
        }

        public DefaultDialogLocalization(CultureInfo culture)
        {
            _culture = culture;
            _localizedStrings = LoadStrings(culture);
        }

        public virtual string OkButtonText => GetString("button_ok");
        public virtual string CancelButtonText => GetString("button_cancel");
        public virtual string YesButtonText => GetString("button_yes");
        public virtual string NoButtonText => GetString("button_no");
        public virtual string LoadingText => GetString("loading");
        public virtual string SelectPlaceholder => GetString("select_placeholder");
        public virtual string HexLabel => GetString("hex_label");
        public virtual string RedLabel => GetString("red_label");
        public virtual string GreenLabel => GetString("green_label");
        public virtual string BlueLabel => GetString("blue_label");
        public virtual string AlphaLabel => GetString("alpha_label");
        public virtual string PresetColorsLabel => GetString("preset_colors_label");
        public virtual string ItemsScrollIndicator => GetString("items_scroll_indicator");

        public virtual string GetString(string key)
        {
            return _localizedStrings.TryGetValue(key, out var value) ? value : key;
        }

        public virtual string GetString(string key, params object[] args)
        {
            var format = GetString(key);
            return string.Format(_culture, format, args);
        }

        protected virtual Dictionary<string, string> LoadStrings(CultureInfo culture)
        {
            // Default English strings
            var strings = new Dictionary<string, string>
            {
                ["button_ok"] = "OK",
                ["button_cancel"] = "Cancel",
                ["button_yes"] = "Yes",
                ["button_no"] = "No",
                ["back_button_text"] = "Back",
                ["loading"] = "Loading...",
                ["select_placeholder"] = "Select an option",
                ["error_title"] = "Error",
                ["warning_title"] = "Warning",
                ["info_title"] = "Information",
                ["success_title"] = "Success",
                ["confirm_title"] = "Confirm",
                ["select_color"] = "Select a color",
                ["not_set"] = "(Not Set)",
                ["hex_label"] = "Hex:",
                ["red_label"] = "Red",
                ["green_label"] = "Green",
                ["blue_label"] = "Blue",
                ["alpha_label"] = "Alpha",
                ["preset_colors_label"] = "Preset Colors",
                ["items_scroll_indicator"] = "{0} items (scroll for more)",
                ["color_red"] = "Red",
                ["color_blue"] = "Blue",
                ["color_green"] = "Green",
                ["color_orange"] = "Orange",
                ["color_purple"] = "Purple",
                ["color_yellow"] = "Yellow",
                ["color_black"] = "Black",
                ["color_white"] = "White",
                ["color_gray"] = "Gray"
            };

            // Override with culture-specific strings if needed
            var languageCode = culture.TwoLetterISOLanguageName.ToLower();

            switch (languageCode)
            {
                case "es": // Spanish
                    strings["button_ok"] = "Aceptar";
                    strings["button_cancel"] = "Cancelar";
                    strings["button_yes"] = "Sí";
                    strings["button_no"] = "No";
                    strings["back_button_text"] = "Atrás";
                    strings["loading"] = "Cargando...";
                    strings["select_placeholder"] = "Seleccione una opción";
                    strings["error_title"] = "Error";
                    strings["warning_title"] = "Advertencia";
                    strings["info_title"] = "Información";
                    strings["success_title"] = "Éxito";
                    strings["confirm_title"] = "Confirmar";
                    strings["select_color"] = "Seleccionar color";
                    strings["not_set"] = "(No establecido)";
                    strings["hex_label"] = "Hex:";
                    strings["red_label"] = "Rojo";
                    strings["green_label"] = "Verde";
                    strings["blue_label"] = "Azul";
                    strings["alpha_label"] = "Alfa";
                    strings["preset_colors_label"] = "Colores predefinidos";
                    strings["items_scroll_indicator"] = "{0} elementos (desplácese para ver más)";
                    strings["color_red"] = "Rojo";
                    strings["color_blue"] = "Azul";
                    strings["color_green"] = "Verde";
                    strings["color_orange"] = "Naranja";
                    strings["color_purple"] = "Púrpura";
                    break;

                case "fr": // French
                    strings["button_ok"] = "OK";
                    strings["button_cancel"] = "Annuler";
                    strings["button_yes"] = "Oui";
                    strings["button_no"] = "Non";
                    strings["back_button_text"] = "Retour";
                    strings["loading"] = "Chargement...";
                    strings["select_placeholder"] = "Sélectionnez une option";
                    strings["error_title"] = "Erreur";
                    strings["warning_title"] = "Avertissement";
                    strings["info_title"] = "Information";
                    strings["success_title"] = "Succès";
                    strings["confirm_title"] = "Confirmer";
                    strings["select_color"] = "Sélectionner une couleur";
                    strings["not_set"] = "(Non défini)";
                    strings["hex_label"] = "Hex:";
                    strings["red_label"] = "Rouge";
                    strings["green_label"] = "Vert";
                    strings["blue_label"] = "Bleu";
                    strings["alpha_label"] = "Alpha";
                    strings["preset_colors_label"] = "Couleurs prédéfinies";
                    strings["items_scroll_indicator"] = "{0} éléments (faites défiler pour plus)";
                    strings["color_red"] = "Rouge";
                    strings["color_blue"] = "Bleu";
                    strings["color_green"] = "Vert";
                    strings["color_orange"] = "Orange";
                    strings["color_purple"] = "Violet";
                    break;

                case "de": // German
                    strings["button_ok"] = "OK";
                    strings["button_cancel"] = "Abbrechen";
                    strings["button_yes"] = "Ja";
                    strings["button_no"] = "Nein";
                    strings["back_button_text"] = "Zurück";
                    strings["loading"] = "Laden...";
                    strings["select_placeholder"] = "Option auswählen";
                    strings["error_title"] = "Fehler";
                    strings["warning_title"] = "Warnung";
                    strings["info_title"] = "Information";
                    strings["success_title"] = "Erfolg";
                    strings["confirm_title"] = "Bestätigen";
                    strings["select_color"] = "Farbe auswählen";
                    strings["not_set"] = "(Nicht gesetzt)";
                    strings["hex_label"] = "Hex:";
                    strings["red_label"] = "Rot";
                    strings["green_label"] = "Grün";
                    strings["blue_label"] = "Blau";
                    strings["alpha_label"] = "Alpha";
                    strings["preset_colors_label"] = "Vordefinierte Farben";
                    strings["items_scroll_indicator"] = "{0} Elemente (scrollen für mehr)";
                    strings["color_red"] = "Rot";
                    strings["color_blue"] = "Blau";
                    strings["color_green"] = "Grün";
                    strings["color_orange"] = "Orange";
                    strings["color_purple"] = "Lila";
                    break;
            }

            return strings;
        }
    }
}