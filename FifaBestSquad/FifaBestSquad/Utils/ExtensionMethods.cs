using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FifaBestSquad.Utils
{
    public static class ExtensionMethods
    {
        public static bool IsGreen(this Player player, Player secondPlayer)
        {
            bool isGreen = ((player.Club == secondPlayer.Club) || (player.Nation == secondPlayer.Nation && player.League == secondPlayer.League));

            return isGreen; 
        }
        public static bool IsAnyGreen(this Player player, IEnumerable<Player> secondPlayers)
        {
            if(secondPlayers == null || player == null)
            {
                return false;
            }
            if (!secondPlayers.Any())
            {
                return true;
            }
            foreach (var secondPlayer in secondPlayers)
            {
                if(!(player.Club == secondPlayer.Club || (player.Nation == secondPlayer.Nation && player.League == secondPlayer.League)))
                {
                    return false;
                }

            }

            return true;
        }

        private static Random rng = new Random();

        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
