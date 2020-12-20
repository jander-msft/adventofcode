using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using static AOC2020.Day20;

namespace AOC2020
{
    public class Day20 : BaseDay<Tile>
    {
        private static Regex LineRegex = new Regex(@"^Tile (?<tile>\d+):$");

        public Day20() : base("Day20", "47213728755493", "1599")
        {
        }

        protected override Tile Parse(StreamReader reader)
        {
            Match lineMatch = LineRegex.Match(reader.ReadLine());

            long id = Int64.Parse(lineMatch.Groups["tile"].Value);

            IList<string> lines = new List<string>();

            string line = null;
            while (!string.IsNullOrEmpty(line = reader.ReadLine()))
            {
                lines.Add(line);
            }

            Tile tile = new Tile()
            {
                Id = id,
                Lines = lines
            };
            tile.Seal();
            return tile;
        }

        protected override string Solve1(Tile[] items)
        {
            // The product of the tile IDs of the corner tiles.
            // Corner tiles only have two matched edges.
            return Link(items)
                .Where(t => t.Matches == 2)
                .Aggregate(1L, (a, t) => a * t.Id)
                .ToString();
        }

        protected override string Solve2(Tile[] items)
        {
            IList<Tile> linked = Link(items);
            Tile upperLeft = linked.First(t => t.TileToLeft == null && t.TileToTop == null);

            // Join all tiles into a single map
            IList<string> map = new List<string>();
            for (Tile leftTile = upperLeft; leftTile != null; leftTile = leftTile.TileToBottom)
            {
                IList<StringBuilder> rowBuilders = new List<StringBuilder>();
                for (int i = 1; i < leftTile.Lines.Count - 1; i++)
                {
                    rowBuilders.Add(new StringBuilder());
                }

                for (Tile currentTile = leftTile; currentTile != null; currentTile = currentTile.TileToRight)
                {
                    // Skip first and last row
                    for (int row = 1; row < currentTile.Lines.Count - 1; row++)
                    {
                        string line = currentTile.Lines[row];
                        // Skip first and last character
                        rowBuilders[row - 1].Append(line.Substring(1, line.Length - 2));
                    }
                }

                foreach (StringBuilder builder in rowBuilders)
                {
                    map.Add(builder.ToString());
                }
            }

            // Generate list of all monster configurations
            IList<Monster> monsters = new List<Monster>();
            monsters.Add(Monster.Default);

            Monster rot1 = Monster.Default.Rotate();
            monsters.Add(rot1);

            Monster rot2 = rot1.Rotate();
            monsters.Add(rot2);

            Monster rot3 = rot2.Rotate();
            monsters.Add(rot3);

            Monster flip = rot3.Flip();
            monsters.Add(flip);

            Monster flipRot1 = flip.Rotate();
            monsters.Add(flipRot1);

            Monster flipRot2 = flipRot1.Rotate();
            monsters.Add(flipRot2);

            Monster flipRot3 = flipRot2.Rotate();
            monsters.Add(flipRot3);

            // Check the number of monsters on the map
            long monsterCount = 0;
            foreach (Monster monster in monsters)
            {
                for (int y = 0; y < (map.Count - monster.Lines.Count + 1); y++)
                {
                    for (int x = 0; x < (map[0].Length - monster.Lines[0].Length + 1); x++)
                    {
                        if (monster.Offsets.All(o => map[y + o.Y][x + o.X] == '#'))
                        {
                            monsterCount++;
                        }
                    }
                }
            }

            // Check the number of # on the map
            long chopCount = 0;
            for (int y = 0; y < map.Count; y++)
            {
                for (int x = 0; x < map[0].Length; x++)
                {
                    if (map[y][x] == '#')
                    {
                        chopCount++;
                    }
                }
            }

            // Calculate the number of # on the map that are not part of monsters
            return (chopCount - Monster.Default.Offsets.Count() * monsterCount).ToString();
        }

        private IList<Tile> Link(Tile[] tiles)
        {
            List<Tile> matches =
                new List<Tile>();

            Tile first = tiles.First();
            matches.Add(first);

            IDictionary<long, IEnumerable<Tile>> candidates = new Dictionary<long, IEnumerable<Tile>>();

            foreach (Tile tile in tiles.Skip(1))
            {
                IList<Tile> permutations = new List<Tile>();

                permutations.Add(tile);

                Tile rot1 = tile.Rotate();
                permutations.Add(rot1);

                Tile rot2 = rot1.Rotate();
                permutations.Add(rot2);

                Tile rot3 = rot2.Rotate();
                permutations.Add(rot3);

                Tile flip = tile.Flip();
                permutations.Add(flip);

                Tile flipRot1 = flip.Rotate();
                permutations.Add(flipRot1);

                Tile flipRot2 = flipRot1.Rotate();
                permutations.Add(flipRot2);

                Tile flipRot3 = flipRot2.Rotate();
                permutations.Add(flipRot3);

                candidates.Add(tile.Id, permutations);
            }

            while (candidates.Count > 0)
            {
                foreach (long tileId in candidates.Keys.ToList())
                {
                    foreach (Tile candidate in candidates[tileId])
                    {
                        if (CheckAndLink(candidate))
                        {
                            matches.Add(candidate);
                            candidates.Remove(tileId);
                            break;
                        }
                    }
                }
            }

            bool CheckAndLink(Tile current)
            {
                bool anyMatch = false;
                foreach (Tile match in matches)
                {
                    if (match.Left == current.Right)
                    {
                        match.TileToLeft = current;
                        current.TileToRight = match;
                        anyMatch = true;
                    }
                    else if (match.Right == current.Left)
                    {
                        match.TileToRight = current;
                        current.TileToLeft = match;
                        anyMatch = true;
                    }
                    else if (match.Top == current.Bottom)
                    {
                        match.TileToTop = current;
                        current.TileToBottom = match;
                        anyMatch = true;
                    }
                    else if (match.Bottom == current.Top)
                    {
                        match.TileToBottom = current;
                        current.TileToTop = match;
                        anyMatch = true;
                    }
                }

                return anyMatch;
            }

            return matches;
        }

        public class Tile
        {
            public long Id { get; set; }

            public IList<string> Lines { get; set; }

            public string Left { get; set; }

            public string Right { get; set; }

            public string Top { get; set; }

            public string Bottom { get; set; }

            public Tile TileToTop { get; set; }

            public Tile TileToBottom { get; set; }

            public Tile TileToLeft { get; set; }

            public Tile TileToRight { get; set; }

            public int Matches
            {
                get
                {
                    int matches = 0;
                    if (null != TileToTop)
                    {
                        matches++;
                    }
                    if (null != TileToBottom)
                    {
                        matches++;
                    }
                    if (null != TileToLeft)
                    {
                        matches++;
                    }
                    if (null != TileToRight)
                    {
                        matches++;
                    }
                    return matches;
                }
            }

            public void Seal()
            {
                string top = Lines[0];
                string bottom = Lines[Lines.Count - 1];

                StringBuilder leftBuilder = new StringBuilder(Lines.Count);
                StringBuilder rightBuilder = new StringBuilder(Lines.Count);
                foreach (string line in Lines)
                {
                    leftBuilder.Append(line[0]);
                    rightBuilder.Append(line[line.Length - 1]);
                }

                Left = leftBuilder.ToString();
                Right = rightBuilder.ToString();
                Top = top;
                Bottom = bottom;
            }

            public Tile Flip()
            {
                Tile tile = new Tile();
                tile.Id = Id;
                tile.Lines = Lines.Reverse().ToList();
                tile.Seal();
                return tile;
            }

            public Tile Rotate()
            {
                IList<StringBuilder> rowBuilders =
                    new List<StringBuilder>();
                for (int i = 0; i < Lines[0].Length; i++)
                    rowBuilders.Add(new StringBuilder());

                foreach (string line in Lines)
                {
                    for (int i = 0; i < line.Length; i++)
                    {
                        rowBuilders[rowBuilders.Count - 1 - i].Append(line[i]);
                    }
                }

                Tile tile = new Tile();
                tile.Id = Id;
                tile.Lines = new List<string>();
                foreach (StringBuilder builder in rowBuilders)
                    tile.Lines.Add(builder.ToString());

                tile.Seal();
                return tile;
            }

            public override string ToString()
            {
                return Id.ToString();
            }
        }

        public class Monster
        {
            private static readonly Lazy<Monster> s_default =
                new Lazy<Monster>(CreateDefault);

            public static readonly Monster Default = s_default.Value;

            public IList<string> Lines { get; set; }

            public IEnumerable<Point> Offsets { get; set; }

            private static Monster CreateDefault()
            {
                Monster monster = new Monster();

                monster.Lines = new List<string>()
                {
                    "                  # ",
                    "#    ##    ##    ###",
                    " #  #  #  #  #  #   "
                };

                monster.Seal();

                return monster;
            }

            public void Seal()
            {
                IList<Point> points = new List<Point>();
                for (int y = 0; y < Lines.Count; y++)
                {
                    for (int x = 0; x < Lines[y].Length; x++)
                    {
                        if (Lines[y][x] == '#')
                        {
                            points.Add(new Point(x, y));
                        }
                    }
                }
                Offsets = points;
            }

            public Monster Flip()
            {
                Monster monster = new Monster();
                monster.Lines = Lines.Reverse().ToList();
                monster.Seal();
                return monster;
            }

            public Monster Rotate()
            {
                IList<StringBuilder> rowBuilders =
                    new List<StringBuilder>();
                for (int i = 0; i < Lines[0].Length; i++)
                    rowBuilders.Add(new StringBuilder());

                foreach (string line in Lines)
                {
                    for (int i = 0; i < line.Length; i++)
                    {
                        rowBuilders[rowBuilders.Count - 1 - i].Append(line[i]);
                    }
                }

                Monster monster = new Monster();
                monster.Lines = new List<string>();
                foreach (StringBuilder builder in rowBuilders)
                    monster.Lines.Add(builder.ToString());

                monster.Seal();
                return monster;
            }
        }
    }
}
