using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace TestCopilot.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PrimesController : ControllerBase
    {

        /**
         * A method that takes an integer and iterates from 0 to that integer and print if the current number is prime or not.
         */
        [HttpGet("list/{number}", Name = "GetPrimes")]
        public string GetPrimes(int number)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i <= number; i++)
            {
                if (IsPrime(i))
                {
                    sb.Append($"{i} is prime\n");
                }
            }
            return sb.ToString();
        }

        private static readonly Dictionary<int, bool> PrimeCache = new Dictionary<int, bool>();

        public static bool IsPrime(int number)
        {
            if (PrimeCache.ContainsKey(number))
            {
                return PrimeCache[number];
            }

            if (number <= 1)
            {
                PrimeCache[number] = false;
                return false;
            }

            if (number == 2)
            {
                PrimeCache[number] = true;
                return true;
            }

            if (number % 2 == 0)
            {
                PrimeCache[number] = false;
                return false;
            }

            var boundary = (int)Math.Floor(Math.Sqrt(number));

            for (int i = 3; i <= boundary; i += 2)
            {
                if (number % i == 0)
                {
                    PrimeCache[number] = false;
                    return false;
                }
            }

            PrimeCache[number] = true;
            return true;
        }
    }
}
