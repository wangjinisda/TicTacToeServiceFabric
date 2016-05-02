var Hello;
(function (Hello) {
    function compute(op, a, b) {
        console.log("the operator is" + Operator[op]);
        // ...  
    }
    var BankAccount = (function () {
        function BankAccount(initially) {
            this.balance = initially;
        }
        BankAccount.prototype.deposit = function (credit) {
            this.balance += credit;
            return this.balance;
        };
        return BankAccount;
    })();
})(Hello || (Hello = {}));
