using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using DragDrop.Commands;
using DragDrop.Commands.Parameters;

namespace DragDrop
{
    internal class DragDropGroup : Grid, IDragDropGroup
    {
        #region Fields
        private DragDropContainer _parentDragDropContainer;
        private Border _dragThumb;
        private EventHandler _onIsDraggableChangedEventHandler;
        private List<UIElement> _groupElements = new List<UIElement>();
        private UIElement _draggedElement;
        private bool _isDraggingActive;
        private Point _relativeDragPoint;
        private Transform _originalTransform;
        private bool _isTouchActivation;
        private Point _previousTouchPosition;
        private bool _isDraggTouchGesture;
        private Point _firstTouchPoint;
        private UIElement _firstTouchUIElement;
        private const double DragDelta = 15.0;
        #endregion
        
        #region Constructor
        internal DragDropGroup(DragDropContainer parentDragDropContainer)
        {
            _parentDragDropContainer = parentDragDropContainer;
            _dragThumb = new Border();
            _dragThumb.HorizontalAlignment = HorizontalAlignment.Left;
            _dragThumb.VerticalAlignment = VerticalAlignment.Top;
            _onIsDraggableChangedEventHandler = new EventHandler((object sender, EventArgs args) =>
            {
                UIElement groupElement = sender as UIElement;
                if (groupElement != null)
                {
                    bool isDraggable = DragDropContainer.GetIsDraggable(groupElement);
                    if (isDraggable)
                    {
                        registerDragSourceHandler(groupElement);
                    }
                    else
                    {
                        unregisterDragSourceHandler(groupElement);
                    }
                }
            });
        }
        #endregion

        #region IDragDropGroup

        /// <summary>
        /// Removes the given group element
        /// </summary>
        /// <param name="groupElement">
        /// Group element
        /// </param>
        public void RemoveGroupElement(UIElement groupElement)
        {
            if (groupElement != null)
            {
                if (_groupElements.Contains(groupElement))
                {
                    if (IsVisible)
                    {
                        if (_draggedElement != null)
                        {
                            DragDropContainer.SetIsDragActive(groupElement, false);
                        }
                        bool isDraggable = DragDropContainer.GetIsDraggable(groupElement);
                        if (isDraggable)
                        {
                            unregisterDragSourceHandler(groupElement);
                        }
                        unregisterIsDraggableChangedHandler(groupElement);
                    }
                    groupElement.IsVisibleChanged -=
                        new DependencyPropertyChangedEventHandler(groupElement_IsVisibleChanged);
                    _groupElements.Remove(groupElement);
                }
            }
        }

        /// <summary>
        /// Adds the given group element
        /// </summary>
        /// <param name="groupElement">
        /// Group element
        /// </param>
        public void AddGroupElement(UIElement groupElement)
        {
            if (groupElement != null)
            {
                if (!_groupElements.Contains(groupElement))
                {
                    _groupElements.Add(groupElement);
                    groupElement.IsVisibleChanged +=
                        new DependencyPropertyChangedEventHandler(groupElement_IsVisibleChanged);
                    if (groupElement.IsVisible)
                    {
                        registerIsDraggableChangedHandler(groupElement);
                        bool isDraggable = DragDropContainer.GetIsDraggable(groupElement);
                        if (isDraggable)
                        {
                            registerDragSourceHandler(groupElement);
                        }
                        if (_draggedElement != null)
                        {
                            DragDropContainer.SetIsDragActive(groupElement, true);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets a flag indicating whether the drag drop group has group
        /// elements
        /// </summary>
        public bool HasGroupElements
        {
            get { return _groupElements.Count > 0; }
        }

        #region dependency properties

        internal static readonly DependencyProperty DragDropGroupNameProperty =
            DependencyProperty.Register("DragDropGroupName", typeof(string), typeof(DragDropGroup),
                new UIPropertyMetadata(string.Empty));

        public string DragDropGroupName
        {
            get { return (string)GetValue(DragDropGroupNameProperty); }
            set { SetValue(DragDropGroupNameProperty, value); }
        }

        #endregion dependency properties

        #endregion

        #region Registering items for drag & drop

        /// <summary>
        /// Registers the drag source handler for the given group element
        /// </summary>
        /// <param name="groupElement">
        /// Group element
        /// </param>
        private void registerDragSourceHandler(UIElement groupElement)
        {
            //mouse events
            groupElement.MouseMove += groupElement_MouseMove;
            groupElement.MouseLeftButtonDown += groupElement_MouseLeftButtonDown;

            //touch events
            groupElement.StylusSystemGesture += groupElement_StylusSystemGesture;

            groupElement.TouchDown += groupElement_TouchDown;
            groupElement.TouchUp += groupElement_TouchUp;
        }


        /// <summary>
        /// Unregisters the drag source handler for the given group element
        /// </summary>
        /// <param name="groupElement">
        /// Group element
        /// </param>
        private void unregisterDragSourceHandler(UIElement groupElement)
        {
            //mouse events
            groupElement.MouseMove -= groupElement_MouseMove;

            //touch events
            groupElement.StylusSystemGesture -= groupElement_StylusSystemGesture;

            groupElement.TouchDown -= groupElement_TouchDown;
            groupElement.TouchUp -= groupElement_TouchUp;
        }

        #endregion

        #region Group state event handlers

        /// <summary>
        /// Handles the "IsVisibleChanged" event of a group element
        /// </summary>
        /// <param name="sender">
        /// Sender of the event
        /// </param>
        /// <param name="args">
        /// Event argument
        /// </param>
        private void groupElement_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs args)
        {
            UIElement groupElement = sender as UIElement;
            if (groupElement != null)
            {
                if (groupElement.IsVisible)
                {
                    registerIsDraggableChangedHandler(groupElement);
                    bool isDraggable = DragDropContainer.GetIsDraggable(groupElement);
                    if (isDraggable)
                    {
                        registerDragSourceHandler(groupElement);
                    }
                    if (_draggedElement != null)
                    {
                        DragDropContainer.SetIsDragActive(groupElement, true);
                    }
                }
                else
                {
                    if (_draggedElement != null)
                    {
                        DragDropContainer.SetIsDragActive(groupElement, false);
                    }
                    bool isDraggable = DragDropContainer.GetIsDraggable(groupElement);
                    if (isDraggable)
                    {
                        unregisterDragSourceHandler(groupElement);
                    }
                    unregisterIsDraggableChangedHandler(groupElement);
                }
            }
        }

        /// <summary>
        /// Registers a is draggable changed handler for the given group element
        /// </summary>
        /// <param name="groupElement">
        /// Group element
        /// </param>
        private void registerIsDraggableChangedHandler(UIElement groupElement)
        {
            DependencyPropertyDescriptor isDraggableDescriptor =
                DependencyPropertyDescriptor.FromProperty(DragDropContainer.IsDraggableProperty,
                    typeof(DragDropGroup));
            if (isDraggableDescriptor != null)
            {
                isDraggableDescriptor.AddValueChanged(groupElement, _onIsDraggableChangedEventHandler);
            }
        }

        /// <summary>
        /// Unregisters a is draggable changed handler for the given group
        /// element
        /// </summary>
        /// <param name="groupElement">
        /// Group element
        /// </param>
        private void unregisterIsDraggableChangedHandler(UIElement groupElement)
        {
            DependencyPropertyDescriptor isDraggableDescriptor =
                DependencyPropertyDescriptor.FromProperty(DragDropContainer.IsDraggableProperty,
                    typeof(DragDropGroup));
            if (isDraggableDescriptor != null)
            {
                isDraggableDescriptor.RemoveValueChanged(groupElement, _onIsDraggableChangedEventHandler);
            }
        }

        #endregion

        #region DragDrop event handlers
        void groupElement_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Debug.WriteLine("groupElement_MouseLeftButtonDown");

            //set point of first touch
            _firstTouchPoint = e.GetPosition(_parentDragDropContainer);

            //set uielement
            _firstTouchUIElement = sender as UIElement;

            //log start point
            Debug.WriteLine("StartPosition: X= "+ _firstTouchPoint.X + ", Y= " + _firstTouchPoint.Y);
        }

        private void groupElement_TouchDown(object sender, TouchEventArgs e)
        {
            Debug.WriteLine("groupElement_TouchDown");
            //Note: Little hack to improve drag from list with PaningMode
            e.Handled = true;
            _isDraggTouchGesture = true;

            //set point of first touch
            _firstTouchPoint = e.GetTouchPoint(_parentDragDropContainer).Position;
            
            //set uielement
            _firstTouchUIElement = sender as UIElement;
           
            //log start point
            Debug.WriteLine("StartPosition: X= " + _firstTouchPoint.X + ", Y= " + _firstTouchPoint.Y);
        }

        private void groupElement_TouchUp(object sender, TouchEventArgs e)
        {
            Debug.WriteLine("groupElement_TouchUp");
            _isDraggTouchGesture = false;

            if (_draggedElement == null)
                return;

            EndDragDrop();
        }

        private void groupElement_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isDraggTouchGesture)
                return;

            if (e.LeftButton != MouseButtonState.Pressed)
                return;

            if (_firstTouchPoint.X == 0.0 && _firstTouchPoint.Y == 0.0)
                return;

            if (_firstTouchUIElement != sender)
                return;

            var currentPoint = e.GetPosition(_parentDragDropContainer);

            Debug.WriteLine("DragActivationPosition: X= " + currentPoint.X + ", Y= " + currentPoint.Y);

            var xDelta = Math.Max(currentPoint.X, _firstTouchPoint.X) - Math.Min(currentPoint.X, _firstTouchPoint.X);
            var yDelta = Math.Max(currentPoint.Y, _firstTouchPoint.Y) - Math.Min(currentPoint.Y, _firstTouchPoint.Y);

            if (xDelta <= DragDelta && yDelta <= DragDelta)
               return;

            var vector1 = MathHelper.GetVectorPoint(_firstTouchPoint,
                new Point(_firstTouchPoint.X + 100, _firstTouchPoint.Y));
            var vector2 = MathHelper.GetVectorPoint(_firstTouchPoint, currentPoint);
            var cosOfAngle = MathHelper.GetCosOfAngleBetweenVectors(vector1, vector2);
            var angle = MathHelper.RadiansToGradus(Math.Acos(cosOfAngle));

            if (currentPoint.Y < _firstTouchPoint.Y)
                angle = 360 - angle;

            var minAngle = DragDropContainer.GetMinDragAngle(_firstTouchUIElement);
            var maxAngle = DragDropContainer.GetMaxDragAngle(_firstTouchUIElement);

            Debug.WriteLine("Angle:" + angle);
            Debug.WriteLine("MinAngle:" + minAngle);
            Debug.WriteLine("MaxAngle:" + maxAngle);

            if (angle < minAngle || angle > maxAngle)
                return;

            Debug.WriteLine("Gesture: Mouse Drag");
            if (TrySetDraggedElement(_firstTouchUIElement))
            {
                Debug.WriteLine("DragElement: " + _firstTouchUIElement.ToString());
                StartDragDrop(false);
                e.Handled = true;
            }
        }

        private void groupElement_StylusSystemGesture(object sender, StylusSystemGestureEventArgs e)
        {
            if (_firstTouchUIElement != sender)
                return;

            switch (e.SystemGesture)
            {
                case SystemGesture.Drag:
                    Debug.WriteLine("Gesture: Touch Drag");

                    if (!_isDraggTouchGesture)
                        return;

                    var currentPoint = e.GetPosition(_parentDragDropContainer);

                    Debug.WriteLine("DragActivationPosition: X= " + currentPoint.X + ", Y= " + currentPoint.Y);

                    var vector1 = MathHelper.GetVectorPoint(_firstTouchPoint, new Point(_firstTouchPoint.X + 100, _firstTouchPoint.Y));
                    var vector2 = MathHelper.GetVectorPoint(_firstTouchPoint, currentPoint);
                    var cosOfAngle = MathHelper.GetCosOfAngleBetweenVectors(vector1, vector2);
                    var angle = MathHelper.RadiansToGradus(Math.Acos(cosOfAngle));

                    if (currentPoint.Y < _firstTouchPoint.Y)
                        angle = 360 - angle;

                    var minAngle = DragDropContainer.GetMinDragAngle(sender as UIElement);
                    var maxAngle = DragDropContainer.GetMaxDragAngle(sender as UIElement);

                    Debug.WriteLine("Angle:" + angle);
                    Debug.WriteLine("MinAngle:" + minAngle);
                    Debug.WriteLine("MaxAngle:" + maxAngle);

                    if (angle < minAngle || angle > maxAngle)
                        return;

                    if (TrySetDraggedElement(sender as UIElement))
                    {
                        Debug.WriteLine("DragElement: " + sender.ToString());
                        StartDragDrop(true);
                        e.Handled = true;
                    }
                    break;

                case SystemGesture.Tap:
                    break;
            }
        }

        private void _parentDragDropContainer_TouchMove(object sender, TouchEventArgs e)
        {
            FrameworkElement draggedFrameworkElement = _draggedElement as FrameworkElement;
            if (draggedFrameworkElement != null)
            {
                var currentPositionMouse = e.GetTouchPoint(_parentDragDropContainer).Position;

                if (Math.Abs(currentPositionMouse.X - _previousTouchPosition.X) <= DragDelta &&
                    Math.Abs(_previousTouchPosition.Y - currentPositionMouse.Y) <= DragDelta)
                {
                    e.Handled = true;
                    return;
                }

                _previousTouchPosition = currentPositionMouse;

                Debug.WriteLine("CureentPoint = x: {0}; Y: {1}", currentPositionMouse.X, currentPositionMouse.Y);

                ((_dragThumb.RenderTransform as TransformGroup).Children[0] as TranslateTransform).X =
                    currentPositionMouse.X - draggedFrameworkElement.ActualWidth / 2;
                ((_dragThumb.RenderTransform as TransformGroup).Children[0] as TranslateTransform).Y =
                    currentPositionMouse.Y - draggedFrameworkElement.ActualHeight / 2;
            }

            UpdateDragDropTargets();
            e.Handled = true;

        }

        private void _parentDragDropContainer_MouseMove(object sender, MouseEventArgs args)
        {
            Debug.WriteLine("DragDropGroup_MouseMove");

            if (!_isTouchActivation)
            {
                UpdateDragThumbPosition();
                UpdateDragDropTargets();
            }
        }

        private void _parentDragDropContainer_MouseEnter(object sender, MouseEventArgs args)
        {
            Debug.WriteLine("DragDropGroup_MouseEnter");
            if (args != null && args.LeftButton == MouseButtonState.Released)
            {
                InternalChildren.Remove(_dragThumb);
                _parentDragDropContainer.PreviewMouseLeftButtonUp -= _parentDragDropContainer_MouseLeftButtonUp;
                _parentDragDropContainer.MouseMove -= _parentDragDropContainer_MouseMove;
                _parentDragDropContainer.MouseEnter -= _parentDragDropContainer_MouseEnter;
                _draggedElement.RenderTransform = _originalTransform;
                foreach (UIElement groupElement in _groupElements)
                {
                    DragDropContainer.SetIsDragActive(groupElement, false);
                    DragDropContainer.SetIsActiveDropTarget(groupElement, false);
                }
                DragDropContainer.SetIsDragged(_draggedElement, false);
                _draggedElement = null;
            }
        }

        private void _parentDragDropContainer_MouseLeftButtonUp(object sender, MouseButtonEventArgs args)
        {
            Debug.WriteLine("DragDropGroup_MouseLeftButtonUp");

            EndDragDrop();
        }

        private void _parentDragDropContainer_TouchUp(object sender, TouchEventArgs e)
        {
            Debug.WriteLine("_parentDragDropContainer_TouchUp");
            if (_draggedElement == null)
                return;

            EndDragDrop();
        }
        #endregion

        #region DragDrop Controlling

        private bool TrySetDraggedElement(UIElement candidate)
        {
            if (_draggedElement != null)
                return false;

            if (candidate == null)
                return false;

            _draggedElement = candidate;
            return true;
        }

        private void StartDragDrop(bool isTouchActivation)
        {
            Debug.WriteLine("Drag Start");

            if (_isDraggingActive)
            {
                Debug.WriteLine("Drag Start Error: _isDraggingActive = true");
                return;
            }

            _originalTransform = _draggedElement.RenderTransform;
            DragDropContainer.SetIsDragged(_draggedElement, true);

            UpdateElementsInGroup();

            _isTouchActivation = isTouchActivation;

            if (_isTouchActivation)
            {
                _parentDragDropContainer.TouchUp += _parentDragDropContainer_TouchUp;
                _parentDragDropContainer.TouchMove += _parentDragDropContainer_TouchMove;
            }
            else
            {
               // _parentDragDropContainer.MouseEnter += _parentDragDropContainer_MouseEnter;
                _parentDragDropContainer.MouseMove += _parentDragDropContainer_MouseMove;
            }

            _parentDragDropContainer.MouseLeftButtonUp += _parentDragDropContainer_MouseLeftButtonUp;

            InitializeDragThumb();

            UpdateDragThumbPosition();
            InternalChildren.Add(_dragThumb);
            _isDraggingActive = true;

            //dragStarted execution
            var dragStartedCommand = DragDropContainer.GetSourceDragStartedCommand(_draggedElement);
            if(dragStartedCommand!=null)
                dragStartedCommand.Execute(null);
        }

        private async void EndDragDrop()
        {
            if (!_isDraggingActive)
            {
                _firstTouchPoint = new Point(0, 0);
                _firstTouchUIElement = null;
                return;
            }
             
            Debug.WriteLine("Drag End");
           
            if (_isTouchActivation)
            {
                _parentDragDropContainer.TouchUp -= _parentDragDropContainer_TouchUp;
                _parentDragDropContainer.TouchMove -= _parentDragDropContainer_TouchMove;
            }
            else
            {
                //_parentDragDropContainer.MouseEnter -= _parentDragDropContainer_MouseEnter;
                _parentDragDropContainer.MouseMove -= _parentDragDropContainer_MouseMove;
            }

            _parentDragDropContainer.MouseLeftButtonUp -= _parentDragDropContainer_MouseLeftButtonUp;

            //dragFinished execution
            var dragFinishedCommand = DragDropContainer.GetSourceDragFinishedCommand(_draggedElement);

            await ProceedDropOnCurrentPosition();

            if (dragFinishedCommand != null)
                dragFinishedCommand.Execute(null);

            _dragThumb.Child = null;
            _dragThumb.Background = null;
            InternalChildren.Remove(_dragThumb);
            DragDropContainer.SetIsDragged(_draggedElement, false);
            _draggedElement = null;

            _isDraggingActive = false;
            _isDraggTouchGesture = false;
            _firstTouchPoint = new Point(0, 0);
            _firstTouchUIElement = null;
        }

        private void UpdateElementsInGroup()
        {
            for (int i = 0; i < _groupElements.Count; i++)
            {
                var groupElement = _groupElements[i];
                DropCommand sourceDropCommand = DragDropContainer.GetSourceDropCommand(_draggedElement);
                object sourceDropCommandParameter = DragDropContainer.GetSourceDropCommandParameter(_draggedElement);
                DropCommand targetDropCommand = DragDropContainer.GetTargetDropCommand(groupElement);
                object targetDropCommandParameter = DragDropContainer.GetTargetDropCommandParameter(groupElement);
                if (sourceDropCommand != null)
                {
                    if (!sourceDropCommand.CanExecute(this, new DropCommandParameter(_draggedElement, groupElement, sourceDropCommandParameter, targetDropCommandParameter)))
                    {
                        continue;
                    }
                }
                if (targetDropCommand != null)
                {
                    if (!targetDropCommand.CanExecute(this, new DropCommandParameter(_draggedElement, groupElement, sourceDropCommandParameter, targetDropCommandParameter)))
                    {
                        continue;
                    }
                }
                bool isDropTarget = DragDropContainer.GetIsDropTarget(groupElement);
                if (isDropTarget)
                {
                    DragDropContainer.SetIsDragActive(groupElement, true);
                }
            }
        }

        private async Task ProceedDropOnCurrentPosition()
        {
            UIElement targetElement = FindDropTargetAtCurrentPosition();

            //reset properties and commands
            for (int i = 0; i < _groupElements.Count; i++)
            {
                var groupElement = _groupElements[i];
                DragDropContainer.SetIsDragActive(groupElement, false);

                bool previousValue = DragDropContainer.GetIsActiveDropTarget(groupElement);

                if (previousValue && _draggedElement != null)
                {
                    SourceLeaveTargetCommand sourceLeaveTargetCommand =
                    DragDropContainer.GetSourceLeaveTargetCommand(groupElement);
                    object sourceLeaveTargetCommandParameter =
                        DragDropContainer.GetSourceLeaveCommandParameter(_draggedElement);

                    if (sourceLeaveTargetCommand != null)
                    {
                        sourceLeaveTargetCommand.Execute(this,
                            new SourceLeaveTargetCommandParameter(_draggedElement, targetElement,
                                sourceLeaveTargetCommandParameter, null));
                    }

                    DragDropContainer.SetIsActiveDropTarget(groupElement, false);
                }
            }

            //Drop Action
            if (targetElement != null)
            {
                Point offset = new Point(((_dragThumb.RenderTransform as TransformGroup).Children[0] as TranslateTransform).X, ((_dragThumb.RenderTransform as TransformGroup).Children[0] as TranslateTransform).Y);

                DropCommand sourceDropCommand = DragDropContainer.GetSourceDropCommand(_draggedElement);
                object sourceDropCommandParameter = DragDropContainer.GetSourceDropCommandParameter(_draggedElement);
                DropCommand targetDropCommand = DragDropContainer.GetTargetDropCommand(targetElement);
                object targetDropCommandParameter = DragDropContainer.GetTargetDropCommandParameter(targetElement);

                if (sourceDropCommand != null)
                {
                    sourceDropCommand.Execute(this, new DropCommandParameter(_draggedElement, targetElement, sourceDropCommandParameter, targetDropCommandParameter, offset));
                }
                if (targetDropCommand != null)
                {
                    targetDropCommand.Execute(this, new DropCommandParameter(_draggedElement, targetElement, sourceDropCommandParameter, targetDropCommandParameter, offset));
                }
            }
            else
            {
                Point offset = new Point(((_dragThumb.RenderTransform as TransformGroup).Children[0] as TranslateTransform).X, ((_dragThumb.RenderTransform as TransformGroup).Children[0] as TranslateTransform).Y);

                DropCommand sourceDropCommand = DragDropContainer.GetSourceDropCommand(_draggedElement);
                object sourceDropCommandParameter = DragDropContainer.GetSourceDropCommandParameter(_draggedElement);
                if (sourceDropCommand != null)
                {
                    sourceDropCommand.Execute(this, new DropCommandParameter(_draggedElement, null, sourceDropCommandParameter, null, offset));
                }

                await EndDraggingAnimationAsync();
            }
        }

        private async Task EndDraggingAnimationAsync()
        {
            var translate_x = new DoubleAnimation()
            {
                From = ((_dragThumb.RenderTransform as TransformGroup).Children[0] as TranslateTransform).X,
                To = _firstTouchPoint.X - _dragThumb.ActualWidth / 2,
                Duration = TimeSpan.FromSeconds(0.2)
            };
            var translate_y = new DoubleAnimation()
            {
                From = ((_dragThumb.RenderTransform as TransformGroup).Children[0] as TranslateTransform).Y,
                To = _firstTouchPoint.Y - _dragThumb.ActualHeight / 2,
                Duration = TimeSpan.FromSeconds(0.2)
            };

            ((_dragThumb.RenderTransform as TransformGroup).Children[0] as TranslateTransform).BeginAnimation(TranslateTransform.XProperty, translate_x);
            ((_dragThumb.RenderTransform as TransformGroup).Children[0] as TranslateTransform).BeginAnimation(TranslateTransform.YProperty, translate_y);

            await Task.Delay(200);
        }

        private async Task DropAnimationAsync()
        {
            var scale_x = new DoubleAnimation()
            {
                From = _dragThumb.ActualWidth,
                To = 0,
                Duration = TimeSpan.FromSeconds(0.5)
            };

            var scale_y = new DoubleAnimation()
            {
                From = _dragThumb.ActualHeight,
                To = 0,
                Duration = TimeSpan.FromSeconds(0.5),
            };

            ((_dragThumb.RenderTransform as TransformGroup).Children[1] as ScaleTransform).BeginAnimation(ScaleTransform.ScaleXProperty, scale_x);
            ((_dragThumb.RenderTransform as TransformGroup).Children[1] as ScaleTransform).BeginAnimation(ScaleTransform.ScaleYProperty, scale_y);

            await Task.Delay(500);
        }

        private UIElement FindDropTargetAtCurrentPosition()
        {
            Point topLeftDraggedElementCorner =
                _dragThumb.TransformToAncestor(_parentDragDropContainer).Transform(new Point(0, 0));

            Point bottomRightDraggedElementCorner = new Point(topLeftDraggedElementCorner.X + _dragThumb.ActualWidth,
                topLeftDraggedElementCorner.Y + _dragThumb.ActualHeight);

            int draggedElementStartX =
                (int) Math.Round(Math.Min(topLeftDraggedElementCorner.X, bottomRightDraggedElementCorner.X));

            int draggedElementStartY =
                (int) Math.Round(Math.Min(topLeftDraggedElementCorner.Y, bottomRightDraggedElementCorner.Y));

            int draggedElementWidth =
                (int) Math.Round(Math.Abs(bottomRightDraggedElementCorner.X - topLeftDraggedElementCorner.X));

            int draggedElementHeight =
                (int) Math.Round(Math.Abs(bottomRightDraggedElementCorner.Y - topLeftDraggedElementCorner.Y));

            Point draggedElementCenter = new Point(topLeftDraggedElementCorner.X + (_dragThumb.ActualWidth/2d),
                topLeftDraggedElementCorner.Y + (_dragThumb.ActualHeight/2d));

            List<KeyValuePair<double, FrameworkElement>> targets = new List<KeyValuePair<double, FrameworkElement>>();

            for (int i = 0; i < _groupElements.Count; i++)
            {
                var groupElement = _groupElements[i];
                if (groupElement == _draggedElement)
                {
                    continue;
                }
                if (groupElement.IsVisible)
                {
                    if (_draggedElement.IsAncestorOf(groupElement))
                    {
                        continue;
                    }

                    bool isDropTarget = DragDropContainer.GetIsDropTarget(groupElement);
                    if (!isDropTarget)
                    {
                        continue;
                    }

                    FrameworkElement groupFrameworkElement = groupElement as FrameworkElement;

                    if (groupFrameworkElement != null)
                    {
                        Point topLeftTargetElementCorner =
                            groupFrameworkElement.TransformToAncestor(_parentDragDropContainer)
                                .Transform(new Point(0d, 0d));
                        Point bottomRightTargetElementCorner =
                            new Point(topLeftTargetElementCorner.X + groupFrameworkElement.ActualWidth,
                                topLeftTargetElementCorner.Y + groupFrameworkElement.ActualHeight);
                        int targetElementStartX =
                            (int) Math.Round(Math.Min(topLeftTargetElementCorner.X, bottomRightTargetElementCorner.X));
                        int targetElementStartY =
                            (int) Math.Round(Math.Min(topLeftTargetElementCorner.Y, bottomRightTargetElementCorner.Y));
                        int targetElementWidth =
                            (int) Math.Round(Math.Abs(bottomRightTargetElementCorner.X - topLeftTargetElementCorner.X));
                        int targetElementHeight =
                            (int) Math.Round(Math.Abs(bottomRightTargetElementCorner.Y - topLeftTargetElementCorner.Y));

                        Point targetElementCenter = new Point(targetElementStartX + (targetElementWidth / 2d), targetElementStartY + (targetElementHeight / 2d));

                        var collisionMode = DragDropContainer.GetTargetCollisionMode(groupElement);

                        switch (collisionMode)
                        {
                            case DestinationCollisionMode.Circle:
                                if (CollisionHelper.CheckPointCircleCollision(draggedElementCenter, targetElementCenter,
                           Math.Min(targetElementWidth, targetElementHeight) / 2.0))
                                {
                                    double offsetX =
                                        Math.Abs(draggedElementCenter.X -
                                                 (topLeftTargetElementCorner.X + (groupFrameworkElement.ActualWidth / 2d)));
                                    double offsetY =
                                        Math.Abs(draggedElementCenter.Y -
                                                 (topLeftTargetElementCorner.Y + (groupFrameworkElement.ActualHeight / 2d)));
                                    targets.Add(
                                        new KeyValuePair<double, FrameworkElement>(
                                            (Math.Sqrt((offsetX * offsetX) + (offsetY * offsetY))), groupFrameworkElement));
                                }
                                break;

                            case DestinationCollisionMode.Rectangle:
                                if (CollisionHelper.CheckPointRectCollision(draggedElementCenter, new Rect(targetElementStartX, targetElementStartY,targetElementWidth,targetElementHeight)))
                                {
                                    double offsetX =
                                        Math.Abs(draggedElementCenter.X -
                                                 (topLeftTargetElementCorner.X + (groupFrameworkElement.ActualWidth / 2d)));
                                    double offsetY =
                                        Math.Abs(draggedElementCenter.Y -
                                                 (topLeftTargetElementCorner.Y + (groupFrameworkElement.ActualHeight / 2d)));
                                    targets.Add(
                                        new KeyValuePair<double, FrameworkElement>(
                                            (Math.Sqrt((offsetX * offsetX) + (offsetY * offsetY))), groupFrameworkElement));
                                }
                                break;
                        }
                    }
                }
            }


            if (targets.Count > 0)
            {
                double nearestDistance = double.MaxValue;
                FrameworkElement nearestDropTarget = null;
                foreach (KeyValuePair<double, FrameworkElement> pair in targets)
                {
                    if (pair.Key < nearestDistance)
                    {
                        nearestDistance = pair.Key;
                        nearestDropTarget = pair.Value;
                    }
                }
                return nearestDropTarget;
            }

            return null;
        }

        /// <summary>
        /// Initializes the drag thumb
        /// </summary>
        private void InitializeDragThumb()
        {
            //using template
            var template = DragDropContainer.GetDragThumbTemplate(_draggedElement);
            var context = DragDropContainer.GetDragThumbContext(_draggedElement);

            if (template != null && context != null)
            {
                ContentControl control = new ContentControl();
                control.ContentTemplate = template;
                control.Content = context;

                _dragThumb.Child = control;

                Binding heightBinding = new Binding();
                heightBinding.Source = control;
                heightBinding.Path = new PropertyPath("Height");
                _dragThumb.SetBinding(Border.HeightProperty, heightBinding);

                Binding widthBinding = new Binding();
                widthBinding.Source = control;
                widthBinding.Path = new PropertyPath("Width");
                _dragThumb.SetBinding(Border.WidthProperty, widthBinding);
                _dragThumb.SnapsToDevicePixels = true;
            }
            else
            {
                //using brush
                FrameworkElement draggedFrameworkElement = _draggedElement as FrameworkElement;
                if (draggedFrameworkElement != null)
                {
                    _dragThumb.Height = draggedFrameworkElement.ActualHeight;
                    _dragThumb.Width = draggedFrameworkElement.ActualWidth;

                    _dragThumb.Background = new VisualBrush(_draggedElement)
                    {
                        AlignmentX = AlignmentX.Center,
                        AlignmentY = AlignmentY.Center,
                        Stretch = Stretch.None
                    };

                    _dragThumb.Cursor = draggedFrameworkElement.Cursor;
                }
            }

            _dragThumb.SnapsToDevicePixels = true;
            _dragThumb.Opacity = 0.7;
            _relativeDragPoint = Mouse.GetPosition(_parentDragDropContainer);

            _dragThumb.RenderTransform = new TransformGroup();
            (_dragThumb.RenderTransform as TransformGroup).Children.Add(new TranslateTransform(_relativeDragPoint.X,
            _relativeDragPoint.Y));
            (_dragThumb.RenderTransform as TransformGroup).Children.Add(new ScaleTransform());
        }

        /// <summary>
        /// Updates the drag thumb position
        /// </summary>
        private void UpdateDragThumbPosition()
        {
            FrameworkElement draggedFrameworkElement = _draggedElement as FrameworkElement;
            if (draggedFrameworkElement != null)
            {
                Point currentPosition = Mouse.GetPosition(_parentDragDropContainer);
                Debug.WriteLine("CureentPoint = x: {0}; Y: {1}", currentPosition.X, currentPosition.Y);

                ((_dragThumb.RenderTransform as TransformGroup).Children[0] as TranslateTransform).X =
                    currentPosition.X - draggedFrameworkElement.ActualWidth / 2;
                ((_dragThumb.RenderTransform as TransformGroup).Children[0] as TranslateTransform).Y =
                    currentPosition.Y - draggedFrameworkElement.ActualHeight / 2;
            }
        }

        private void UpdateDragDropTargets()
        {
            UIElement dropTarget = FindDropTargetAtCurrentPosition();

            for(int i=0; i< _groupElements.Count; i++)
            {
                var groupElement = _groupElements[i];
                if (groupElement != dropTarget)
                {
                    bool previousValue = DragDropContainer.GetIsActiveDropTarget(groupElement);

                    if (previousValue && _draggedElement != null)
                    {
                        SourceLeaveTargetCommand sourceLeaveTargetCommand =
                        DragDropContainer.GetSourceLeaveTargetCommand(groupElement);
                        object sourceLeaveTargetCommandParameter =
                            DragDropContainer.GetSourceLeaveCommandParameter(_draggedElement);

                        if (sourceLeaveTargetCommand != null)
                        {
                            sourceLeaveTargetCommand.Execute(this,
                                new SourceLeaveTargetCommandParameter(_draggedElement, dropTarget,
                                    sourceLeaveTargetCommandParameter, null));
                        }

                        DragDropContainer.SetIsActiveDropTarget(groupElement, false);
                    }
                }
            }

            if (dropTarget != null)
            {
                bool previousValue = DragDropContainer.GetIsActiveDropTarget(dropTarget);

                if (!previousValue && _draggedElement != null)
                {
                    SourceReachTargetCommand sourceReachTargetCommand =
                        DragDropContainer.GetSourceReachTargetCommand(dropTarget);
                    object sourceReachTargetCommandParameter =
                        DragDropContainer.GetSourceReachCommandParameter(_draggedElement);

                    if (sourceReachTargetCommand != null)
                    {
                        sourceReachTargetCommand.Execute(this,
                            new SourceReachTargetCommandParameter(_draggedElement, dropTarget,
                                sourceReachTargetCommandParameter, null));
                    }

                    DragDropContainer.SetIsActiveDropTarget(dropTarget, true);
                }

            }
        }
        #endregion
    }
}
