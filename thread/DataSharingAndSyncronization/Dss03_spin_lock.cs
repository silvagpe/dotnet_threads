public class Dss03_spin_lock
{
    class BankAccount
    {
        public int Balance { get; private set; }

        public void Deposit(int amount)
        {
            Balance += amount;
        }

        public void Withdraw(int amount)
        {
            Balance -= amount;
        }
    }

    public static void Run()
    {
        var ba = new BankAccount();
        var tasks = new List<Task>();

        SpinLock sl = new SpinLock();
        for (int i = 0; i < 10; i++)
        {
            tasks.Add(Task.Factory.StartNew(() =>
            {
                for (int i = 0; i < 100; i++)
                {
                    var lockTaken = false;
                    try
                    {
                        sl.Enter(ref lockTaken);
                        ba.Deposit(1);
                    }
                    finally
                    {
                        if (lockTaken) sl.Exit();
                    }
                }
            }));

            tasks.Add(Task.Factory.StartNew(() =>
            {
                for (int i = 0; i < 100; i++)
                {
                    var lockTaken = false;
                    try
                    {
                        sl.Enter(ref lockTaken);
                        ba.Withdraw(1);
                    }
                    finally
                    {
                        if (lockTaken) sl.Exit();
                    }
                }
            }));

        }
        Console.WriteLine($"Spin Lock. Final balance is {ba.Balance}.");
    }
}