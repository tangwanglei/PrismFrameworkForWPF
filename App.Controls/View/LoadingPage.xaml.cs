using App.Infrastucture;
using System.Windows;
using System.Windows.Media.Animation;

namespace App.Pages.View
{
    /// <summary>
    /// LoadingControl.xaml 的交互逻辑
    /// </summary>
    public partial class LoadingPage : AppBasePage
    {
        public LoadingPage()
        {
            InitializeComponent();

            this.Loaded += new RoutedEventHandler(LoadingControl_OnLoaded);
            this.Loaded -= new RoutedEventHandler(UninstallLoadingControl_OnLoaded);
        }

        private void LoadingControl_OnLoaded(object sender, RoutedEventArgs e)
        {
            Storyboard story = this.Resources["Storyboard1"] as Storyboard;
            story.Begin();
        }

        void UninstallLoadingControl_OnLoaded(object sender, RoutedEventArgs e)
        {
            Storyboard story = this.Resources["Storyboard1"] as Storyboard;
            story.Stop();
        }
    }
}
