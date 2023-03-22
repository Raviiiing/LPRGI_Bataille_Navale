using System;
using System.Collections.Generic;

using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Jeu
{
    
    public partial class Parametre
    {
        [JsonProperty("nbLignes")]
        public long NbLignes { get; set; }

        [JsonProperty("nbColonnes")]
        public long NbColonnes { get; set; }

        [JsonProperty("bateaux")]
        public Bateau[] Bateaux { get; set; }
    }

    public partial class Bateau
    {
        [JsonProperty("taille")]
        public long Taille { get; set; }

        [JsonProperty("nom")]
        public string Nom { get; set; }
    }
}
