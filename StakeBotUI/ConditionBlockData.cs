using System;

namespace StakeBotUI
{
    [Serializable]
    public class ConditionBlockData
    {
        public string Id          { get; set; } = Guid.NewGuid().ToString("N").Substring(0, 8);
        public string Type        { get; set; } = "bets";           // "bets" | "profit"

        // ON side
        public string OnType      { get; set; } = "everyStreakOf";  // every, everyStreakOf, firstStreakOf, streakGreaterThan, streakLowerThan, greaterThan, greaterThanOrEqualTo, lowerThan, lowerThanOrEqualTo
        public decimal OnValue    { get; set; } = 1;
        public string BetType     { get; set; } = "lose";           // bet | win | lose
        public string ProfitType  { get; set; } = "profit";         // balance | loss | profit

        // DO side
        public string DoType      { get; set; } = "increaseByPercentage"; // see execute() in condition-builder.js
        public decimal DoValue    { get; set; } = 100;
    }
}
