namespace EarlySite.Core.Utils
{
    using System;
    
    /// <summary>
    /// 位置计算工具
    /// </summary>
    public class PositionUtils
    {
        /// <summary>
        /// 根据距离与角度计算原点的远点坐标
        /// </summary>
        /// <param name="origin">原点坐标</param>
        /// <param name="distance">距离(按米计算)</param>
        /// <param name="angle">角度(0°-360°)</param>
        /// <returns></returns>
        public static Position CaculateFarawayPosition(Position origin,double distance,double angle)
        {
            Position farposition = new Position();
            //距离
            distance = distance / 1000;
            //获取左下角坐标
            double longitude = origin.Longitude + ((distance * Math.Sin(angle * Math.PI / 180)) / (111 * Math.Cos(origin.Latitude * Math.PI / 180)));
            double latitude = origin.Latitude + ((distance * Math.Cos(angle * Math.PI / 180)) / 111);

            farposition.Longitude = longitude;
            farposition.Latitude = latitude;
            return farposition;

        }

    }

    public class Position
    {

        /// <summary>
        /// 经度
        /// </summary>
        public double Longitude { get; set; }

        /// <summary>
        /// 纬度
        /// </summary>
        public double Latitude { get; set; }

        
    }

}
