namespace Quantum.ResourcesTutorial {

    open Microsoft.Quantum.Canon;
    open Microsoft.Quantum.Intrinsic;

    operation SayHello() : Unit {
        use summands1 = Qubit[3];
        use summands2 = Qubit[3];
        use target1 = Qubit();
        use target2 = Qubit();
        ApplyOp(summands1, target1);
        ApplyOp(summands2, target2);
    }

    operation ApplyOp(summands : Qubit[], target : Qubit) : Unit is Adj + Ctl {
        use aux = Qubit();
        CX(summands[0], target);
        within {
            for i in 1..Length(summands) - 1 {
                CX(summands[i], aux);
            }
        } apply {
            CX(aux, target);
        }
    }
}
