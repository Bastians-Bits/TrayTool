namespace TrayTool.Model
{
    public class Movement
    {
        public enum Direction
        {
            UP, DOWN, LEFT, RIGHT
        }

        public Direction Moevement { get; set; }
        public BaseModel Item { get; set; }
        public int Index { get; set; }
    }
}
