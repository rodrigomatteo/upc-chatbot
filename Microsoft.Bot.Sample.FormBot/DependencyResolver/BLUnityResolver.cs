namespace FormBot.DependencyResolver
{
    public class BLUnityResolver
    {
        public static void RegisterTypes(SimpleInjector.Container oIUnityContainer, string database)
        {
            DADependencyRegister.RegisterTypes(oIUnityContainer, database);
        }
    }
}