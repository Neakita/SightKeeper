namespace SightKeeper.Domain.Model.Common;

public class Game
{
	public Game(string title, string processName)
	{
		Title = title;
		ProcessName = processName;
	}
	
	private Game(int id, string title, string processName)
	{
		Id = id;
		Title = title;
		ProcessName = processName;
	}

	public int Id { get; private set; }
	
	public string Title { get; set; }
	
	public string ProcessName { get; private set; }

	public virtual ICollection<Abstract.Model> Models { get; } = new List<Abstract.Model>();
}