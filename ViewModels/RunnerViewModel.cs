using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using Xunit;

namespace xunit.gui.wpf.ViewModels
{
    public class RunnerViewModel : ViewModelBase, IRunnerViewModel
    {
        MultiAssemblyTestEnvironment mate = new MultiAssemblyTestEnvironment();

        public RunnerViewModel()
        {
            AssemblyLoadCommand = new DelegateCommand(OnAssemblyLoadCommand, CanAssemblyLoad);
        }

        #region Commands

        public ICommand AssemblyLoadCommand { get; set; }

        private bool CanAssemblyLoad()
        {
            return true;
        }

        private void OnAssemblyLoadCommand()
        {
            OpenFileDialog ofd = new OpenFileDialog()
            {
                Title       = "Select Test Assembly",
                Multiselect = false,
                Filter      = "Libraries (*.dll)|*.dll|Executables (*.exe)|*.exe|All files (*.*)|*.*"
            };

            if (ofd.ShowDialog() == true)
            {
                var testAssembly = mate.Load(ofd.FileName, null, true);

                this.TestAssemblies.Add(testAssembly);
            }
        }

        #endregion

        #region Properties

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
