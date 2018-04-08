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
    }
}
