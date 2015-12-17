using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Media;
using DragDrop;

namespace SampleApp
{
    public class DragTargetViewModel : INotifyPropertyChanged
    {
        public DragTargetViewModel()
        {
            ItemDropCommand = new DropCommand((sender, args) =>
            {
                if(args == null)
                    return;

                ItemViewModel item = args.DropSourceParameter as ItemViewModel;

                if (item != null)
                    Color = item.Color;
            });
        }
        private SolidColorBrush _color;

        public SolidColorBrush Color { get { return _color; }
            set
            {
                _color = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Color"));
            } 
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand ItemDropCommand { get; private set; }
    }
}
