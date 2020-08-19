using System.Collections.Generic;

namespace HandwritingLibrary
{
    public class MatchCollector
    {
        public List<CharacterMatch> matches = new List<CharacterMatch>();
        public readonly int limit;
        public int count = 0;

        public MatchCollector(int limit)
        {
            this.limit = limit;
            for (int i = 0; i < limit; i++)
            {
                matches.Add(null);
            }
        }

        public int findSlot(double score)
        {
            int ix;
            for (ix = 0; ix < count; ix++)
            {
                if (matches[ix].Score < score)
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
            for (var i = ix; i < matches.Count - 1; ++i)
            {
                matches[i] = matches[i + 1];
            }
            count--;
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
            for (int i = matches.Count - 1; i > pos; i--)
            {
                matches[i] = matches[i - 1];
            }
            if (count < matches.Count)
            {
                count++;
            }
            matches[pos] = currentMatch;
        }
    }
}
