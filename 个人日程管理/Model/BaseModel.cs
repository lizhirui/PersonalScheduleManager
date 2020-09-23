using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace 个人日程管理.Model
{
    class BaseModel : ICloneable
    {
        public object Clone()
        {
            var childType = GetType();
            var newObj = Activator.CreateInstance(childType,null);
            var fiList = childType.GetFields(BindingFlags.Public | BindingFlags.NonPublic);

            foreach(var fi in fiList)
            {
                fi.SetValue(newObj,fi.GetValue(this));
            }

            return newObj;
        }
    }
}
