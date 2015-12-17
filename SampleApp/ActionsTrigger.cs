using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace SampleApp
{
    public class ActionsTrigger : TriggerBase<UIElement>
    {
        #region Protected Members
        protected override void OnAttached()
        {
            base.OnAttached();

            if (base.AssociatedObject != null)
            {
                AttachHandlers();
            }
        }
        protected override void OnDetaching()
        {
            if (base.AssociatedObject != null)
            {
                DetachHandlers();
            }
            base.OnDetaching();
        }

        #endregion

        #region ActionType
        public ActionType ActionType
        {
            get { return (ActionType)GetValue(ActionTypeProperty); }
            set { SetValue(ActionTypeProperty, value); }
        }
        public static readonly DependencyProperty ActionTypeProperty =
            DependencyProperty.Register("ActionType", typeof(ActionType)
            , typeof(ActionsTrigger)
            , new UIPropertyMetadata(null));
        #endregion

        #region Private methods
        void AttachHandlers()
        {
            base.AssociatedObject.MouseLeftButtonUp += AssociatedObject_MouseDown;
            base.AssociatedObject.StylusSystemGesture += AssociatedObject_StylusSystemGesture;
        }

        void DetachHandlers()
        {
            base.AssociatedObject.MouseLeftButtonUp -= AssociatedObject_MouseDown;
            base.AssociatedObject.StylusSystemGesture -= AssociatedObject_StylusSystemGesture;
        }

        void AssociatedObject_StylusSystemGesture(object sender, StylusSystemGestureEventArgs e)
        {
            if (e.SystemGesture == SystemGesture.Tap)
            {
                e.Handled = true;
                base.InvokeActions(null);
            }
        }

        void AssociatedObject_MouseDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            base.InvokeActions(null);
        }
        #endregion
    }
}
