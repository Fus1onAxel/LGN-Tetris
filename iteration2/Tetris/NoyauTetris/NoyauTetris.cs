namespace Tetris.NoyauTetris;

public class NoyauTetris
{

}

public class Tetrino
{
    public static Position[][] TetrinosTab = new Position[][]
    {
        // carre
        new Position[] 
        { 
            new Position(0, 0), new Position(1, 0),
            new Position(0, -1), new Position(1, -1) 
        },
        // barre horizontale
        new Position[] 
        { 
            new Position(0, 0), new Position(1, 0),
            new Position(2, 0), new Position(3, 0) 
        },
        // barre verticale
        new Position[] 
        { 
            new Position(0, 0), new Position(0, -1),
            new Position(0, -2), new Position(0, -3) 
        }
    }
}

public class JeuTetris
{
    public int LargeurGrille;
    public int HauteurGrille;

    public JeuTetris()
    {
        LargeurGrille = 12;
        HauteurGrille = 15;
    }
}

/** Position
    La classe position enregistre une position sur la grille
    @author Axel DAVID
    @param x la coordonnée horizontale dans l'espace dans la grille des carrés
    @param y la coordonnée verticale dans l'espace dans la grille des carrés
*/
public class Position
{
    public int X;
    public int Y;

    public Position(int abs, int ord)
    {
        abs = X;
        ord = Y;
    }

    public void Gauche()
    {
        if(X > 0)
        {
            X = X - 1;
        }
    }
    public void Droite()
    {
        if(X < 11)
        {
            X = X + 1;
        }
    }
    public void Bas()
    {
        if(Y < 14)
        {
            Y = Y - 1;
        }
    }
}

public enum TetrinoCouleur
{
    white = 0, //absence de carré en blanc
    black = 1, //cadre et le tour des carrés en noir
    blue = 2,  //reste des couleurs pour les tetrinos
    green = 3,
    red = 4,
    yellow = 5,
    violet = 6,
    orange = 7
}