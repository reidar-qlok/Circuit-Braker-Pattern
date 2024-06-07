namespace Circuit_Braker_Pattern
{
    internal class CircuitBreaker
    {
        private int failureThreshold; // Antal tillåtna misslyckanden innan kretsen öppnas
        private int retryTimePeriod; // Tid (i sekunder) innan ett nytt försök görs
        private int consecutiveFailures; // Antal på varandra följande misslyckanden
        private DateTime circuitOpenTime; // Tidpunkt då kretsen öppnades

        public CircuitBreaker(int failureThreshold, int retryTimePeriod)
        {
            this.failureThreshold = failureThreshold;
            this.retryTimePeriod = retryTimePeriod;
            this.consecutiveFailures = 0;
            this.circuitOpenTime = DateTime.MinValue; // Indikerar att kretsen är stängd från början
        }

        public void Execute(Action action)
        {
            // Kontrollera om kretsen är öppen
            if (IsCircuitOpen())
            {
                // Om kretsen är öppen, kontrollera om återhämtningstiden har gått ut
                if (DateTime.Now >= circuitOpenTime.AddSeconds(retryTimePeriod))
                {
                    // Återställ kretsen om återhämtningstiden har passerat
                    circuitOpenTime = DateTime.MinValue;
                    consecutiveFailures = 0;
                }
                else
                {
                    // Om återhämtningstiden inte har passerat, kasta undantag
                    throw new CircuitBreakerOpenException();
                }
            }

            try
            {
                // Försök att utföra operationen
                action.Invoke();
                // Om operationen lyckas, återställ kretsen
                Reset();
            }
            catch (Exception)
            {
                // Om operationen misslyckas, öka antalet på varandra följande misslyckanden
                consecutiveFailures++;
                if (consecutiveFailures >= failureThreshold)
                {
                    // Om feltröskeln nås, öppna kretsen och spara tidpunkten
                    circuitOpenTime = DateTime.Now;
                    throw new CircuitBreakerOpenException();
                }
                else
                {
                    // Kasta vidare det ursprungliga undantaget om feltröskeln inte nås
                    throw;
                }
            }
        }

        // Kontrollera om kretsen är öppen
        private bool IsCircuitOpen()
        {
            return circuitOpenTime != DateTime.MinValue;
        }

        // Återställ kretsens tillstånd
        private void Reset()
        {
            consecutiveFailures = 0;
            circuitOpenTime = DateTime.MinValue;
        }
    }
}
