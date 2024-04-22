using System;
using System.Collections.Generic;

namespace SeaDog
{
    public class MB : IPlayer
    {
        private readonly string _name;

        public MB(string name)
        {
            _name = name;
        }

        public string Name => _name;

        public Move Move(Board board)
        {
            List<Boat> playerBoats = board.Boats(_name);
            Boat boat = playerBoats[0];
            List<Location> fish = board.Fish();

            // Matrice de 199x199 donnant les coefficients d'influence d'un poisson tout autour de lui
            double[,] poids = new double[199, 199];                  // Cette matrice pourrait n'être calculée qu'une fois, mais il est demandé de ne coder que la stratégie, 
            for (int i = 0; i < 199; i++)
            {                               // je l'ai donc laissée dans la fonction move
                for (int j = 0; j < 199; j++)
                {
                    if (Math.Abs(i - 99) > Math.Abs(j - 99))
                    {
                        poids[i, j] = 1 / Math.Pow(2, Math.Abs(i - 99));
                    }
                    else
                    {
                        poids[i, j] = 1 / Math.Pow(2, Math.Abs(j - 99));
                    }
                }
            }

            double[,] heatmap = new double[100, 100];                   // Matrice donnant la position des poissons et d'une "zone d'influence". Plus le poissons est proche 
            for (int i = 0; i < 100; i++)
            {                                  // de la case considérée, plus son influence sur sa valeur est importante
                for (int j = 0; j < 100; j++)
                {

                    heatmap[i, j] = 0;
                }
            }

            int xb = boat.Location.Row;
            int yb = boat.Location.Column;


            for (int fish_ind = 0; fish_ind < fish.Count; fish_ind++)
            {
                int xf = fish[fish_ind].Row;
                int yf = fish[fish_ind].Column;


                for (int i = xb - 1; i < xb + 2; i++)
                {                           // On ne remplit la heatmap que pour les cases adjacentes au bateau
                    for (int j = yb - 1; j < yb + 2; j++)
                    {
                        if (!((i > 99) || (i < 0) || (j > 99) || (j < 0)))
                        {
                            heatmap[i, j] += poids[99 + (i - xf), 99 + (j - yf)];
                        }
                    }
                }
            }


            Move move = new Move(boat.Id, 0, 0);


            double[] cases_adj = new double[] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };                   // On récupère les valeurs des cases adjacentes au bateau

            for (int i = xb - 1; i < xb + 2; i++)
            {
                for (int j = yb - 1; j < yb + 2; j++)
                {
                    if ((i < 100) && (i >= 0) && (j < 100) && (j >= 0))
                    {


                        cases_adj[(j + 1 - yb) + 3 * (i + 1 - xb)] = heatmap[i, j];
                    }
                }
            }


            int max_ind = 0;
            for (int i = 1; i < 9; i++)
            {
                if ((cases_adj[i] > cases_adj[max_ind]) && (i != 4))
                {
                    max_ind = i;
                }
            }


            if (max_ind == 0)                                           // Le bateau se dirige vers la case à plus forte valeur
            {
                move = new Move(boat.Id, -1, -1);
            }
            else if (max_ind == 1)
            {
                move = new Move(boat.Id, -1, 0);
            }
            else if (max_ind == 2)
            {
                move = new Move(boat.Id, -1, +1);
            }
            else if (max_ind == 3)
            {
                move = new Move(boat.Id, 0, -1);
            }
            else if (max_ind == 5)
            {
                move = new Move(boat.Id, 0, +1);
            }
            else if (max_ind == 6)
            {
                move = new Move(boat.Id, +1, -1);
            }
            else if (max_ind == 7)
            {
                move = new Move(boat.Id, +1, 0);
            }
            else
            {
                move = new Move(boat.Id, +1, +1);
            }
            return move;
        }

        public IPlayer Clone() => new MB(_name);
    }
}
