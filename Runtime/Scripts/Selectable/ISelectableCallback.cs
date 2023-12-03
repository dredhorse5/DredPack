namespace DredPack.SelectableSystem
{
    public interface ISelectableCallback
    {
        public void OnSelect(bool state);
        public void SetElement(Selector.Element element);
    }
}