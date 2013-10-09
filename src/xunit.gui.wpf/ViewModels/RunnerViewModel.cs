using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
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
        private readonly MultiAssemblyTestEnvironment mate = new MultiAssemblyTestEnvironment();
        private readonly string mruFilename = "xunit.gui.wpf.mru";

        public RunnerViewModel()
        {
            this.TestAssemblies = new ObservableCollection<TestAssembly>();
            this.Assemblies = new ObservableCollection<AssemblyViewModel>();

            this.AssemblyLoadCommand = new DelegateCommand(OnAssemblyLoadCommand, CanAssemblyLoad);
            this.LoadMruCommand = new DelegateCommand(OnLoadMru);

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
    }
}
