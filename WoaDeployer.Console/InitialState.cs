using System;
using System.Collections.Generic;
using System.Linq;

namespace Deployer.Console
{
    public static class InitialState
    {
        public static Dictionary<string, object> Create(IEnumerable<string> assignments, IEnumerable<Requirement> requirements)
        {
            var dict = ConvertToDictionary(assignments);

            var associate = Associate(dict, requirements.SelectMany(r => r.Items));

            return associate;
        }

        private static Dictionary<string, object> Associate(Dictionary<string, string> dict, IEnumerable<RequirementItem> requirements)
        {
            return dict
                .Join(requirements, a => a.Key, r => r.Name,
                    (tuple, requirement) => new { requirement.Name, Value = GetValue(requirement, tuple.Value) })
                .ToDictionary(a => a.Name, a => a.Value);
        }

        private static object GetValue(RequirementItem requirementItem, string strValue)
        {
            switch (requirementItem)
            {
                case DoubleRequirementItem:
                    return double.Parse(strValue);
                case IntRequirementItem:
                    return int.Parse(strValue);
                case StringRequirementItem:
                    return strValue;
                default:
                    throw new ArgumentOutOfRangeException(nameof(requirementItem));
            }
        }

        private static Dictionary<string, string> ConvertToDictionary(IEnumerable<string> set)
        {
            return set.Select(s =>
            {
                var strings = s.Split("=");
                return new { Key = strings[0], Value = strings[1] };
            }).ToDictionary(x => x.Key, x => x.Value);
        }
    }
}