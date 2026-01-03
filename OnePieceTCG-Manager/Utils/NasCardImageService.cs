using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace OnePieceTCG_Manager.Utils
{
    public static class NasCardImageService
    {
        private static readonly string NasRoot = @"\\192.168.1.50\OnePieceTCG";

        public static List<string> GetImagesByCardId(string cardId)
        {
            if (string.IsNullOrWhiteSpace(cardId))
                return new List<string>();

            string expansion = cardId.Split('-')[0];
            string expansionPath = Path.Combine(NasRoot, expansion);

            if (!Directory.Exists(expansionPath))
                return new List<string>();

            return Directory.GetFiles(expansionPath, $"{cardId}*.png")
                            .OrderBy(f => f)
                            .ToList();
        }
    }
}
