namespace ModularMonolith.BlazorApp.Components.BoardGames;

public sealed class BoardGameEditorViewModel
{
  public BoardGameEditorViewModel(string name, decimal price)
  {
    Name = name;
    Price = price;
  }

  public string Name { get; set; }
  public decimal Price { get; set; }
}
