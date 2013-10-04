using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xunit.gui.wpf.Models;

namespace xunit.gui.wpf.ViewModels
{
    public class ClassViewModel : ViewModelBase
    {
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
    }
}
