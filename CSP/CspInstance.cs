using System;
using System.Collections.Generic;
using System.Linq;

namespace CSP
{
    public class CspInstance
    {
        public CspInstance()
        {
        }

        private readonly HashSet<Variable> variables = new();
        public IReadOnlySet<Variable> Variables { get => variables; } 

        private readonly HashSet<Restriction> restrictions = new();
        public IReadOnlySet<Restriction> Restrictions { get => restrictions; }

        private readonly List<Pair> result = new();
        public IReadOnlyList<Pair> Result { get => result; }

        public Stack<Action<IList<Pair>>> ResultRules { get; private set; } = new();
        public int[] GetResult()
        {
            while(ResultRules.Count > 0)
            {
                var rule = ResultRules.Pop();
                rule(this.result);
            }
            int[] coloringResult = new int[Result.Count];
            foreach (var pair in Result)
            {
                coloringResult[pair.Variable.Id] = pair.Color.Value;
            }
            return coloringResult;
        }

        #region result
        public void AddToResult(Pair pair)
        {
#if DEBUG
            if (!Variables.Any(v => v == pair.Variable)) throw new ApplicationException("Trying to use variable that is not a part of the instance");
#endif
            foreach (var restrictionPair in pair.Color.Restrictions)
            {
                RemoveRestriction(pair, restrictionPair);
                foreach (var pair2 in restrictionPair.Color.Restrictions)
                {
                    RemoveRestriction(pair2, restrictionPair);
                }
                RemoveColor(restrictionPair);
            }
            RemoveVariable(pair.Variable);
            result.Add(pair);
        }
        public void AddToResult(Variable variable, Color color) => AddToResult(new Pair(variable, color));
        #endregion

        #region restriction
        public void AddRestriction(Restriction restriction)
        {
            if(restriction.Pair1.Color == restriction.Pair2.Color)
            {
                this.RemoveColor(restriction.Pair1);
                return;
            }
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
                var removed1 = restriction.Pair1.Color.RemoveRestriction(restriction.Pair2);
                var removed2 = restriction.Pair2.Color.RemoveRestriction(restriction.Pair1);
#if DEBUG
                if ((!removed1 || !removed2)) throw new ApplicationException("Restriction not removed");
#endif
            }
            else
            {
                throw new ApplicationException("Restriction does not exist");
            }
        }
        public void RemoveRestriction(Pair pair1, Pair pair2) => RemoveRestriction(new Restriction(pair1, pair2));
        #endregion

        #region color
       
        public void AddColor(Variable variable, Color color)
        {
            variable.avalibleColors.Add(color);
            foreach (var restriction in color.Restrictions)
            {
                AddRestriction(new Pair(variable, color), restriction);
            }
        }

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
        public void AddVariableAndColorsRestrictions(List<Color> variableColors)
        {
            var v = new Variable(0);
            AddVariable(v);
            foreach (var c in variableColors)
            {
                AddColor(v, c);
            }

        }

        public void AddVariable(Variable v)
        {
            variables.Add(v);
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
            variables.Remove(v);
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
                cloned.AddVariable(clonedVariable);
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

            cloned.ResultRules = new(this.ResultRules);

            return cloned;
        }

        /// <summary>
        /// Clones CspInstance and returns variable and color corresponding to <paramref name="v"/> and <paramref name="c"/> in the copy created. If there's no corresponding variable and color returns null.
        /// </summary>
        /// <returns></returns>
        public (CspInstance instance, Variable v, Color c) CloneAndReturnCorresponding(Variable v, Color c)
        {
            var (instance, vArr, cArr) =  CloneAndReturnCorresponding(new Variable[] { v }, new Color[] { c });
            return (instance, vArr[0], cArr[0]);
        }
           
        public (CspInstance instance, Variable[] vArr, Color[] cArr) CloneAndReturnCorresponding(Variable[] vArr, Color[] cArr)
        {
            if (vArr.Length != cArr.Length)
                throw new ArgumentException("vArr.Length must be equal to cArr.Length");

            Pair[] iP = new Pair[vArr.Length];
            for (int i = 0; i < vArr.Length; i++)
            {
                iP[i] = new Pair(vArr[i], cArr[i]);
            }
            (var instance2, var i2p) = CloneAndReturnCorresponding(iP);
            Variable[] i2v = new Variable[vArr.Length];
            Color[] i2c = new Color[vArr.Length];
            for (int i = 0; i < vArr.Length; i++)
            {
                i2v[i] = i2p[i].Variable;
                i2c[i] = i2p[i].Color;
            }

            return (instance2, i2v, i2c);
        }

        public (CspInstance instance, Pair[] pArr) CloneAndReturnCorresponding(params Pair[] pArr)
        {
            var clonedInstance = Clone();
            var correspondingPairs = new Pair[pArr.Length];

            foreach (var clonedVaraible in clonedInstance.Variables)
            {
                for (int i = 0; i < pArr.Length; i++)
                {
                    if (pArr[i].Variable.Id == clonedVaraible.Id)
                    {
                        foreach (var clonedColor in clonedVaraible.AvalibleColors)
                        {
                            if (clonedColor.Value == pArr[i].Color.Value)
                            {
                                correspondingPairs[i] = new Pair(clonedVaraible,clonedColor);
                                break;
                            }
                        }
                    }
                }
            }

            return (clonedInstance, correspondingPairs);
        }
    }
}
