using UnityEngine;

namespace DustEngine
{
    [AddComponentMenu("Dust/Support/Parallax Instance")]
    public class DuParallaxInstance : DuMonoBehaviour
    {
        [SerializeField]
        internal DuParallax m_ParentParallax = null;
        public DuParallax parentParallax => m_ParentParallax;

        //--------------------------------------------------------------------------------------------------------------

        public void Initialize(DuParallax duParallax)
        {
            m_ParentParallax = duParallax;
        }
    }
}
