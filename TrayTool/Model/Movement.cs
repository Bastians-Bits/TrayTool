namespace TrayTool.Model
{
    public class Movement
    {
        public enum Direction
        {
            NONE, UP, DOWN, LEFT, RIGHT
        }

        public Direction Moevement { get; set; }
        public BaseModel Item { get; set; }
        public int Index { get; set; }
    }
}
