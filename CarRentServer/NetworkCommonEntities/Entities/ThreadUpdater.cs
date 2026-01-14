namespace NetworkCommonEntities.Entities
{
    public class ThreadUpdater
    {
        public ThreadUpdater(Threading threading)
        {
            _mainThread = threading;
        }

        public void UpdateThread()
        {
            while (true)
            {
                _mainThread.UpdateMain();
                Thread.Sleep(10);
            }
        }
        private Threading _mainThread;
    }
}
