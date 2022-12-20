﻿using System.ComponentModel.DataAnnotations;

namespace SightKeeper.DAL.Members.Common;

public class ItemClass
{
	[Key] public Guid Id { get; private set; }
	public string Name { get; set; }

	
	public ItemClass(string name) => Name = name;


	private ItemClass(Guid id, string name)
	{
		Id = id;
		Name = name;
	}
}