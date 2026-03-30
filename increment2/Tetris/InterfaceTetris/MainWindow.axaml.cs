/* Fichier MainWindow.axaml.cs
* Gère l'interface du jeu de Tetris : la fenêtre graphique et 
* l'ensemble des interactions du jeu.
* Auteurs : Axel, Uriel, Ivan, Zakaria
* Version : alpha
*/

using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using System;
using Avalonia.Threading;
using Tetris.NoyauTetris;

namespace Tetris.InterfaceTetris;

/**
* @class MainWindow
* @brief Gère la fenêtre principale du jeu de Tetris, et l'ensemble des interactions du jeu.
* @author Axel, Uriel, Ivan, Zakaria
*/
public partial class MainWindow : Window
{
    /** 
    * @brief Minuteur qui déclenche régulièrement un évènement pour faire descendre le tetrino.
    * @author Zakaria
    */
    public DispatcherTimer Minuteur;

    /** 
    * @brief Attribut qui définit le jeu en cours.
    */
    public JeuTetris Jeu;

    /**
    * @brief Constructeur de la fenêtre principale. Initialise l'interface, le jeu et les événements.
    * @author Uriel
    */
    public MainWindow()
    {
        InitializeComponent();

        // Défini la taille de la fenêtre à partir des constantes
        Width = 400;
        Height = 800;
        // Définit le texte de InfoText
        InfoText.Text = "Zone de texte";
        // Défini la taille du canvas à partir des constantes
        TetrisCanvas.Width = 288;
        TetrisCanvas.Height = 342;
        // Défini la taille des boutons à partir des constantes
        StartButton.Width = 200;
        StartButton.Height = 30;
        QuitButton.Width = 200;
        QuitButton.Height = 40; 

        // Initialise le jeu
        Jeu = new JeuTetris();

        // Initialise le minuteur pour faire descendre le tetrino courant toutes les 500 millisecondes
        Minuteur = new DispatcherTimer();
        Minuteur.Interval = TimeSpan.FromMilliseconds(500);
        Minuteur.Tick += (s, e) => { BasInterface();};   

        // détecte le clic sur le bouton Démarrer, déclenche l'évènement Demarrer, puis appelle la méthode DemarrerInterface
        StartButton.Click += (s, e) => { DemarrerInterface();};
        // détecte le clic sur le bouton Quitter, déclenche l'évènement Quiter, puis ferme la fenêtre
        QuitButton.Click += (s, e) => { Close();};
        // détecte la pression d'une touche du clavier, et déclenche l'évènement correspondant
        KeyDown += (s, e) =>
        {
            if (e.Key == Key.Left)
            {
                GaucheInterface();
            }
            else if (e.Key == Key.Right)
            {
                DroiteInterface();
            }
            else if (e.Key == Key.X)
            {
                RotationDroiteInterface();
            }
            else if (e.Key == Key.W)
            {
                RotationGaucheInterface();
            }
            else if (e.Key == Key.Down)
            {
                TombeInterface();
            }
        };
        DessinerCadre();
    } 

    /**
    * @brief Dessine un rectangle dans le TetrisCanvas, à la position (x, y), de largeur width, 
    * de hauteur height (en pixels) et de couleur couleur.
    * @param x Coordonnée horizontale en pixels
    * @param y Coordonnée verticale en pixels
    * @param with Largeur du rectangle
    * @param height Hauteur du rectangle
    * @param couleur Couleur du rectangle
    */
    public void DessinerRectangle(int x, int y, int with, int height, Avalonia.Media.IBrush couleur)
    {
        TetrisCanvas.Children.Add(new Avalonia.Controls.Shapes.Rectangle
        {
            Width = with,
            Height = height,
            Fill = couleur,
            Margin = new Thickness(x, y, 0, 0) 
        });
    }

    /**
    * @brief Traduit un entier représentant une couleur en une brosse Avalonia correspondante.
    * @param color Code couleur (TetrinoCouleur)
    * @return IBrush correspondant à la couleur
    * @author Zakaria
    */
    public Avalonia.Media.IBrush TranslateColor(int color)
    {
        switch (color)
        {
            case 0:
                return Avalonia.Media.Brushes.White;
            case 1:
                return Avalonia.Media.Brushes.Black;
            case 2:
                return Avalonia.Media.Brushes.Blue;
            case 3:
                return Avalonia.Media.Brushes.Green;
            case 4:
                return Avalonia.Media.Brushes.Red;
            case 5:
                return Avalonia.Media.Brushes.Yellow;
            case 6:
                return Avalonia.Media.Brushes.Violet;
            case 7:
                return Avalonia.Media.Brushes.Orange;
            default:
                Console.WriteLine("La couleur doit être comprise entre 0 et 7");
                return Avalonia.Media.Brushes.White;
        }
    }

    /**
    * @brief Dessine le cadre de jeu (bordure noire et fond blanc).
    * @author Uriel
    */
    public void DessinerCadre()
    {
        DessinerRectangle(0, 0, 288, 342, TranslateColor((int)TetrinoCouleur.black));
        DessinerRectangle(12, 0, 264, 330, TranslateColor((int)TetrinoCouleur.white));
    }

    /**
    * @brief Dessine un carré sur la scène en fonction des coordonnées et de la couleur.
    * @param x Coordonnée horizontale dans la grille
    * @param y Coordonnée verticale dans la grille
    * @param couleur Couleur du carré
    * @author Ivan
    */
    public void DessinerCarre(int x, int y, Avalonia.Media.IBrush couleur)
    {
        // Création du carré qui correspond à la bordure
        DessinerRectangle((x*22+12)-1, y*22, 22, 22, TranslateColor((int)TetrinoCouleur.black));
        // Remplissage du fond du carré par création d'un autre carré
        DessinerRectangle((x*22+12)+1, y*22+2, 18, 18, couleur);
    }

    /**
    * @brief Dessine le jeu : cadre et tetrino courant.
    * @author Axel
    */
    public void DessinerJeu()
    {
        // Efface le canvas
        TetrisCanvas.Children.Clear();
        // Dessine le cadre
        DessinerCadre();
        // Dessine le tetrino courant s'il existe
        if (Jeu.TetrinoCourant != null)
        {
            var positions = Jeu.TetrinoCourant.Positions();
            var couleur = TranslateColor((int)Jeu.TetrinoCourant.Couleur);
            foreach (var pos in positions)
            {
                // On ne dessine que les carrés visibles (y >= 0)
                if (pos.Y >= 0)
                    DessinerCarre(pos.X, pos.Y, couleur);
            }
        }
    }

    /**
    * @brief Démarre ou réinitialise le jeu et le minuteur.
    * @author Axel
    */
    public void DemarrerInterface()
    {
        Jeu.Demarrer();
        DessinerJeu();
        Minuteur.Start();
    }

    /**
    * @brief Déplace le tetrino courant d'une case vers la droite et met à jour l'affichage.
    * @author Zakaria
    */
    public void DroiteInterface()
    {
        Jeu.Droite();
        DessinerJeu();
    }

    /**
    * @brief Déplace le tetrino courant d'une case vers la gauche et met à jour l'affichage.
    * @author Zakaria
    */
    public void GaucheInterface()
    {
        Jeu.Gauche();
        DessinerJeu();
    }

    /**
    * @brief Fait descendre le tetrino courant d'une case et met à jour l'affichage.
    * Appelée par le minuteur.
    * @author Zakaria
    */
    public void BasInterface()
    {
        Jeu.Bas();
        DessinerJeu();
    }

    /**
    * @brief Fait tomber le tetrino courant jusqu'en bas et met à jour l'affichage.
    * @author Axel
    */
    public void TombeInterface()
    {
        Jeu.Tombe();
        DessinerJeu();
    }

    /** ... */
    public void RotationDroiteInterface()
    {
        Console.WriteLine("Rotation à droit à coder...");
    }

    /** ... */
    public void RotationGaucheInterface()
    {
        Console.WriteLine("Rotation à gauche à coder...");
    }
}