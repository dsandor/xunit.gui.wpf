using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using Xunit;
using xunit.gui.wpf.Models;

namespace xunit.gui.wpf.ViewModels
{
    public class RunnerViewModel : ViewModelBase, IRunnerViewModel
    {
        MultiAssemblyTestEnvironment mate = new MultiAssemblyTestEnvironment();

        public RunnerViewModel()
        {
            this.TestAssemblies = new ObservableCollection<TestAssembly>();
            this.Assemblies = new ObservableCollection<AssemblyViewModel>();

            this.AssemblyLoadCommand = new DelegateCommand(OnAssemblyLoadCommand, CanAssemblyLoad);

            this.TestAssemblies.CollectionChanged += TestAssembliesOnCollectionChanged;
        }

        private void TestAssembliesOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            //var traits = mate.EnumerateTraits();
            var assemblies = mate.EnumerateTestAssemblies();
            //var methods = mate.EnumerateTestMethods();


        }

        #region Commands

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
                var testAssembly = mate.Load(ofd.FileName, null, true);

                this.TestAssemblies.Add(testAssembly);

                var assembly = new AssemblyViewModel(testAssembly);
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
        
        #endregion
    }
}
