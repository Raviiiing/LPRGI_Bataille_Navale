using System;
using System.Collections.Generic;

using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Jeu
{
    
    public partial class Parametres
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

        public int X { get; set; } = 0;
        public int Y { get; set; } = 0;
        
        public int Orientation { get; set; } = 0;


        public bool VerifyRange(int bateauX, int bateauY, long gridX, long gridY)
        {
            int PosX = bateauX;
            int PosY = bateauY;
            if (PosX > gridX || PosX < 0 || PosY > gridY || PosY < 0)
            {
                return false;
            }
            if (Orientation == 0)
            {
                int maxPosX = (int)(PosX + Taille);
                if (maxPosX > gridX)
                {
                    return false;
                }
            }
            else
            {
                int maxPosY = (int)(PosY + Taille);
                if (maxPosY > gridY)
                {
                    return false;
                }
            }
            return true;
        }

    }

    public class Mer
    {
        private int X { get; set; }
        private int Y { get; set; }
    }
}
