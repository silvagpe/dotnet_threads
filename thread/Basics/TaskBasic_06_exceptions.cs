public class TaskBasic_06_exceptions
{

    private static void Test()
    {
        System.Console.WriteLine("Thread exceptions ");
        var t = Task.Factory.StartNew(() =>
        {
            throw new InvalidOperationException("Can't do this!") { Source = "t" };
        });

        var t2 = Task.Factory.StartNew(() =>
        {
            throw new AccessViolationException("Can't access this!") { Source = "t2" };
        });

        Task.WhenAll(t, t2);       
    }

    public static void Run()
    {
        try
        {
            Test();
        }
        catch (AggregateException ae)
        {
            Console.WriteLine(ae.Message);
            foreach (var e in ae.InnerExceptions)
            {
                Console.WriteLine($"Exception {e.GetType()} from {e.Source}");
            }
        }
        catch (Exception exp)
        {
            Console.WriteLine(exp.Message);
        }




    }
}