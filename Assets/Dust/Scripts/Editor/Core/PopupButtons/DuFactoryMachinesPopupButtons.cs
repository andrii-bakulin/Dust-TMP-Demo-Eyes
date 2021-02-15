using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    public class DuFactoryMachinesPopupButtons : DuPopupButtons
    {
        private DuFactoryEditor m_Factory;

        //--------------------------------------------------------------------------------------------------------------

        public static void AddMachine(System.Type type, string title)
        {
            AddEntity("FactoryMachines", type, title);
        }

        //--------------------------------------------------------------------------------------------------------------

        public static DuPopupButtons Popup(DuFactoryEditor factory)
        {
            var popup = new DuFactoryMachinesPopupButtons();
            popup.m_Factory = factory;

            GenerateColumn(popup, "FactoryMachines", "Machines");

            return popup;
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override bool OnButtonClicked(CellRecord cellRecord)
        {
            DuFactoryMachineEditor.AddFactoryMachineComponentByType((m_Factory.target as DuMonoBehaviour).gameObject, cellRecord.type);
            return true;
        }
    }
}
