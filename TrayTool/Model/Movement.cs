namespace TrayTool.Model
{
    /// <summary>
    /// Model to store a item-movement on the display
    /// </summary>
    public class Movement
    {
        public enum Direction
        {
            NONE, UP, DOWN, LEFT, RIGHT
        }

        /// <summary>
        /// The direction of the movement
        /// </summary>
        public Direction Moevement { get; set; }
        /// <summary>
        /// The item which is moved
        /// </summary>
        public BaseModel Item { get; set; }
        /// <summary>
        /// The index of the parent item, in case the object is moved to a lower level
        /// </summary>
        public int Index { get; set; }
    }
}
