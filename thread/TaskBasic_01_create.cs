public class TaskBasic_01_create
{
    private static void write(object o){

        int i   =  200;
        while (i-- > 0)
        {
            Console.Write(o);
        }
    }

    public static void Run(){

        //Criar uma thread e chamar via .start
        var t = new Task(write, "hi");
        t.Start();

        //Criar e executar uma thread com a facotry
        Task.Factory.StartNew(write, 123);

        //Executar o método diretamente.
        write("?");

        //Resultado esperado. Que todos os valores se misturem
        
    }
    
}