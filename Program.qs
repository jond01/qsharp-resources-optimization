namespace Quantum.ResourcesTutorial {

    open Microsoft.Quantum.Canon;
    open Microsoft.Quantum.Intrinsic;

    operation QuantumOperation() : Unit {
        let numSummands = 3;
        use (summands1, summands2) = (Qubit[numSummands], Qubit[numSummands]);
        use (target1, target2) = (Qubit(), Qubit());
        SumQubits(summands1, target1);
        SumQubits(summands2, target2);
    }

    operation SumQubits(summands : Qubit[], target : Qubit) : Unit is Adj + Ctl {
        use aux = Qubit();
        CNOT(summands[0], target);
        within {
            for i in 1..Length(summands) - 1 {
                CNOT(summands[i], aux);
            }
        } apply {
            CNOT(aux, target);
        }
    }
}
