using System.Windows;

namespace DragDrop.Commands.Parameters
{
   public class SourceActionTargetCommandParameter
    {
        #region Constructors
        public SourceActionTargetCommandParameter(UIElement dropSource, UIElement dropTarget, object sourceParameter,
            object targetParameter)
        {
            Source = dropSource;
            Target = dropTarget;
            SourceParameter = sourceParameter;
            TargetParameter = targetParameter;
        }
        #endregion

        #region Properties
        public UIElement Source { get; protected set; }

        public UIElement Target { get; protected set; }

        public object SourceParameter { get; protected set; }

        public object TargetParameter { get; protected set; }
        #endregion
    }
}
