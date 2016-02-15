using System.Windows;

namespace DragDrop.Commands.Parameters
{
   public class SourceLeaveTargetCommandParameter : SourceActionTargetCommandParameter
   {
       public SourceLeaveTargetCommandParameter(UIElement dropSource, UIElement dropTarget, object sourceParameter,
           object targetParameter)
           : base(dropSource, dropTarget, sourceParameter, targetParameter) { }
   }
}
