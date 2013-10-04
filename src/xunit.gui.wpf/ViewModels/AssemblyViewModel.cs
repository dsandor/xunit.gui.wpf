using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using xunit.gui.wpf.Models;

namespace xunit.gui.wpf.ViewModels
{
    public class AssemblyViewModel : ViewModelBase
    {
        public AssemblyViewModel(TestAssembly testAssembly)
        {
            foreach (var c in testAssembly.EnumerateClasses())
            {
                // Todo complete this.. 
            }
        }

        /// <summary>
        /// Private backing variable for ResultStatus property.
        /// </summary>
        private ResultStatus resultStatus;

        /// <summary>
        /// Public property named ResultStatus backed by a private variable named resultStatus that
        /// calls SafeNotify("ResultStatus"); when the value changes.
        /// </summary>	
        public ResultStatus ResultStatus
        {
            get { return this.resultStatus; }
            set
            {
                this.resultStatus = value;
                SafeNotify("ResultStatus");
            }
        }

        /// <summary>
        /// Private backing variable for Classes property.
        /// </summary>
        private ObservableCollection<ClassViewModel> classes;

        /// <summary>
        /// Public property named Classes backed by a private variable named myProperty that
        /// calls SafeNotify("Classes"); when the value changes.
        /// </summary>	
        public ObservableCollection<ClassViewModel> Classes
        {
            get { return this.classes; }
            set
            {
                this.classes = value;
                SafeNotify("Classes");
            }
        }
    }
}
