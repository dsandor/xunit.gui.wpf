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
    public class ClassViewModel : ViewModelBase
    {
        public ClassViewModel(TestClass testClass)
        {
            this.Methods = new ObservableCollection<MethodViewModel>();

            foreach (var m in testClass.EnumerateTestMethods())
            {
                this.Methods.Add(new MethodViewModel(m));    
            }

            this.ResultStatus = ResultStatus.NotExecuted;
            this.Name = testClass.TypeName;
        }

        /// <summary>
        /// Private backing variable for Methods property.
        /// </summary>
        private ObservableCollection<MethodViewModel> methods;

        /// <summary>
        /// Public property named Methods backed by a private variable named methods that
        /// calls SafeNotify("Methods"); when the value changes.
        /// </summary>	
        public ObservableCollection<MethodViewModel> Methods
        {
            get { return this.methods; }
            set
            {
                this.methods = value;
                SafeNotify("Methods");
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
        /// Private backing variable for Name property.
        /// </summary>
        private string name;

        /// <summary>
        /// Public property named Name backed by a private variable named name that
        /// calls SafeNotify("Name"); when the value changes.
        /// </summary>	
        public string Name
        {
            get { return this.name; }
            set
            {
                this.name = value;
                SafeNotify("Name");
            }
        }
    }
}
