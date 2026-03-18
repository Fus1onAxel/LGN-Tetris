namespace Tetris.NoyauTetris;

public class NoyauTetris
{

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