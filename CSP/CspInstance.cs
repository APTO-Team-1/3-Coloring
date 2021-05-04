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

        #region result
        public void AddToResult(Pair pair)
        {
            foreach (var restrictionPair in pair.Color.Restrictions)
            {
                RemoveRestriction(pair, restrictionPair);
                foreach (var pair2 in restrictionPair.Color.Restrictions)
                {
                    RemoveRestriction(pair2, restrictionPair);
                }
                RemoveColor(restrictionPair.Variable, restrictionPair.Color);
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
        #endregion

        #region restriction
        public void AddRestriction(Restriction restriction)
        {
            if (restriction.Pair1.Variable == restriction.Pair2.Variable) return;
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
        #endregion

        #region color
        public void AddColor(Pair pair)
        {
            foreach (var restriction in pair.Color.Restrictions)
            {
                AddRestriction(pair, restriction);
            }
            pair.Variable.avalibleColors.Add(pair.Color);
        }
        public void AddColor(Variable variable, Color color) => AddColor(new Pair(variable, color));

        public void RemoveColor(Pair pair)
        {
            foreach (var restriction in pair.Color.Restrictions)
            {
                RemoveRestriction(pair, restriction);
            }
            pair.Variable.avalibleColors.Remove(pair.Color);
        }
        public void RemoveColor(Variable variable, Color color) => RemoveColor(new Pair(variable, color));

        #endregion

        #region variable
        public void AddVariable(IEnumerable<Color> variableColors)
        {
            var v = new Variable(variableColors);
            Variables.Add(v);
            foreach (var c in variableColors)
            {
                AddColor(new Pair(v, c));
            }
            
        }

        public void RemoveVariable(Variable v)
        {
            foreach (var c in v.AvalibleColors)
            {
                foreach (var restriction in c.Restrictions)
                {
                    RemoveRestriction(new Pair(v, c), restriction);
                }
            }
            Variables.Remove(v);
        }

        #endregion
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

        /// <summary>
        /// Clones CspInstance and returns variable and color corresponding to <paramref name="v"/> and <paramref name="c"/> in the copy created. If there's no corresponding variable and color returns null.
        /// </summary>
        /// <returns></returns>
        public (CspInstance instance, Variable v, Color c) CloneAndReturnCorresponding(Variable v, Color c)
        {
            var res =  CloneAndReturnCorresponding(new Variable[] { v }, new Color[] { c });
            return (res.instance, res.vArr[0], res.cArr[0]);
        }
            

        public (CspInstance instance, Variable[] vArr, Color[] cArr) CloneAndReturnCorresponding(Variable[] vArr, Color[] cArr)
        {
            Variable[] correspondingVs = new Variable[vArr.Length];
            Color[] correspondingCs = new Color[cArr.Length];

            var cloned = new CspInstance();
            var variableDict = new Dictionary<int, Variable>();
            foreach (var variable in this.Variables)
            {
                var clonedVariable = new Variable(variable.AvalibleColors.Count)
                {
                    Id = variable.Id
                };

                var idxV = Array.IndexOf(vArr, variable);
                if (idxV!=-1)
                    correspondingVs[idxV] = clonedVariable;

                cloned.Variables.Add(clonedVariable);
                variableDict.Add(clonedVariable.Id, clonedVariable);
                for (int i = 0; i < variable.AvalibleColors.Count; i++)
                {
                    clonedVariable.AvalibleColors[i].Value = variable.AvalibleColors[i].Value;

                    var idxC = Array.IndexOf(cArr, variable.AvalibleColors[i]);
                    if (idxC != -1)
                        correspondingCs[idxC] = clonedVariable.AvalibleColors[i];
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

            return (cloned, correspondingVs, correspondingCs);
        }
    }
}
