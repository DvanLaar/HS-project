using System;
using System.Collections.Generic;
using System.Linq;

namespace TourneyPredict
{
    class Program
    {
        static void Main()
        {
            List<string> decks = new List<string>(new[] { "big druid", "malygos druid", "taunt druid", "token druid", "deathrattle hunter", "secret hunter", "spell hunter", "tempo mage", "control priest", "odd rogue", "quest rogue", "shudderwock shaman", "controllock", "cubelock", "evenlock", "zoolock", "odd warrior"});
            double[][] winrates = {
                new[] {50.00, 59.61, 50.86, 62.79, 39.31, 57.51, 58.39, 39.59, 62.42, 51.42, 44.57, 60.78, 59.78, 52.69, 52.86, 46.92, 38.68},
                new[] {39.97, 50.00, 41.31, 47.01, 40.99, 37.95, 51.64, 49.65, 72.93, 60.46, 46.60, 66.33, 56.75, 53.16, 46.99, 40.16, 47.41},
                new[] {49.13, 57.95, 50.00, 59.69, 42.00, 57.20, 58.33, 46.02, 59.92, 51.23, 40.33, 46.27, 69.78, 59.87, 56.13, 44.10, 60.06},
                new[] {36.98, 52.62, 40.17, 50.00, 51.98, 51.25, 58.42, 44.96, 51.37, 57.41, 54.46, 70.07, 43.34, 40.03, 38.19, 43.53, 58.76},
                new[] {60.61, 58.70, 57.94, 47.88, 50.00, 45.91, 51.52, 55.70, 52.68, 43.07, 38.81, 61.96, 72.26, 56.12, 54.10, 45.03, 63.85},
                new[] {42.48, 61.85, 42.78, 48.58, 54.03, 50.00, 55.79, 62.42, 51.11, 53.78, 62.24, 48.60, 55.10, 55.99, 51.00, 54.39, 31.31},
                new[] {41.60, 47.94, 41.61, 41.47, 48.42, 44.10, 50.00, 62.73, 47.53, 54.27, 52.96, 45.33, 56.13, 47.21, 49.51, 53.17, 33.79},
                new[] {60.40, 50.28, 53.90, 55.01, 44.26, 37.52, 37.21, 50.00, 54.54, 36.00, 90.64, 61.21, 74.77, 69.50, 60.61, 43.89, 28.81},
                new[] {37.57, 26.84, 39.92, 48.52, 47.23, 48.88, 52.10, 45.39, 50.00, 43.54, 34.47, 50.67, 62.58, 62.34, 56.25, 57.82, 36.71},
                new[] {48.57, 39.44, 48.71, 42.51, 56.90, 46.17, 45.69, 63.92, 56.39, 50.00, 67.36, 45.65, 55.47, 62.41, 56.25, 57.66, 23.77},
                new[] {54.96, 53.10, 59.55, 45.46, 61.13, 37.67, 46.99, 09.32, 65.52, 32.59, 50.00, 70.34, 60.24, 48.03, 56.70, 24.36, 78.75},
                new[] {39.21, 33.39, 53.61, 29.86, 37.97, 51.25, 54.55, 38.70, 49.32, 54.28, 29.61, 50.00, 46.39, 44.48, 45.21, 45.17, 76.06},
                new[] {40.21, 42.85, 30.04, 56.55, 27.66, 44.75, 43.77, 25.16, 37.08, 44.46, 39.38, 53.45, 50.00, 39.76, 29.65, 58.92, 55.89},
                new[] {47.30, 46.55, 39.93, 59.83, 43.69, 43.94, 52.71, 30.40, 37.65, 37.43, 51.96, 55.32, 60.11, 50.00, 42.56, 50.50, 48.31},
                new[] {46.81, 52.63, 43.77, 61.68, 45.76, 48.81, 50.29, 39.29, 43.69, 43.66, 43.25, 54.68, 70.28, 57.39, 50.00, 58.58, 48.45},
                new[] {52.91, 59.76, 55.86, 56.39, 54.92, 45.55, 46.81, 56.06, 42.17, 42.28, 75.59, 54.75, 41.05, 49.45, 41.33, 50.00, 32.55},
                new[] {61.17, 51.81, 39.84, 40.90, 36.03, 68.58, 65.98, 71.12, 62.96, 76.15, 21.10, 23.74, 43.99, 51.42, 51.46, 67.38, 50.00}
            };

            for (int i = 0; i < decks.Count; i++)
            {
                for (int j = 0; j < decks.Count; j++)
                {
                    double wina = winrates[i][j], winb = winrates[j][i];
                    winrates[i][j] = wina / (wina + winb) * 100.0;
                    winrates[j][i] = winb / (wina + winb) * 100.0;
                }
            }

            Console.WriteLine("Who is player one?");
            string p1 = Console.ReadLine();
            Console.WriteLine("What decks does he play?");
            int[] p1d = {
                decks.IndexOf(Console.ReadLine()),
                decks.IndexOf(Console.ReadLine()),
                decks.IndexOf(Console.ReadLine()),
                decks.IndexOf(Console.ReadLine())
            };

            Console.WriteLine("Who is player two?");
            string p2 = Console.ReadLine();
            Console.WriteLine("What decks does he play?");
            int[] p2d = {
                decks.IndexOf(Console.ReadLine()),
                decks.IndexOf(Console.ReadLine()),
                decks.IndexOf(Console.ReadLine()),
                decks.IndexOf(Console.ReadLine())
            };

            //Methode 1
            double[] m1P1 = new double[4];
            for (int k = 0; k < 4; k++)
            {
                int i = p1d[k];
                double avg = 0;
                foreach (int j in p2d)
                {
                    avg += winrates[i][j];
                    //Console.WriteLine("Winrate: " + winrates[i][j] + ", " + (100 - winrates[i][j]));
                }
                avg /= 4;
                m1P1[k] = avg;
            }
            double max = m1P1.Max();
            int index = -1;
            for (int k = 0; k < 4; k++)
            {
                if (m1P1[k] == max)
                    index = k;
            }

            Console.WriteLine("The banned deck for " + p1 + " is: " + decks[p1d[index]]);

            double[] m1P2 = new double[4];
            for (int k = 0; k < 4; k++)
            {
                int i = p2d[k];
                double avg = 0;
                foreach (int j in p1d)
                {
                    avg += winrates[i][j];
                    //Console.WriteLine("Winrate: " + winrates[i][j] + ", " + (100 - winrates[i][j]));
                }
                avg /= 4;
                m1P2[k] = avg;
                //Console.WriteLine("Average: " + avg);
            }
            max = m1P2.Max();
            index = -1;
            for (int k = 0; k < 4; k++)
            {
                if (m1P2[k] == max)
                    index = k;
            }
            Console.WriteLine("The banned deck for " + p2 + " is: " + decks[p2d[index]]);

            //Methode 2
            double[] m2P1 = new double[4];
            for (int k = 0; k < 4; k++)
            {
                int i = p1d[k];
                double avg = 0, mx = 100;
                foreach (int j in p2d)
                { 
                    avg += winrates[i][j];
                    if (winrates[i][j] < mx)
                        mx = winrates[i][j];
                }
                avg -= mx;
                avg /= 3;
                m2P1[k] = avg;
            }
            max = m2P1.Max();
            index = -1;
            for (int k = 0; k < 4; k++)
            {
                if (m2P1[k] == max)
                    index = k;
            }

            Console.WriteLine("The banned deck for " + p1 + " is: " + decks[p1d[index]]);

            double[] m2P2 = new double[4];
            for (int k = 0; k < 4; k++)
            {
                int i = p2d[k];
                double avg = 0, mx = 100;
                foreach (int j in p1d)
                {
                    avg += winrates[i][j];
                    if (winrates[i][j] < mx)
                        mx = winrates[i][j];
                }
                avg -= mx;
                avg /= 3;
                m2P2[k] = avg;
            }
            max = m2P2.Max();
            index = -1;
            for (int k = 0; k < 4; k++)
            {
                if (m2P2[k] == max)
                    index = k;
            }

            Console.WriteLine("The banned deck for " + p2 + " is: " + decks[p2d[index]]);
            Console.WriteLine("...");

            //Methode 3
            double[,] table = new double[4, 4];
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    table[i, j] = winrates[p1d[i]][p2d[j]];
                }
            }
            int vertban = 0, horban = 0, teller = 0;
            bool statement = true;
            while (statement)
            {
                Console.WriteLine("The banned deck for " + p1 + " is: " + decks[p1d[vertban]]);
                Console.WriteLine("The banned deck for " + p2 + " is: " + decks[p2d[horban]]);
                statement = false;
                //gegeven p1ban, wat is p2ban?
                int newban = vertban;
                double[] avgs = new double[4];
                for (int i = 0; i < 4; i++)
                {
                    double avg = 0;
                    for (int j = 0; j < 4; j++)
                    {
                        if (j == horban)
                            continue;
                        avg += table[i, j];
                    }
                    avg /= 3;
                    avgs[i] = avg;
                }
                double maxim = 0;
                for (int i = 0; i < 4; i++)
                {
                    if (avgs[i] > maxim)
                    {
                        maxim = avgs[i];
                        newban = i;
                    }
                }
                if (newban != vertban)
                    statement = true;
                vertban = newban;

                //gegeven p2ban, wat is p1ban?
                avgs = new double[4];
                for (int j = 0; j < 4; j++)
                {
                    double avg = 0;
                    for (int i = 0; i < 4; i++)
                    {
                        if (i == vertban)
                            continue;
                        avg += table[i, j];
                    }
                    avg /= 3;
                    avgs[j] = avg;
                }
                maxim = 100;
                for (int i = 0; i < 4; i++)
                {
                    if (avgs[i] < maxim)
                    {
                        maxim = avgs[i];
                        newban = i;
                    }
                }
                if (newban != horban)
                    statement = true;
                horban = newban;
                Console.WriteLine("...");
                if (teller > 16)
                    break;
                teller++;
            }

            int maxlength = 0;
            for (int i = 0; i < 4; i++)
            {
                if (decks[p2d[i]].Length > maxlength)
                    maxlength = decks[p2d[i]].Length;
            }
            string space = "";
            for (int i = 0; i < maxlength; i++)
                space += " ";
            string line = "";
            for (int i = 0; i < maxlength; i++)
                line += "--";

            Console.WriteLine(space + "|" + decks[p1d[0]] + "|" + decks[p1d[1]] + "|" + decks[p1d[2]] + "|" + decks[p1d[3]]);
            Console.WriteLine(line.Substring(0, maxlength) + "+" + line.Substring(0, decks[p1d[0]].Length) + "+" + line.Substring(0, decks[p1d[1]].Length) + "+" + line.Substring(0, decks[p1d[2]].Length) + "+" + line.Substring(0, decks[p1d[3]].Length));
            Console.WriteLine(decks[p2d[0]] + space.Substring(0, maxlength - decks[p2d[0]].Length) + "|" + table[0, 0].ToString("N2") + space.Substring(0, decks[p1d[0]].Length - 5) + "|" + table[1, 0].ToString("N2") + space.Substring(0, decks[p1d[1]].Length - 5) + "|" + table[2, 0].ToString("N2") + space.Substring(0, decks[p1d[2]].Length - 5) + "|" + table[3, 0].ToString("N2") + space.Substring(0, decks[p1d[3]].Length - 5));
            Console.WriteLine(line.Substring(0, maxlength) + "+" + line.Substring(0, decks[p1d[0]].Length) + "+" + line.Substring(0, decks[p1d[1]].Length) + "+" + line.Substring(0, decks[p1d[2]].Length) + "+" + line.Substring(0, decks[p1d[3]].Length));
            Console.WriteLine(decks[p2d[1]] + space.Substring(0, maxlength - decks[p2d[1]].Length) + "|" + table[0, 1].ToString("N2") + space.Substring(0, decks[p1d[0]].Length - 5) + "|" + table[1, 1].ToString("N2") + space.Substring(0, decks[p1d[1]].Length - 5) + "|" + table[2, 1].ToString("N2") + space.Substring(0, decks[p1d[2]].Length - 5) + "|" + table[3, 1].ToString("N2") + space.Substring(0, decks[p1d[3]].Length - 5));
            Console.WriteLine(line.Substring(0, maxlength) + "+" + line.Substring(0, decks[p1d[0]].Length) + "+" + line.Substring(0, decks[p1d[1]].Length) + "+" + line.Substring(0, decks[p1d[2]].Length) + "+" + line.Substring(0, decks[p1d[3]].Length));
            Console.WriteLine(decks[p2d[2]] + space.Substring(0, maxlength - decks[p2d[2]].Length) + "|" + table[0, 2].ToString("N2") + space.Substring(0, decks[p1d[0]].Length - 5) + "|" + table[1, 2].ToString("N2") + space.Substring(0, decks[p1d[1]].Length - 5) + "|" + table[2, 2].ToString("N2") + space.Substring(0, decks[p1d[2]].Length - 5) + "|" + table[3, 2].ToString("N2") + space.Substring(0, decks[p1d[3]].Length - 5));
            Console.WriteLine(line.Substring(0, maxlength) + "+" + line.Substring(0, decks[p1d[0]].Length) + "+" + line.Substring(0, decks[p1d[1]].Length) + "+" + line.Substring(0, decks[p1d[2]].Length) + "+" + line.Substring(0, decks[p1d[3]].Length));
            Console.WriteLine(decks[p2d[3]] + space.Substring(0, maxlength - decks[p2d[3]].Length) + "|" + table[0, 3].ToString("N2") + space.Substring(0, decks[p1d[0]].Length - 5) + "|" + table[1, 3].ToString("N2") + space.Substring(0, decks[p1d[1]].Length - 5) + "|" + table[2, 3].ToString("N2") + space.Substring(0, decks[p1d[2]].Length - 5) + "|" + table[3, 3].ToString("N2") + space.Substring(0, decks[p1d[3]].Length - 5));
            Console.ReadLine();
        }
    }
}
