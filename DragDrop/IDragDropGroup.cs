using System.Windows;

namespace DragDrop
{
    internal interface IDragDropGroup
    {
        void RemoveGroupElement(UIElement groupElement);
        void AddGroupElement(UIElement groupElement);
        bool HasGroupElements { get; }
        string DragDropGroupName { get; set; }
    }
}
