using System.Windows;

namespace DragDrop
{
    /// <summary>
    /// Represents a drop command parameter
    /// </summary>
    public class DropCommandParameter
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
        {
            DropSource = dropSource;
            DropTarget = dropTarget;
            DropSourceParameter = dropSourceParameter;
            DropTargetParameter = dropTargetParameter;
            Offset = offset;
        }
        
        #endregion

        #region Properties
        /// <summary>
        /// Gets the drop source
        /// </summary>
        public UIElement DropSource
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the drop target
        /// </summary>
        public UIElement DropTarget
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the drop source parameter
        /// </summary>
        public object DropSourceParameter
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the drop target parameter
        /// </summary>
        public object DropTargetParameter
        {
            get;
            private set;
        }

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
