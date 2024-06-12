using TestCopilot.Api.Controllers;

namespace TestCopilot.Api.Tests
{
    [TestClass]
    public class WeatherForecastControllerUnitTest
    {
        [DataTestMethod]
        [DataRow(-1, false)]
        [DataRow(0, false)]
        [DataRow(1, false)]
        [DataRow(2, true)]
        [DataRow(3, true)]
        [DataRow(4, false)]
        [DataRow(5, true)]
        [DataRow(6, false)]
        [DataRow(7, true)]
        [DataRow(8, false)]
        [DataRow(9, false)]
        [DataRow(10, false)]
        [DataRow(11, true)]
        [DataRow(12, false)]
        [DataRow(13, true)]
        [DataRow(14, false)]
        [DataRow(15, false)]
        [DataRow(16, false)]
        [DataRow(17, true)]
        [DataRow(18, false)]
        [DataRow(19, true)]
        [DataRow(20, false)]
        [DataRow(21, false)]
        [DataRow(22, false)]
        [DataRow(23, true)]
        [DataRow(24, false)]
        [DataRow(25, false)]
        [DataRow(26, false)]
        [DataRow(27, false)]
        [DataRow(28, false)]
        [DataRow(29, true)]
        [DataRow(30, false)]
        [DataRow(31, true)]
        [DataRow(32, false)]
        [DataRow(33, false)]
        [DataRow(34, false)]
        [DataRow(35, false)]
        [DataRow(36, false)]
        [DataRow(37, true)]
        [DataRow(38, false)]
        [DataRow(39, false)]
        [DataRow(40, false)]
        [DataRow(41, true)]
        [DataRow(42, false)]
        [DataRow(43, true)]
        [DataRow(44, false)]
        [DataRow(45, false)]
        [DataRow(46, false)]
        [DataRow(47, true)]
        [DataRow(48, false)]
        [DataRow(49, false)]
        [DataRow(50, false)]
        public void IsPrime_InputIsPrimeNumber_ReturnsExpectedResult(int number, bool expectedResult)
        {
            // Arrange

            // Act
            var result = PrimesController.IsPrime(number);

            // Assert
            Assert.AreEqual(expectedResult, result);
        }
    }
}