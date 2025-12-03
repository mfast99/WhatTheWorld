using System;
using System.Collections.Generic;
using System.Text;

namespace WhatTheWorld.Domain
{
    public record Country(
        string Code,      // "DE", "US", "FR"
        string Name,      // "Germany", "United States"
        string Capital,   // "Berlin", "Washington D.C."
        string FlagEmoji, // "🇩🇪", "🇺🇸"
        double Lat,       // 52.52 (Berlin)
        double Lon        // 13.405 (Berlin)
    );

    public record NewsArticle(string Title, string Url, string Source);
    public record WeatherData(double TempCelsius, string Description, string IconUrl);
}
