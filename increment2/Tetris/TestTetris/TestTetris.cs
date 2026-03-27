using Tetris.NoyauTetris;
using Xunit;

namespace TestTetris;

/**
 * @class UnitTest1
 * @brief Contient les tests unitaires pour les classes du noyau Tetris.
 * @author Axel, Uriel, Ivan, Zakaria
 */
public class UnitTest1
{
    /**
     * @test TestPositionDeplacement
     * @brief Vérifie les déplacements gauche, droite et bas d'une position.
     * @author Ivan
     */
    [Fact]
    public void TestPositionDeplacement()
    {
        var pos = new Position(5, 5);
        pos.DeplacerGauche();
        Assert.Equal(4, pos.X);
        pos.DeplacerDroite();
        Assert.Equal(5, pos.X);
        pos.DeplacerBas();
        Assert.Equal(6, pos.Y);
    }

    /**
     * @test TestTetrinoPositionsCarre
     * @brief Vérifie que les positions globales du tetrino carré sont correctes.
     * @author Axel
     */
    [Fact]
    public void TestTetrinoPositionsCarre()
    {
        var tetrino = new Tetrino();
        tetrino.Indice = 0; // carré
        tetrino.PositionOrigine = new Position(2, 3);
        var positions = tetrino.Positions();
        Assert.Contains(positions, p => p.X == 2 && p.Y == 3);
        Assert.Contains(positions, p => p.X == 3 && p.Y == 3);
        Assert.Contains(positions, p => p.X == 2 && p.Y == 2);
        Assert.Contains(positions, p => p.X == 3 && p.Y == 2);
    }

    /**
     * @test TestJeuTetrisDroiteGauche
     * @brief Vérifie le déplacement à droite puis à gauche du tetrino courant.
     * @author Zakaria
     */
    [Fact]
    public void TestJeuTetrisDroiteGauche()
    {
        var jeu = new JeuTetris();
        jeu.Demarrer();
        int xInit = jeu.TetrinoCourant.PositionOrigine.X;
        jeu.Droite();
        Assert.Equal(xInit + 1, jeu.TetrinoCourant.PositionOrigine.X);
        jeu.Gauche();
        Assert.Equal(xInit, jeu.TetrinoCourant.PositionOrigine.X);
    }

    /**
     * @test TestJeuTetrisBasTombe
     * @brief Vérifie le déplacement vers le bas et la chute complète du tetrino courant.
     * @author Uriel
     */
    [Fact]
    public void TestJeuTetrisBasTombe()
    {
        var jeu = new JeuTetris();
        jeu.Demarrer();
        int yInit = jeu.TetrinoCourant.PositionOrigine.Y;
        jeu.Bas();
        Assert.Equal(yInit + 1, jeu.TetrinoCourant.PositionOrigine.Y);

        // Test Tombe : le tetrino doit être réinitialisé en haut (Y == 0)
        jeu.Tombe();
        Assert.Equal(0, jeu.TetrinoCourant.PositionOrigine.Y);
    }

    /**
     * @test TestTetrinoNouveauTetrino
     * @brief Vérifie la génération aléatoire d'un nouveau tetrino (position et couleur).
     * @author Axel
     */
    [Fact]
    public void TestTetrinoNouveauTetrino()
    {
        var tetrino = new Tetrino();
        tetrino.NouveauTetrino(12);
        Assert.InRange(tetrino.PositionOrigine.X, 0, 11);
        Assert.Equal(0, tetrino.PositionOrigine.Y);
        Assert.InRange((int)tetrino.Couleur, 2, 7); // Couleurs possibles sauf blanc/noir
    }
}
