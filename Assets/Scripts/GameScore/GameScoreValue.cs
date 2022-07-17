namespace GameScore
{
    public class GameScoreValue
    {
        public int invasions { get; }
        public int bpm { get; }

        public GameScoreValue(int invasionsDefeated, int bpmValue)
        {
            invasions = invasionsDefeated;
            bpm = bpmValue;
        }
    }
}