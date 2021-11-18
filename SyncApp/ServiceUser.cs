using System.ServiceModel;

namespace SyncApp
{
    class ServiceUser
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public OperationContext operationContext { get; set; }
    }
}
