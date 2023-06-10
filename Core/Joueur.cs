    namespace Core;

    public class Joueur
    {
        public Joueur(string nom, Cellule[,] grille)
        {
            Nom = nom;
            Grille = grille;
            Bateaux = new List<Bateau>();
        }

        public string Nom { get; set; }
        public Cellule[,] Grille { get; set; }
        public List<Bateau> Bateaux { get; set; }
        public bool aGagne { get; set; } = false;
    }