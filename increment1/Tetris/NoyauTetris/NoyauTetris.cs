namespace NoyauTetris;

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

enum TetrinoCouleur
{
    white,
    black,
    blue,
    green,
    red,
    yellow,
    violet,
    orange
}