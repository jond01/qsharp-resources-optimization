using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Quantum.Simulation.Simulators.QCTraceSimulators;
using Quantum.ResourcesTutorial;

namespace host
{
    static class ResourcesProgram
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


        }

        // See:
        // https://docs.microsoft.com/en-us/dotnet/api/microsoft.quantum.simulation.simulators.qctracesimulators.qctracesimulatorconfiguration
        static QCTraceSimulatorConfiguration GetConfig(bool optimizeDepth, bool enableRestrictedReuse = false)
        {
            var config = new QCTraceSimulatorConfiguration();
            config.OptimizeDepth = optimizeDepth;
            config.UseWidthCounter = true;
            config.UseDepthCounter = true;
            config.UsePrimitiveOperationsCounter = true;
            config.EnableRestrictedReuse = enableRestrictedReuse;
            SetConfigDepth(config);
            return config;
        }

        // See:
        // https://docs.microsoft.com/en-us/azure/quantum/machines/qc-trace-simulator/width-counter
        static async Task Main(string[] args)
        {
            var optimizeDepthSimulator = new QCTraceSimulator(
                GetConfig(optimizeDepth: true, enableRestrictedReuse: false)
            );
            var encourageReuseSimulator = new QCTraceSimulator(
                GetConfig(optimizeDepth: false, enableRestrictedReuse: false)
            );

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
                string csvSummary = sim.ToCSV()[MetricsCountersNames.widthCounter];
                string optimizeDepth = sim.GetConfigurationCopy().OptimizeDepth.ToString();
                System.IO.File.WriteAllText($"data/optimizeDepth={optimizeDepth}.csv", csvSummary);
            }
        }
    }
}
