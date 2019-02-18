using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
