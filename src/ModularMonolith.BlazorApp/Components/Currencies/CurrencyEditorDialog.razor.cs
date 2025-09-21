namespace ModularMonolith.BlazorApp.Components.Currencies;

public sealed partial class CurrencyEditorDialog
: IDialogContentComponent<CurrencyEditViewModel>
{
  [Parameter]
  public required CurrencyEditViewModel Content { get; set; }

  [CascadingParameter]
  public FluentDialog Dialog { get; set; } = default!;

  private async Task SaveAsync()
  {
    await Dialog.CloseAsync(Content);
  }

  private async Task CancelAsync()
  {
    await Dialog.CancelAsync();
  }
}
