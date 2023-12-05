using System.Collections;
using DredPack.UI.WindowAnimations;

namespace DredPack.UI
{
    public interface IWindowAnimation
    {
        public void Init(Window owner);

        public void OnInit(Window owner);

        public IEnumerator UpdateOpen(AnimationParameters parameters);
        public void SetOpenTime(float time, AnimationParameters parameters);

        public IEnumerator UpdateClose(AnimationParameters parameters);
        public void SetCloseTime(float time, AnimationParameters parameters);
    }
}