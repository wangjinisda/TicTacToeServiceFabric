namespace Hello {
    const enum Operator {
        ADD,
        DIV,
        MUL,
        SUB
    }

    function compute(op: Operator, a: number, b: number) {
        console.log("the operator is" + Operator[op]);  
        // ...  
    }

    class BankAccount {
        balance: number;
        constructor(initially: number) {
            this.balance = initially;
        }
        deposit(credit: number) {
            this.balance += credit;
            return this.balance;
        }
    }
}