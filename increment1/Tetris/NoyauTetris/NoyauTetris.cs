namespace NoyauTetris;

public class NoyauTetris
{

}

public class JeuTetris
{
    public int LargeurGrille;
    public int HauteurGrille;

    public JeuTetris(int l, int h)
    {
        LargeurGrille = l;
        HauteurGrille = h;
    }
}

enum TetrinoCouleur
{
    black,
    white,
    blue,
    green,
    red,
    yellow,
    violet,
    orange
}