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
    white, //absence de carré en blanc
    black, //cadre et le tour des carrés en noir
    blue,  //reste des couleurs pour les tetrinos
    green,
    red,
    yellow,
    violet,
    orange
}