using SimpleInjector;
using Upecito.Data.Interface;
using Upecito.Interface;
using Upecito.Model;

namespace Upecito.Business
{
    public class ChatLogManager : IChatLog
    {
        private Container container;

        public ChatLogManager(Container container)
        {
            this.container = container;
        }

        public ChatLog CrearChatLog(ChatLog model)
        {
            var chatLogData = container.GetInstance<IChatLogData>();
            return chatLogData.Crear(model);
        }
    }
}
