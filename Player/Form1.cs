using SeaDog;
using System;
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
            logRtb.AppendText($"Random vs Chaser{Environment.NewLine}");
            RandomWalk rw = new RandomWalk("RW");
            Fisherman fm = new Fisherman("FM");
            string result = RunMatch(rw, fm, n);
            logRtb.AppendText($"{result} {Environment.NewLine}");
        }

        private string RunMatch(IPlayer player1, IPlayer player2, int numberOfRuns)
        {
            int[] results = new int[numberOfRuns];

            Parallel.For(0, numberOfRuns, r =>
            {
                IPlayer p1 = player1.Clone(),
                    p2 = player2.Clone();
                Master master = new Master(p1, p2, 50, 10);
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
