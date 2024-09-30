using NUnit.Framework;

namespace KartGame.EditModeTests
{
    [TestFixture]
    public class MathUtilsTestFixture
    {
        [TestCase(0, 1, 10, ExpectedResult = true)]
        [TestCase(1, 0, 10, ExpectedResult = true)]
        [TestCase(0, 10, 1, ExpectedResult = false)]
        [TestCase(10, 0, 1, ExpectedResult = false)]
        [TestCase(0, 0, 0, ExpectedResult = true)]
        public bool IsApproximately(float a, float b, float epsilon)
        {
            return MathUtils.IsApproximately(a, b, epsilon);
        }
    }
}