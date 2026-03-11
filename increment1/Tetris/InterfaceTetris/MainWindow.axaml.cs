/* Fichier MainWindow.axaml.cs
 * Gère l'interface du jeu de Tetris : la fenêtre graphique et 
 * l'ensemble des interactions du jeu.
 * Auteur : ...
 * Version : alpha
 */

using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using System;
using Avalonia.Threading;
using NoyauTetris;
// à ajouter à partir de l'itération 1
//using NoyauTetris;

namespace InterfaceTetris;

/* Gère la fenêtre principale du jeu de Tetris, et l'ensemble des interactions du jeu. */
public partial class MainWindow : Window
{
    /* Minuteur qui déclanche régulièrement un évènement. */
    public DispatcherTimer Minuteur;
    
    public MainWindow()
    {
        InitializeComponent();
        // Défini la taille de la fenêtre à partir des constantes
        Width = 300;
        Height = 600;
        // Définit le texte de InfoText
        InfoText.Text = "Zone de texte";
        // Défini la taille du canvas à partir des constantes
        TetrisCanvas.Width = 200;
        TetrisCanvas.Height = 400;
        // Défini la taille des boutons à partir des constantes
        StartButton.Width = 200;
        StartButton.Height = 30;
        QuitButton.Width = 200;
        QuitButton.Height = 40; 
        // Initialise le minuteur pour faire descendre le tetrino courant toutes les 500 milisecondes
        Minuteur = new DispatcherTimer();
        Minuteur.Interval = TimeSpan.FromMilliseconds(500);
        Minuteur.Tick += (s, e) => { BasInterface();};   
        // détecte le clic sur le bouton Démarrer, déclanche l'évènement Demarrer, puis appelle la méthode DemarrerTetris
        StartButton.Click += (s, e) => { DemarrerInterface();};
        // détecte le clic sur le bouton Quitter, déclanche l'évènement Quiter, puis ferme la fenêtre
        QuitButton.Click += (s, e) => { Close();};
        // détecte la pression d'une touche du clavier, et déclanche l'évènement correspondant
        KeyDown += (s, e) =>
        {
            // Choix des touches à modifier si besoin (voir la documentation de l'énumération Key)
            if (e.Key == Key.Left)
            {
                GaucheInterface();
            }
            else if (e.Key == Key.Right)
            {
                DroiteInterface();
            }
            else if (e.Key == Key.X)
            // si vous disposer d'un pavé numérique, choisir Key.PageUp
            {
                RotationDroiteInterface();
            }
            else if (e.Key == Key.W)
            // si vous disposer d'un pavé numérique, choisir Key.Home
            {
                RotationGaucheInterface();
            }
            else if (e.Key == Key.Down)
            {
                TombeInterface();
            }
        };
    } 

    /* Dessine un rectangle dans le TetrisCanvas, à la position (x, y), de largeur width, 
    de hauteur height (en pixels) et de couleur couleur. */
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
    Prend un int et renvoie une couleur 
    */
    public IImmutableSolidColorBrush TranslateColor(int color)
    {
        switch (color)
        {
            case = 0:
                return IImmutableSolidColorBrush White { get; };
                break;

            case = 1:
                return IImmutableSolidColorBrush Black { get; };
                break;

            case = 2:
                return IImmutableSolidColorBrush Blue { get; };
                break;

            case = 3:
                return IImmutableSolidColorBrush Green { get; };
                break;

            case = 4:
                return IImmutableSolidColorBrush Red { get; };
                break;

            case = 5:
                return IImmutableSolidColorBrush Yellow { get; };
                break;

            case = 6:
                return IImmutableSolidColorBrush Violet { get; };
                break;

            case = 7:
                return IImmutableSolidColorBrush Orange { get; };
                break;

            default:
                Console.WriteLine("la couleur n'est pas possible, elle doit etre comprise entre 0 et 7");
                break;
        }
    }



    /** DessinerCarre
        La fonction dessine un carré sur scène en fonction des coordonnées données et la couleur
        @author IvanZarembovskyi
        @param x la coordonnée horizontale dans l'espace dans la grille des carrés
        @param y la coordonnée verticale dans l'espace dans la grille des carrés
        @param couleur la couleur du remplissage du fond du carré
    */
    public void DessinerCarre(int x, int y, Avalonia.Media.IBrush couleur)
    {
        //Les coordonnées sont conver
        //Création du carré qui correspond à la bordure
        DessinerRectangle((x*22+12), y*22, 22, 22, TranslateColor(1));
        //Remplissage du fond du carré par création d'un autre carré
        DessinerRectangle((x*22+12), y*22, 20, 20, couleur);
    }


    /* DessinerCarre
        La fonction dessine le cadre de jeu qui se compose d'un cadre noir au fond pour la marge des cotes et du bas et un cadre blanc centré au dessus qui est plus petit.
        @author UrielLENQUETTE
    */

    public void DessinerCadre()
    {
        int coteCadrePixel = 22;
        int pixelLargeurGrilleInterieur = NoyauTetris.JeuTetris.LargeurGrille * coteCadrePixel;
        int pixelHauteurGrilleInterieur = NoyauTetris.JeuTetris.HauteurTetris * coteCadrePixel;
        int pixelLargeurGrilleExterieur = pixelHauteurGrilleInterieur + 12*2;
        int pixelHauteurGrilleExterieur = pixelHauteurGrilleInterieur + 12;
        DessinerRectangle(0, 0, pixelLargeurGrilleExterieur, pixelHauteurGrilleExterieur, TranslateColor(TetrinoCouleur.black));
        DessinerRectangle(12, 0, pixelLargeurGrilleInterieur, pixelHauteurGrilleInterieur, TranslateColor(TetrinoCouleur.white));
    }

    /* ... */
    public void DemarrerInterface()
    {
        Console.WriteLine("Démarrage du jeu de Tetris à coder...");
        DessinerCadre();
        DessinerCarre(0, 0, TranslateColor(TetrinoCouleur.red));
        DessinerCarre(1, 1, TranslateColor(TetrinoCouleur.yellow));
        DessinerCarre(2, 2, TranslateColor(TetrinoCouleur.blue));
    }

    /* ... */
    public void DroiteInterface()
    {
        Console.WriteLine("Déplacement à droite à coder...");
    }

    /* ... */
    public void GaucheInterface()
    {
        Console.WriteLine("Déplacement à gauche à coder...");
    }

    /* ... */
    public void BasInterface()
    {
        Console.WriteLine("Déplacement en bas à coder...");
    }

    /* ... */
    public void TombeInterface()
    {
        Console.WriteLine("Déplacement rapide en bas à coder...");

    }

    /* ... */
    public void RotationDroiteInterface()
    {
        Console.WriteLine("Rotation à droit à coder...");
    }

    /* ... */
    public void RotationGaucheInterface()
    {
        Console.WriteLine("Rotation à gauche à coder...");
    }
}