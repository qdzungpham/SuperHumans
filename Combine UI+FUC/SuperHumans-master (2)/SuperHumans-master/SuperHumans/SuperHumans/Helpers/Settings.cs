using Plugin.Settings;
using Plugin.Settings.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace SuperHumans.Helpers
{
    public static class Settings
    {
        private static ISettings AppSettings => CrossSettings.Current;

        public static string UIMode
        {
            get => AppSettings.GetValueOrDefault(nameof(UIMode), "basic");

            set => AppSettings.AddOrUpdateValue(nameof(UIMode), value);
        }
    }
}
