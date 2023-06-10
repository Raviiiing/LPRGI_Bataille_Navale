using Core;
using Microsoft.Maui.Controls;

namespace interfaceG
{
    public partial class MainPage : ContentPage
    {
        private Jeu jeu;
        private int nbColonnes;
        private int nbLignes;

        public MainPage()
        {
            InitializeComponent();
            jeu = new Jeu();
            LoadConfig();
        }

        private async void LoadConfig()
        {
            await jeu.GetConfig();
            nbColonnes = jeu.nbColonnes;
            nbLignes = jeu.nbLignes;
            CreateGrids();
            FillGrids();
        }

        private void CreateGrids()
        {
            grilleJoueur1.Children.Clear();
            grilleJoueur2.Children.Clear();

            for (int i = 0; i < nbColonnes; i++)
            {
                grilleJoueur1.ColumnDefinitions.Add(new ColumnDefinition());
            }

            for (int j = 0; j < nbLignes; j++)
            {
                grilleJoueur1.RowDefinitions.Add(new RowDefinition());
            }

            for (int i = 0; i < nbColonnes; i++)
            {
                grilleJoueur2.ColumnDefinitions.Add(new ColumnDefinition());
            }

            for (int j = 0; j < nbLignes; j++)
            {
                grilleJoueur2.RowDefinitions.Add(new RowDefinition());
            }
        }

        private void FillGrids()
        {
            for (int i = 0; i < nbColonnes; i++)
            {
                for (int j = 0; j < nbLignes; j++)
                {
                    var celluleButtonJoueur1 = new Button
                    {
                        Text = "~",
                        HorizontalOptions = new LayoutOptions { Alignment = LayoutAlignment.Fill, Expands = true },
                        VerticalOptions = new LayoutOptions { Alignment = LayoutAlignment.Fill, Expands = true },
                        HeightRequest = 50,
                        WidthRequest = 50,
                        Margin = new Thickness(10)
                    };

                    celluleButtonJoueur1.Clicked += CelluleButton_Clicked;

                    Grid.SetColumn(celluleButtonJoueur1, i);
                    Grid.SetRow(celluleButtonJoueur1, j);
                    grilleJoueur1.Children.Add(celluleButtonJoueur1);
                }
            }

            // Remplir la grille du joueur 2 avec des boutons
            for (int i = 0; i < nbColonnes; i++)
            {
                for (int j = 0; j < nbLignes; j++)
                {
                    var celluleButtonJoueur2 = new Button
                    {
                        Text = "~",
                        HorizontalOptions = new LayoutOptions { Alignment = LayoutAlignment.Fill, Expands = true },
                        VerticalOptions = new LayoutOptions { Alignment = LayoutAlignment.Fill, Expands = true },
                        HeightRequest = 50,
                        WidthRequest = 50,
                        Margin = new Thickness(10)
                    };

                    celluleButtonJoueur2.Clicked += CelluleButton_Clicked;

                    Grid.SetColumn(celluleButtonJoueur2, i);
                    Grid.SetRow(celluleButtonJoueur2, j);
                    grilleJoueur2.Children.Add(celluleButtonJoueur2);
                }
            }
        }




        private async void CelluleButton_Clicked(object sender, EventArgs e)
        {
            // Gestionnaire d'événements pour le clic sur un bouton de la grille
            var button = (Button)sender;

            // Vérifier si le bouton est dans la grille du joueur 1 ou du joueur 2 et afficher un message
            if (grilleJoueur1.Children.Contains(button))
            {
                var column = Grid.GetColumn(button);
                var row = Grid.GetRow(button);
                await DisplayAlert("Joueur 1", $"Vous avez cliqué sur la cellule {column}, {row} du joueur 1", "OK");
            }
            else if (grilleJoueur2.Children.Contains(button))
            {
                var column = Grid.GetColumn(button);
                var row = Grid.GetRow(button);
                await DisplayAlert("Joueur 2", $"Vous avez cliqué sur la cellule {column}, {row} du joueur 2", "OK");
            }


        }

    }
}
