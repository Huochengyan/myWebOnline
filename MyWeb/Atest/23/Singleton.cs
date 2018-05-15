using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atest._23
{
    /// <summary>
    /// 普通单例模式  单线程没问题
    /// </summary>
    public class Singleton
    {
        /// <summary>
        /// 定义一个静态变量来保存类的实例
        /// </summary>
        private static Singleton uniqueInstance;

        /// <summary>
        /// 定义私有 构造函数， 使得外界不能创建改类的实例
        /// </summary>
        private Singleton()
        {

        }
        /// <summary>
        /// 定义共有方法 提供一个全局访问点，同时你也可以定义 共有属性来提供全局访问点
        /// </summary>
        /// <returns></returns>
        public static Singleton GetInstance()
        {
            //如果类的实例不存在则创建 否则直接返回
            if (uniqueInstance == null)
            {
                uniqueInstance = new Singleton();
             }
            return uniqueInstance;
        }
    }
}
/// <summary>
/// 上面这种解决方案确实可以解决多线程的问题,但是上面代码对于
/// 每个线程都会对线程辅助对象locker加锁之后再判断实例是否存在，
/// 对于这个操作完全没有必要的，因为当第一个线程创建了该类的实例之后，
/// 后面的线程此时只需要直接判断（uniqueInstance==null）为假，
/// 此时完全没必要对线程辅助对象加锁之后再去判断，
/// 所以上面的实现方式增加了额外的开销，损失了性能，
/// 为了改进上面实现方式的缺陷，
/// 我们只需要在lock语句前面加一句（uniqueInstance==null）的判断
/// 就可以避免锁所增加的额外开销，这种实现方式我们就叫它 “双重锁定”，
/// 下面具体看看实现代码的：
/// </summary>
namespace Atest._231
{
    /// <summary>
    /// 普通单例模式  多线程  线程没问题
    /// </summary>
    public class Singleton
    {
        /// <summary>
        /// 定义一个静态变量来保存类的实例
        /// </summary>
        private static Singleton uniqueInstance;

        /// <summary>
        /// 定义一个标识 确保线程同步
        /// </summary>
        private static readonly object locker = new object();

        /// <summary>
        /// 定义私有 构造函数， 使得外界不能创建改类的实例
        /// </summary>
        private Singleton()
        {

        }
        /// <summary>
        /// 定义共有方法 提供一个全局访问点，同时你也可以定义 共有属性来提供全局访问点
        /// </summary>
        /// <returns></returns>
        public static Singleton GetInstance()
        {
            // 当第一个线程运行到这里时，此时会对locker对象 "加锁"，
            // 当第二个线程运行该方法时，首先检测到locker对象为"加锁"状态，该线程就会挂起等待第一个线程解锁
            // lock语句运行完之后（即线程运行完之后）会对该对象"解锁"

            // 双重锁定只需要一句判断就可以了
            if (uniqueInstance == null)
            {
                lock (locker)
                {
                    //如果类的实例不存在则创建 否则直接返回
                    if (uniqueInstance == null)
                    {
                        uniqueInstance = new Singleton();
                    }
                }
            }
            return uniqueInstance;
        }



    }
}
