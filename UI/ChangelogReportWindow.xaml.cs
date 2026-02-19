using System;
using System.IO;
using System.Threading;
using System.Windows;
using Microsoft.Win32;

namespace TeamUpdates.UI
{
    /// <summary>
    /// Dialog for viewing changelog reports
    /// </summary>
    public partial class ChangelogReportWindow : Window
    {
        private readonly string _reportText;

        public ChangelogReportWindow(string reportText)
        {
            InitializeComponent();

            _reportText = reportText;
            ReportTextBlock.Text = reportText;
        }

        private void CopyButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SetClipboardWithRetry(_reportText);
                MessageBox.Show("Report copied to clipboard.",
                              "Success",
                              MessageBoxButton.OK,
                              MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error copying to clipboard: {ex.Message}",
                              "Error",
                              MessageBoxButton.OK,
                              MessageBoxImage.Error);
            }
        }

        private static void SetClipboardWithRetry(string text, int retries = 5)
        {
            for (int i = 0; i < retries; i++)
            {
                try
                {
                    Clipboard.SetDataObject(text, true);
                    return;
                }
                catch (Exception)
                {
                    if (i == retries - 1) throw;
                    Thread.Sleep(50 * (i + 1));
                }
            }
        }

        private void ExportButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var saveDialog = new SaveFileDialog
                {
                    Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*",
                    FileName = $"changelog_report_{DateTime.Now:yyyyMMdd_HHmmss}.txt"
                };

                if (saveDialog.ShowDialog() == true)
                {
                    File.WriteAllText(saveDialog.FileName, _reportText);
                    MessageBox.Show("Report exported successfully.", 
                                  "Success", 
                                  MessageBoxButton.OK, 
                                  MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error exporting report: {ex.Message}", 
                              "Error", 
                              MessageBoxButton.OK, 
                              MessageBoxImage.Error);
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
