using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using xunit.gui.wpf.Models;

namespace xunit.gui.wpf.Converters
{
    public class ResultStatusToVisualBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (!(value is ResultStatus)) return null;

            var status = (ResultStatus)value;

            switch (status)
            {
                case ResultStatus.Executing:
                    return ExecutingBrush;
                case ResultStatus.Failed:
                    return FailedBrush;
                case ResultStatus.NotExecuted:
                    return NotExecutedBrush;
                case ResultStatus.Passed:
                    return PassedBrush;
                case ResultStatus.Skipped:
                    return SkippedBrush;
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public VisualBrush ExecutingBrush   { get; set; }
        public VisualBrush FailedBrush      { get; set; }
        public VisualBrush NotExecutedBrush { get; set; }
        public VisualBrush PassedBrush      { get; set; }
        public VisualBrush SkippedBrush     { get; set; }
    }
}
