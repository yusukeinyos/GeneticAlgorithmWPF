using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GeneticAlgorithmWPF.Control
{
    public class NumericTextBox : TextBox
    {
        static NumericTextBox()
        {
            // IMEを無効化
            InputMethod.IsInputMethodEnabledProperty.OverrideMetadata(typeof(NumericTextBox), new FrameworkPropertyMetadata(false));
        }

        public NumericTextBox()
        {
            // ペーストのキーボードショートカットを無効化
            InputBindings.Add(new KeyBinding(ApplicationCommands.NotACommand, Key.V, ModifierKeys.Control));
            InputBindings.Add(new KeyBinding(ApplicationCommands.NotACommand, Key.Insert, ModifierKeys.Shift));
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            // 数値以外、または数値の入力に関係しないキーが押された場合、イベントを処理済みに
            if (!(Key.D0 <= e.Key && e.Key <= Key.D9 ||
                  Key.NumPad0 <= e.Key && e.Key <= Key.NumPad9 ||
                  Key.Back == e.Key || Key.Delete == e.Key || Key.Tab == e.Key ||
                  Key.Left <= e.Key && e.Key <= Key.Down) ||
                (Keyboard.Modifiers & ModifierKeys.Shift) > 0)
            {
                e.Handled = true;
            }
            OnKeyDown(e);
        }

        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            Focus();
            e.Handled = true;
        }

        // 右クリックを無効化
        protected override void OnPreviewMouseRightButtonUp(MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        protected override void OnPreviewLostKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            if (!int.TryParse(Text, out int _))
                Text = "0";  //e.Handled = true;
        }

        protected override void OnGotFocus(RoutedEventArgs e)
        {
            SelectAll();
            e.Handled = true;
        }
    }

    public static class NumericTextBoxExtensions
    {
        public static bool TryInt(this TextBox textbox, out int value)
        {
            value = 0;
            return textbox != null && int.TryParse(textbox.Text, out value);
        }

        public static int ParseInt(this TextBox textbox, int defaultValue = 0)
        {
            return textbox != null && int.TryParse(textbox.Text, out int value) ? value : defaultValue;
        }

        public static bool TryLong(this TextBox textbox, out long value)
        {
            value = 0;
            return textbox != null && long.TryParse(textbox.Text, out value);
        }

        public static long ParseLong(this TextBox textbox, int defaultValue = 0)
        {
            return textbox != null && long.TryParse(textbox.Text, out long value) ? value : defaultValue;
        }

        public static bool TryFloat(this TextBox textBox, out float value)
        {
            value = 0;
            return textBox != null && float.TryParse(textBox.Text, out value);
        }

        public static float ParseFloat(this TextBox textBox, float defaultValue = 0)
        {
            return textBox != null && float.TryParse(textBox.Text, out float value) ? value : defaultValue;
        }
    }
}
