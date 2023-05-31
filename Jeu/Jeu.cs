using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jeu
{
    internal class Jeu
    {
        private Joueur joueur1 { get; set; }
        private Joueur joueur2 { get; set; }

        public Jeu()
        {
            this.joueur1 = new Joueur("Joueur 1");
            this.joueur2 = new Joueur("Joueur 2");
        }
    }
}
