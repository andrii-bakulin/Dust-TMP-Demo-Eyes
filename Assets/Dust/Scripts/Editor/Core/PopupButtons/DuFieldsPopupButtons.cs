using UnityEngine;
using UnityEditor;

namespace DustEngine.DustEditor
{
    public class DuFieldsPopupButtons : DuPopupButtons
    {
        private DuFieldsMapEditor m_FieldsMap;

        //--------------------------------------------------------------------------------------------------------------

        public static void AddBasicField(System.Type type, string title)
        {
            AddEntity("Fields.Basic", type, title);
        }

        public static void AddFactoryField(System.Type type, string title)
        {
            AddEntity("Fields.Factory", type, title);
        }

        public static void Add2DField(System.Type type, string title)
        {
            AddEntity("Fields.2D", type, title);
        }

        public static void Add3DField(System.Type type, string title)
        {
            AddEntity("Fields.3D", type, title);
        }

        public static void AddMathField(System.Type type, string title)
        {
            AddEntity("Fields.Math", type, title);
        }

        //--------------------------------------------------------------------------------------------------------------

        public static DuPopupButtons Popup(DuFieldsMapEditor fieldsMap)
        {
            var popup = new DuFieldsPopupButtons();
            popup.m_FieldsMap = fieldsMap;

            GenerateColumn(popup, "Fields.Basic", "Basic Fields");

            if (fieldsMap.fieldsMapInstance.fieldsMapMode == DuFieldsMap.FieldsMapMode.FactoryMachine)
                GenerateColumn(popup, "Fields.Factory", "Factory Fields");

            GenerateColumn(popup, "Fields.2D", "2D Fields");
            GenerateColumn(popup, "Fields.3D", "3D Fields");
            GenerateColumn(popup, "Fields.Math", "Math Fields");

            return popup;
        }

        //--------------------------------------------------------------------------------------------------------------

        protected override bool OnButtonClicked(CellRecord cellRecord)
        {
            DuFieldEditor.AddFieldComponentByType(m_FieldsMap.GetParentGameObject(), cellRecord.type);
            return true;
        }
    }
}
