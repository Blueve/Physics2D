using Physics2D.Common;

namespace Physics2D
{
    /// <summary>
    /// 单位换算类
    /// 用于转换物理世界以及图形渲染世界的数值
    /// </summary>
    public static class ConvertUnits
    {
        #region 私有字段
        /// <summary>
        /// 显示尺寸至模拟尺寸转换比例
        /// </summary>
        private static double _displayUnitsToSimUnitsRatio = 50;

        /// <summary>
        /// 模拟尺寸至显示尺寸转换比例
        /// </summary>
        private static double _simUnitsToDisplayUnitsRatio = 1 / _displayUnitsToSimUnitsRatio;

        #endregion

        #region 公开的方法
        /// <summary>
        /// 设置转换比例
        /// </summary>
        /// <param name="displayUnitsPerSimUnit"></param>
        public static void SetDisplayUnitToSimUnitRatio(double displayUnitsPerSimUnit)
        {
            _displayUnitsToSimUnitsRatio = displayUnitsPerSimUnit;
            _simUnitsToDisplayUnitsRatio = 1 / displayUnitsPerSimUnit;
        }

        #endregion

        #region 转换到显示尺寸
        /// <summary>
        /// 转换到显示尺寸
        /// </summary>
        /// <param name="simUnits"></param>
        /// <returns></returns>
        public static int ToDisplayUnits(this double simUnits)
        {
            return (int)(simUnits * _displayUnitsToSimUnitsRatio);
        }

        /// <summary>
        /// 转换到显示尺寸
        /// </summary>
        /// <param name="simUnits"></param>
        /// <returns></returns>
        public static int ToDisplayUnits(this int simUnits)
        {
            return (int)(simUnits * _displayUnitsToSimUnitsRatio);
        }

        /// <summary>
        /// 转换到显示尺寸
        /// </summary>
        /// <param name="simUnits"></param>
        /// <returns></returns>
        public static Vector2D ToDisplayUnits(this Vector2D simUnits)
        {
            return simUnits * _displayUnitsToSimUnitsRatio;
        }

        #endregion

        #region 转换到物理世界尺寸
        /// <summary>
        /// 转换到模拟尺寸
        /// </summary>
        /// <param name="displayUnits"></param>
        /// <returns></returns>
        public static double ToSimUnits(this double displayUnits)
        {
            return displayUnits / _displayUnitsToSimUnitsRatio;
        }

        /// <summary>
        /// 转换到模拟尺寸
        /// </summary>
        /// <param name="displayUnits"></param>
        /// <returns></returns>
        public static double ToSimUnits(this int displayUnits)
        {
            return displayUnits / _displayUnitsToSimUnitsRatio;
        }

        /// <summary>
        /// 转换到模拟尺寸
        /// </summary>
        /// <param name="displayUnits"></param>
        /// <returns></returns>
        public static Vector2D ToSimUnits(this Vector2D displayUnits)
        {
            return displayUnits / _displayUnitsToSimUnitsRatio;
        }
        #endregion
    }
}