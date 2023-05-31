using Jeu;
using Newtonsoft.Json;

Api Api = new Api("https://api-lprgi.natono.biz/api/GetConfig", "lprgi_api_key_2023");

string content = await Api.GetApiContent();

Parametres parametre = JsonConvert.DeserializeObject<Parametres>(content);

if(parametre == null)
{
    throw new Exception("Failed to deserialize JSON string to object.");
}
Console.ForegroundColor = ConsoleColor.Gray;
Console.WriteLine("Nombre de lignes: " + parametre.NbLignes);
Console.WriteLine("Nombre de colonnes: " + parametre.NbColonnes);
Console.WriteLine("Bateaux: ");
foreach (Bateau bateau in parametre.Bateaux)
{
    Console.WriteLine("Taille: " + bateau.Taille + " Nom: " + bateau.Nom);
}
Console.ResetColor();



/* Démarrage du jeu */


Joueur joueur1 = new Joueur("Joueur 1");
Joueur joueur2 = new Joueur("Joueur 2");
joueur1.bateaux = parametre.Bateaux;
joueur2.bateaux = parametre.Bateaux;

Console.WriteLine("Joueur 1, placez vos bateaux");
foreach (Bateau bateau in joueur1.bateaux)
{
    //Le joueur place le bateau avec les touches UP, DOWN, LEFT, RIGHT et R pour tourner le bateau puis ENTER pour valider
    ConsoleKeyInfo key;
    do
    {
        Console.Clear();
        Console.WriteLine("Placez le bateau "+ bateau.Nom +" de taille " + bateau.Taille + " en " + bateau.Orientation);
        joueur1.printGrid(parametre);
        key = Console.ReadKey(true);
        //Console.WriteLine("X: " + bateau.X + " Y: " + bateau.Y + " Orientation: " + bateau.Orientation);
        switch (key.Key)
        {
            case ConsoleKey.UpArrow:
                if (bateau.VerifyRange(bateau.X, bateau.Y - 1, parametre.NbColonnes, parametre.NbLignes))
                {
                    bateau.Y--;
                }
                break;
            case ConsoleKey.DownArrow:
                if (bateau.VerifyRange(bateau.X, bateau.Y + 1, parametre.NbColonnes, parametre.NbLignes))
                {
                    bateau.Y++;
                }
                break;
            case ConsoleKey.LeftArrow:
                if (bateau.VerifyRange(bateau.X - 1, bateau.Y, parametre.NbColonnes, parametre.NbLignes))
                {
                    bateau.X--;
                }
                break;
            case ConsoleKey.RightArrow:
                if (bateau.VerifyRange(bateau.X + 1, bateau.Y, parametre.NbColonnes, parametre.NbLignes))
                {
                    bateau.X++;
                }
                break;
            case ConsoleKey.R:
                if (bateau.Orientation == 1) bateau.Orientation = 0;
                else bateau.Orientation = 1;
                bateau.X = 0;
                bateau.Y = 0;
                break;
        }
        //Console.WriteLine("X: " + bateau.X + " Y: " + bateau.Y + " Orientation: " + bateau.Orientation);
    } while (key.Key != ConsoleKey.Enter);
}