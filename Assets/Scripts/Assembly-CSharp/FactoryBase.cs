using System;
using System.Collections.Generic;

public abstract class FactoryBase<T_Key, T_Product> where T_Product : FactoryProduct
{
	protected Dictionary<T_Key, T_Product> dictionary = new Dictionary<T_Key, T_Product>();

	public void Register(T_Key key, T_Product product)
	{
		dictionary.Add(key, product);
	}

	public virtual object Create(T_Key key)
	{
		T_Product value;
		if (dictionary.TryGetValue(key, out value))
		{
			return value.Clone();
		}
		return null;
	}
}
