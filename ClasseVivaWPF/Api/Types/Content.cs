using System;

namespace ClasseVivaWPF.Api.Types
{
    public record class Content(int IdContenuto,
                                string App, 
                                int Ordine,
                                object[] Tags,
                                DateTime Inizio, 
                                DateTime Fine,
                                DateTime? Scadenza,
                                string Tipo, 
                                string Titolo,
                                string? Link,
                                string MediaType,
                                string PanoramicaImg,
                                string? PanoramicaPos, 
                                string Gallery,
                                ContentDetail[] ContentDetail,
                                RelatedContentDetail[] Related,
                                bool OpensExternally,
                                string AccessibilityLabel) : ApiObject
    {
        public const string TYPE_HOME_TOP = "Home top";
        public const string TYPE_MINIGAMES = "Minigames";
        public const string TYPE_PILLOLE = "Pillole";
        public const string TYPE_POPFESSORI = "Popfessori";

        public const string PANORAMIC_TOP = "top";
        public const string PANORAMIC_BANNER = "banner";

    }
}
