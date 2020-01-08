using System.Collections.Generic;
using System.Text.RegularExpressions;
using CommonInterfaces.Pacman;

namespace Server {
    internal static class CoinFactory {
        public static List<Coin> GetCoins(int coinSize, int gridSize) {
            // Matches pairs "(x, y)"
            var coordRegex = new Regex(@"\((?<x>\d+),\s*(?<y>\d+)\)");
            var coinResources = Properties.Resources.Coins;
            var matchCollection = coordRegex.Matches(coinResources);
            var coinList = new List<Coin>();

            int i = 1;
            foreach (Match match in matchCollection) {
                var x = int.Parse(match.Groups["x"].Value);
                var y = int.Parse(match.Groups["y"].Value);
                var coin = new Coin($"coin{i++}", coinSize);
                coin.SetGridPosition(gridSize, x, y);
                coinList.Add(coin);
            }
            return coinList;
        }
    }
}
