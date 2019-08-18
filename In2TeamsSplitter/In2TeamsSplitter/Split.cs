using In2TeamsSplitter.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace In2TeamsSplitter
{
    public class Split
    {
        public class Player
        {
            public string Name { get; set; }
            public long Rating { get; set; }
        }

        private static int _TotalPlayers;
        private static Player[] _Players;

        private static bool FindSwappables(Player[] team1, Player[] team2, long diff, out int player1, out int player2)
        {
            player1 = 0;
            player2 = 0;
            long? minDiff = null;//trying to obtain the minimum distance between the swap effect and the current difference
            long halfDiff = Convert.ToInt64(Math.Abs(Math.Ceiling((double)diff / 2)));
            long swapEffect = 0; //effect of swapping items between the 2 teams
            //shortcut evaluation
            if (diff > 0 && team1[team1.Length - 1].Rating - team2[0].Rating <= diff)
            {
                player1 = team1.Length - 1;
                player2 = 0;
                return true;
            }
            if (diff < 0 && team2[team2.Length - 1].Rating - team1[0].Rating <= diff)
            {
                player1 = 0;
                player2 = team2.Length - 1;
                return true;
            }

            for (int i = 0; i < team1.Length && swapEffect != diff; i++)
            {
                long rating = team1[i].Rating;
                int? foundIndex = null;
                if (diff > 0) //S1 > S2
                    foundIndex = FindPlayerToSwapWith(team2, 0, team2.Length - 1, rating - (halfDiff > rating ? 0 : halfDiff), Math.Max(rating - halfDiff * 2, 0), rating);
                else
                    foundIndex = FindPlayerToSwapWith(team2, 0, team2.Length - 1, rating + halfDiff, rating, rating + halfDiff * 2);

                if (foundIndex.HasValue)
                {
                    swapEffect = team1[i].Rating - team2[foundIndex.Value].Rating;
                    swapEffect = Math.Abs(swapEffect);
                    if (swapEffect != 0 && (!minDiff.HasValue || Math.Abs(diff - swapEffect) < minDiff))
                    {
                        minDiff = diff - swapEffect;
                        player1 = i;
                        player2 = foundIndex.Value;
                    }
                }
            }
            return minDiff != null;
        }

        private static int FindInsertIndex(Player[] list, int start, int end, long value)
        {
            int diff = end - start;
            if (value <= list[start].Rating) return start;// V-----S-----------E
            if (value >= list[end].Rating) return end;//       S-----------E-----V
            if (diff == 1)                                //       S-----V-----E
                return end;

            if (list[start + diff / 2].Rating > value)
                end = start + diff / 2;
            else
                start = start + diff / 2;
            return FindInsertIndex(list, start, end, value);
        }

        private static int? FindPlayerToSwapWith(Player[] list, int start, int end, long value, long lowerBound, long upperBound)
        {
            if (upperBound < list[start].Rating || lowerBound > list[end].Rating)
                return null;
            if (value <= list[start].Rating) return start;
            if (value >= list[end].Rating) return end;
            int diff = end - start;
            //termination condition
            if (diff == 1)//choose between the only 2 players in this range
                return MakeChoice(list, start, end, value, lowerBound, upperBound);
            if (list[start + diff / 2].Rating > value)
                end = start + diff / 2;
            else
                start = start + diff / 2;
            return FindPlayerToSwapWith(list, start, end, value, lowerBound, upperBound);
        }

        private static int? MakeChoice(Player[] list, int start, int end, long value, long lowerBound, long upperBound)
        {
            Player ps = list[start], pe = list[end];
            bool startValid = IsValid(ps, lowerBound, upperBound), endValid = IsValid(pe, lowerBound, upperBound);
            if (!startValid)
            {
                if (endValid)
                    return end;
                return null;
            }
            if (!endValid)
                return start;
            bool endIsCloser = Math.Abs(pe.Rating - value) < Math.Abs(ps.Rating - value);
            if (endIsCloser)
                return end;
            return start;
        }

        private static bool IsValid(Player p, long lowerBound, long upperBound)
        {
            return p.Rating >= lowerBound && p.Rating <= upperBound;
        }

        private static void Cut(out Player[] team1, out Player[] team2)
        {
            team1 = new Player[_TotalPlayers / 2];
            team2 = new Player[_TotalPlayers / 2];
            for (int i = 0; i < _Players.Length / 2; i++)
                team1[i] = _Players[i];
            for (int i = _Players.Length / 2, counter = 0; i < _Players.Length; i++, counter++)
                team2[counter] = _Players[i];
            team1 = team1.OrderBy(x => x.Rating).ToArray();
            team2 = team2.OrderBy(x => x.Rating).ToArray();
        }

        private static void Swap(Player[] team1, Player[] team2, int team1Index, int team2Index, ref long sum1, ref long sum2)
        {
            Player p1 = team1[team1Index];
            Player p2 = team2[team2Index];
            int insertIndex = CorrectInsertIndex(team2, team2Index, p1, FindInsertIndex(team2, 0, team2.Length - 1, p1.Rating));

            ShiftPlayers(team2, team2Index, insertIndex);
            team2[insertIndex] = p1;

            insertIndex = FindInsertIndex(team1, 0, team1.Length - 1, p2.Rating);
            insertIndex = CorrectInsertIndex(team1, team1Index, p2, insertIndex);

            ShiftPlayers(team1, team1Index, insertIndex);
            team1[insertIndex] = p2;

            sum1 += p2.Rating - p1.Rating;
            sum2 += p1.Rating - p2.Rating;
        }

        private static int CorrectInsertIndex(Player[] team2, int team2Index, Player p1, int insertIndex)
        {
            if (insertIndex == team2Index) return insertIndex;

            if (insertIndex < team2Index)
            {
                if (insertIndex < team2.Length - 1 && team2[insertIndex].Rating < p1.Rating)
                    insertIndex++;
            }
            else
            {
                if (insertIndex > 0 && team2[insertIndex].Rating > p1.Rating)
                    insertIndex--;
            }
            return insertIndex;
        }

        private static void ShiftPlayers(Player[] list, int removeFrom, int insertAt)
        {
            if (insertAt <= removeFrom)
                for (int i = removeFrom; i > insertAt; i--)
                    list[i] = list[i - 1];
            else
                for (int i = removeFrom; i < insertAt; i++)
                    list[i] = list[i + 1];
        }

        private static void In2TeamsSplit(out Player[] team1, out Player[] team2)
        {
            //_Players = Fold(_Players);
            Cut(out team1, out team2);

            var sum1 = team1.Sum(x => x.Rating);
            var sum2 = team2.Sum(x => x.Rating);

            long diff = sum1 - sum2;
            var startDiff = diff;
            double percentageComplete = 0, previousPercentage = 0;
            Console.WriteLine();
            Console.Write("\r0 %");
            while (diff != 0 && FindSwappables(team1, team2, diff, out int s1, out int s2))
            {
                Swap(team1, team2, s1, s2, ref sum1, ref sum2);
                diff = sum1 - sum2;

                percentageComplete = (1 - (double)diff / (double)startDiff) * 100;
                if (percentageComplete - previousPercentage >= 1)
                {
                    previousPercentage = percentageComplete;
                    Console.Write(string.Format("\r{0:f2}%", percentageComplete));
                }
            }
            Console.WriteLine("\r100 %   ");
        }

        public static void Splitter(ObservableCollection<TeamMateItem> Squad)
        {
            var tempList = new List<TeamMateItem>();
            if (Squad.Count % 2 != 0)
            {
                _TotalPlayers = Squad.Count - 1;
                tempList = Squad.OrderByDescending(x => x.Level).ToList();
            }
            else
            {
                _TotalPlayers = Squad.Count;
                tempList = Squad.ToList();
            }

            _Players = new Player[_TotalPlayers];


            for (int i = 0; i < _TotalPlayers; i++)
            {
                _Players[i] = new Player { Name = tempList[i].Name, Rating = tempList[i].Level };
            }

            In2TeamsSplit(out Player[] team1, out Player[] team2);


            if (Squad.Count % 2 != 0)
            {
                if (team1.Sum(x => x.Rating) > team2.Sum(x => x.Rating))
                {
                    Array.Resize(ref team2, team2.Length + 1);
                    team2[team2.Length - 1] = new Player { Name = tempList.Last().Name, Rating = tempList.Last().Level };
                }
                else
                {
                    Array.Resize(ref team1, team1.Length + 1);
                    team1[team1.Length - 1] = new Player { Name = tempList.Last().Name, Rating = tempList.Last().Level };
                }
            }

            Application.Current.MainPage = new ResultPage(team1, team2);
        }
    }
}
