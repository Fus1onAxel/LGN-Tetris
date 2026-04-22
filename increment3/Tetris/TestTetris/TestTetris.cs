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

        // Force le tetrino à être un carré (indice 0) en position connue pour ce test
        jeu.TetrinoCourant.Indice = 0; // carré
        jeu.TetrinoCourant.PositionOrigine = new Position(0, 0);

        jeu.Bas();
        Assert.Equal(1, jeu.TetrinoCourant.PositionOrigine.Y);

        // Test Tombe : le tetrino doit être réinitialisé en haut (Y == 0) après la chute
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
        tetrino.NouveauTetrino(10);
        Assert.InRange(tetrino.PositionOrigine.X, 0, 9);
        Assert.Equal(0, tetrino.PositionOrigine.Y);
        Assert.InRange((int)tetrino.Couleur, 2, 7); // Couleurs possibles sauf blanc/noir
    }

    /**
    * @test TestGrilleInitiale
    * @brief Vérifie que la grille est bien initialisée à zéro (vide) au démarrage.
    * @author Axel
    */
    [Fact]
    public void TestGrilleInitiale()
    {
        var jeu = new JeuTetris();
        jeu.Demarrer();
        for (int x = 0; x < jeu.LargeurGrille; x++)
        {
            for (int y = 0; y < jeu.HauteurGrille; y++)
            {
                Assert.Equal(0, jeu.Grille[x, y]);
            }
        }
    }

    /**
    * @test TestDimensionsGrille
    * @brief Vérifie que la grille a bien les dimensions 10x20.
    * @author Axel
    */
    [Fact]
    public void TestDimensionsGrille()
    {
        var jeu = new JeuTetris();
        Assert.Equal(10, jeu.LargeurGrille);
        Assert.Equal(20, jeu.HauteurGrille);
    }

    /**
    * @test TestFigerTetrinoInscritDansGrille
    * @brief Vérifie que FigerTetrino inscrit les 4 carrés du tetrino dans la grille.
    * @author Axel
    */
    [Fact]
    public void TestFigerTetrinoInscritDansGrille()
    {
        var jeu = new JeuTetris();
        jeu.Demarrer();
        // Force le tetrino à être un carré (indice 0) à une position connue
        jeu.TetrinoCourant.Indice = 0; // carré : (0,0),(1,0),(0,-1),(1,-1)
        jeu.TetrinoCourant.PositionOrigine = new Position(3, 5);
        jeu.TetrinoCourant.Couleur = TetrinoCouleur.red;

        jeu.FigerTetrino();

        // Les cases (3,5) et (4,5) doivent être rouges
        Assert.Equal((int)TetrinoCouleur.red, jeu.Grille[3, 5]);
        Assert.Equal((int)TetrinoCouleur.red, jeu.Grille[4, 5]);
        // La case (3,4) doit être rouge (Y=-1 relatif => Y=4 absolu)
        Assert.Equal((int)TetrinoCouleur.red, jeu.Grille[3, 4]);
        Assert.Equal((int)TetrinoCouleur.red, jeu.Grille[4, 4]);
    }

    /**
    * @test TestFigerTetrinoNouveauTetrino
    * @brief Vérifie qu'après FigerTetrino un nouveau tetrino est généré (Y == 0).
    * @author Axel
    */
    [Fact]
    public void TestFigerTetrinoNouveauTetrino()
    {
        var jeu = new JeuTetris();
        jeu.Demarrer();
        jeu.TetrinoCourant.Indice = 0;
        jeu.TetrinoCourant.PositionOrigine = new Position(0, 5);
        jeu.FigerTetrino();
        // Après figement, un nouveau tetrino doit être en haut
        Assert.Equal(0, jeu.TetrinoCourant.PositionOrigine.Y);
    }

    /**
    * @test TestSupprimerLignePleine
    * @brief Vérifie que les lignes pleines sont bien supprimées de la grille.
    * @author Axel
    */
    [Fact]
    public void TestSupprimerLignePleine()
    {
        var jeu = new JeuTetris();
        jeu.Demarrer();
        // Remplit entièrement la dernière ligne (y = HauteurGrille - 1)
        int yPlein = jeu.HauteurGrille - 1;
        for (int x = 0; x < jeu.LargeurGrille; x++)
            jeu.Grille[x, yPlein] = (int)TetrinoCouleur.red;

        jeu.SupprimerLignesPleines();

        // La ligne du bas doit maintenant être vide
        for (int x = 0; x < jeu.LargeurGrille; x++)
            Assert.Equal((int)TetrinoCouleur.white, jeu.Grille[x, yPlein]);
    }

    /**
    * @test TestDemarrerReinitialiseLaGrille
    * @brief Vérifie que Demarrer remet bien la grille à zéro (cas d'un redémarrage).
    * @author Axel
    */
    [Fact]
    public void TestDemarrerReinitialiseLaGrille()
    {
        var jeu = new JeuTetris();
        jeu.Demarrer();
        // Pollue la grille avec des valeurs non nulles
        jeu.Grille[0, 0] = (int)TetrinoCouleur.blue;
        jeu.Grille[5, 10] = (int)TetrinoCouleur.orange;
        // Redémarre le jeu
        jeu.Demarrer();
        // La grille doit être entièrement vide
        Assert.Equal((int)TetrinoCouleur.white, jeu.Grille[0, 0]);
        Assert.Equal((int)TetrinoCouleur.white, jeu.Grille[5, 10]);
    }

    /**
    * @test TestBasFigeLeTetrinoEnBas
    * @brief Vérifie que Bas fige le tetrino quand il atteint le bas de la grille.
    * @author Uriel
    */
    [Fact]
    public void TestBasFigeLeTetrinoEnBas()
    {
        var jeu = new JeuTetris();
        jeu.Demarrer();
        // Place le tetrino carré juste au-dessus du bas
        jeu.TetrinoCourant.Indice = 0; // carré : positions relatives (0,0),(1,0),(0,-1),(1,-1)
        jeu.TetrinoCourant.PositionOrigine = new Position(0, jeu.HauteurGrille - 1);
        jeu.TetrinoCourant.Couleur = TetrinoCouleur.green;

        jeu.Bas();

        // Le tetrino doit avoir été figé : la case (0, HauteurGrille-1) doit être verte
        Assert.Equal((int)TetrinoCouleur.green, jeu.Grille[0, jeu.HauteurGrille - 1]);
        // Un nouveau tetrino doit être généré en haut
        Assert.Equal(0, jeu.TetrinoCourant.PositionOrigine.Y);
    }

    /**
    * @test TestCollisionGaucheAvecGrille
    * @brief Vérifie que le tetrino ne peut pas se déplacer à gauche si la case est occupée.
    * @author Zakaria
    */
    [Fact]
    public void TestCollisionGaucheAvecGrille()
    {
        var jeu = new JeuTetris();
        jeu.Demarrer();
        // Place le tetrino barre horizontale (indice 1)
        jeu.TetrinoCourant.Indice = 1; // (0,0),(1,0),(2,0),(3,0)
        jeu.TetrinoCourant.PositionOrigine = new Position(2, 5);
        // Occupe la case à gauche du tetrino
        jeu.Grille[1, 5] = (int)TetrinoCouleur.blue;

        int xAvant = jeu.TetrinoCourant.PositionOrigine.X;
        jeu.Gauche();
        // Le tetrino ne doit pas avoir bougé
        Assert.Equal(xAvant, jeu.TetrinoCourant.PositionOrigine.X);
    }

    /**
    * @test TestCollisionDroiteAvecGrille
    * @brief Vérifie que le tetrino ne peut pas se déplacer à droite si la case est occupée.
    * @author Zakaria
    */
    [Fact]
    public void TestCollisionDroiteAvecGrille()
    {
        var jeu = new JeuTetris();
        jeu.Demarrer();
        // Place le tetrino barre horizontale (indice 1) : (0,0),(1,0),(2,0),(3,0)
        jeu.TetrinoCourant.Indice = 1;
        jeu.TetrinoCourant.PositionOrigine = new Position(0, 5);
        // Occupe la case à droite du tetrino (x=4)
        jeu.Grille[4, 5] = (int)TetrinoCouleur.red;

        int xAvant = jeu.TetrinoCourant.PositionOrigine.X;
        jeu.Droite();
        // Le tetrino ne doit pas avoir bougé
        Assert.Equal(xAvant, jeu.TetrinoCourant.PositionOrigine.X);
    }
}