using System;
using System.Collections.Generic;

namespace CSP
{
    public class CspInstance
    {
        public CspInstance()
        {
        }
        public ISet<Variable> Variables { get; } = new HashSet<Variable>();

        private readonly HashSet<Restriction> restrictions = new();
        public IReadOnlySet<Restriction> Restrictions { get => restrictions; }

        private readonly List<Pair> result = new();
        public IReadOnlyList<Pair> Result { get => result; }

        public void AddToResult(Pair pair)
        {
            foreach (var restrictionPair in pair.Color.Restrictions)
            {
                RemoveRestriction(pair, restrictionPair);
                foreach (var pair2 in restrictionPair.Color.Restrictions)
                {
                    RemoveRestriction(pair2, restrictionPair);
                }
                restrictionPair.Variable.AvalibleColors.Remove(restrictionPair.Color);
            }
            foreach (var color in pair.Variable.AvalibleColors)
            {
                foreach (var restrictionPair in color.Restrictions)
                {
                    RemoveRestriction(new Pair(pair.Variable, color), restrictionPair);
                }
            }
            Variables.Remove(pair.Variable);
            result.Add(pair);
        }
        public void AddToResult(Variable variable, Color color) => AddToResult(new Pair(variable, color));

        public void AddRestriction(Restriction restriction)
        {
            if (!restrictions.Contains(restriction))
            {
                restrictions.Add(restriction);
                restriction.Pair1.Color.AddRestriction(restriction.Pair2);
                restriction.Pair2.Color.AddRestriction(restriction.Pair1);
            }
        }
        public void AddRestriction(Pair pair1, Pair pair2) => AddRestriction(new Restriction(pair1, pair2));


        public void RemoveRestriction(Restriction restriction)
        {
            if (restrictions.Contains(restriction))
            {
                restrictions.Remove(restriction);
                restriction.Pair1.Color.RemoveRestriction(restriction.Pair2);
                restriction.Pair2.Color.RemoveRestriction(restriction.Pair1);
            }
        }
        public void RemoveRestriction(Pair pair1, Pair pair2) => RemoveRestriction(new Restriction(pair1, pair2));

        public CspInstance Clone()
        {
            var cloned = new CspInstance();
            var variableDict = new Dictionary<int, Variable>();
            foreach (var variable in this.Variables)
            {
                var clonedVariable = new Variable(variable.AvalibleColors.Count)
                {
                    Id = variable.Id
                };
                cloned.Variables.Add(clonedVariable);
                variableDict.Add(clonedVariable.Id, clonedVariable);
                for (int i = 0; i < variable.AvalibleColors.Count; i++)
                {
                    clonedVariable.AvalibleColors[i].Value = variable.AvalibleColors[i].Value;
                }
            }

            foreach (var restriction in this.restrictions)
            {
                var clonedVariable1 = variableDict[restriction.Pair1.Variable.Id];
                var clonedVariable2 = variableDict[restriction.Pair2.Variable.Id];
                var color1 = clonedVariable1.AvalibleColors.Find(c => c.Value == restriction.Pair1.Color.Value);
                var color2 = clonedVariable2.AvalibleColors.Find(c => c.Value == restriction.Pair2.Color.Value);
                cloned.AddRestriction(new Pair(clonedVariable1, color1), new Pair(clonedVariable2, color2));
            }

            foreach (var pair in this.result)
            {
                var clonedVariable = new Variable(pair.Variable.AvalibleColors.Count)
                {
                    Id = pair.Variable.Id
                };
                for (int i = 0; i < pair.Variable.AvalibleColors.Count; i++)
                {
                    clonedVariable.AvalibleColors[i].Value = pair.Variable.AvalibleColors[i].Value;
                }
                var color = clonedVariable.AvalibleColors.Find(c => c.Value == pair.Color.Value);
                cloned.result.Add(new Pair(clonedVariable, color)); // dont use AddResult, becouse variable is not in Variable set
            }

            return cloned;
        }
    }
}
