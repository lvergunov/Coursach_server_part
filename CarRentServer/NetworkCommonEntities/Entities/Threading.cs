namespace NetworkCommonEntities.Entities
{
    public class Threading
    {
        private readonly List<Action> executeOnMainThread = new List<Action>();
        private readonly List<Action> copiedOnMainThread = new List<Action>();
        private bool isOnMainThread = false;
        public void ExecuteOnMainThread(Action action)
        {
            lock (executeOnMainThread)
            {
                executeOnMainThread.Add(action);
                isOnMainThread = true;
            }
        }
        public void UpdateMain()
        {
            if (isOnMainThread)
            {
                copiedOnMainThread.Clear();
                lock (executeOnMainThread)
                {
                    copiedOnMainThread.AddRange(executeOnMainThread);
                    executeOnMainThread.Clear();
                    isOnMainThread = false;
                }
                for (int i = 0; i < copiedOnMainThread.Count; i++)
                {
                    copiedOnMainThread[i]();
                }
            }
        }
    }
}
