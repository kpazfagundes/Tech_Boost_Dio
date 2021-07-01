using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace CafeDaManhaSincrono
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Task<Torrada> taskDeTorrada = TorrarEPassarManteigaNoPaoAsync(2);
            Task<Ovo> taskDeOvos = FritarOvosAsync(2);
            Task<Bacon> taskDeBacon = FritarBaconAsync(2);

            var tasksDoCafeDaManha = new List<Task> { taskDeTorrada, taskDeOvos, taskDeBacon };
            while (tasksDoCafeDaManha.Count > 0)
            {
                Task taskFinalizada = await Task.WhenAny(tasksDoCafeDaManha);
                if (taskFinalizada == taskDeOvos)
                {
                    Console.WriteLine("Ovos estão rontos");
                }
                else if (taskFinalizada == taskDeBacon)
                {
                    Console.WriteLine("Bacon está pronto");
                }
                else if (taskFinalizada == taskDeTorrada)
                {
                    Console.WriteLine("Pão está pronto");
                }
                tasksDoCafeDaManha.Remove(taskFinalizada);
            }

            Cafe chicaraDeCafe = CoarCafe("Copo 1");
            Console.WriteLine("Café está pronto");
        }

        private static async Task<Torrada> TorrarEPassarManteigaNoPaoAsync(int fatias)
        {
            for (int fatia = 0; fatia < fatias; fatia++)
            {
                Console.WriteLine("Colocando fatia de pão na torradeira");
            }
            Console.WriteLine("Iniciando a tostagem...");
            Task.Delay(3000).Wait();
            Console.WriteLine("Removendo pão da torradeira");
            Console.WriteLine("Passando manteiga no pão");
            Console.WriteLine("Colocando pão no prato");

            return await Task.FromResult(new Torrada());
        }

        private static Cafe CoarCafe(string cafe)
        {
            Console.WriteLine("Coando café");

            return new Cafe();
        }

        private static async Task<Bacon> FritarBaconAsync(int fatias)
        {
            Console.WriteLine($"Colocando {fatias} fatias de bacon na panela");
            Console.WriteLine("Tostando primeiro lado do bacon...");
            await Task.Delay(3000);
            for (int fatia = 0; fatia < fatias; fatia++)
            {
                Console.WriteLine("Virando a fatia de bacon");
            }
            Console.WriteLine("Tostando segundo lado do bacon...");
            await Task.Delay(4000);
            Console.WriteLine("Colocando o bacon no prato");

            return new Bacon();
        }

        private static async Task<Ovo> FritarOvosAsync(int quantosOvos)
        {
            Console.WriteLine("Esquentando a panela de ovos...");
            await Task.Delay(3000);
            for (int ovo = 1; ovo == quantosOvos; ovo++)
            {
                Console.WriteLine("Quebrando ovo {0}", ovo);
            }
            Console.WriteLine("Fritando ovos ...");
            await Task.Delay(3000);
            Console.WriteLine("Colocando ovos no prato");

            return new Ovo();
        }
    }

    public class Ovo {
        public Ovo(){ }
    }

    public class Bacon {
        public Bacon(){ }
    }

    public class Torrada {
        public Torrada(){ }
    }

    public class Cafe {
        public Cafe(){ }
    }
}
