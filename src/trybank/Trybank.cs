namespace Trybank.Lib;

public class TrybankLib
{
    public bool Logged;
    public int loggedUser;

    //0 -> Número da conta
    //1 -> Agência
    //2 -> Senha
    //3 -> Saldo
    public int[,] Bank;
    public int registeredAccounts;
    private int maxAccounts = 50;

    public TrybankLib()
    {
        loggedUser = -99;
        registeredAccounts = 0;
        Logged = false;
        Bank = new int[maxAccounts, 4];
    }

    // 1. Construa a funcionalidade de cadastrar novas contas
    public void RegisterAccount(int number, int agency, int pass)
    {
        for (int i = 0; i < maxAccounts; i++)
        {
            if (Bank[i, 0] == number && Bank[i, 1] == agency)
            {
                throw new ArgumentException("A conta já está sendo usada!");
            }
        }

        Bank[registeredAccounts, 0] = number;
        Bank[registeredAccounts, 1] = agency;
        Bank[registeredAccounts, 2] = pass;
        Bank[registeredAccounts, 3] = 0;

        registeredAccounts++;
    }

    // 2. Construa a funcionalidade de fazer Login
    public void Login(int number, int agency, int pass)
    {
        if (Logged)
        {
            throw new AccessViolationException("Usuário já está logado");
        }

        for (int i = 0; i < maxAccounts; i++)
        {
            if (Bank[i, 0] == number && Bank[i, 1] == agency && Bank[i, 2] == pass)
            {
                Logged = true;
                loggedUser = i;
                return;
            }
            else if (Bank[i, 0] == number && Bank[i, 1] == agency && Bank[i, 2] != pass)
            {
                throw new ArgumentException("Senha incorreta");
            }
        }
        throw new ArgumentException("Agência + Conta não encontrada");
    }

    // 3. Construa a funcionalidade de fazer Logout
    public void Logout()
    {
        if (!Logged)
        {
            throw new AccessViolationException("Usuário não está logado");
        }
        Logged = !Logged;
        loggedUser = -99;
    }

    // 4. Construa a funcionalidade de checar o saldo
    public int CheckBalance()
    {
        if (!Logged)
        {
            throw new AccessViolationException("Usuário não está logado");
        }
        int balance = Bank[registeredAccounts, 3];

        return balance;
    }

    // 5. Construa a funcionalidade de depositar dinheiro
    public void Deposit(int value)
    {
        if (!Logged)
        {
            throw new AccessViolationException("Usuário não está logado");
        }

        Bank[loggedUser, 3] = value;

    }

    // 6. Construa a funcionalidade de sacar dinheiro
    public void Withdraw(int value)
    {
        if (!Logged)
        {
            throw new AccessViolationException("Usuário não está logado");
        }

        if (value > Bank[loggedUser, 3])
        {
            throw new InvalidOperationException("Saldo insuficiente");
        }

        Bank[loggedUser, 3] -= value;
    }

    // 7. Construa a funcionalidade de transferir dinheiro entre contas
    public void Transfer(int destinationNumber, int destinationAgency, int value)
    {
        if (!Logged)
        {
            throw new AccessViolationException("Usuário não está logado");
        }

        if (value > Bank[loggedUser, 3])
        {
            throw new InvalidOperationException("Saldo insuficiente");
        }

        Bank[loggedUser, 3] -= value;
        
        for (int i = 0; i < maxAccounts; i++)
        {
            if (Bank[i, 0] == destinationNumber && Bank[i, 1] == destinationAgency)
            {
                Bank[i, 3] += value;
            }
        }
    }
}
