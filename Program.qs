namespace Quantum.ResourcesTutorial {

    open Microsoft.Quantum.Canon;
    open Microsoft.Quantum.Intrinsic;

    operation ApplyOracles() : Unit {
        let numQubits = 3;
        use (qubits1, qubits2) = (Qubit[numQubits], Qubit[numQubits]);
        ApplyZebraOracle(qubits1);
        ApplyZebraOracle(qubits2);
    }

    operation ApplyZebraOracle(qubits : Qubit[]) : Unit is Adj + Ctl {
        use aux = Qubit();
        within {
            // Prepare |-1⟩ state in aux
            X(aux);
            H(aux);
        } apply {
            // Phase kickback
            within {
                for i in 0..Length(qubits) - 1 {
                    if (i % 2 == 0) {
                        X(qubits[i]);
                    }
                }
            } apply {
                Controlled X(qubits, aux);
            }
        }
    }
}
