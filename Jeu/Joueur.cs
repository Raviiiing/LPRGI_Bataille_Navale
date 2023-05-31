using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jeu
{
    internal class Joueur
    {
        public String nom { get; set; }
        public int score { get; set; }
        public Bateau[] bateaux { get; set; }

        public Joueur(String nom)
        {
            this.nom = nom;
            this.score = 0;
        }
        
        public void printGrid(Parametres parametre)
        {
            char[,] grid = new char[parametre.NbLignes, parametre.NbColonnes];
            //fill grid 
            for (int i = 0; i < parametre.NbLignes; i++)
            {
                for (int j = 0; j < parametre.NbColonnes; j++)
                {
                    grid[i, j] = '~';
                }
            }
            foreach (Bateau bateau in bateaux)
            {
                for (int i = 0; i < bateau.Taille; i++)
                {
                    if (bateau.Orientation == 1)
                    {
                        grid[bateau.Y + i, bateau.X] = nom.First();
                    }
                    else
                    {
                        grid[bateau.Y, bateau.X + i] = nom.First();
                    }
                }
            }
            for (int i = 0; i < parametre.NbLignes; i++)
            {
                for (int j = 0; j < parametre.NbColonnes; j++)
                {
                    Console.Write(grid[i, j]);
                }
                Console.WriteLine();
            }
        }
    }
}
