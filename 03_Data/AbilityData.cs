namespace Splatoon2
{
    public class AbilityData
    {
        public readonly string name;
        public readonly string description;
        public readonly EffectPercentage effectPercentage;
    }

    public struct EffectPercentage
    {
        public readonly float inkUsage;
        public readonly float speed;
        public readonly float hp;
    }
}