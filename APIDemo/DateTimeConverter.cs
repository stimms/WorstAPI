﻿using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace APIDemo
{
    internal class DateTimeConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Write(
           Utf8JsonWriter writer,
           DateTime dateTimeValue,
           JsonSerializerOptions options) =>
               writer.WriteStringValue(dateTimeValue.ToString(
                   "MM/dd/yyyy H:mm zzz", CultureInfo.InvariantCulture));
    }
}
