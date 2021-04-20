using Xunit;
using CSP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSP.Tests
{
    public class CspInstanceTests
    {
        [Fact()]
        public void AddRestrictionsTest()
        {
            var csp = new CspInstance();
            var color1 = new Color(1);
            var color2 = new Color(2);
            var restriction = new Restriction(color1, color2);
            csp.AddRestriction(restriction);
            Assert.Equal(1, csp.Restrictions.Count);
            csp.AddRestriction(restriction);
            Assert.Equal(1, csp.Restrictions.Count);

        }

        [Fact()]
        public void AddRestrictionsTest2()
        {
            var csp = new CspInstance();
            var color1 = new Color(1);
            var color2 = new Color(2);
            var restriction = new Restriction(color1, color2);
            var restriction2 = new Restriction(color1, color2);
            csp.AddRestriction(restriction);
            csp.AddRestriction(restriction2);
            Assert.Equal(1, csp.Restrictions.Count);

        }

        [Fact()]
        public void AddRestrictionsTest3()
        {
            var csp = new CspInstance();
            var color1 = new Color(1);
            var color2 = new Color(2);
            var color3 = new Color(3);
            var restriction = new Restriction(color1, color2);
            var restriction2 = new Restriction(color1, color3);
            csp.AddRestriction(restriction);
            csp.AddRestriction(restriction2);
            Assert.Equal(2, csp.Restrictions.Count);


        }
        [Fact()]
        public void RemoveRestrictionsTest()
        {
            var csp = new CspInstance();
            var color1 = new Color(1);
            var color2 = new Color(2);
            var color3 = new Color(3);
            var restriction = new Restriction(color1, color2);
            var restriction2 = new Restriction(color1, color3);
            csp.AddRestriction(restriction);
            Assert.Equal(1, csp.Restrictions.Count);
            csp.RemoveRestriction(restriction2);
            Assert.Equal(1, csp.Restrictions.Count);
            csp.RemoveRestriction(restriction);
            Assert.Equal(0, csp.Restrictions.Count);

        }

        [Fact()]
        public void AddVariableTest()
        {
            var csp = new CspInstance();
            var variable = new Variable(3);
            csp.Variables.Add(variable);
            Assert.Equal(1, csp.Variables.Count);
            csp.Variables.Add(variable);
            Assert.Equal(1, csp.Variables.Count);
            var variable2 = new Variable(4);
            csp.Variables.Add(variable2);
            Assert.Equal(2, csp.Variables.Count);
        }

        [Fact()]
        public void RemoveVariableTest()
        {
            var csp = new CspInstance();
            var variable = new Variable(3);
            var variable2 = new Variable(4);
            csp.Variables.Add(variable);
            Assert.Equal(1, csp.Variables.Count);
            csp.Variables.Remove(variable2);
            Assert.Equal(1, csp.Variables.Count);
            csp.Variables.Remove(variable);
            Assert.Equal(0, csp.Variables.Count);
        }

        [Fact()]
        public void AddRestrictionToColorTest()
        {
            var csp = new CspInstance();
            var variable = new Variable(3);
            var variable2 = new Variable(4);
            csp.Variables.Add(variable);
            csp.Variables.Add(variable2);

            var color1 = variable.AvalibleColors[0];
            var color2 = variable2.AvalibleColors[0];
            csp.AddRestriction(new Restriction(color1, color2));
            Assert.Equal(1, color1.Restrictions.Count);
            Assert.Equal(1, color2.Restrictions.Count);
            Assert.Contains(color2, color1.Restrictions);
            Assert.Contains(color1, color2.Restrictions);
            Assert.DoesNotContain(color2, color2.Restrictions);
            Assert.DoesNotContain(color1, color1.Restrictions);

        }

        [Fact()]
        public void RemoveRestrictionFromColorTest()
        {
            var csp = new CspInstance();
            var variable = new Variable(3);
            var variable2 = new Variable(4);
            csp.Variables.Add(variable);
            csp.Variables.Add(variable2);

            var color1 = variable.AvalibleColors[0];
            var color2 = variable2.AvalibleColors[0];
            var color3 = variable2.AvalibleColors[1];
            csp.AddRestriction(new Restriction(color1, color2));
            csp.AddRestriction(new Restriction(color1, color3));
            Assert.Equal(2, color1.Restrictions.Count);

            csp.RemoveRestriction(new Restriction(color1, color2));
            Assert.Equal(1, color1.Restrictions.Count);
            Assert.DoesNotContain(color2, color1.Restrictions);
            Assert.DoesNotContain(color1, color2.Restrictions);
            Assert.Contains(color3, color1.Restrictions);

            csp.RemoveRestriction(new Restriction(color1, color3));
            Assert.Equal(0, color1.Restrictions.Count);
            Assert.DoesNotContain(color3, color1.Restrictions);

        }
    }
}