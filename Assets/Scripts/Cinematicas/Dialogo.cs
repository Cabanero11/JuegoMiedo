using UnityEngine;

[CreateAssetMenu(fileName = "NuevoDialogo", menuName = "SistemaDeDialogos/Dialogo", order = 1)]
public class Dialogo : ScriptableObject
{
    [TextArea(3, 10)] // Para que el campo de texto sea más grande en el Inspector
    public string[] lineasDeDialogo;
}
