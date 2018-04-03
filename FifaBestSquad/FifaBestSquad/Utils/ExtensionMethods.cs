using System;
using System.Collections.Generic;
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
    }
}
