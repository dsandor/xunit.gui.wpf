using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using Xunit;
using xunit.gui.wpf.Models;

namespace xunit.gui.wpf.ViewModels
{
    public class RunnerViewModel : ViewModelBase, IRunnerViewModel, ITestMethodRunnerCallback
    {
        private readonly MultiAssemblyTestEnvironment mate = new MultiAssemblyTestEnvironment();
        private readonly string mruFilename = "xunit.gui.wpf.mru";

        public RunnerViewModel()
        {
            this.TestAssemblies = new ObservableCollection<TestAssembly>();
            this.Assemblies = new ObservableCollection<AssemblyViewModel>();

            this.AssemblyLoadCommand         = new DelegateCommand(OnAssemblyLoadCommand, CanAssemblyLoad);
            this.LoadMruCommand              = new DelegateCommand(OnLoadMru);
            this.ExecuteAllTestsCommand      = new DelegateCommand(this.OnExecuteAllTests);
            this.ExecuteSelectedTestsCommand = new DelegateCommand(this.OnExecuteSelectedTests);

            this.TestAssemblies.CollectionChanged += TestAssembliesOnCollectionChanged;

            if (File.Exists(mruFilename))
            {
                this.MRUs = this.MRUs.LoadFromXml(mruFilename);
            }
            else
            {
                this.MRUs = new MostRecentlyUsedList();
            }
        }

        private void TestAssembliesOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            var assemblies = mate.EnumerateTestAssemblies();

            foreach (var a in assemblies)
            {
                this.Assemblies.Add(new AssemblyViewModel(a));
            }
        }

        #region Commands

        public ICommand ExecuteAllTestsCommand { get; set; }

        private void OnExecuteAllTests()
        {
            Task.Factory.StartNew(() =>
            {
                mate.Run(mate.EnumerateTestMethods(), this);
            });
        }

        public ICommand ExecuteSelectedTestsCommand { get; set; }

        private void OnExecuteSelectedTests()
        {

        }

        public ICommand LoadMruCommand { get; set; }

        private void OnLoadMru(object parameter)
        {
            var mostRecentlyUsedItem = parameter as MostRecentlyUsedItem;
            if (mostRecentlyUsedItem != null) LoadAssembly(mostRecentlyUsedItem.Filename);
        }

        public ICommand AssemblyLoadCommand { get; set; }

        private bool CanAssemblyLoad()
        {
            return true;
        }

        private void OnAssemblyLoadCommand()
        {
            var ofd = new OpenFileDialog()
            {
                Title       = "Select Test Assembly",
                Multiselect = false,
                Filter      = "Libraries (*.dll)|*.dll|Executables (*.exe)|*.exe|All files (*.*)|*.*"
            };

            if (ofd.ShowDialog() == true)
            {
                LoadAssembly(ofd.FileName);
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Private backing variable for Assemblies property.
        /// </summary>
        private ObservableCollection<AssemblyViewModel> assemblies;

        /// <summary>
        /// Public property named Assemblies backed by a private variable named assemblies that
        /// calls SafeNotify("Assemblies"); when the value changes.
        /// </summary>	
        public ObservableCollection<AssemblyViewModel> Assemblies
        {
            get { return this.assemblies; }
            set
            {
                this.assemblies = value;
                SafeNotify("Assemblies");
            }
        }

        /// <summary>
        /// Private backing variable for TestAssemblies property.
        /// </summary>
        private ObservableCollection<TestAssembly> testAssemblies;

        /// <summary>
        /// Public property named TestAssemblies backed by a private variable named testAssemblies that
        /// calls SafeNotify("TestAssemblies"); when the value changes.
        /// </summary>	
        public ObservableCollection<TestAssembly> TestAssemblies
        {
            get { return this.testAssemblies; }
            set
            {
                this.testAssemblies = value;
                SafeNotify();
            }
        }

        /// <summary>
        /// Private backing variable for MRUs property.
        /// </summary>
        private MostRecentlyUsedList mrus;

        /// <summary>
        /// Public property named MRUs backed by a private variable named mrus that
        /// calls SafeNotify("MRUs"); when the value changes.
        /// </summary>	
        public MostRecentlyUsedList MRUs
        {
            get { return this.mrus; }
            set
            {
                this.mrus = value;
                SafeNotify();
            }
        }

        #endregion

        private void LoadAssembly(string filename)
        {
            var testAssembly = mate.Load(filename, null, true);

            this.TestAssemblies.Add(testAssembly);

            var assembly = new AssemblyViewModel(testAssembly);

            if (this.MRUs.Count(p => System.String.Compare(p.Filename, filename, StringComparison.OrdinalIgnoreCase) == 0) == 0)
            {
                this.MRUs.Add(new MostRecentlyUsedItem() {Filename = filename, LastUsed = DateTime.Now});
                this.MRUs.PersistToXmlFile(this.mruFilename);
            }
        }

        #region ITestMethodRunnerCallback methods.

        public void AssemblyFinished(TestAssembly testAssembly, int total, int failed, int skipped, double time)
        {

            var assemblyViewModel = this.Assemblies.First(p => p.TestAssembly == testAssembly);

            if (failed > 0)
            {
                App.Current.Dispatcher.Invoke(() => { assemblyViewModel.ResultStatus = ResultStatus.Failed; });

            }
            else
            {
                App.Current.Dispatcher.Invoke(() => { assemblyViewModel.ResultStatus = ResultStatus.Passed; });
            }
        }

        public void AssemblyStart(TestAssembly testAssembly)
        {
            var assemblyViewModel = this.Assemblies.First(p => p.TestAssembly == testAssembly);
            assemblyViewModel.ResultStatus = ResultStatus.Executing;
            App.Current.Dispatcher.Invoke(() => { assemblyViewModel.ResultStatus = ResultStatus.Executing; });
        }

        public bool ClassFailed(TestClass testClass, string exceptionType, string message, string stackTrace)
        {
            var c = (from assembly in this.Assemblies
                from cl in assembly.Classes
                where cl.TestClass == testClass
                select cl).First();

            App.Current.Dispatcher.Invoke(() => { c.ResultStatus = ResultStatus.Failed; });

            return true;
        }

        public void ExceptionThrown(TestAssembly testAssembly, Exception exception)
        {
            var assemblyViewModel = (from assembly in this.Assemblies
                                   where assembly.TestAssembly == testAssembly
                                   select assembly).First();

            App.Current.Dispatcher.Invoke(() => { assemblyViewModel.ResultStatus = ResultStatus.Failed; });
        }

        public bool TestFinished(TestMethod testMethod)
        {
            var methodViewModel = (from assemblyViewModel in this.Assemblies
                                   from classViewModel in assemblyViewModel.Classes
                                   from method in classViewModel.Methods
                                   where method.TestMethod == testMethod
                                   select method).First();

            App.Current.Dispatcher.Invoke(() => { methodViewModel.ResultStatus = ResultStatus.Passed; });

            return true;
        }

        public bool TestStart(TestMethod testMethod)
        {
            var methodViewModel = (from assemblyViewModel in this.Assemblies
                from classViewModel in assemblyViewModel.Classes
                from method in classViewModel.Methods
                where method.TestMethod == testMethod
                select method).First();

            App.Current.Dispatcher.Invoke(() => { methodViewModel.ResultStatus = ResultStatus.Executing; });

            return true;
        }
        #endregion

    }
}
