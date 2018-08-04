using DiscreteMathCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Approximation
{
    public class MainViewModel : BaseViewModel
    {
        private Dictionary<string, Func<double, double>> FFunctions =
            new Dictionary<string, Func<double, double>>();

        public MainViewModel()
        {
            this.FFunctions.Add("Sin(x)", Math.Sin);
            this.FFunctions.Add("Exp(x)", Math.Exp);
            this.FFunctions.Add("Sqrt(x)", Math.Sqrt);
            this.FFunctions.Add("Sqr(x)", new Func<double, double>(x => Math.Pow(x, 2)));

            this.FApproximator = new Approximator(this.FFunctions.Values.First());
        }

        private Approximator FApproximator;

        public Dictionary<string, Func<double, double>> Functions
        {
            get { return this.FFunctions; }
        }

        public static string SelectedFunctionPropertyName = "SelectedFunction";
        private string FSelectedFunction;
        public string SelectedFunction
        {
            get { return this.FSelectedFunction; }
            set
            {
                this.FSelectedFunction = value;
                this.FApproximator.Func = this.Function;
                NotifyPropertyChanged(SelectedFunctionPropertyName);
            }
        }

        public Func<double, double> Function
        {
            get
            {
                return this.FFunctions[this.SelectedFunction];
            }
        }

        public static string FromPropertyName = "From";
        public double From
        {
            get { return this.FApproximator.From; }
            set
            {
                this.FApproximator.From = value;
                NotifyPropertyChanged(FromPropertyName);
            }
        }

        public static string ToPropertyName = "To";
        public double To
        {
            get { return this.FApproximator.To; }
            set
            {
                this.FApproximator.To = value;
                NotifyPropertyChanged(ToPropertyName);
            }
        }

        public static string DegreePropertyName = "Degree";
        public int Degree
        {
            get { return this.FApproximator.Degree; }
            set
            {
                this.FApproximator.Degree = value;
                NotifyPropertyChanged(DegreePropertyName);
            }
        }

        public static string PolynomPropertyName = "Polynom";
        private Polynom<double, Real> FPolynom;
        public Polynom<double, Real> Polynom
        {
            get { return this.FPolynom; }
            set
            {
                this.FPolynom = value;
                NotifyPropertyChanged(PolynomPropertyName);
            }
        }

        #region =Commands=

        private DelegateCommand FApproximateCommand = 
            new DelegateCommand(ExecuteApproximate, CanExecuteApproximate);

        public DelegateCommand ApproximateCommand
        {
            get { return this.FApproximateCommand; }
        }

        private static void ExecuteApproximate(object aCommandData)
        {
            var _vm = (MainViewModel)aCommandData;
            var _polynom = _vm.FApproximator.GetPolynom();
            _vm.Polynom = _polynom;
        }

        private static bool CanExecuteApproximate(object aCommandData)
        {
            return true;
        }
        # endregion

    }
}
