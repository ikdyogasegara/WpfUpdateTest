using System;
using System.Diagnostics;
using System.Windows;
using Squirrel;

namespace WPFAppCore
{
    public partial class MainWindow : Window
    {
        private UpdateManager _manager;

        public MainWindow()
        {
            InitializeComponent();
            AddVersionNumber();
            CheckVersionUpdate();
            // Loaded += MainWindow_Loaded;
        }

        private void AddVersionNumber()
        {
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            var versionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
            Dispatcher.Invoke(() =>
            {
                this.Title += $" v.{versionInfo.FileVersion}";
            });
        }

        private async void CheckVersionUpdate()
        {
            try
            {
                using var mgr = await UpdateManager.GitHubUpdateManager(
                    "https://github.com/ikdyogasegara/WpfUpdateTest");
                CurrentVersionTextBox.Text = mgr.CurrentlyInstalledVersion().ToString();
                var release = await mgr.UpdateApp();
                MessageBox.Show($"Pembaruan Aplikasi Tersedia {release.Version}...Tutup dan jalankan ulang aplikasi");
                Process.Start(Application.ResourceAssembly.Location);
                Application.Current.Shutdown();
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to check update ; "+e.Message);
            }
        }
        

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                _manager = await UpdateManager
                    .GitHubUpdateManager(@"https://github.com/ikdyogasegara/WpfUpdateTest");
        
                CurrentVersionTextBox.Text = _manager.CurrentlyInstalledVersion().ToString();
            }
            catch (Exception exception)
            {
                Console.WriteLine("Failed to check update ; "+exception.Message);
            }
            
        }

        private async void CheckForUpdatesButton_Click(object sender, RoutedEventArgs e)
        {
            var updateInfo = await _manager.CheckForUpdate();
            UpdateButton.IsEnabled = updateInfo.ReleasesToApply.Count > 0;
        }

        private async void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            await _manager.UpdateApp();
            MessageBox.Show("Updated succesfuly!");
            Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();
        }
    }
}
