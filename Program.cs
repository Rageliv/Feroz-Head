<<<<<<< HEAD
using System;

namespace FerozHead
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (FerozHead game = new FerozHead())
            {
                game.Run();
            }
        }
    }
#endif
}

=======
using System;

namespace FerozHead
{
#if WINDOWS || XBOX
    static class Program
    {
        static void Main(string[] args)
        {
            using (FerozHead game = new FerozHead())
            {
                game.Run();
            }
        }
    }
#endif
}

>>>>>>> a6a3562046bd0b8fe59da158ab333508b5e37c31
