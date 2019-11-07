using FindingExtremumes.GUI.Infrastructure;
using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FindingExtremumes.GUI.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private string formula = "pow(x, 2) + x + 1";

        public string Formula
        {
            get => formula;
            set
            {
                formula = value;
                RaisePropertyChanged(nameof(Formula));
            }
        }

        private double epsillion = 0.5;

        public double Epsillion
        {
            get => epsillion;
            set
            {
                epsillion = value;
                RaisePropertyChanged(nameof(Epsillion));
            }
        }

        private double delta = 0.01;

        public double Delta
        {
            get => delta;
            set
            {
                delta = value;
                RaisePropertyChanged(nameof(Delta));
            }
        }

        private double start = -100;

        public double Start
        {
            get => start;
            set
            {
                start = value;
                RaisePropertyChanged(nameof(Start));
            }
        }

        private double end = 100;

        public double End
        {
            get => end;
            set
            {
                end = value;
                RaisePropertyChanged(nameof(End));
            }
        }

        private double result;

        public double Result
        {
            get => result;
            set
            {
                result = value;
                RaisePropertyChanged(nameof(Result));
            }
        }

        public ICommand FindExtremume => new RelayCommand(Start_FindExtremume, q => true);

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public async void Start_FindExtremume(object param)
        {
            var compiler = new Compiler.Compiler();
            var compilerResult = await compiler.Compile(Formula);
            var lambda = compiler.GetLambda(compilerResult);

            Result = await Task.Run(() => EstimateExtremume(lambda));
        }

        /// <summary>
        /// Estimates extremume with given inputs.
        /// </summary>
        private double EstimateExtremume(Func<double, double> lambda)
        {
            var y = 0;
            var a = Start;
            var b = End;

            while ((b - a + delta) / 2 > Epsillion)
            {
                var x1 = (a + b - Delta) / 2;
                var x2 = (a + b + Delta) / 2;

                var i1 = lambda(x1);
                var i2 = lambda(x2);

                if (i1 < i2)
                {
                    b = x2;
                }
                else
                {
                    a = x1;
                }
            }

            return Enumerable.Min(new[] { lambda(a), lambda(b) });
        }
    }
}
