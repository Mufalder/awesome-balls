using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AwesomeBalls
{
	public class ResourceProperty<T> where T : Object
	{

		private string searchName;

		private T value;
		public T Value
		{
			get
			{
				if (value == null)
				{
					value = Resources.Load<T>(searchName);
				}
				return value;
			}
		}

		public ResourceProperty(string searchName)
		{
			this.searchName = searchName;
		}

	}
}