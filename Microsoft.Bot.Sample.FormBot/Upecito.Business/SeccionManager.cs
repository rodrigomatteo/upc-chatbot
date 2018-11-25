using SimpleInjector;
using Upecito.Data.Interface;
using Upecito.Interface;
using Upecito.Model;

namespace Upecito.Business
{
    public class SeccionManager
    {
        private Container container;

        public SeccionManager(Container container)
        {
            this.container = container;
        }
    }
}