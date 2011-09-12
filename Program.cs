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

