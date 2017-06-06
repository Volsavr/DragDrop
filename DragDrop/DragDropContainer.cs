using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using DragDrop.Commands;

namespace DragDrop
{
    /// <summary>
    /// Represents a drag drop container
    /// </summary>
    public class DragDropContainer : Grid
    {
        #region Attached properties
        static public readonly DependencyProperty DragDropGroupNameProperty = DependencyProperty.RegisterAttached("DragDropGroupName", typeof(string), typeof(DragDropContainer), new UIPropertyMetadata(string.Empty, OnDragDropGroupNameChanged));
        static public string GetDragDropGroupName(DependencyObject obj)
        {
            return (string)obj.GetValue(DragDropGroupNameProperty);
        }
        static public void SetDragDropGroupName(DependencyObject obj, string value)
        {
            obj.SetValue(DragDropGroupNameProperty, value);
        }

        static public readonly DependencyProperty IsDraggableProperty = DependencyProperty.RegisterAttached("IsDraggable", typeof(bool), typeof(DragDropContainer), new UIPropertyMetadata(false));
        static public bool GetIsDraggable(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsDraggableProperty);
        }
        static public void SetIsDraggable(DependencyObject obj, bool value)
        {
            obj.SetValue(IsDraggableProperty, value);
        }

        static public readonly DependencyProperty IsDropTargetProperty = DependencyProperty.RegisterAttached("IsDropTarget", typeof(bool), typeof(DragDropContainer), new UIPropertyMetadata(true));
        static public bool GetIsDropTarget(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsDropTargetProperty);
        }
        static public void SetIsDropTarget(DependencyObject obj, bool value)
        {
            obj.SetValue(IsDropTargetProperty, value);
        }

        static public readonly DependencyProperty DragThumbTemplateProperty = DependencyProperty.RegisterAttached("DragThumbTemplate", typeof(DataTemplate), typeof(DragDropContainer), new UIPropertyMetadata(null));
        static public DataTemplate GetDragThumbTemplate(DependencyObject obj)
        {
            return (DataTemplate)obj.GetValue(DragThumbTemplateProperty);
        }
        static public void SetDragThumbTemplate(DependencyObject obj, DataTemplate value)
        {
            obj.SetValue(DragThumbTemplateProperty, value);
        }

        static public readonly DependencyProperty DragThumbContextProperty = DependencyProperty.RegisterAttached("DragThumbContext", typeof(object), typeof(DragDropContainer), new UIPropertyMetadata(null));
        static public object GetDragThumbContext(DependencyObject obj)
        {
            return (object)obj.GetValue(DragThumbContextProperty);
        }
        static public void SetDragThumbContext(DependencyObject obj, object value)
        {
            obj.SetValue(SourceDropCommandParameterProperty, value);
        }

        static public readonly DependencyProperty TargetCollisionModeProperty = DependencyProperty.RegisterAttached("TargetCollisionMode", typeof(DestinationCollisionMode), typeof(DragDropContainer), new UIPropertyMetadata(DestinationCollisionMode.Circle));
        static public DestinationCollisionMode GetTargetCollisionMode(DependencyObject obj)
        {
            return (DestinationCollisionMode)obj.GetValue(TargetCollisionModeProperty);
        }
        static public void SetTargetCollisionMode(DependencyObject obj, DestinationCollisionMode value)
        {
            obj.SetValue(TargetCollisionModeProperty, value);
        }

        static public readonly DependencyProperty SourceDragStartedCommandProperty = DependencyProperty.RegisterAttached("SourceDragStartedCommand", typeof(ICommand), typeof(DragDropContainer), new UIPropertyMetadata(null));
        static public ICommand GetSourceDragStartedCommand(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(SourceDragStartedCommandProperty);
        }
        static public void SetSourceDragStartedCommand(DependencyObject obj, ICommand value)
        {
            obj.SetValue(SourceDragStartedCommandProperty, value);
        }

        static public readonly DependencyProperty SourceDragStartedCommandParameterProperty = DependencyProperty.RegisterAttached("SourceDragStartedCommandParameter", typeof(object), typeof(DragDropContainer), new UIPropertyMetadata(null));
        static public object GetSourceDragStartedCommandParameterProperty(DependencyObject obj)
        {
            return (object)obj.GetValue(SourceDragStartedCommandParameterProperty);
        }
        static public void SetSourceDragStartedCommandParameterProperty(DependencyObject obj, object value)
        {
            obj.SetValue(SourceDragStartedCommandParameterProperty, value);
        }

        static public readonly DependencyProperty SourceDragFinishedCommandProperty = DependencyProperty.RegisterAttached("SourceDragFinishedCommand", typeof(ICommand), typeof(DragDropContainer), new UIPropertyMetadata(null));
        static public ICommand GetSourceDragFinishedCommand(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(SourceDragFinishedCommandProperty);
        }
        static public void SetSourceDragFinishedCommand(DependencyObject obj, ICommand value)
        {
            obj.SetValue(SourceDragFinishedCommandProperty, value);
        }



        static public readonly DependencyProperty SourceDragFinishedCommandParameterProperty = DependencyProperty.RegisterAttached("SourceDragFinishedCommandParameter", typeof(object), typeof(DragDropContainer), new UIPropertyMetadata(null));
        static public object GetSourceDragFinishedCommandParameterProperty(DependencyObject obj)
        {
            return (object)obj.GetValue(SourceDragFinishedCommandParameterProperty);
        }
        static public void SetSourceDragFinishedCommandParameterProperty(DependencyObject obj, object value)
        {
            obj.SetValue(SourceDragFinishedCommandParameterProperty, value);
        }

        static public readonly DependencyProperty SourceDropCommandProperty = DependencyProperty.RegisterAttached("SourceDropCommand", typeof(DropCommand), typeof(DragDropContainer), new UIPropertyMetadata(null));
        static public DropCommand GetSourceDropCommand(DependencyObject obj)
        {
            return (DropCommand)obj.GetValue(SourceDropCommandProperty);
        }
        static public void SetSourceDropCommand(DependencyObject obj, DropCommand value)
        {
            obj.SetValue(SourceDropCommandProperty, value);
        }

        static public readonly DependencyProperty TargetDropCommandProperty = DependencyProperty.RegisterAttached("TargetDropCommand", typeof(DropCommand), typeof(DragDropContainer), new UIPropertyMetadata(null));
        static public DropCommand GetTargetDropCommand(DependencyObject obj)
        {
            return (DropCommand)obj.GetValue(TargetDropCommandProperty);
        }
        static public void SetTargetDropCommand(DependencyObject obj, DropCommand value)
        {
            obj.SetValue(TargetDropCommandProperty, value);
        }

        static public readonly DependencyProperty SourceDropCommandParameterProperty = DependencyProperty.RegisterAttached("SourceDropCommandParameter", typeof(object), typeof(DragDropContainer), new UIPropertyMetadata(null));
        static public object GetSourceDropCommandParameter(DependencyObject obj)
        {
            return (object)obj.GetValue(SourceDropCommandParameterProperty);
        }
        static public void SetSourceDropCommandParameter(DependencyObject obj, object value)
        {
            obj.SetValue(SourceDropCommandParameterProperty, value);
        }

        static public readonly DependencyProperty TargetDropCommandParameterProperty = DependencyProperty.RegisterAttached("TargetDropCommandParameter", typeof(object), typeof(DragDropContainer), new UIPropertyMetadata(null));
        static public object GetTargetDropCommandParameter(DependencyObject obj)
        {
            return (object)obj.GetValue(TargetDropCommandParameterProperty);
        }
        static public void SetTargetDropCommandParameter(DependencyObject obj, object value)
        {
            obj.SetValue(TargetDropCommandParameterProperty, value);
        }

        static public readonly DependencyProperty SourceReachCommandParameterProperty = DependencyProperty.RegisterAttached("SourceReachCommandParameter", typeof(object), typeof(DragDropContainer), new UIPropertyMetadata(null));
        static public object GetSourceReachCommandParameter(DependencyObject obj)
        {
            return (object)obj.GetValue(SourceReachCommandParameterProperty);
        }
        static public void SetSourceReachCommandParameter(DependencyObject obj, object value)
        {
            obj.SetValue(SourceReachCommandParameterProperty, value);
        }

        static public readonly DependencyProperty SourceLeaveCommandParameterProperty = DependencyProperty.RegisterAttached("SourceLeaveCommandParameter", typeof(object), typeof(DragDropContainer), new UIPropertyMetadata(null));
        static public object GetSourceLeaveCommandParameter(DependencyObject obj)
        {
            return (object)obj.GetValue(SourceLeaveCommandParameterProperty);
        }
        static public void SetSourceLeaveCommandParameter(DependencyObject obj, object value)
        {
            obj.SetValue(SourceLeaveCommandParameterProperty, value);
        }

        static public readonly DependencyProperty SourceLeaveTargetCommandProperty = DependencyProperty.RegisterAttached("SourceLeaveTargetCommand", typeof(SourceLeaveTargetCommand), typeof(DragDropContainer), new UIPropertyMetadata(null));
        static public SourceLeaveTargetCommand GetSourceLeaveTargetCommand(DependencyObject obj)
        {
            return (SourceLeaveTargetCommand)obj.GetValue(SourceLeaveTargetCommandProperty);
        }
        static public void SetSourceLeaveTargetCommand(DependencyObject obj, SourceLeaveTargetCommand value)
        {
            obj.SetValue(SourceLeaveTargetCommandProperty, value);
        }

        static public readonly DependencyProperty SourceReachTargetCommandProperty = DependencyProperty.RegisterAttached("SourceReachTargetCommand", typeof(SourceReachTargetCommand), typeof(DragDropContainer), new UIPropertyMetadata(null));
        static public SourceReachTargetCommand GetSourceReachTargetCommand(DependencyObject obj)
        {
            return (SourceReachTargetCommand)obj.GetValue(SourceReachTargetCommandProperty);
        }
        static public void SetSourceReachTargetCommand(DependencyObject obj, SourceReachTargetCommand value)
        {
            obj.SetValue(SourceReachTargetCommandProperty, value);
        }

        static private readonly DependencyPropertyKey IsDraggedKey = DependencyProperty.RegisterAttachedReadOnly("IsDragged", typeof(bool), typeof(DragDropContainer), new PropertyMetadata(false));
        static public readonly DependencyProperty IsDraggedProperty = IsDraggedKey.DependencyProperty;
        static public bool GetIsDragged(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsDraggedProperty);
        }
        static internal void SetIsDragged(DependencyObject obj, bool value)
        {
            obj.SetValue(IsDraggedKey, value);
        }

        static private readonly DependencyPropertyKey IsDragActiveKey = DependencyProperty.RegisterAttachedReadOnly("IsDragActive", typeof(bool), typeof(DragDropContainer), new PropertyMetadata(false));
        static public readonly DependencyProperty IsDragActiveProperty = IsDragActiveKey.DependencyProperty;
        static public bool GetIsDragActive(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsDragActiveProperty);
        }
        static internal void SetIsDragActive(DependencyObject obj, bool value)
        {
            obj.SetValue(IsDragActiveKey, value);
        }

        static private readonly DependencyPropertyKey IsActiveDropTargetKey = DependencyProperty.RegisterAttachedReadOnly("IsActiveDropTarget", typeof(bool), typeof(DragDropContainer), new PropertyMetadata(false));
        static public readonly DependencyProperty IsActiveDropTargetProperty = IsActiveDropTargetKey.DependencyProperty;
        static public bool GetIsActiveDropTarget(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsActiveDropTargetProperty);
        }
        static public void SetIsActiveDropTarget(DependencyObject obj, bool value)
        {
            obj.SetValue(IsActiveDropTargetKey, value);
        }

        static internal readonly DependencyProperty ParentDragDropContainerProperty = DependencyProperty.RegisterAttached("ParentDragDropContainer", typeof(DragDropContainer), typeof(DragDropContainer), new UIPropertyMetadata(null));
        static internal DragDropContainer GetParentDragDropContainer(DependencyObject obj)
        {
            return (DragDropContainer)obj.GetValue(ParentDragDropContainerProperty);
        }
        static internal void SetParentDragDropContainer(DependencyObject obj, DragDropContainer value)
        {
            obj.SetValue(ParentDragDropContainerProperty, value);
        }

        static public readonly DependencyProperty MinDragAngleProperty = DependencyProperty.RegisterAttached("MinDragAngle", typeof(double), typeof(DragDropContainer), new UIPropertyMetadata(0.0));
        static public double GetMinDragAngle(DependencyObject obj)
        {
            return (double)obj.GetValue(MinDragAngleProperty);
        }
        static public void SetMinDragAngle(DependencyObject obj, double value)
        {
            obj.SetValue(MinDragAngleProperty, value);
        }

        static public readonly DependencyProperty MaxDragAngleProperty = DependencyProperty.RegisterAttached("MaxDragAngle", typeof(double), typeof(DragDropContainer), new UIPropertyMetadata(360.0));
        static public double GetMaxDragAngle(DependencyObject obj)
        {
            return (double)obj.GetValue(MaxDragAngleProperty);
        }
        static public void SetMaxDragAngle(DependencyObject obj, double value)
        {
            obj.SetValue(MaxDragAngleProperty, value);
        }
        #endregion attached properties

        #region Static Methods
        /// <summary>
        /// Handles the "Changed" event of the "DragDropGroupName" attached
        /// property
        /// </summary>
        /// <param name="sender">
        /// Sender of the event
        /// </param>
        /// <param name="args">
        /// Event argument
        /// </param>
        static private void OnDragDropGroupNameChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            FrameworkElement groupElement = sender as FrameworkElement;
            if (groupElement != null)
            {
                if (groupElement.IsLoaded)
                {
                    DragDropContainer dragDropContainer = FindParent<DragDropContainer>(groupElement);
                    if (dragDropContainer != null)
                    {
                        SetParentDragDropContainer(groupElement, dragDropContainer);
                        dragDropContainer.OnDragDropGroupNameChanged(groupElement, args.OldValue as string, args.NewValue as string);
                    }
                    groupElement.Unloaded -= new RoutedEventHandler(groupElement_Unloaded);
                    groupElement.Loaded -= new RoutedEventHandler(groupElement_Loaded);
                    groupElement.Unloaded += new RoutedEventHandler(groupElement_Unloaded);
                    groupElement.IsVisibleChanged -= groupElement_IsVisibleChanged;
                    groupElement.IsVisibleChanged += groupElement_IsVisibleChanged;
                }
                else
                {
                    groupElement.Unloaded -= new RoutedEventHandler(groupElement_Unloaded);
                    groupElement.Loaded -= new RoutedEventHandler(groupElement_Loaded);
                    groupElement.IsVisibleChanged -= groupElement_IsVisibleChanged;
                    groupElement.Loaded += new RoutedEventHandler(groupElement_Loaded);
                }
            }
        }

        static void groupElement_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            FrameworkElement groupElement = sender as FrameworkElement;
            if (groupElement != null)
            {
                DragDropContainer dragDropContainer = FindParent<DragDropContainer>(groupElement);
                if (dragDropContainer != null)
                {
                    if (groupElement.IsVisible)
                    {
                        SetParentDragDropContainer(groupElement, dragDropContainer);
                        dragDropContainer.OnDragDropGroupNameChanged(groupElement, null,
                            GetDragDropGroupName(groupElement));
                    }
                    else
                    {
                        dragDropContainer.OnDragDropGroupNameChanged(groupElement, GetDragDropGroupName(groupElement), null);
                    }
                }
            }
        }

        /// <summary>
        /// Handles the "Loaded" event of a group element
        /// </summary>
        /// <param name="sender">
        /// Sender of the event
        /// </param>
        /// <param name="args">
        /// Event argument
        /// </param>
        public static void groupElement_Loaded(object sender, RoutedEventArgs args)
        {
            FrameworkElement groupElement = sender as FrameworkElement;
            if (groupElement != null)
            {
                DragDropContainer dragDropContainer = FindParent<DragDropContainer>(groupElement);
                if (dragDropContainer != null && args != null)
                {
                    SetParentDragDropContainer(groupElement, dragDropContainer);
                    dragDropContainer.OnDragDropGroupNameChanged(groupElement, null, GetDragDropGroupName(groupElement));
                }
                groupElement.Unloaded -= new RoutedEventHandler(groupElement_Unloaded);
                groupElement.Loaded -= new RoutedEventHandler(groupElement_Loaded);
                groupElement.Unloaded += new RoutedEventHandler(groupElement_Unloaded);
                groupElement.IsVisibleChanged -= groupElement_IsVisibleChanged;
                groupElement.IsVisibleChanged += groupElement_IsVisibleChanged;
            }
        }

        /// <summary>
        /// Handles the "Unloaded" event of a group element
        /// </summary>
        /// <param name="sender">
        /// Sender of the event
        /// </param>
        /// <param name="args">
        /// Event argument
        /// </param>
        static private void groupElement_Unloaded(object sender, RoutedEventArgs args)
        {
            FrameworkElement groupElement = sender as FrameworkElement;

            if (groupElement != null)
            {
                DragDropContainer dragDropContainer = FindParent<DragDropContainer>(groupElement);
                if (dragDropContainer != null && args != null)
                {
                    dragDropContainer.OnDragDropGroupNameChanged(groupElement, GetDragDropGroupName(groupElement), null);
                }
                groupElement.Unloaded -= new RoutedEventHandler(groupElement_Unloaded);
                groupElement.Loaded -= new RoutedEventHandler(groupElement_Loaded);
                groupElement.IsVisibleChanged -= groupElement_IsVisibleChanged;
                groupElement.Loaded += new RoutedEventHandler(groupElement_Loaded);
            }
        }

        /// <summary>
        /// Handles the "Changed" event of the "DragDropGroupName" attached
        /// property of the given node element
        /// </summary>
        /// <param name="groupElement">
        /// Group element
        /// </param>
        /// <param name="oldDragDropGroupName">
        /// Old drag drop group name
        /// </param>
        /// <param name="newDragDropGroupName">
        /// New drag drop group name
        /// </param>
        private void OnDragDropGroupNameChanged(FrameworkElement groupElement, string oldDragDropGroupName, string newDragDropGroupName)
        {
            if (groupElement != null)
            {
                if (!string.IsNullOrEmpty(oldDragDropGroupName))
                {
                    IDragDropGroup dragDropGroup = FindDragDropGroup(oldDragDropGroupName);
                    if (dragDropGroup != null)
                    {
                        dragDropGroup.RemoveGroupElement(groupElement);
                        if (!dragDropGroup.HasGroupElements)
                        {
                            InternalChildren.Remove(dragDropGroup as UIElement);
                        }
                    }
                }
                if (!string.IsNullOrEmpty(newDragDropGroupName))
                {
                    IDragDropGroup dragDropGroup = FindDragDropGroup(newDragDropGroupName);
                    if (dragDropGroup != null)
                    {
                        dragDropGroup.AddGroupElement(groupElement);
                    }
                    else
                    {
                        dragDropGroup = new DragDropGroup(this);
                        dragDropGroup.DragDropGroupName = newDragDropGroupName;
                        dragDropGroup.AddGroupElement(groupElement);
                        InternalChildren.Add(dragDropGroup as UIElement);
                    }
                }
            }
        }

        /// <summary>
        /// Finds the drag drop group with the given name
        /// </summary>
        /// <param name="dragDropGroupName">
        /// Drag drop group name
        /// </param>
        /// <returns>
        /// Drag drop group with the given name or null
        /// </returns>
        private IDragDropGroup FindDragDropGroup(string dragDropGroupName)
        {
            foreach (UIElement uiElement in InternalChildren)
            {
                IDragDropGroup dragDropGroup = uiElement as IDragDropGroup;
                if (dragDropGroup != null && dragDropGroup.DragDropGroupName == dragDropGroupName)
                {
                    return dragDropGroup;
                }
            }
            return null;
        }

        /// <summary>
        /// Finds the parent element of the given type for the given child
        /// element
        /// </summary>
        /// <typeparam name="T">
        /// Type of the parent to find
        /// </typeparam>
        /// <param name="childElement">
        /// Child element
        /// </param>
        /// <returns>
        /// Parent element or null
        /// </returns>
        static private T FindParent<T>(DependencyObject childElement) where T : DependencyObject
        {
            DependencyObject visualParent = VisualTreeHelper.GetParent(childElement);
            if (visualParent == null)
            {
                return null;
            }
            T parent = visualParent as T;
            if (parent != null)
            {
                return parent;
            }
            else
            {
                return FindParent<T>(visualParent);
            }
        }
        #endregion
    }
}
