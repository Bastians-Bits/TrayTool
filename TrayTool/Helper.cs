using System.Collections.Generic;

namespace TrayTool
{
    public class Helper
    {
        public static List<T> ConvertList<T, J>(List<J> objs) where J : T
        {
            List<T> retList = new List<T>();

            foreach (J obj in objs)
            {
                retList.Add(obj);
            }

            return retList;
        }

    }
}
