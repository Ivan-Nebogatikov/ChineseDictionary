using System;
using System.Collections.Generic;
using System.Text;

namespace HandwritingLibrary
{
    class CharacterMatch
    {
		public readonly char Character;
		public readonly double Score;

		public CharacterMatch(char character, double score)
		{
			this.Character = character;
			this.Score = score;
		}
	
	}
}
