namespace Trybank.Lib;

public class TrybankLib
{
    public bool Logged;
    public int loggedUser;
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

    public void RegisterAccount(int number, int agency, int pass)
    {
        for (int i = 0; i < registeredAccounts; i++)
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

    public void Login(int number, int agency, int pass)
    {
        if (Logged)
        {
            throw new AccessViolationException("Usuário já está logado");
        }
        
        for (int i = 0; i < registeredAccounts; i++)
        {
            if (Bank[i, 0] == number && Bank[i, 1] == agency)
            {
                if (Bank[i, 2] == pass)
                {
                    Logged = true;
                    loggedUser = i;
                    return;
                }
                else
                {
                    throw new ArgumentException("Senha incorreta");
                }
            }
        }
        throw new ArgumentException("Agência + Conta não encontrada");
    }

    public void Logout()
    {
        if (!Logged)
        {
            throw new AccessViolationException("Usuário não está logado");
        }

        Logged = false;
        loggedUser = -99;
    }

    public int CheckBalance()
    {
        if (!Logged)
        {
            throw new AccessViolationException("Usuário não está logado");
        }

        return Bank[loggedUser, 3];
    }

    public void Deposit(int value)
    {
        if (!Logged)
        {
            throw new AccessViolationException("Usuário não está logado");
        }

        Bank[loggedUser, 3] += value;
    }

    public void Withdraw(int value)
    {
        if (!Logged)
        {
            throw new AccessViolationException("Usuário não está logado");
        }
        if (Bank[loggedUser, 3] < value)
        {
            throw new InvalidOperationException("Saldo insuficiente");
        }
        
        Bank[loggedUser, 3] -= value;
    }

    public void Transfer(int destinationNumber, int destinationAgency, int value)
    {
        if (!Logged)
        {
            throw new AccessViolationException("Usuário não está logado");
        }

        for (int i = 0; i < registeredAccounts; i++)
        {
            if (Bank[i, 0] == destinationNumber && Bank[i, 1] == destinationAgency)
            {
                if (Bank[loggedUser, 3] < value)
                {
                    throw new InvalidOperationException("Saldo insuficiente");
                }
                
                Bank[loggedUser, 3] -= value;
                Bank[i, 3] += value;
                return;
            }
        }
    }
}
