using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Quantum.Simulation.Simulators.QCTraceSimulators;
using Quantum.ResourcesTutorial;

namespace Host
{
    static class ResourcesProgram
    {
        static int DefaultPrimitiveTime = 1;

        // Configure depth counting:
        // Count every relevant primitive in the depth metric.
        // By default, only T primitives are counted, i.e. the depth is T-depth.
        static void SetPrimitiveTimes(QCTraceSimulatorConfiguration config)
        {
            foreach (var primitive in Enum.GetNames<PrimitiveOperationsGroups>())
            {
                config.TraceGateTimes[Enum.Parse<PrimitiveOperationsGroups>(primitive)] =
                DefaultPrimitiveTime;
            }
        }

        // See:
        // https://learn.microsoft.com/en-us/dotnet/api/microsoft.quantum.simulation.simulators.qctracesimulators.qctracesimulatorconfiguration?view=quantum-dotnet-latest
        static QCTraceSimulatorConfiguration GetConfig(bool optimizeDepth = false)
        {
            var config = new QCTraceSimulatorConfiguration();
            config.UseWidthCounter = true;
            config.UseDepthCounter = true;
            SetPrimitiveTimes(config);
            config.OptimizeDepth = optimizeDepth;
            return config;
        }

        // See:
        // https://learn.microsoft.com/en-us/azure/quantum/machines/qc-trace-simulator/
        static async Task Main(string[] args)
        {
            var optimizeDepthSimulator = new QCTraceSimulator(GetConfig(optimizeDepth: true));
            var encourageReuseSimulator = new QCTraceSimulator(GetConfig(optimizeDepth: false));

            await Task.WhenAll(
                ApplyOracles.Run(optimizeDepthSimulator),
                ApplyOracles.Run(encourageReuseSimulator)
            );

            foreach (var sim in new List<QCTraceSimulator> { optimizeDepthSimulator, encourageReuseSimulator })
            {
                double depth = sim.GetMetric<ApplyOracles>(MetricsNames.DepthCounter.Depth);
                double width = sim.GetMetric<ApplyOracles>(MetricsNames.WidthCounter.ExtraWidth);
                Console.WriteLine(sim.Name);
                Console.WriteLine($"Depth: {depth}, width: {width}.");
                string csvSummary = sim.ToCSV()[MetricsCountersNames.widthCounter];
                string optimizeDepth = sim.GetConfigurationCopy().OptimizeDepth.ToString();
                System.IO.File.WriteAllText($"data/optimizeDepth={optimizeDepth}.csv", csvSummary);
            }
        }
    }
}
