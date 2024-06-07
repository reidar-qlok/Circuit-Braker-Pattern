namespace Circuit_Braker_Pattern
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Skapa en instans av CircuitBreaker med ett feltröskelvärde på 3 och en återhämtningstid på 10 sekunder
            CircuitBreaker circuitBreaker = new CircuitBreaker(failureThreshold: 3, retryTimePeriod: 10);

            try
            {
                // Använd circuit breaker för att utföra en operation
                circuitBreaker.Execute(() =>
                {
                    // Anrop till en extern resurs som kan misslyckas
                    Console.WriteLine("Försöker utföra operation...");
                    // Simulerad operation som slumpmässigt misslyckas
                    Random random = new Random();
                    if (random.Next(0, 2) == 0)
                    {
                        throw new Exception("Operationen misslyckades.");
                    }
                    Console.WriteLine("Operationen lyckades.");
                });
            }
            catch (CircuitBreakerOpenException ex)
            {
                // Hantera felhantering när kretsen är öppen
                Console.WriteLine("Circuit breaker är öppen: " + ex.Message);
            }
            catch (Exception ex)
            {
                // Hantera andra typer av fel
                Console.WriteLine("Ett fel inträffade: " + ex.Message);
            }
        }
    }
}
