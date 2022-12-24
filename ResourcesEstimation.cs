using System;
using System.Threading.Tasks;
using Microsoft.Quantum.Simulation.Simulators.QCTraceSimulators;
using Quantum.ResourcesTutorial;

namespace Host
{
    static class ResourcesProgram
    {
        static int DefaultPrimitiveTime = 1;

        // Configure depth counting:
        // Count every relevant primitive in the depth metric.
        // By default only T primitives are counted, i.e. the depth is T-depth.
        static void SetPrimitiveTimes(QCTraceSimulatorConfiguration config)
        {
            foreach (var primitive in Enum.GetNames<PrimitiveOperationsGroups>())
            {
                config.TraceGateTimes[Enum.Parse<PrimitiveOperationsGroups>(primitive)] =
                DefaultPrimitiveTime;
            }
        }

        // See:
        // https://docs.microsoft.com/en-us/dotnet/api/microsoft.quantum.simulation.simulators.qctracesimulators.qctracesimulatorconfiguration
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
        // https://docs.microsoft.com/en-us/azure/quantum/machines/qc-trace-simulator/width-counter
        static async Task Main(string[] args)
        {
            var sim = new QCTraceSimulator(GetConfig());
            await SayHello.Run(sim);

            double depth = sim.GetMetric<SayHello>(MetricsNames.DepthCounter.Depth);
            double width = sim.GetMetric<SayHello>(MetricsNames.WidthCounter.ExtraWidth);

            Console.WriteLine(sim.Name);
            Console.WriteLine($"Depth: {depth}, width: {width}.");
        }
    }
}
