using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Quantum.Simulation.Simulators.QCTraceSimulators;
using Quantum.ResourcesTest;

namespace host
{
    static class Program
    {
        static int DefaultPrimitiveDepth = 1;

        // Configure depth counting:
        // Count every relevant primitive in the depth metric.
        // By default only T primitives are counted, i.e. the depth is T-depth.
        static void SetConfigDepth(QCTraceSimulatorConfiguration config)
        {
            foreach (var primitive in Enum.GetNames<PrimitiveOperationsGroups>())
            {
                config.TraceGateTimes[Enum.Parse<PrimitiveOperationsGroups>(primitive)] =
                DefaultPrimitiveDepth;
            }

            Console.WriteLine("Primitives depth:");
            foreach (var kvp in config.TraceGateTimes)
            {
                Console.WriteLine(kvp);
            }
        }

        // See:
        // https://docs.microsoft.com/en-us/dotnet/api/microsoft.quantum.simulation.simulators.qctracesimulators.qctracesimulatorconfiguration
        static QCTraceSimulatorConfiguration GetConfig(bool optimizeDepth)
        {
            var config = new QCTraceSimulatorConfiguration();
            config.OptimizeDepth = optimizeDepth;
            config.UseWidthCounter = true;
            config.UseDepthCounter = true;
            SetConfigDepth(config);
            return config;
        }

        // See:
        // https://docs.microsoft.com/en-us/azure/quantum/machines/qc-trace-simulator/width-counter
        static async Task Main(string[] args)
        {
            var optimizeDepthSimulator = new QCTraceSimulator(GetConfig(optimizeDepth: true));
            var encourageReuseSimulator = new QCTraceSimulator(GetConfig(optimizeDepth: false));

            await Task.WhenAll(
                SayHello.Run(optimizeDepthSimulator),
                SayHello.Run(encourageReuseSimulator)
            );

            foreach (var sim in new List<QCTraceSimulator> { optimizeDepthSimulator, encourageReuseSimulator })
            {
                double depth = sim.GetMetric<SayHello>(MetricsNames.DepthCounter.Depth);
                double width = sim.GetMetric<SayHello>(MetricsNames.WidthCounter.ExtraWidth);
                Console.WriteLine(sim.Name);
                Console.WriteLine($"Depth: {depth}, width: {width}.");
            }
        }
    }
}
