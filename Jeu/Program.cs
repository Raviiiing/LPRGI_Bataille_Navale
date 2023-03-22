using Jeu;
using Newtonsoft.Json;

Api Api = new Api("https://api-lprgi.natono.biz/api/GetConfig", "lprgi_api_key_2023");

string content = await Api.GetApiContent();

Parametre parametre = JsonConvert.DeserializeObject<Parametre>(content);

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