using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atest
{
    //泛型约束
    public  class GenericClass
    {
        public static T Get<T>(T t)
            where T :class  //泛型加约束
        {
            return default(T);//会根据T的类型返会相同类型
        }

    }
}
