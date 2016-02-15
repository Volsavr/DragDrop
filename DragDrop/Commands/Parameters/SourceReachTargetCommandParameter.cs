using System.Windows;

namespace DragDrop.Commands.Parameters
{
    public class SourceReachTargetCommandParameter : SourceActionTargetCommandParameter
    {
        #region Constructors
        public SourceReachTargetCommandParameter(UIElement source, UIElement target)
            : base(source, target, null, null)
        {
        }

        public SourceReachTargetCommandParameter(UIElement dropSource, UIElement dropTarget, object sourceParameter,
            object targetParameter)
            : base(dropSource, dropTarget, sourceParameter, targetParameter)
        {
        }
        #endregion
    }
}
