using System;
using System.Threading.Tasks;

namespace CafeDaManhaSincrono
{
    class Program
    {
        static void Main(string[] args)
        {
            Torrada torrada = TorrarEPassarManteigaNoPao(2);
            Console.WriteLine("Pão está pronto");

            Ovo ovos = FritarOvos(2);
            Console.WriteLine("Os ovos estão prontos");

            Bacon bacon = FritarBacon(2);
            Console.WriteLine("O Bacon está pronto");

            Cafe chicaraDeCafe = CoarCafe();
            Console.WriteLine("Café está pronto");
        }

        private static void PassarManteiga(Torrada pao) =>
            Console.WriteLine("Passando manteiga no pão");

        private static Torrada TorrarEPassarManteigaNoPao(int fatias)
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

            return new Torrada();
        }

        private static Cafe CoarCafe()
        {
            Console.WriteLine("Coando café");
            return new Cafe();
        }

        private static Bacon FritarBacon(int fatias)
        {
            Console.WriteLine($"Colocando {fatias} fatias de bacon na panela");
            Console.WriteLine("Tostando primeiro lado do bacon...");
            Task.Delay(3000);
            for (int fatia = 0; fatia < fatias; fatia++)
            {
                Console.WriteLine("Virando a fatia de bacon");
            }
            Console.WriteLine("Tostando segundo lado do bacon...");
            Task.Delay(4000);
            Console.WriteLine("Colocando o bacon no prato");

            return new Bacon();
        }

        private static Ovo FritarOvos(int quantosOvos)
        {
            Console.WriteLine("Esquentando a panela de ovos...");
            Task.Delay(3000);
            for (int ovo = 1; ovo <= quantosOvos; ovo++)
            {
                Console.WriteLine("Quebrando ovo {0}", ovo);
            }
            Console.WriteLine("Fritando ovos ...");
            Task.Delay(3000);
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
