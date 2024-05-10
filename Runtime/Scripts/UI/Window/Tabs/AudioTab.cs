using System;
using DredPack.UI.Some;

namespace DredPack.UI.Tabs
{

    [Serializable]
    public class AudioTab : WindowTab, IWindowCallback
    {
        public override int InspectorDrawSort => 150;
        public SCPAudio[] List;


        public void OnStartOpen() => Play(StatesForChanged.StartOpen);
        public void OnStartClose() => Play(StatesForChanged.StartClose);
        public void OnStartSwitch(bool state) => Play(StatesForChanged.StartSwitch);

        public void OnEndOpen() => Play(StatesForChanged.EndOpen);
        public void OnEndClose() => Play(StatesForChanged.EndClose);
        public void OnEndSwitch(bool state) => Play(StatesForChanged.EndSwitch);

        public void OnStateChanged(StatesRead state)
        {
        }

        public void Play(StatesForChanged state)
        {
            for (int i = 0; i < List.Length; i++)
                List[i]?.TryExecute(state);
        }
    }
}