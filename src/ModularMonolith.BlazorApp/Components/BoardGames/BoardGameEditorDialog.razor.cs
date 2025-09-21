namespace ModularMonolith.BlazorApp.Components.BoardGames;

public sealed partial class BoardGameEditorDialog
: IDialogContentComponent<BoardGameEditorViewModel>
{
  [Parameter]
  public required BoardGameEditorViewModel Content { get; set; }

  [CascadingParameter]
  public FluentDialog Dialog { get; set; } = default!;

  private async Task SaveAsync()
  {
    await Dialog.CloseAsync(Content).ConfigureAwait(true);
  }

  private async Task CancelAsync()
  {
    await Dialog.CancelAsync().ConfigureAwait(true);
  }
}
