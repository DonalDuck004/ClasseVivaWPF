using System;

namespace ClasseVivaWPF.Utils.Themes
{
    public class ThemeCreator
    {
        public Type? Type { get; init; }
        public string? Name { get; init; }
        public ITheme? INSTANCE { get; private set; }

        private ThemeCreator()
        {

        }

        public static ThemeCreator New<T>(T instance) where T : ITheme
        {
            return new()
            {
                Type = null,
                Name = instance.Name,
                INSTANCE = instance
            };
        }

        public static ThemeCreator New<T>() where T : ITheme
        {
            return new()
            {
                Type = typeof(T),
                Name = typeof(T).Name
            };
        }

        public static ThemeCreator New<T>(string Name) where T : IDynamicTheme, new()
        {
            return new()
            {
                Type = typeof(T),
                Name = Name
            };
        }

        public ITheme Create()
        {
            if (this.INSTANCE is null)
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
