using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using CommonInterfaces;

namespace Client {
    public class TracefileReader {
        private readonly string _filePath;
        public Dictionary<int, Input> Movements { get; set; } = new Dictionary<int, Input>();
        private int _currentRound = 0;

        public TracefileReader(string filePath) {
            _filePath = filePath;
        }

        public void Init() {
            string fileText = System.IO.File.ReadAllText(_filePath);
            Regex regex = new Regex(@"\s*(?<round>\d+),\s*(?<movement>(UP|DOWN|LEFT|RIGHT))");
            MatchCollection matches = regex.Matches(fileText);

            int i = 0;
            foreach (Match match in matches) {
                int round = Int32.Parse(match.Groups["round"].Value);
                string movement = match.Groups["movement"].Value;
                Input input = new Input();

                if (round != i) throw new Exception("Non-sequential round in tracefile");
                switch (movement) {
                    case "UP" :
                        input.Direction = Direction.Up;
                        break;
                    case "DOWN" :
                        input.Direction = Direction.Down;
                        break;
                    case "LEFT" :
                        input.Direction = Direction.Left;
                        break;
                    case "RIGHT" :
                        input.Direction = Direction.Right;
                        break;
                    default :
                        throw new Exception("Invalid movement in tracefile");
                }              
                Movements[round] = input;
                i++;
            }
        }

        public Input Next() {
            if (Movements.Count == 0 || _currentRound >= Movements.Count) return null;
            return Movements[_currentRound++];
        }

        public void Reset() {
            _currentRound = 0;
        }
    }
}
