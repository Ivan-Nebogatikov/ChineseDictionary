using System;
using System.Collections.Generic;
using System.Text;

namespace HandwritingLibrary
{
    public class CharacterMatch
    {
		public readonly char Character;
		public readonly double Score;

		public CharacterMatch(char character, double score)
		{
			Character = character;
			Score = score;
		}
	
	}
}
