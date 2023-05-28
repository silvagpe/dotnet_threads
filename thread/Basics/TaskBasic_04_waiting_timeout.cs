public class TaskBasic_04_waiting_timeout
{
    private static void SpinWaitUntil(){
        System.Console.WriteLine("Spin waint start");

        SpinWait.SpinUntil(()=>false, TimeSpan.FromSeconds(4));

        System.Console.WriteLine("Spin waint has been finished");
    }


    private static void BombDisarm(){
        var cts = new CancellationTokenSource();
        var token = cts.Token;

        var t = new Task(()=>{
            System.Console.WriteLine("Press any key to disarm... you have 5 seconds!");
            bool cancelled = token.WaitHandle.WaitOne(TimeSpan.FromSeconds(5));

            Console.WriteLine(cancelled? "Bomb disarmed!" : "BOOOM!!!");
        }, token);
        t.Start();

        Console.ReadKey();
        cts.Cancel();           
    }


    public static void Run()
    {
        //SpinWaitUntil();
        BombDisarm();
    }
}