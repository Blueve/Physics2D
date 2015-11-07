using Physics2D.Common;
using Physics2D.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Physics2D.Object.Tools
{
    public interface IPin
    {
        /// <summary>
        /// 把自己固定到物理世界中
        /// 返回锚点
        /// </summary>
        /// <param name="world"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        Handle Pin(World world, Vector2D position);

        /// <summary>
        /// 把自己从物理世界中解除固定
        /// </summary>
        /// <param name="world"></param>
        /// <returns></returns>
        void UnPin(World world);
    }
}
