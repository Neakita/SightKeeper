using SightKeeper.Backend.Models.Abstract;
using SightKeeper.DAL;
using SightKeeper.DAL.Members.Abstract;

namespace SightKeeper.Backend.Models;

public abstract class DatabaseModelEditor<T> :  ModelEditor<T>, IDisposable where T : Model
{
	public T EditableModel { get; }
	public abstract bool CanSaveChanges { get; }


	protected DatabaseModelEditor(T model, IAppDbContext dbContext)
	{
		EditableModel = model;
		_dbContext = dbContext;
	}

	public virtual void SaveChanges()
	{
		if (!CanSaveChanges) throw new InvalidOperationException("Cannot save changes");
	}

	public void DiscardChanges() => _dbContext.RollBack(EditableModel);


	private readonly IAppDbContext _dbContext;

	protected virtual void Dispose(bool disposing)
	{
		if (disposing) _dbContext.Dispose();
	}

	public void Dispose()
	{
		Dispose(true);
		GC.SuppressFinalize(this);
	}
}
