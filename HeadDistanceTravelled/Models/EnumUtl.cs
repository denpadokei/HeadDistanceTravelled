using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeadDistanceTravelled.Models
{
    /// <summary>
    /// 拡張メソッド用クラス
    /// </summary>
    public static class EnumExtention
    {
        /// <summary>
        /// <see cref="Enum"/>の<see cref="DescriptionAttribute"/>属性に指定された文字列を取得する拡張メソッドです。
        /// </summary>
        /// <param name="value">文字列を取得したい<see cref="Enum"/></param>
        /// <returns></returns>
        public static string GetDescription(this Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            return Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute attribute
                ? attribute.Description
                : value.ToString();
        }
    }

    internal class EnumUtl
    {
        public static T GetEnumValue<T>(string txt) where T : Enum 
        {
            return Enum.GetValues(typeof(T)).OfType<T>().FirstOrDefault(x => string.Equals(x.GetDescription(), txt));
        }
    }
}
