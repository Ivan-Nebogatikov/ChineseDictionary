using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml.Schema;

namespace HandwritingLibrary
{
    public class MatchCollector
    {
       private List<CharacterMatch> matches = new List<CharacterMatch>();

        public readonly int limit;
        public static int count = 0;

        public MatchCollector(int limit)
        {
            this.limit = limit;
        }

        public int findSlot(double score)
        {
            int ix;
            for (ix = 0; ix < count; ix++)
            {
                if (matches[ix].Character < score)
                {
                    return ix;
                }

            }
            return ix;
        }

        public bool removeExistingLower(CharacterMatch currentMatch)
        {
            var ix = -1;
            for (var i = 0; i != count; i++)
            {
                if (matches[i].Character == currentMatch.Character)
                {
                    ix = i;
                    break;
                }
            }
            if (ix == -1)
            {
                return false;

            }
            if (currentMatch.Score <= matches[ix].Score)
            {
                return true;
            }
            for (var i = ix; i < matches.Count - 1; i++)
            {
                matches[i] = matches[i + 1];
                count--;
            }
            return false;
        }

        public void AddMatch(CharacterMatch currentMatch)
        {
            if (count == matches.Count && currentMatch.Score <= matches[matches.Count - 1].Score)
            {
                return;
            }
            if (removeExistingLower(currentMatch))
            {
                return;
            }
            var pos = findSlot(currentMatch.Score);

            for (int i = matches.Count - 1; i > (int)pos; i--)
            {
                matches[i] = matches[i - 1];
                matches[(int)pos] = currentMatch;
            }
            if (count < matches.Count)
            {
                count++;
            }
            
        }

        public char [] getMatches()
        {
            char[] result = new char[matches.Count];
            for (int i = 0; i != matches.Count; i++)
            {
                result[i] = matches[i].Character;
               
            }
            return result;
        }
     

    }
}
        








