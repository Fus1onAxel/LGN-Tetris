using System;

namespace Tetris.NoyauTetris;

/**
* @class NoyauTetris
* @brief Classe principale du noyau du jeu (à compléter si besoin).
* @author Axel
*/
public class NoyauTetris
{

}

/**
* @class JeuTetris
* @brief Gère la logique du jeu Tetris (grille, tetrino courant, déplacements).
* @author Axel
*/
public class JeuTetris
{
    /** Largeur de la grille de jeu. */
    public int LargeurGrille;
    /** Hauteur de la grille de jeu. */
    public int HauteurGrille;
    /** Tetrino actuellement en cours de chute. */
    public Tetrino TetrinoCourant;
    /**
    * @brief Grille représentant les carrés figés (tetrinos posés).
    * Chaque case contient une valeur de TetrinoCouleur (0 = vide).
    * Dimensions : [LargeurGrille][HauteurGrille]
    * @author Axel
    */
    public int[,] Grille;

    /**
    * @brief Constructeur : initialise la grille et le tetrino courant à null.
    * @author Axel
    */
    public JeuTetris()
    {
        LargeurGrille = 10;
        HauteurGrille = 20;
        TetrinoCourant = null;
        Grille = new int[LargeurGrille, HauteurGrille];
    }

    /**
    * @brief Initialise ou réinitialise le jeu avec une grille vide et un nouveau tetrino courant.
    * Peut être appelée à tout moment pour recommencer le jeu depuis le début.
    * @author Axel
    */
    public void Demarrer()
    {
        // Réinitialise la grille (toutes les cases à 0 = vide)
        Grille = new int[LargeurGrille, HauteurGrille];
        TetrinoCourant = new Tetrino();
        TetrinoCourant.NouveauTetrino(LargeurGrille);
    }

    /**
    * @brief Vérifie si une position donnée est occupée dans la grille figée.
    * @param x Abscisse à vérifier
    * @param y Ordonnée à vérifier
    * @return true si la case est occupée ou hors grille, false sinon
    * @author Axel
    */
    public bool EstOccupee(int x, int y)
    {
        if (x < 0 || x >= LargeurGrille || y >= HauteurGrille)
            return true;
        if (y < 0)
            return false; // zone de spawn au-dessus de la grille, toujours libre
        return Grille[x, y] != (int)TetrinoCouleur.white;
    }

    /**
    * @brief Fige le tetrino courant dans la grille, puis supprime les lignes complètes,
    * et génère un nouveau tetrino courant.
    * @author Axel
    */
    public void FigerTetrino()
    {
        if (TetrinoCourant == null) return;

        // Inscrit chaque carré du tetrino dans la grille
        foreach (var pos in TetrinoCourant.Positions())
        {
            if (pos.Y >= 0 && pos.Y < HauteurGrille && pos.X >= 0 && pos.X < LargeurGrille)
                Grille[pos.X, pos.Y] = (int)TetrinoCourant.Couleur;
        }

        // Supprime les lignes pleines
        SupprimerLignesPleines();

        // Génère un nouveau tetrino
        TetrinoCourant.NouveauTetrino(LargeurGrille);
    }

    /**
    * @brief Supprime toutes les lignes complètes de la grille et fait descendre les lignes supérieures.
    * @author Axel
    */
    public void SupprimerLignesPleines()
    {
        // Parcourt du bas vers le haut
        for (int y = HauteurGrille - 1; y >= 0; y--)
        {
            bool lignePleine = true;
            for (int x = 0; x < LargeurGrille; x++)
            {
                if (Grille[x, y] == (int)TetrinoCouleur.white)
                {
                    lignePleine = false;
                    break;
                }
            }
            if (lignePleine)
            {
                // Décale toutes les lignes au-dessus vers le bas
                for (int row = y; row > 0; row--)
                {
                    for (int x = 0; x < LargeurGrille; x++)
                        Grille[x, row] = Grille[x, row - 1];
                }
                // Vide la ligne du haut
                for (int x = 0; x < LargeurGrille; x++)
                    Grille[x, 0] = (int)TetrinoCouleur.white;
                // Retraite la même ligne y car elle a changé
                y++;
            }
        }
    }

    /**
    * @brief Déplace le tetrino courant d'une case vers la droite si possible.
    * @author Zakaria
    */
    public void Droite()
    {
        if (TetrinoCourant == null) return;
        // Vérifie que tous les carrés restent dans le cadre et ne chevauchent pas la grille
        foreach (var pos in TetrinoCourant.Positions())
        {
            if (pos.X + 1 >= LargeurGrille || EstOccupee(pos.X + 1, pos.Y))
                return;
        }
        TetrinoCourant.PositionOrigine.DeplacerDroite();
    }

    /**
    * @brief Déplace le tetrino courant d'une case vers la gauche si possible.
    * @author Zakaria
    */
    public void Gauche()
    {
        if (TetrinoCourant == null) return;
        foreach (var pos in TetrinoCourant.Positions())
        {
            if (pos.X - 1 < 0 || EstOccupee(pos.X - 1, pos.Y))
                return;
        }
        TetrinoCourant.PositionOrigine.DeplacerGauche();
    }

    /**
    * @brief Déplace le tetrino courant d'une case vers le bas.
    * Fige le tetrino si le mouvement est bloqué (bas de grille ou carré figé en dessous).
    * @author Uriel
    */
    public void Bas()
    {
        if (TetrinoCourant == null) return;
        foreach (var pos in TetrinoCourant.Positions())
        {
            if (pos.Y + 1 >= HauteurGrille || EstOccupee(pos.X, pos.Y + 1))
            {
                // Fige le tetrino et en génère un nouveau
                FigerTetrino();
                return;
            }
        }
        TetrinoCourant.PositionOrigine.DeplacerBas();
    }

    /**
    * @brief Fait tomber le tetrino courant jusqu'à ce qu'il soit bloqué, puis le fige.
    * @author Uriel
    */
    public void Tombe()
    {
        if (TetrinoCourant == null) return;
        int maxDescente = 0;
        bool peutDescendre = true;
        while (peutDescendre)
        {
            foreach (var pos in TetrinoCourant.Positions())
            {
                if (pos.Y + 1 + maxDescente >= HauteurGrille || EstOccupee(pos.X, pos.Y + 1 + maxDescente))
                {
                    peutDescendre = false;
                    break;
                }
            }
            if (peutDescendre)
                maxDescente++;
        }
        for (int i = 0; i < maxDescente; i++)
            TetrinoCourant.PositionOrigine.DeplacerBas();

        // Fige le tetrino après la chute
        FigerTetrino();
    }
}

/**
* @enum TetrinoCouleur
* @brief Enumération des couleurs possibles pour les tetrinos et la grille.
* @author Ivan
*/
public enum TetrinoCouleur
{
    white = 0, // absence de carré en blanc
    black = 1, // cadre et le tour des carrés en noir
    blue = 2,  // reste des couleurs pour les tetrinos
    green = 3,
    red = 4,
    yellow = 5,
    violet = 6,
    orange = 7
}

/**
* @class Position
* @brief Représente la position d'un carré dans la grille (coordonnées X, Y).
* @author Ivan
*/
public class Position
{
    /** Abscisse du carré. */
    public int X;
    /** Ordonnée du carré. */
    public int Y;

    /**
    * @brief Constructeur de position.
    * @param x Abscisse initiale
    * @param y Ordonnée initiale
    * @author Ivan
    */
    public Position(int posx, int posy)
    {
        X = posx;
        Y = posy;
    }

    /**
    * @brief Déplace la position vers la gauche (X - 1).
    * @author Ivan
    */
    public void DeplacerGauche()
    {
        X = X - 1;
    }

    /**
    * @brief Déplace la position vers la droite (X + 1).
    * @author Ivan
    */
    public void DeplacerDroite()
    {
        X = X + 1;
    }

    /**
    * @brief Déplace la position vers le bas (Y + 1).
    * @author Ivan
    */
    public void DeplacerBas()
    {
        Y = Y + 1;
    }
}

/**
* @class Tetrino
* @brief Représente un tetrino (forme, couleur, position, génération aléatoire).
* @author Axel
*/
public class Tetrino
{
    /** Tableau statique des positions locales des 3 tetrinos. */
    public static Position[][] TetrinosTab = new Position[][]
    {
        // carré
        new Position[] { new Position(0, 0), new Position(1, 0), new Position(0, -1), new Position(1, -1) },
        // barre horizontale
        new Position[] { new Position(0, 0), new Position(1, 0), new Position(2, 0), new Position(3, 0) },
        // barre verticale
        new Position[] { new Position(0, 0), new Position(0, -1), new Position(0, -2), new Position(0, -3) }
    };

    /** Tableau statique des couleurs possibles. */
    public static TetrinoCouleur[] CouleursTetrinos = new TetrinoCouleur[]
    {
        TetrinoCouleur.red,
        TetrinoCouleur.green,
        TetrinoCouleur.yellow,
        TetrinoCouleur.blue,
        TetrinoCouleur.violet,
        TetrinoCouleur.orange
    };

    /** Indice du tetrino courant dans TetrinosTab. */
    public int Indice { get; set; }
    /** Position de l'origine du tetrino dans le repère du jeu. */
    public Position PositionOrigine { get; set; }
    /** Couleur du tetrino courant. */
    public TetrinoCouleur Couleur { get; set; }

    private static Random random = new Random();

    /**
    * @brief Constructeur : carré rouge en (0,0) par défaut.
    * @author Axel
    */
    public Tetrino()
    {
        Indice = 0;
        PositionOrigine = new Position(0, 0);
        Couleur = TetrinoCouleur.red;
    }

    /**
    * @brief Retourne les 4 positions du tetrino dans le repère du jeu.
    * @return Tableau de 4 positions globales.
    * @author Axel
    */
    public Position[] Positions()
    {
        Position[] local = TetrinosTab[Indice];
        Position[] global = new Position[4];
        for (int i = 0; i < 4; i++)
        {
            global[i] = new Position(
                PositionOrigine.X + local[i].X,
                PositionOrigine.Y + local[i].Y
            );
        }
        return global;
    }

    /**
    * @brief Tire un nouveau tetrino aléatoire (indice, position d'origine valide, couleur).
    * @param largeurGrille Largeur de la grille pour placer le tetrino correctement.
    * @author Axel
    */
    public void NouveauTetrino(int largeurGrille)
    {
        Indice = random.Next(TetrinosTab.Length);
        Couleur = CouleursTetrinos[random.Next(CouleursTetrinos.Length)];

        // Calcul de la largeur du tetrino pour éviter de sortir du cadre
        int largeurTetrino = 0;
        foreach (var pos in TetrinosTab[Indice])
        {
            if (pos.X > largeurTetrino)
                largeurTetrino = pos.X;
        }
        // Position d'origine X valide (pour que le tetrino ne dépasse pas à droite)
        int maxX = largeurGrille - largeurTetrino - 1;
        int x = random.Next(0, Math.Max(1, maxX + 1));
        PositionOrigine = new Position(x, 0);
    }
}