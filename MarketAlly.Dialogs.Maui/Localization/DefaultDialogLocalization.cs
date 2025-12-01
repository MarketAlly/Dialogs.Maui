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
        public virtual string DismissText => GetString("action_dismiss");
        public virtual string UndoText => GetString("action_undo");
        public virtual string RetryText => GetString("action_retry");

        // Date/Time picker strings
        public virtual string DateLabel => GetString("date_label");
        public virtual string TimeLabel => GetString("time_label");
        public virtual string SelectDateText => GetString("select_date");
        public virtual string SelectTimeText => GetString("select_time");
        public virtual string TodayText => GetString("today_text");
        public virtual string NowText => GetString("now_text");
        public virtual string ClearText => GetString("clear_text");

        // Validation strings
        public virtual string ValidationRequired => GetString("validation_required");
        public virtual string ValidationMinLength => GetString("validation_min_length");
        public virtual string ValidationMaxLength => GetString("validation_max_length");
        public virtual string ValidationInvalidFormat => GetString("validation_invalid_format");
        public virtual string ValidationInvalidEmail => GetString("validation_invalid_email");
        public virtual string ValidationInvalidPhone => GetString("validation_invalid_phone");

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
                ["color_gray"] = "Gray",
                ["action_dismiss"] = "DISMISS",
                ["action_undo"] = "UNDO",
                ["action_retry"] = "RETRY",
                ["action_view"] = "VIEW",
                ["action_open"] = "OPEN",
                // Date/Time picker strings
                ["date_label"] = "Date",
                ["time_label"] = "Time",
                ["select_date"] = "Select Date",
                ["select_time"] = "Select Time",
                ["today_text"] = "Today",
                ["now_text"] = "Now",
                ["clear_text"] = "Clear",
                // Validation strings
                ["validation_required"] = "This field is required",
                ["validation_min_length"] = "Minimum {0} characters required",
                ["validation_max_length"] = "Maximum {0} characters allowed",
                ["validation_invalid_format"] = "Invalid format",
                ["validation_invalid_email"] = "Invalid email address",
                ["validation_invalid_phone"] = "Invalid phone number"
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
                    strings["action_dismiss"] = "CERRAR";
                    strings["action_undo"] = "DESHACER";
                    strings["action_retry"] = "REINTENTAR";
                    strings["action_view"] = "VER";
                    strings["action_open"] = "ABRIR";
                    // Date/Time picker strings
                    strings["date_label"] = "Fecha";
                    strings["time_label"] = "Hora";
                    strings["select_date"] = "Seleccionar fecha";
                    strings["select_time"] = "Seleccionar hora";
                    strings["today_text"] = "Hoy";
                    strings["now_text"] = "Ahora";
                    strings["clear_text"] = "Borrar";
                    // Validation strings
                    strings["validation_required"] = "Este campo es obligatorio";
                    strings["validation_min_length"] = "Se requieren al menos {0} caracteres";
                    strings["validation_max_length"] = "Máximo {0} caracteres permitidos";
                    strings["validation_invalid_format"] = "Formato inválido";
                    strings["validation_invalid_email"] = "Dirección de correo inválida";
                    strings["validation_invalid_phone"] = "Número de teléfono inválido";
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
                    strings["action_dismiss"] = "FERMER";
                    strings["action_undo"] = "ANNULER";
                    strings["action_retry"] = "RÉESSAYER";
                    strings["action_view"] = "VOIR";
                    strings["action_open"] = "OUVRIR";
                    // Date/Time picker strings
                    strings["date_label"] = "Date";
                    strings["time_label"] = "Heure";
                    strings["select_date"] = "Sélectionner la date";
                    strings["select_time"] = "Sélectionner l'heure";
                    strings["today_text"] = "Aujourd'hui";
                    strings["now_text"] = "Maintenant";
                    strings["clear_text"] = "Effacer";
                    // Validation strings
                    strings["validation_required"] = "Ce champ est obligatoire";
                    strings["validation_min_length"] = "Minimum {0} caractères requis";
                    strings["validation_max_length"] = "Maximum {0} caractères autorisés";
                    strings["validation_invalid_format"] = "Format invalide";
                    strings["validation_invalid_email"] = "Adresse e-mail invalide";
                    strings["validation_invalid_phone"] = "Numéro de téléphone invalide";
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
                    strings["action_dismiss"] = "SCHLIEßEN";
                    strings["action_undo"] = "RÜCKGÄNGIG";
                    strings["action_retry"] = "WIEDERHOLEN";
                    strings["action_view"] = "ANSEHEN";
                    strings["action_open"] = "ÖFFNEN";
                    // Date/Time picker strings
                    strings["date_label"] = "Datum";
                    strings["time_label"] = "Uhrzeit";
                    strings["select_date"] = "Datum auswählen";
                    strings["select_time"] = "Uhrzeit auswählen";
                    strings["today_text"] = "Heute";
                    strings["now_text"] = "Jetzt";
                    strings["clear_text"] = "Löschen";
                    // Validation strings
                    strings["validation_required"] = "Dieses Feld ist erforderlich";
                    strings["validation_min_length"] = "Mindestens {0} Zeichen erforderlich";
                    strings["validation_max_length"] = "Maximal {0} Zeichen erlaubt";
                    strings["validation_invalid_format"] = "Ungültiges Format";
                    strings["validation_invalid_email"] = "Ungültige E-Mail-Adresse";
                    strings["validation_invalid_phone"] = "Ungültige Telefonnummer";
                    break;

                case "zh": // Chinese
                    strings["button_ok"] = "确定";
                    strings["button_cancel"] = "取消";
                    strings["button_yes"] = "是";
                    strings["button_no"] = "否";
                    strings["back_button_text"] = "返回";
                    strings["loading"] = "加载中...";
                    strings["select_placeholder"] = "请选择一个选项";
                    strings["error_title"] = "错误";
                    strings["warning_title"] = "警告";
                    strings["info_title"] = "信息";
                    strings["success_title"] = "成功";
                    strings["confirm_title"] = "确认";
                    strings["select_color"] = "选择颜色";
                    strings["not_set"] = "(未设置)";
                    strings["hex_label"] = "十六进制:";
                    strings["red_label"] = "红色";
                    strings["green_label"] = "绿色";
                    strings["blue_label"] = "蓝色";
                    strings["alpha_label"] = "透明度";
                    strings["preset_colors_label"] = "预设颜色";
                    strings["items_scroll_indicator"] = "{0} 个项目 (滚动查看更多)";
                    strings["color_red"] = "红色";
                    strings["color_blue"] = "蓝色";
                    strings["color_green"] = "绿色";
                    strings["color_orange"] = "橙色";
                    strings["color_purple"] = "紫色";
                    strings["action_dismiss"] = "关闭";
                    strings["action_undo"] = "撤销";
                    strings["action_retry"] = "重试";
                    strings["action_view"] = "查看";
                    strings["action_open"] = "打开";
                    // Date/Time picker strings
                    strings["date_label"] = "日期";
                    strings["time_label"] = "时间";
                    strings["select_date"] = "选择日期";
                    strings["select_time"] = "选择时间";
                    strings["today_text"] = "今天";
                    strings["now_text"] = "现在";
                    strings["clear_text"] = "清除";
                    // Validation strings
                    strings["validation_required"] = "此字段为必填项";
                    strings["validation_min_length"] = "至少需要 {0} 个字符";
                    strings["validation_max_length"] = "最多允许 {0} 个字符";
                    strings["validation_invalid_format"] = "格式无效";
                    strings["validation_invalid_email"] = "电子邮件地址无效";
                    strings["validation_invalid_phone"] = "电话号码无效";
                    break;

                case "ja": // Japanese
                    strings["button_ok"] = "OK";
                    strings["button_cancel"] = "キャンセル";
                    strings["button_yes"] = "はい";
                    strings["button_no"] = "いいえ";
                    strings["back_button_text"] = "戻る";
                    strings["loading"] = "読み込み中...";
                    strings["select_placeholder"] = "オプションを選択";
                    strings["error_title"] = "エラー";
                    strings["warning_title"] = "警告";
                    strings["info_title"] = "情報";
                    strings["success_title"] = "成功";
                    strings["confirm_title"] = "確認";
                    strings["select_color"] = "色を選択";
                    strings["not_set"] = "(未設定)";
                    strings["hex_label"] = "16進数:";
                    strings["red_label"] = "赤";
                    strings["green_label"] = "緑";
                    strings["blue_label"] = "青";
                    strings["alpha_label"] = "透明度";
                    strings["preset_colors_label"] = "プリセットカラー";
                    strings["items_scroll_indicator"] = "{0} 件 (スクロールで詳細表示)";
                    strings["color_red"] = "赤";
                    strings["color_blue"] = "青";
                    strings["color_green"] = "緑";
                    strings["color_orange"] = "オレンジ";
                    strings["color_purple"] = "紫";
                    strings["action_dismiss"] = "閉じる";
                    strings["action_undo"] = "元に戻す";
                    strings["action_retry"] = "再試行";
                    strings["action_view"] = "表示";
                    strings["action_open"] = "開く";
                    // Date/Time picker strings
                    strings["date_label"] = "日付";
                    strings["time_label"] = "時刻";
                    strings["select_date"] = "日付を選択";
                    strings["select_time"] = "時刻を選択";
                    strings["today_text"] = "今日";
                    strings["now_text"] = "現在";
                    strings["clear_text"] = "クリア";
                    // Validation strings
                    strings["validation_required"] = "この項目は必須です";
                    strings["validation_min_length"] = "{0} 文字以上必要です";
                    strings["validation_max_length"] = "{0} 文字以内で入力してください";
                    strings["validation_invalid_format"] = "形式が無効です";
                    strings["validation_invalid_email"] = "メールアドレスが無効です";
                    strings["validation_invalid_phone"] = "電話番号が無効です";
                    break;

                case "pt": // Portuguese
                    strings["button_ok"] = "OK";
                    strings["button_cancel"] = "Cancelar";
                    strings["button_yes"] = "Sim";
                    strings["button_no"] = "Não";
                    strings["back_button_text"] = "Voltar";
                    strings["loading"] = "Carregando...";
                    strings["select_placeholder"] = "Selecione uma opção";
                    strings["error_title"] = "Erro";
                    strings["warning_title"] = "Aviso";
                    strings["info_title"] = "Informação";
                    strings["success_title"] = "Sucesso";
                    strings["confirm_title"] = "Confirmar";
                    strings["select_color"] = "Selecionar cor";
                    strings["not_set"] = "(Não definido)";
                    strings["hex_label"] = "Hex:";
                    strings["red_label"] = "Vermelho";
                    strings["green_label"] = "Verde";
                    strings["blue_label"] = "Azul";
                    strings["alpha_label"] = "Alfa";
                    strings["preset_colors_label"] = "Cores predefinidas";
                    strings["items_scroll_indicator"] = "{0} itens (role para mais)";
                    strings["color_red"] = "Vermelho";
                    strings["color_blue"] = "Azul";
                    strings["color_green"] = "Verde";
                    strings["color_orange"] = "Laranja";
                    strings["color_purple"] = "Roxo";
                    strings["action_dismiss"] = "FECHAR";
                    strings["action_undo"] = "DESFAZER";
                    strings["action_retry"] = "TENTAR NOVAMENTE";
                    strings["action_view"] = "VER";
                    strings["action_open"] = "ABRIR";
                    // Date/Time picker strings
                    strings["date_label"] = "Data";
                    strings["time_label"] = "Hora";
                    strings["select_date"] = "Selecionar data";
                    strings["select_time"] = "Selecionar hora";
                    strings["today_text"] = "Hoje";
                    strings["now_text"] = "Agora";
                    strings["clear_text"] = "Limpar";
                    // Validation strings
                    strings["validation_required"] = "Este campo é obrigatório";
                    strings["validation_min_length"] = "Mínimo de {0} caracteres necessários";
                    strings["validation_max_length"] = "Máximo de {0} caracteres permitidos";
                    strings["validation_invalid_format"] = "Formato inválido";
                    strings["validation_invalid_email"] = "Endereço de e-mail inválido";
                    strings["validation_invalid_phone"] = "Número de telefone inválido";
                    break;

                case "it": // Italian
                    strings["button_ok"] = "OK";
                    strings["button_cancel"] = "Annulla";
                    strings["button_yes"] = "Sì";
                    strings["button_no"] = "No";
                    strings["back_button_text"] = "Indietro";
                    strings["loading"] = "Caricamento...";
                    strings["select_placeholder"] = "Seleziona un'opzione";
                    strings["error_title"] = "Errore";
                    strings["warning_title"] = "Avviso";
                    strings["info_title"] = "Informazione";
                    strings["success_title"] = "Successo";
                    strings["confirm_title"] = "Conferma";
                    strings["select_color"] = "Seleziona colore";
                    strings["not_set"] = "(Non impostato)";
                    strings["hex_label"] = "Hex:";
                    strings["red_label"] = "Rosso";
                    strings["green_label"] = "Verde";
                    strings["blue_label"] = "Blu";
                    strings["alpha_label"] = "Alfa";
                    strings["preset_colors_label"] = "Colori predefiniti";
                    strings["items_scroll_indicator"] = "{0} elementi (scorri per altri)";
                    strings["color_red"] = "Rosso";
                    strings["color_blue"] = "Blu";
                    strings["color_green"] = "Verde";
                    strings["color_orange"] = "Arancione";
                    strings["color_purple"] = "Viola";
                    strings["action_dismiss"] = "CHIUDI";
                    strings["action_undo"] = "ANNULLA";
                    strings["action_retry"] = "RIPROVA";
                    strings["action_view"] = "VISUALIZZA";
                    strings["action_open"] = "APRI";
                    // Date/Time picker strings
                    strings["date_label"] = "Data";
                    strings["time_label"] = "Ora";
                    strings["select_date"] = "Seleziona data";
                    strings["select_time"] = "Seleziona ora";
                    strings["today_text"] = "Oggi";
                    strings["now_text"] = "Adesso";
                    strings["clear_text"] = "Cancella";
                    // Validation strings
                    strings["validation_required"] = "Questo campo è obbligatorio";
                    strings["validation_min_length"] = "Sono richiesti almeno {0} caratteri";
                    strings["validation_max_length"] = "Sono consentiti al massimo {0} caratteri";
                    strings["validation_invalid_format"] = "Formato non valido";
                    strings["validation_invalid_email"] = "Indirizzo e-mail non valido";
                    strings["validation_invalid_phone"] = "Numero di telefono non valido";
                    break;
            }

            return strings;
        }
    }
}