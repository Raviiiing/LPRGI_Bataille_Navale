using API;
using Newtonsoft.Json;
using System;

namespace Core;

public class Jeu
{
    public List<Bateau> bateaux;

    public int nbColonnes;
    public int nbLignes;


    private async Task GetConfig()
    {
        var api = new Api("https://api-lprgi.natono.biz/api/GetConfig", "lprgi_api_key_2023");
        try
        {
            var config = JsonConvert.DeserializeObject<Config>(await api.GetApiContent()) ?? throw new InvalidOperationException();
            if(config == null)
            {
                ConsoleLogger.Log("Aucune config trouvé, impossible de lancer le jeu.", LogLevel.Error);
                throw new ArgumentNullException();
            }
            else
            {
                nbColonnes = config.nbColonnes;
                nbLignes = config.nbLignes;
                bateaux = config.bateaux;

                
                bateaux.ForEach(b => {
                    //Génération d'un Unique ID pour chaques bateaux (nécaissaire plus tard pour identifier les bateaux dans la grille)
                    Guid uuid = Guid.NewGuid();
                    string uuidString = uuid.ToString();
                    b.UUID = uuidString;
                });
            }
        }
        catch (JsonException ex)
        {
            throw new Exception("Failed to deserialize JSON string to dynamic object.", ex);
        }
    }
    private bool PeutPlacer(Cellule[,] grille, Bateau bateau, int x, int y, Direction direction, bool apercu = false)
    {
        if (bateau == null) throw new ArgumentNullException(nameof(bateau));

        switch (direction)
        {
            case Direction.Vertical:
                if (x + bateau.Taille > grille.GetLength(0)) return false;
                if (x < 0 || y < 0) return false;
                if (x + 1 > grille.GetLength(0) || y + 1 > grille.GetLength(1)) return false;

                if (!apercu)
                    for (var t = 0; t < bateau.Taille; t++)
                        if (grille[x + t, y].Bateau != null)
                            return false;

                break;

            case Direction.Horizontal:
                if (y + bateau.Taille > grille.GetLength(1)) return false;
                if (x < 0 || y < 0) return false;
                if (x + 1 > grille.GetLength(0) || y + 1 > grille.GetLength(1)) return false;

                if (!apercu)
                    for (var t = 0; t < bateau.Taille; t++)
                        if (grille[x, y + t].Bateau != null)
                            return false;

                break;

            default:
                throw new ArgumentException("Direction inconnue", nameof(direction));
        }

        return true;
    }

    private void AfficherGrille(Cellule[,] grille)
    {
        for (var i = 0; i < grille.GetLength(0); i++)
        {
            for (var j = 0; j < grille.GetLength(1); j++)
                if (grille[i, j].Bateau == null)
                    Console.Write('~');
                else
                    Console.Write('B');
            Console.WriteLine();
        }
    }

    private void PlacerBateau(Cellule[,] grille, Bateau bateau, int x, int y, Direction direction)
    {
        switch (direction)
        {
            case Direction.Vertical:
                for (var i = 0; i < bateau.Taille; i++) grille[x + i, y].Bateau = bateau;
                break;

            case Direction.Horizontal:
                for (var i = 0; i < bateau.Taille; i++) grille[x, y + i].Bateau = bateau;
                break;

            default:
                throw new ArgumentException("Direction inconnue", nameof(direction));
        }
    }

    public Cellule[,] GetEmptyGrille()
    {
        var grille = new Cellule[nbLignes, nbColonnes];
        for (var i = 0; i < nbColonnes; i++)
        for (var j = 0; j < nbLignes; j++)
            grille[i, j] = new Cellule();

        return grille;
    }

    private void AfficherApercuBateau(Cellule[,] grille, Bateau bateau, int x, int y, Direction direction)
    {
        for (var i = 0; i < grille.GetLength(1); i++)
        {
            for (var j = 0; j < grille.GetLength(0); j++)
            {
                // Vérifier si le bateau serait à cette position
                var bateauApercu = false;
                switch (direction)
                {
                    case Direction.Vertical:
                        bateauApercu = i >= x && i < x + bateau.Taille && j == y;
                        break;

                    case Direction.Horizontal:
                        bateauApercu = j >= y && j < y + bateau.Taille && i == x;
                        break;
                }

                if (bateauApercu && grille[i, j].Bateau != null)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write('X');
                    Console.ResetColor();
                }
                else if (bateauApercu && grille[i, j].Bateau == null)
                {
                    Console.Write('X');
                }
                else if (grille[i, j].Bateau != null)
                {
                    Console.Write('B');
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write('~');
                    Console.ResetColor();
                }
            }

            Console.WriteLine();
        }
    }

    private void UtilisateurPlaceBateau(Bateau bateau, Cellule[,] grille)
    {
        var continuer = true;
        int posX = 0, posY = 0;
        var direction = Direction.Horizontal;
        do
        {
            Console.Clear();
            ConsoleLogger.Log("Placez votre bateau:", LogLevel.Info);
            ConsoleLogger.Log("Utilisez les flèches pour vous déplacer, R pour tourner le bateau et Espace pour valider", LogLevel.Info);
            AfficherApercuBateau(grille, bateau, posX, posY, direction);
            var key = Console.ReadKey(true);
            switch (key.Key)
            {
                case ConsoleKey.UpArrow:
                    if (PeutPlacer(grille, bateau, posX - 1, posY, direction, true))
                        posX--;
                    break;
                case ConsoleKey.DownArrow:
                    if (PeutPlacer(grille, bateau, posX + 1, posY, direction, true))
                        posX++;
                    break;
                case ConsoleKey.LeftArrow:
                    if (PeutPlacer(grille, bateau, posX, posY - 1, direction, true))
                        posY--;
                    break;
                case ConsoleKey.RightArrow:
                    if (PeutPlacer(grille, bateau, posX, posY + 1, direction, true))
                        posY++;
                    break;
                case ConsoleKey.R:
                    if (PeutPlacer(grille, bateau, posX, posY,
                            direction == Direction.Horizontal ? Direction.Vertical : Direction.Horizontal, true))
                        direction = direction == Direction.Horizontal ? Direction.Vertical : Direction.Horizontal;
                    break;
            }

            if (key.Key == ConsoleKey.Enter && PeutPlacer(grille, bateau, posX, posY, direction))
                continuer = false;
        } while (continuer);

        PlacerBateau(grille, bateau, posX, posY, direction);
    }

    private void AfficherGrilleTouche(Cellule[,] grille)
    {
        for (var i = 0; i < grille.GetLength(0); i++)
        {
            for (var j = 0; j < grille.GetLength(1); j++)
                if ((grille[i, j].Bateau == null && grille[i, j].EstTouche == false) ||
                    (grille[i, j].Bateau != null && grille[i, j].EstTouche == false))
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write('~');
                    Console.ResetColor();
                }
                else if (grille[i, j].Bateau == null && grille[i, j].EstTouche)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write('X');
                    Console.ResetColor();
                }
                else if (grille[i, j].Bateau != null && grille[i, j].EstTouche)
                {
                    
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write('X');
                    Console.ResetColor();
                }

            Console.WriteLine();
        }
    }

    private void JoueurToucheCellule(Joueur joueur)
    {
        var grille = joueur.Grille;
        var continuer = true;
        int posX = 0, posY = 0;
        do
        {
            ConsoleLogger.Log(joueur.Nom +  " Placez votre missile prédator air sol v12 Kim Jong Un:", LogLevel.Info);
            ConsoleLogger.Log("Utilisez les flèches pour déplacer le missile et Espace pour valider", LogLevel.Info);
            AfficherGrilleToucheAvecMissile(grille, posX, posY);
            var key = Console.ReadKey(true);
            
            switch (key.Key)
            {
                case ConsoleKey.UpArrow:
                    if (MissileInRange(grille, posX - 1, posY)) posX--;
                    break;
                case ConsoleKey.DownArrow:
                    if (MissileInRange(grille, posX + 1, posY)) posX++;
                    break;
                case ConsoleKey.LeftArrow:
                    if (MissileInRange(grille, posX, posY - 1)) posY--;
                    break;
                case ConsoleKey.RightArrow:
                    if (MissileInRange(grille, posX, posY + 1)) posY++;
                    break;
                case ConsoleKey.Enter:
                    if (grille[posX, posY].EstTouche == false)
                        continuer = false;
                    break;
            }
        } while (continuer);
        Console.Clear();
        grille[posX, posY].EstTouche = true;
        if (grille[posX, posY].EstTouche && grille[posX, posY].Bateau != null)
        {
            bool cooled = EstBateauCouleDansGrille(grille, grille[posX, posY].Bateau.UUID);
            if (cooled)
            {
                ConsoleLogger.Log("Vous avez coulé le " + grille[posX, posY].Bateau.Nom + " de " + grille[posX, posY].Bateau.Taille + " mètres", LogLevel.Info);
            }
            else
            {
                ConsoleLogger.Log("Vous avez touché un bateau !", LogLevel.Info);
            }
        }
        else
        {
            ConsoleLogger.Log("Tir raté, vous avez tué une famille de poisson rouge !", LogLevel.Info);
        }
    }

    private bool MissileInRange(Cellule[,] grille, int y, int x)
    {
        return y >= 0 && y < grille.GetLength(0) && x >= 0 && x < grille.GetLength(1);
    }

    private void AfficherGrilleToucheAvecMissile(Cellule[,] grille, int xMissile, int yMissile)
    {
        for (var i = 0; i < grille.GetLength(0); i++)
        {
            for (var j = 0; j < grille.GetLength(1); j++)

                if (i == xMissile && j == yMissile && grille[i, j].EstTouche == false)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write('M');
                    Console.ResetColor();
                }
                else if (i == xMissile && j == yMissile && grille[i, j].EstTouche)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write('M');
                    Console.ResetColor();
                }
                else if ((grille[i, j].Bateau == null && grille[i, j].EstTouche == false) ||
                         (grille[i, j].Bateau != null && grille[i, j].EstTouche == false))
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write('~');
                    Console.ResetColor();
                }
                else if (grille[i, j].Bateau == null && grille[i, j].EstTouche)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write('X');
                    Console.ResetColor();
                }
                else if (grille[i, j].Bateau != null && grille[i, j].EstTouche && !EstBateauCouleDansGrille(grille, grille[i, j].Bateau.UUID))
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write('B');
                    Console.ResetColor();
                }
                else if (grille[i, j].Bateau != null && grille[i, j].EstTouche && EstBateauCouleDansGrille(grille, grille[i, j].Bateau.UUID))
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write('C');
                    Console.ResetColor();
                }

            Console.WriteLine("");
        }
    }
    
    private bool JoueurAGagne(Joueur joueur)
    {
        var grille = joueur.Grille;
        foreach (var cellule in grille)
            if (cellule.Bateau != null && cellule.EstTouche == false)
                return false;

        return true;
    }

    public bool EstBateauCouleDansGrille(Cellule[,] grille, string bateauUUID)
    {
        foreach (var cellule in grille)
        {
            if (cellule.Bateau != null && cellule.Bateau.UUID == bateauUUID && !cellule.EstTouche)
            {
                // Une cellule du bateau n'a pas été touchée, donc le bateau n'est pas coulé
                return false;
            }
        }

        // Toutes les cellules du bateau ont été touchées
        return true;
    }

    public async Task Run()
    {

        await GetConfig();

        var joueur1 = new Joueur("Joueur 1", GetEmptyGrille());
        var joueur2 = new Joueur("Joueur 2", GetEmptyGrille());

        //Placement des bateaux pour les joueurs
        bateaux.ForEach(b => { UtilisateurPlaceBateau(b, joueur1.Grille); });
        bateaux.ForEach(b => { UtilisateurPlaceBateau(b, joueur2.Grille); });

        do{
            JoueurToucheCellule(joueur1);
            joueur1.aGagne = JoueurAGagne(joueur1);
            if (!joueur1.aGagne){
                JoueurToucheCellule(joueur2);
                joueur2.aGagne = JoueurAGagne(joueur2);
            }
        } while (joueur2.aGagne == false && joueur1.aGagne == false);

        if (joueur1.aGagne)
            ConsoleLogger.Log("Joueur 1 a gagné !", LogLevel.Info);
        else
            ConsoleLogger.Log("Joueur 2 a gagné !", LogLevel.Info);
    }
}