using UnityEngine;

[CreateAssetMenu(fileName = "NuevoDialogo", menuName = "SistemaDeDialogos/Dialogo", order = 1)]
public class Dialogo : ScriptableObject
{
    [System.Serializable]
    public struct LineasDeDialogo
    {
        [TextArea(4, 10)] // Para que el campo de texto sea m�s grande en el Inspector
        public string textoLineaDialogo;
        public string nombreNPC;
    }

    public LineasDeDialogo[] lineasDeDialogo;

    /** EJEMPLOS DE USO PARA ETIQUETAS HTML
     * <color=red> Este texto es rojo </color>
     * <b> Negrita </b>
     * <color=#00FF00> Estoy en verde </color>
     * <color=blue><b><i> Este texto es azul, en negrita y cursiva </i></b></color>
     */
}
