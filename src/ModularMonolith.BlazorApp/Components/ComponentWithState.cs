namespace ModularMonolith.BlazorApp.Components;

public class ComponentWithState
  : ComponentBase
{
  [Inject]
  public required State State
  {
    get; set;
  }

  protected override async Task OnInitializedAsync()
  {
    State.SetCurrentPage(this);
    await base.OnInitializedAsync();
  }
}
