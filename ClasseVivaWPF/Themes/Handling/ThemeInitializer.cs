using ClasseVivaWPF.Themes.Abs;
using System;

namespace ClasseVivaWPF.Themes.Handling
{
    public class ThemeInitializer
    {
        public bool FromFile { get; init; }
        public Type? Type { get; init; }
        public string? Name { get; init; }
        public ITheme? INSTANCE { get; private set; }

        private ThemeInitializer()
        {

        }

        public static ThemeInitializer New<T>(T instance) where T : ITheme
        {
            return new()
            {
                Type = null,
                Name = instance.Name,
                INSTANCE = instance,
                FromFile = false
            };
        }

        public static ThemeInitializer New<T>() where T : ITheme
        {
            return new()
            {
                Type = typeof(T),
                Name = typeof(T).Name,
                FromFile = false
            };
        }

        public static ThemeInitializer New<T>(string Name) where T : IDynamicTheme, new()
        {
            return new()
            {
                Type = typeof(T),
                Name = Name,
                FromFile = false
            };
        }

        public static ThemeInitializer NewFromFile(string Name)
        {
            return new()
            {
                Name = Name,
                FromFile = true
            };
        }

        public ITheme Create()
        {
            if (this.FromFile)
            {
                this.INSTANCE = ThemeOperations.GetFromFile(this.Name!);
            }
            else if (this.INSTANCE is null)
            {
                this.INSTANCE = (ITheme)Activator.CreateInstance(Type!)!;

                if (this.INSTANCE is IDynamicTheme x)
                    x.Name = this.Name!;
            }

            return this.INSTANCE;
        }

        public bool UnsetInstance()
        {
            if (this.Type is null)
                return false;

            this.INSTANCE = null;
            return true;
        }
    }
}
