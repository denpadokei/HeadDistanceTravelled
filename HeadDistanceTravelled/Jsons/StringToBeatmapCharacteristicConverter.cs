using HeadDistanceTravelled.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeadDistanceTravelled.Jsons
{
    public class StringToBeatmapCharacteristicConverter : StringEnumConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null) {
                writer.WriteNull();
                return;
            }

            Enum @enum = (Enum)value;
            string text = @enum.ToString("G");
            if (char.IsNumber(text[0]) || text[0] == '-') {
                writer.WriteValue(value);
                return;
            }
            string value2 = @enum.GetDescription();
            if (value2 == null) {
                writer.WriteNull();
            }
            else {
                writer.WriteValue(value2);
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            
            try {
                if (EnumUtl.TryGetEnumValue<BeatmapCharacteristic>(base.ReadJson(reader, objectType, existingValue, serializer)?.ToString(), out var val)) {
                    return val;
                }
                else {
                    return null;
                }
            }
            catch (JsonSerializationException) {
                return null;
            }
        }
    }
}
