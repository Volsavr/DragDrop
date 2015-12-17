using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace SampleApp
{
    public class MouseActionsTrigger : TriggerBase<UIElement>
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
        public MouseActionType ActionType
        {
            get { return (MouseActionType)GetValue(ActionTypeProperty); }
            set { SetValue(ActionTypeProperty, value); }
        }
        public static readonly DependencyProperty ActionTypeProperty =
            DependencyProperty.Register("ActionType", typeof(MouseActionType)
            , typeof(MouseActionsTrigger)
            , new UIPropertyMetadata(null));

        #endregion

        #region Private methods
        void AttachHandlers()
        {
            base.AssociatedObject.MouseDown += AssociatedObject_MouseDown;
        }

        void DetachHandlers()
        {
            base.AssociatedObject.MouseDown -= AssociatedObject_MouseDown;
        }

        void AssociatedObject_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount > 1 && ActionType == MouseActionType.MouseLeftButtonDoubleClick)
            {
              //  e.Handled = true;
                base.InvokeActions(null);
            }
            else if (ActionType == MouseActionType.MouseLeftButtonClick)
            {
                base.InvokeActions(null);
               // e.Handled = true;
            }
        }
        #endregion
    }
}

