using System;
using System.ComponentModel;
using System.Linq;

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
        public static bool TryGetEnumValue<T>(string txt, out T val) where T : Enum
        {
            val = default;
            if (Enum.GetValues(typeof(T)).OfType<T>().Any(x => string.Equals(x.ToString(), txt))) {
                val = Enum.GetValues(typeof(T)).OfType<T>().FirstOrDefault(x => string.Equals(x.ToString(), txt));
                return true;
            }
            else {
                return false;
            }
        }

        public static bool TryGetEnumValueUserDescription<T>(string txt, out T val) where T : Enum
        {
            val = default;
            if (Enum.GetValues(typeof(T)).OfType<T>().Any(x => string.Equals(x.GetDescription(), txt))) {
                val = Enum.GetValues(typeof(T)).OfType<T>().FirstOrDefault(x => string.Equals(x.GetDescription(), txt));
                return true;
            }
            else {
                return false;
            }
        }
    }
}
