using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using static AOC2020.Day22;

namespace AOC2020
{
    public class Day22 : BaseDay<Player>
    {
        private static Regex LineRegex = new Regex(@"^Player (?<player>\d):$");

        public Day22() : base("Day22", "32489", "35676")
        {
        }

        protected override Player Parse(StreamReader reader)
        {
            Match lineMatch = LineRegex.Match(reader.ReadLine());

            IList<int> deck = new List<int>();
            string card;
            while (!string.IsNullOrEmpty(card = reader.ReadLine()))
            {
                deck.Add(Int32.Parse(card));
            }

            return new Player()
            {
                Id = Int32.Parse(lineMatch.Groups["player"].Value),
                Deck = deck
            };
        }

        protected override string Solve1(Player[] items)
        {
            return PlayAndScore(items[0].Deck, items[1].Deck, part2: false).ToString();
        }

        protected override string Solve2(Player[] items)
        {
            return PlayAndScore(items[0].Deck, items[1].Deck, part2: true).ToString();
        }

        private static int PlayAndScore(IEnumerable<int> player1, IEnumerable<int> player2, bool part2)
        {
            static int HashDecks(IEnumerable<int> deck1, IEnumerable<int> deck2)
            {
                HashCode code = new HashCode();
                foreach (int card in deck1)
                    code.Add(card);
                code.Add(0);
                foreach (int card in deck2)
                    code.Add(card);
                return code.ToHashCode();
            }

            (bool, IEnumerable<int>) Play(IEnumerable<int> player1, IEnumerable<int> player2)
            {
                HashSet<int> historyHashes = new HashSet<int>();

                Queue<int> player1Deck = new Queue<int>(player1);
                Queue<int> player2Deck = new Queue<int>(player2);

                if (part2)
                {
                    historyHashes.Add(HashDecks(player1, player2));
                }

                while (player1Deck.Count > 0 && player2Deck.Count > 0)
                {
                    int card1 = player1Deck.Dequeue();
                    int card2 = player2Deck.Dequeue();

                    // Check if recursive game is needed.
                    bool player1WinsRound = false;
                    if (part2 && card1 <= player1Deck.Count && card2 <= player2Deck.Count)
                    {
                        // Only play with as many cards as the value of the drawn card.
                        (player1WinsRound, _) = Play(
                            player1Deck.Take(card1),
                            player2Deck.Take(card2));
                    }
                    else
                    {
                        player1WinsRound = card1 > card2;
                    }

                    // Add cards to winner's deck
                    if (player1WinsRound)
                    {
                        player1Deck.Enqueue(card1);
                        player1Deck.Enqueue(card2);
                    }
                    else
                    {
                        player2Deck.Enqueue(card2);
                        player2Deck.Enqueue(card1);
                    }

                    if (part2)
                    {
                        // If the decks were seen previously, then player 1 wins.
                        if (!historyHashes.Add(HashDecks(player1Deck, player2Deck)))
                        {
                            return (true, player1Deck);
                        }
                    }
                }

                if (player1Deck.Count > 0)
                {
                    return (true, player1Deck);
                }
                else
                {
                    return (false, player2Deck);
                }
            }

            return Play(player1, player2).Item2.Reverse().Select((v, i) => v * (i + 1)).Sum();
        }

        public class Player
        {
            public int Id { get; set; }

            public IEnumerable<int> Deck { get; set; }
        }
    }
}
