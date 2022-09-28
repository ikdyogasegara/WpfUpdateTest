using System;
using System.Diagnostics;
using System.Windows;
using Squirrel;

namespace WPFAppCore
{
    public partial class MainWindow : Window
    {
        // private UpdateManager _manager;

        public MainWindow()
        {
            InitializeComponent();
            AddVersionNumber();
            CheckVersionUpdate();
            // Loaded += MainWindow_Loaded;
        }

        private void AddVersionNumber()
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
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
        

        // private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        // {
        //     try
        //     {
        //         _manager = await UpdateManager
        //             .GitHubUpdateManager(@"https://github.com/ikdyogasegara/WpfUpdateTest");
        //
        //         CurrentVersionTextBox.Text = _manager.CurrentlyInstalledVersion().ToString();
        //     }
        //     catch{}
        //     
        // }

        // private async void CheckForUpdatesButton_Click(object sender, RoutedEventArgs e)
        // {
        //     var updateInfo = await _manager.CheckForUpdate();
        //
        //     if (updateInfo.ReleasesToApply.Count > 0)
        //     {
        //         UpdateButton.IsEnabled = true;
        //     }
        //     else
        //     {
        //         UpdateButton.IsEnabled = false;
        //     }
        // }

        // private async void UpdateButton_Click(object sender, RoutedEventArgs e)
        // {
        //     await _manager.UpdateApp();
        //
        //     MessageBox.Show("Updated succesfuly!");
        //     
        //     System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
        //     Application.Current.Shutdown();
        // }
    }
}
