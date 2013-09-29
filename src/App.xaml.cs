using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using HaveBox;
using xunit.gui.wpf.ViewModels;
using xunit.gui.wpf.Views;

namespace xunit.gui.wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static Container Container { get; set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            Container = new Container();
            Container.Configure(config => config.For<IRunnerViewModel>().Use<RunnerViewModel>().AsSingleton());
            Container.Configure(config => config.For<RunnerView>().Use<RunnerView>());

            base.OnStartup(e);

            var runnerView = Container.GetInstance<RunnerView>();
            runnerView.Show();
        }
    }
}
