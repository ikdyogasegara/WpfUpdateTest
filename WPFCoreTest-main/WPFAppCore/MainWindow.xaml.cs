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

            Loaded += MainWindow_Loaded;
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                _manager = await UpdateManager
                    .GitHubUpdateManager(@"https://github.com/ikdyogasegara/WpfUpdateTest");

                CurrentVersionTextBox.Text = _manager.CurrentlyInstalledVersion().ToString();
            }
            catch{}
            
        }

        private async void CheckForUpdatesButton_Click(object sender, RoutedEventArgs e)
        {
            var updateInfo = await _manager.CheckForUpdate();

            if (updateInfo.ReleasesToApply.Count > 0)
            {
                UpdateButton.IsEnabled = true;
            }
            else
            {
                UpdateButton.IsEnabled = false;
            }
        }

        private async void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            await _manager.UpdateApp();

            MessageBox.Show("Updated succesfuly!");
        }
    }
}
