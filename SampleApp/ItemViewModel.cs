using System;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Media;

namespace SampleApp
{
    public class ItemViewModel : INotifyPropertyChanged
    {
        public ItemViewModel()
        {
            TapCommand = new RelayCommand(obj =>
            {
                Random random = new Random();
                int r = random.Next(255);
                int g = random.Next(255);
                int b = random.Next(255);
                Color = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, (byte)r, (byte)g, (byte)b));

                if(PropertyChanged!=null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Color"));
            });
        }   
        
        public SolidColorBrush Color { get; set; }
        public ICommand TapCommand { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
