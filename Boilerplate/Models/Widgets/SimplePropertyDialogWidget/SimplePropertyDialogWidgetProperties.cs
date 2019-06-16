using Kentico.Forms.Web.Mvc;
using Kentico.PageBuilder.Web.Mvc;

public sealed class SimplePropertyDialogWidgetProperties : IWidgetProperties
{
    [EditingComponent(Kentico.Forms.Web.Mvc.IntInputComponent.IDENTIFIER, Order = 0, Label = "Number")]
    public int Number { get; set; }
}