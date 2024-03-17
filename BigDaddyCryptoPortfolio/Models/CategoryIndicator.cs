using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigDaddyCryptoPortfolio.Models
{
    public class CategoryIndicator : INotifyPropertyChanged
    {
        public string CategoryName { get; set; }
        public Color CategoryColor { get; set; }

        private Color _startColor;
        public Color StartColor
        {
            get => _startColor;
            set
            {
                _startColor = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StartColor)));
            }
        }

        private Color _endColor;
        public Color EndColor
        {
            get => _endColor;
            set
            {
                _endColor = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(EndColor)));
            }
        }

        private double _percentage;
        public double Percentage
        {
            get => _percentage;
            set
            {
                _percentage = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Percentage)));
            }
        }

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsSelected)));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
