namespace Circuit_Braker_Pattern
{
    internal class CircuitBreakerOpenException : Exception
    {
        public CircuitBreakerOpenException() : base("Circuit breaker is open.")
        {
        }
    }
}
