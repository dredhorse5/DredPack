namespace DredPack.UI.Some
{
    
    public interface IWindowCallback
    {
        public void OnStartOpen();
        public void OnStartClose();
        public void OnStartSwitch(bool state);

        public void OnEndOpen();
        public void OnEndClose();
        public void OnEndSwitch(bool state);

        public void OnStateChanged(StatesRead state);
    }
}