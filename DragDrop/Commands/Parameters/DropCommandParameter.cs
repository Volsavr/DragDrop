using System.Windows;

namespace DragDrop.Commands.Parameters
{
    /// <summary>
    /// Represents a drop command parameter
    /// </summary>
    public class DropCommandParameter : SourceActionTargetCommandParameter
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the DropCommandParameter class by the
        /// given parameter
        /// </summary>
        /// <param name="dropSource">
        /// Drop source
        /// </param>
        /// <param name="dropTarget">
        /// Drop target
        /// </param>
        public DropCommandParameter(UIElement dropSource, UIElement dropTarget)
            : this(dropSource, dropTarget, null, null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the DropCommandParameter class by the
        /// given parameter
        /// </summary>
        /// <param name="dropSource">
        /// Drop source
        /// </param>
        /// <param name="dropTarget">
        /// Drop target
        /// </param>
        /// <param name="offset">
        /// Offset
        /// </param>
        public DropCommandParameter(UIElement dropSource, UIElement dropTarget, Point? offset)
            : this(dropSource, dropTarget, null, null, offset)
        {
        }

        /// <summary>
        /// Initializes a new instance of the DropCommandParameter class by the
        /// given parameter
        /// </summary>
        /// <param name="dropSource">
        /// Drop source
        /// </param>
        /// <param name="dropTarget">
        /// Drop target
        /// </param>
        /// <param name="dropSourceParameter">
        /// Drop source parameter
        /// </param>
        /// <param name="dropTargetParameter">
        /// Drop target parameter
        /// </param>
        public DropCommandParameter(UIElement dropSource, UIElement dropTarget, object dropSourceParameter, object dropTargetParameter)
            : this(dropSource, dropTarget, dropSourceParameter, dropTargetParameter, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the DropCommandParameter class by the
        /// given parameter
        /// </summary>
        /// <param name="dropSource">
        /// Drop source
        /// </param>
        /// <param name="dropTarget">
        /// Drop target
        /// </param>
        /// <param name="dropSourceParameter">
        /// Drop source parameter
        /// </param>
        /// <param name="dropTargetParameter">
        /// Drop target parameter
        /// </param>
        /// <param name="offset">
        /// Offset
        /// </param>
        public DropCommandParameter(UIElement dropSource, UIElement dropTarget, object dropSourceParameter, object dropTargetParameter, Point? offset)
              : base(dropSource, dropTarget, dropSourceParameter, dropTargetParameter)
        {
           Offset = offset;
        }
        
        #endregion

        #region Properties

        /// <summary>
        /// Gets the offset
        /// </summary>
        public Point? Offset
        {
            get;
            private set;
        }
        
        #endregion
    }

}
