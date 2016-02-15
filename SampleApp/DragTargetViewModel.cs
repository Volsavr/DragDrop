using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Media;
using DragDrop.Commands;

namespace SampleApp
{
    public class DragTargetViewModel : INotifyPropertyChanged
    {
        #region Fields

        private bool _isActive;
        private SolidColorBrush _color;
        private SolidColorBrush _previousColor;
        #endregion

        #region Constructor

        public DragTargetViewModel()
        {
            ItemDropCommand = new DropCommand((sender, args) =>
            {
                if (args == null)
                    return;

                ItemViewModel item = args.SourceParameter as ItemViewModel;

                if (item != null)
                {
                    Color = item.Color;
                    _previousColor = Color;
                }
            });

            SourceReachCommand = new SourceReachTargetCommand((sender, args) =>
            {
                _previousColor = Color;
                Color = new SolidColorBrush(Colors.BlanchedAlmond);
            });


            SourceLeaveCommand = new SourceLeaveTargetCommand((sender, args) =>
            {
                Color = _previousColor;
            });
        }

        #endregion


        #region Properties

        public SolidColorBrush Color
        {
            get { return _color; }
            set
            {
                _color = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Color"));
            }
        }

        public bool IsActive
        {
            get { return _isActive; }
            set { _isActive = value; }
        }

        #endregion

        #region Commands
        public ICommand ItemDropCommand { get; private set; }
        public ICommand SourceReachCommand { get; private set; }
        public ICommand SourceLeaveCommand { get; private set; }
        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

    }
}
