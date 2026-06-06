using System;
using System.IO;
using System.Windows;
using Microsoft.Win32;

namespace FloorLayout
{
    public partial class DebugWindow : Window
    {
        private static DebugWindow _instance;
        private int _lineCount = 0;

        public DebugWindow()
        {
            InitializeComponent();
            _instance = this;

            // Subscribe to window closing event
            this.Closing += DebugWindow_Closing;
        }

        private void DebugWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Don't actually close, just hide
            e.Cancel = true;
            this.Hide();
        }

        /// <summary>
        /// Get or create the singleton debug window instance
        /// </summary>
        public static DebugWindow Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DebugWindow();
                }
                return _instance;
            }
        }

        /// <summary>
        /// Show the debug window
        /// </summary>
        public static void Show()
        {
            Instance.Show();
            Instance.Activate();
        }

        /// <summary>
        /// Write a line to the debug output
        /// </summary>
        public static void WriteLine(string message)
        {
            Instance.WriteLineInternal(message);
        }

        /// <summary>
        /// Write a formatted line to the debug output
        /// </summary>
        public static void WriteLine(string format, params object[] args)
        {
            Instance.WriteLineInternal(string.Format(format, args));
        }

        /// <summary>
        /// Write a line with timestamp
        /// </summary>
        public static void WriteLineWithTime(string message)
        {
            string timestamp = DateTime.Now.ToString("HH:mm:ss.fff");
            Instance.WriteLineInternal($"[{timestamp}] {message}");
        }

        /// <summary>
        /// Write a separator line
        /// </summary>
        public static void WriteSeparator(string title = null)
        {
            if (string.IsNullOrEmpty(title))
            {
                Instance.WriteLineInternal("═══════════════════════════════════════════════════════════════");
            }
            else
            {
                int totalWidth = 63;
                int titleLength = title.Length + 2; // +2 for spaces
                int leftPad = (totalWidth - titleLength) / 2;
                int rightPad = totalWidth - titleLength - leftPad;

                string separator = new string('═', leftPad) + " " + title + " " + new string('═', rightPad);
                Instance.WriteLineInternal(separator);
            }
        }

        /// <summary>
        /// Clear the debug output
        /// </summary>
        public static void Clear()
        {
            Instance.ClearInternal();
        }

        /// <summary>
        /// Internal method to write a line (must be called on UI thread)
        /// </summary>
        private void WriteLineInternal(string message)
        {
            // Ensure we're on the UI thread
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(() => WriteLineInternal(message));
                return;
            }

            txtDebugOutput.AppendText(message + Environment.NewLine);
            _lineCount++;
            txtLineCount.Text = $"Lines: {_lineCount}";

            // Auto-scroll if enabled
            if (chkAutoScroll.IsChecked == true)
            {
                scrollViewer.ScrollToEnd();
            }

            // Update status
            txtStatus.Text = $"Last update: {DateTime.Now:HH:mm:ss}";
        }

        /// <summary>
        /// Internal method to clear output
        /// </summary>
        private void ClearInternal()
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(() => ClearInternal());
                return;
            }

            txtDebugOutput.Clear();
            _lineCount = 0;
            txtLineCount.Text = "Lines: 0";
            txtStatus.Text = "Cleared";
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            ClearInternal();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "Text Files (*.txt)|*.txt|Log Files (*.log)|*.log|All Files (*.*)|*.*";
            dlg.DefaultExt = ".txt";
            dlg.FileName = $"FloorLayout_Debug_{DateTime.Now:yyyyMMdd_HHmmss}.txt";

            if (dlg.ShowDialog() == true)
            {
                try
                {
                    File.WriteAllText(dlg.FileName, txtDebugOutput.Text);
                    MessageBox.Show($"Log saved to:\n{dlg.FileName}", "Log Saved",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error saving log:\n{ex.Message}", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
