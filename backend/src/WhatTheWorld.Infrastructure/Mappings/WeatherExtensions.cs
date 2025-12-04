using System;
using System.Collections.Generic;
using System.Text;
using WhatTheWorld.Domain;

namespace WhatTheWorld.Infrastructure.Mappings
{
    public static class WeatherExtensions
    {
        public static WeatherDto? ToDto(this WeatherEntity? weather)
        {
            if (weather == null)
                return null;

            return new WeatherDto(
                weather.Id,
                weather.Time,
                weather.TempCelsius,
                weather.TempFahrenheit,
                weather.Description,
                weather.IconUrl
            );
        }
    }
}
