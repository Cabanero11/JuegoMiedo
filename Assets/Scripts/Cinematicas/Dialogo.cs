using UnityEngine;

[CreateAssetMenu(fileName = "NuevoDialogo", menuName = "SistemaDeDialogos/Dialogo", order = 1)]
public class Dialogo : ScriptableObject
{
    [TextArea(3, 10)] // Para que el campo de texto sea más grande en el Inspector
    public string[] lineasDeDialogo;

    /** EJEMPLOS DE USO PARA ETIQUETAS HTML
     * <color=red> Este texto es rojo </color>
     * <b> Negrita </b>
     * <color=#00FF00> Estoy en verde </color>
     * <color=blue><b><i> Este texto es azul, en negrita y cursiva </i></b></color>
     */
}
