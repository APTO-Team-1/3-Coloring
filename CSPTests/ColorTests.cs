using Xunit;

namespace CSP.Tests
{
    public class ColorTests
    {
        [Fact()]
        public void ConstructorTest()
        {
            var color = new Color(1);
            Assert.Equal(1, color.Value);
        }
    }
}