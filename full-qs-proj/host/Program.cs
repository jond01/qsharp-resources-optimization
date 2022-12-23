using Microsoft.Quantum.Simulation.Simulators;
using quantum;  // The quantum library in Q#

namespace full_proj_host
{
    static class Program
    {
        async static Task Main(string[] args)
        {
          using var sim = new QuantumSimulator();
          var result = await quantum.HelloQ.Run(sim);
          Console.WriteLine($"Simulation result: {result}");
        }
    }
}
