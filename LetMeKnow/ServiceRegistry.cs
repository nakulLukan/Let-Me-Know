using LetMeKnow.Entities;
using LetMeKnow.Services;
using Nancy.TinyIoc;

namespace LetMeKnow
{
    public class Registry
    {
        public static TinyIoCContainer Container { get; } = new TinyIoCContainer();

        public static void Register()
        {
            Container.Register<AppDbContext>().AsSingleton();
            Container.Register<VaccineService>();
        }
    }
}
