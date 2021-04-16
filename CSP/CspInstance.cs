using System;
using System.Collections;
using System.Collections.Generic;

namespace CSP
{
    public class CspInstance
    {
        public CspInstance()
        {

        }
        public ISet<Variable> Variables { get;  } = new HashSet<Variable>();

        private HashSet<Restriction> restrictions = new HashSet<Restriction>();

        public IReadOnlySet<Restriction> Restrictions { get => restrictions; }

        public void AddRestriction(Restriction restriction)
        {
            if(!restrictions.Contains(restriction))
            {
                restrictions.Add(restriction);
                restriction.Color1.AddRestriction(restriction.Color2);
                restriction.Color2.AddRestriction(restriction.Color1);
            }
        }

        public void RemoveRestriction(Restriction restriction)
        {
            if (restrictions.Contains(restriction))
            {
                restrictions.Remove(restriction);
                restriction.Color1.RemoveRestriction(restriction.Color2);
                restriction.Color2.RemoveRestriction(restriction.Color1);
            }
        }
    }
}
