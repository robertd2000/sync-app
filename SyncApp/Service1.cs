using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace SyncApp
{
    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени класса "Service1" в коде и файле конфигурации.
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class Service1 : IService1
    {
        List<ServiceUser> users = new List<ServiceUser>();
        int nextId = 1;
        public int Connect(string name)
        {
            ServiceUser user = new ServiceUser()
            {
                Id = nextId,
                Name = name,
                operationContext = OperationContext.Current
            };
            nextId++;
            SendMsg(user.Name + " подключился", 0);
            users.Add(user);
            return user.Id;
        }

        public void Disconnect(int id)
        {
            var user = users.FirstOrDefault(i => i.Id == id);
            if (user!= null)
            {
                users.Remove(user);
                SendMsg(user.Name + " отключился", 0);
            }
        }

        public void SendMsg(string msg, int id)
        {
            foreach (var item in users)
            {
                string answer = DateTime.Now.ToShortTimeString();
                var user = users.FirstOrDefault(i => i.Id == id);
                if (user != null)
                {
                    answer += ": " + user.Name + " ";
                }

                answer += msg;

                item.operationContext.GetCallbackChannel<IService1Callback>().MessageCallback(answer);
            }
        }
    }
}
