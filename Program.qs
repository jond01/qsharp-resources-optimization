namespace Quantum.ResourcesTutorial {

    open Microsoft.Quantum.Canon;
    open Microsoft.Quantum.Intrinsic;
    open Microsoft.Quantum.Math;

    // @EntryPoint()
    operation QuantumOperation() : Unit {
        use qubits = Qubit[3];
        ApplyToEachCA(H, qubits);
        X(qubits[0]);
        Y(qubits[1]);
        Rz(PI() / 2.0, qubits[2]);
        CNOT(qubits[0], qubits[2]);
        CNOT(qubits[1], qubits[2]);
        HY(qubits[1]);
        CNOT(qubits[0], qubits[1]);

        // Add measurements and `AssertMeasurementProbability` calls here.
        // https://learn.microsoft.com/en-us/azure/quantum/machines/qc-trace-simulator/#providing-the-probability-of-measurement-outcomes
    }
}
