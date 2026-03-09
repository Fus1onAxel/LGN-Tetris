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
    black = IImmutableSolidColorBrush Black { get; },
    white = IImmutableSolidColorBrush White { get; },
    blue = IImmutableSolidColorBrush Blue { get; },
    green = IImmutableSolidColorBrush Green { get; },
    red = IImmutableSolidColorBrush Red { get; },
    yellow = IImmutableSolidColorBrush Yellow { get; },
    violet = IImmutableSolidColorBrush Violet { get; },
    orange = IImmutableSolidColorBrush Orange { get; }
}