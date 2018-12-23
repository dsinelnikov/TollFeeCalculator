namespace TollFeeCalculator.Core.Rules
{
    public static class RuleExtensions
    {
        public static Rule AddRule(this Rule rule, Rule nextRule)
        {
            rule.SetNextRule(nextRule);
            return nextRule;
        }
    }
}
