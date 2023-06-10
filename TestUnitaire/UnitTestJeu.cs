using API;
using Core;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using Moq;
using System.Globalization;
using System.Text.RegularExpressions;
using WireMock.Server;



namespace TestUnitaire
{
    public class UnitTestJeu
    {

        // Vérifie si la fonction PeutPlacer  retourne le résultat attendu en fonction
        // des paramètres fournis.
        [Theory]
        [InlineData(5, 5, Direction.Vertical, true, true)]
        [InlineData(0, 0, Direction.Vertical, false, true)]
        [InlineData(5, 5, Direction.Horizontal, false, true)]
        [InlineData(3, 3, Direction.Vertical, false, false)]
        [InlineData(0, 0, Direction.Horizontal, false, false)]
        [InlineData(3, 3, Direction.Vertical, false, true)]
        public void TestPeutPlacer(int x, int y, Direction direction, bool apercu, bool expectedResult)
        {
            var grille = new Cellule[10, 10];
            var bateau = new Bateau { Taille = 3 };
            var jeu = new Jeu();
            var result = jeu.PeutPlacer(grille, bateau, x, y, direction, apercu);
            
            for (int i = 0; i < grille.GetLength(0); i++)
            {
                for (int j = 0; j < grille.GetLength(1); j++)
                {
                    grille[i, j] = new Cellule();
                }
            }

            Assert.Equal(expectedResult, result);
        }

        // Vérifie si la fonction PlacerBateau place correctement un bateau
        // de taille spécifiée à la position donnée sur la grille avec une direction verticale.
        [Fact]
        public void TestPlacerBateauVertical()
        {
            var grille = new Cellule[10, 10];
            var bateau = new Bateau { Taille = 3 };
            var x = 5;
            var y = 5;
            var direction = Direction.Vertical;
            var jeu = new Jeu();
            
            for (int i = 0; i < grille.GetLength(0); i++)
            {
                for (int j = 0; j < grille.GetLength(1); j++)
                {
                    grille[i, j] = new Cellule();
                }
            }

            jeu.PlacerBateau(grille, bateau, x, y, direction);

            for (var i = 0; i < bateau.Taille; i++)
            {
                var cellule = direction == Direction.Vertical ? grille[x + i, y] : grille[x, y + i];
                Assert.Equal(bateau, cellule.Bateau);
            }
        }

        // Vérifie si la fonction PlacerBateau place correctement un bateau de
        // taille spécifiée à la position donnée sur la grille avec une direction horizontale.
        [Fact]
        public void TestPlacerBateauHorizontal()
        {
            var grille = new Cellule[10, 10];
            var bateau = new Bateau { Taille = 4 };
            var x = 2;
            var y = 3;
            var direction = Direction.Horizontal;
            var jeu = new Jeu();
         
            for (int i = 0; i < grille.GetLength(0); i++)
            {
                for (int j = 0; j < grille.GetLength(1); j++)
                {
                    grille[i, j] = new Cellule();
                }
            }

            jeu.PlacerBateau(grille, bateau, x, y, direction);

            for (var i = 0; i < bateau.Taille; i++)
            {
                var cellule = grille[x, y + i];
                Assert.Equal(bateau, cellule.Bateau);
            }
        }


        // Vérifie si la fonction GetEmptyGrille
        // de la classe Jeu renvoie une grille vide avec les dimensions spécifiées.
        [Fact]
        public void TestGetEmptyGrille()
        {
            var jeu = new Jeu();
            var grille = jeu.GetEmptyGrille();
            jeu.nbLignes = 5;
            jeu.nbColonnes = 5;


            Assert.Equal(5, grille.GetLength(0));
            Assert.Equal(5, grille.GetLength(1));

            for (int i = 0; i < grille.GetLength(0); i++)
            {
                for (int j = 0; j < grille.GetLength(1); j++)
                {
                    var cellule = grille[i, j];
                    Assert.NotNull(cellule);
                    Assert.Null(cellule.Bateau);
                }
            }
        }


        // Teste l'affichage de l'aperçu du bateau en position verticale lorsque
        // la case contient un bateau
        [Fact]
        public void TestAfficherApercuBateauVerticalBateauPresent()
        {
            var jeu = new Jeu();
            var grille = new Cellule[5, 5];
            var bateau = new Bateau { Taille = 3 };
            var consoleOutput = new StringWriter();
            var output = consoleOutput.ToString();

            for (int i = 0; i < grille.GetLength(0); i++)
            {
                for (int j = 0; j < grille.GetLength(1); j++)
                {
                    grille[i, j] = new Cellule();
                }
            }

            grille[1, 2].Bateau = new Bateau();

            Console.SetOut(consoleOutput);

            jeu.AfficherApercuBateau(grille, bateau, 1, 2, Direction.Vertical);

            Assert.Equal("~~~~~\r\n~~X~~\r\n~~X~~\r\n~~X~~\r\n~~~~~\r\n", output);
        }


        // Teste l'affichage de l'aperçu du bateau en position verticale lorsque la case est vide
        [Fact]
        public void TestAfficherApercuBateauVerticalBateauAbsent()
        {
            var jeu = new Jeu();
            var grille = new Cellule[5, 5];
            var bateau = new Bateau { Taille = 3 };
            var consoleOutput = new StringWriter();
            var output = consoleOutput.ToString();

            for (int i = 0; i < grille.GetLength(0); i++)
            {
                for (int j = 0; j < grille.GetLength(1); j++)
                {
                    grille[i, j] = new Cellule();
                }
            }


            Console.SetOut(consoleOutput);

            jeu.AfficherApercuBateau(grille, bateau, 1, 2, Direction.Vertical);

            Assert.Equal("~~~~~\r\n~~X~~\r\n~~X~~\r\n~~X~~\r\n~~~~~\r\n", output);
        }


        // Teste l'affichage de l'aperçu du bateau en position horizontale lorsque
        // la case contient un bateau
        [Fact]
        public void TestAfficherApercuBateauHorizontalBateauPresent()
        {
            var jeu = new Jeu();
            var grille = new Cellule[5, 5];
            var bateau = new Bateau { Taille = 4 };
            var consoleOutput = new StringWriter();
            var output = consoleOutput.ToString();
            

            for (int i = 0; i < grille.GetLength(0); i++)
            {
                for (int j = 0; j < grille.GetLength(1); j++)
                {
                    grille[i, j] = new Cellule();
                }
            }

            grille[2, 1].Bateau = new Bateau();

            Console.SetOut(consoleOutput);

            jeu.AfficherApercuBateau(grille, bateau, 2, 1, Direction.Horizontal);
            Assert.Equal("~~~~~\r\n~~~~~\r\n~XXXX\r\n~~~~~\r\n~~~~~\r\n", output);
        }


        // Teste l'affichage de l'aperçu du bateau en position horizontale lorsque la case est vide
        [Fact]
        public void TestAfficherApercuBateauHorizontalBateauAbsent()
        {
            var jeu = new Jeu();
            var grille = new Cellule[5, 5];
            var bateau = new Bateau { Taille = 4 };
            var consoleOutput = new StringWriter();
            var output = consoleOutput.ToString();

            for (int i = 0; i < grille.GetLength(0); i++)
            {
                for (int j = 0; j < grille.GetLength(1); j++)
                {
                    grille[i, j] = new Cellule();
                }
            }

            Console.SetOut(consoleOutput);
            jeu.AfficherApercuBateau(grille, bateau, 2, 1, Direction.Horizontal);  
            Assert.Equal("~~~~~\r\n~~~~~\r\n~XXXX\r\n~~~~~\r\n~~~~~\r\n", output);
        }


        // Teste le scénario où le joueur touche un bateau avec son missile
        [Fact]
        public void TestJoueurToucheCelluleTouche()
        {
            var grille = new Cellule[5, 5];
            var bateau = new Bateau { Taille = 3 };
            var joueur = new Joueur("John Doe", grille);
            Jeu jeu = new Jeu();
            object consoleOutput = null;

            for (int i = 0; i < grille.GetLength(0); i++)
            {
                for (int j = 0; j < grille.GetLength(1); j++)
                {
                    grille[i, j] = new Cellule(); 
                }
            }

            grille[2, 2].Bateau = bateau;


            jeu.JoueurToucheCellule(joueur);

            Assert.True(grille[2, 2].EstTouche);
            Assert.Contains("Vous avez touché un bateau !", consoleOutput.ToString());
        }


        // Teste le scénario où le joueur rate un bateau
        [Fact]
        public void TestJoueurToucheCelluleManque()
        {
            var grille = new Cellule[5, 5];
            var joueur = new Joueur("John Doe", grille);
            Jeu jeu = new Jeu();
            object consoleOutput = null;


            for (int i = 0; i < grille.GetLength(0); i++)
            {
                for (int j = 0; j < grille.GetLength(1); j++)
                {
                    grille[i, j] = new Cellule();
                }
            }

            jeu.JoueurToucheCellule(joueur);

            Assert.True(grille[0, 0].EstTouche);
            Assert.Contains("Tir raté, vous avez tué une famille de poisson rouge !", consoleOutput.ToString());
        }


        // Vérifie si la fonction AfficherGrilleToucheAvecMissile
        // affiche le missile lorsqu'il ne touche pas de cellule
        [Fact]
        public void TestAfficherGrilleToucheAvecMissileManque()
        {
            var grille = new Cellule[5, 5];
            Jeu jeu = new Jeu();

            var consoleOutput = CaptureConsoleOutput(() => jeu.AfficherGrilleToucheAvecMissile(grille, 2, 2));

            Assert.Contains("\u001b[32mM\u001b[0m", consoleOutput);
        }


        // Vérifie si la fonction AfficherGrilleToucheAvecMissile
        // affiche le missile lorsqu'il touche une cellule
        [Fact]
        public void TestAfficherGrilleToucheAvecMissileTouche()
        {
            var grille = new Cellule[5, 5];
            Jeu jeu = new Jeu();

            var consoleOutput = CaptureConsoleOutput(() => jeu.AfficherGrilleToucheAvecMissile(grille, 2, 2));

            Assert.Contains("\u001b[31mM\u001b[0m", consoleOutput);
        }


        // Vérifie si la fonction JoueurAGagne retourne true
        // lorsque toutes les cellules de la grille ont été touchées
        [Fact]
        public void TestJoueurAGagneTrue()
        {
            var grille = new Cellule[5, 5];
            var joueur = new Joueur("John Doe", grille);
            Jeu jeu = new Jeu();

            foreach (var cellule in grille)
            {
                cellule.Bateau = new Bateau();
                cellule.EstTouche = true;
            }

            bool result = jeu.JoueurAGagne(joueur);

            Assert.True(result);
        }


        // Vérifie si la fonction JoueurAGagne retourne false
        // lorsque toutes les cellules de la grille ne sont pas touchées
        [Fact]
        public void TestJoueurAGagneFalse()
        {
            var grille = new Cellule[5, 5];
            var joueur = new Joueur("John Doe", grille);
            Jeu jeu = new Jeu();

            grille[0, 0].Bateau = new Bateau();
            grille[0, 0].EstTouche = true;
            grille[1, 1].Bateau = new Bateau();
            grille[1, 1].EstTouche = true;

            bool result = jeu.JoueurAGagne(joueur);

            Assert.False(result);
        }



        // Vérifie si la fonction EstBateauCouleDansGrille retourne faux
        // lorsque toutes les cellules du bateau ne sont pas touchées
        [Fact]
        public void TestEstBateauCouleDansGrilleFalse()
        {
            var grille = new Cellule[5, 5];
            var bateauUUID = "123456";
            var jeu = new Jeu();

            for (int i = 0; i < grille.GetLength(0); i++)
            {
                for (int j = 0; j < grille.GetLength(1); j++)
                {
                    var cellule = new Cellule();
                    if (i == 2 && j == 2)
                        cellule.Bateau = new Bateau { UUID = bateauUUID };
                    grille[i, j] = cellule;
                }
            }

            bool result = jeu.EstBateauCouleDansGrille(grille, bateauUUID);

            Assert.False(result);
        }

        // Vérifie si la fonction EstBateauCouleDansGrille retourne vrai
        // lorsque toutes les cellules du bateau sont touchées
        [Fact]
        public void TestEstBateauCouleDansGrilleTrue()
        {
            var grille = new Cellule[5, 5];
            var bateauUUID = "123456";
            var jeu = new Jeu();

            for (int i = 0; i < grille.GetLength(0); i++)
            {
                for (int j = 0; j < grille.GetLength(1); j++)
                {
                    var cellule = new Cellule { EstTouche = true };
                    if (i == 2 && j == 2)
                        cellule.Bateau = new Bateau { UUID = bateauUUID };
                    grille[i, j] = cellule;
                }
            }

            bool result = jeu.EstBateauCouleDansGrille(grille, bateauUUID);

            Assert.True(result);
        }

        // Permet de capturer la sortie console pendant l'exécution d'une action
        private string CaptureConsoleOutput(Action action){
            var writer = new StringWriter();
            var originalOut = Console.Out;
            Console.SetOut(writer);

            try
            {
                action.Invoke();
                return writer.ToString();
            }
            finally
            {
                Console.SetOut(originalOut);
                writer.Dispose();
            }
        }
    }
}