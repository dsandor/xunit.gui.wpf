using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using xunit.gui.wpf.Models;

namespace xunit.gui.wpf.ViewModels
{
    public class MethodViewModel : ViewModelBase
    {
        public MethodViewModel(TestMethod testMethod, ClassViewModel parentClassViewModel)
        {
            this.TestMethod      = testMethod;
            this.Name            = testMethod.MethodName;
            this.ExecutionStatus = ExecutionStatus.NotExecuted;
            this.ResultStatus    = ResultStatus.NotExecuted;
            this.Parent          = parentClassViewModel;
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

        /// <summary>
        /// Private backing variable for ExecutionStatus property.
        /// </summary>
        private ExecutionStatus executionStatus;

        /// <summary>
        /// Public property named ExecutionStatus backed by a private variable named executionStatus that
        /// calls SafeNotify("ExecutionStatus"); when the value changes.
        /// </summary>	
        public ExecutionStatus ExecutionStatus
        {
            get { return this.executionStatus; }
            set
            {
                this.executionStatus = value;
                SafeNotify("ExecutionStatus");
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
        /// Private backing variable for Output property.
        /// </summary>
        private string output;

        /// <summary>
        /// Public property named Output backed by a private variable named output that
        /// calls SafeNotify("Output"); when the value changes.
        /// </summary>	
        public string Output
        {
            get { return this.output; }
            set
            {
                this.output = value;
                SafeNotify("Output");
            }
        }

        public TestMethod TestMethod { get; set; }

        public ClassViewModel Parent { get; set; }
    }
}
