using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace SampleApp
{
    public class MainViewModel
    {
        public MainViewModel()
        {
            Item1 = new ItemViewModel() {Color = new SolidColorBrush(Colors.Red)};
            Item2 = new ItemViewModel() { Color = new SolidColorBrush(Colors.CadetBlue) };
            Item3 = new ItemViewModel() { Color = new SolidColorBrush(Colors.BlueViolet) };
            Item4 = new ItemViewModel() { Color = new SolidColorBrush(Colors.Blue) };

            Random random = new Random();
            Items = new List<ItemViewModel>();
            for (int i = 0; i < 15; i++)
            {
                int r = random.Next(255);
                int g = random.Next(255);
                int b = random.Next(255);
                var item = new ItemViewModel() { Color = new SolidColorBrush(Color.FromArgb(255, (byte)r, (byte)g, (byte)b)) };
                Items.Add(item);
            }

            Targer = new DragTargetViewModel() { Color = new SolidColorBrush(Colors.Aquamarine) };
        }

        public List<ItemViewModel> Items { get; set; }

        public ItemViewModel Item1 { get; set; }
        public ItemViewModel Item2 { get; set; }
        public ItemViewModel Item3 { get; set; }
        public ItemViewModel Item4 { get; set; }

        public DragTargetViewModel Targer { get; set; }
    }
}
