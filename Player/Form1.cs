using SeaDog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Player
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int n = 100;
            List<IPlayer> players = new List<IPlayer>()
            {
                new RandomWalk("Random"),
                new Fisherman("Qobalt"),
                new Naive("Naive"),
                new KH("Kristy"),
                new MB("MB")
            };

            for (int i = 0; i < players.Count - 1; i++)
                for (int j = i + 1; j < players.Count; j++)
                {
                    logRtb.AppendText($"{players[i].Name} vs {players[j].Name}{Environment.NewLine}{RunMatch(players[i], players[j], n)}{Environment.NewLine}");
                    Update();
                }
        }

        private string RunMatch(IPlayer player1, IPlayer player2, int numberOfRuns)
        {
            int[] results = new int[numberOfRuns];

            Parallel.For(0, numberOfRuns, r =>
            {
                IPlayer p1 = player1.Clone(),
                    p2 = player2.Clone();
                Master master = new Master(p1, p2, 50, 1);
                Log log = new Log();
                master.Run(log);
                results[r] = master.Winner == "Draw" ? 0 : (master.Winner == p1.Name ? 1 : 2);
            });

            int p1Victories = results.Count(i => i == 1),
                p2Victories = results.Count(i => i == 2);

            if (p1Victories == p2Victories)
                return "Draw";

            return p1Victories > p2Victories ? $"{player1.Name} : {p1Victories * 1.0 / (p1Victories + p2Victories):P2} " :
                $"{player2.Name} : {p2Victories * 1.0 / (p1Victories + p2Victories):P2} ";
        }
    }
}
