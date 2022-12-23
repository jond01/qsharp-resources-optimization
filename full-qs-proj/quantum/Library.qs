namespace quantum {

    open Microsoft.Quantum.Canon;
    open Microsoft.Quantum.Intrinsic;

    operation HelloQ() : Result {
        Message("Hello quantum world!");
        use qubit = Qubit();
        H(qubit);
        return M(qubit);
    }
}
