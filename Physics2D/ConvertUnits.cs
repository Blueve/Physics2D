using Physics2D.Common;

namespace Physics2D
{
    /// <summary>
    /// 单位换算类
    /// 用于转换物理世界以及图形渲染世界的数值
    /// </summary>
    public static class ConvertUnits
    {
        #region 私有属性

        private static double _displayUnitsToSimUnitsRatio = 50;
        private static double _simUnitsToDisplayUnitsRatio = 1 / _displayUnitsToSimUnitsRatio;

        #endregion 私有属性

        #region 公开的方法

        public static void SetDisplayUnitToSimUnitRatio(double displayUnitsPerSimUnit)
        {
            _displayUnitsToSimUnitsRatio = displayUnitsPerSimUnit;
            _simUnitsToDisplayUnitsRatio = 1 / displayUnitsPerSimUnit;
        }

        #endregion 公开的方法

        #region 转换到显示尺寸

        public static int ToDisplayUnits(this double simUnits)
        {
            return (int)(simUnits * _displayUnitsToSimUnitsRatio);
        }

        public static int ToDisplayUnits(this int simUnits)
        {
            return (int)(simUnits * _displayUnitsToSimUnitsRatio);
        }

        public static Vector2D ToDisplayUnits(this Vector2D simUnits)
        {
            return simUnits * _displayUnitsToSimUnitsRatio;
        }

        #endregion 转换到显示尺寸

        #region 转换到物理世界尺寸

        public static double ToSimUnits(this double displayUnits)
        {
            return displayUnits / _displayUnitsToSimUnitsRatio;
        }

        public static double ToSimUnits(this int displayUnits)
        {
            return displayUnits / _displayUnitsToSimUnitsRatio;
        }

        public static Vector2D ToSimUnits(this Vector2D displayUnits)
        {
            return displayUnits / _displayUnitsToSimUnitsRatio;
        }

        #endregion 转换到物理世界尺寸
    }
}